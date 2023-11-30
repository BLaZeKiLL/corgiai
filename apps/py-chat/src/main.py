import time
import os
import requests
import json
import regex

from dotenv import load_dotenv
from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware

from dtos import (
    Question,
    Quiz
)

from neo import (
    connect_neo_graph,
    create_vector_index,
    get_random_question
)

from chains import (
    load_embeddings,
    load_llm,
    configure_qa_llm_chain_factory,
    configure_quiz_llm_chain
)

load_dotenv(".env")

url = os.getenv("NEO4J_URI")
username = os.getenv("NEO4J_USERNAME")
password = os.getenv("NEO4J_PASSWORD")
ollama_base_url = os.getenv("OLLAMA_BASE_URL")
embedding_model_name = os.getenv("EMBEDDING_MODEL")
llm_name = os.getenv("LLM")

# Remapping for Langchain Neo4j integration
os.environ["NEO4J_URL"] = url


neo_graph = connect_neo_graph(url, username, password)

embeddings, dimension = load_embeddings(ollama_base_url, embedding_model_name)

# create_constraints(neo_graph) now this is shared via the instance not on a driver level but this app won't be doing any writes so i guess we don't do this here
create_vector_index(neo_graph, dimension)

llm = load_llm(llm_name, config={"ollama_base_url": ollama_base_url})

get_chain = configure_qa_llm_chain_factory(llm, embeddings, url, username, password)
quiz_chain = configure_quiz_llm_chain()

app = FastAPI()

origins = ["*"]

app.add_middleware(
    CORSMiddleware,
    allow_origins=origins,
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

@app.get("/")
def read_root():
    return {"Hello": "Corgi Py Chat"}


@app.post("/query")
def question_query(question: Question):
    start_time = time.time()

    chain = get_chain(question.site)

    result = chain({
        "question": question.text,
        "chat_history": []
    }, callbacks=[])

    end_time = time.time()

    return {
        "model": llm_name,
        "duration": end_time - start_time,
        "text": result["answer"]
    }

@app.get("/question/{tag}")
def get_question(tag: str):
    return {
        "question": get_random_question(neo_graph, tag)
    }

@app.get("/quiz/{tag}")
def quiz_question(tag: str):
    start_time = time.time()

    prompt = quiz_chain.format_prompt(question = get_random_question(neo_graph, tag))
    output = llm(prompt.to_messages())

    end_time = time.time()

    json_string = regex.search(r'```json\n(.*?)```', output.content, regex.DOTALL).group(1)

    return {
        "status": "sucess",
        "model": llm_name,
        "duration": end_time - start_time,
        "quiz": json.loads(json_string)
    }