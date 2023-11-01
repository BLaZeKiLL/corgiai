import time
import os
import requests

from dotenv import load_dotenv
from fastapi import FastAPI

from neo import (
    connect_neo_graph,
    create_constraints
)

load_dotenv(".env")

url = os.getenv("NEO4J_URI")
username = os.getenv("NEO4J_USERNAME")
password = os.getenv("NEO4J_PASSWORD")
ollama_base_url = os.getenv("OLLAMA_BASE_URL")
embedding_model_name = os.getenv("EMBEDDING_MODEL")

# Remapping for Langchain Neo4j integration
os.environ["NEO4J_URL"] = url

neo_graph = connect_neo_graph(url, username, password)

create_constraints(neo_graph)

app = FastAPI()

@app.get("/")
def read_root():
    return {"Hello": "Corgi Py Chat"}