from langchain.embeddings import (
    OllamaEmbeddings,
    SentenceTransformerEmbeddings
)

from langchain.schema.embeddings import Embeddings
from langchain.chat_models.base import BaseChatModel
from langchain.chat_models import ChatOllama
from langchain.vectorstores.neo4j_vector import Neo4jVector
from langchain.chains import RetrievalQAWithSourcesChain
from langchain.chains.qa_with_sources import load_qa_with_sources_chain
from langchain.prompts.chat import (
    ChatPromptTemplate,
    SystemMessagePromptTemplate,
    HumanMessagePromptTemplate,
)

# Should use $ENV:EMBEDDING_MODEL to choose an embedding model
# dimensions are used by vector index and is model specific
def load_embeddings(model: str, config: dict) -> tuple[Embeddings, int]:
    if model == 'llama2':
        embeddings = OllamaEmbeddings(
            base_url=config["base_url"],
            model=model
        )
        dimensions = 4096
    else: # sentence_transformer
        embeddings = SentenceTransformerEmbeddings(
            model_name="all-MiniLM-L6-v2",
            cache_folder="/embedding_model"
        )
        dimensions = 384

    return embeddings, dimensions


def load_llm(model: str, config: dict) -> BaseChatModel:
    if len(model):
        return ChatOllama(
            temperature=0,
            base_url=config["ollama_base_url"],
            model=model,
            # streaming=True,
            # seed=2,
            top_k=10,  # A higher value (100) will give more diverse answers, while a lower value (10) will be more conservative.
            top_p=0.3,  # Higher value (0.95) will lead to more diverse text, while a lower value (0.5) will generate more focused text.
            num_ctx=3072,  # Sets the size of the context window used to generate the next token.
        )


def configure_llm_chain_factory(llm: BaseChatModel, embeddings: Embeddings, graph_url: str, graph_username: str, graph_password: str):
    general_system_template = """ 
    Use the following pieces of context to answer the question at the end.
    The context contains question-answer pairs and their hyperlinks to a StackExchange forum post.
    You should prefer information from accepted or more upvoted answers.
    Make sure to rely on information from the answers and not on questions to provide accurate responses.
    When you find particular answer in the context useful, make sure to cite it in the answer using the link.
    If you don't know the answer, just say that you don't know, don't try to make up an answer.
    ----
    {summaries}
    ----
    Each answer you generate should contain a section at the end of hyperlinks to 
    a StackExchange forum questions and answers you found useful, which are described under source value in metadata.
    You can only use hyperlinks that are present in the context, dont try to make up hyperlinks. 
    always add hyperlinks to the end of the answer in the style of citations.
    Generate concise answers with references sources section of hyperlinks to 
    relevant a StackExchange forum questions only at the end of the answer.
    """

    general_user_template = "Question:```{question}```"

    messages = [
        SystemMessagePromptTemplate.from_template(general_system_template),
        HumanMessagePromptTemplate.from_template(general_user_template),
    ]

    qa_prompt = ChatPromptTemplate.from_messages(messages)

    qa_chain = load_qa_with_sources_chain(
        llm,
        chain_type="stuff",
        prompt=qa_prompt,
    )

    def factory(site: str):
        # I can inject site here but that would mean we will have to maintain 1 chain per site
        # Question and score are getting injected from the chain, there should be a way to inject site
        # but question might be coming from index since we are passing the index name
        # ! NOTE : Text injection like this in not safe
        retrieval_query = """
        WITH node AS question, score AS similarity
        """ + 'MATCH (question) -[:SITE]-> (s:Site {name: "' + site + '"})' + """
        CALL  { with question
            MATCH (question)<-[:ANSWERS]-(answer)
            WITH answer
            ORDER BY answer.is_accepted DESC, answer.score DESC
            WITH collect(answer)[..2] as answers
            RETURN reduce(str='', answer IN answers | str + 
                    '\n### Answer (Accepted: '+ answer.is_accepted +
                    ' Score: ' + answer.score+ '): '+  answer.body + '\n') as answerTexts
        } 
        RETURN '##Question: ' + question.title + '\n' + question.body + '\n' 
            + answerTexts AS text, similarity as score, {source: question.link} AS metadata
        ORDER BY similarity ASC // so that best answers are the last
        """

        knowledge_graph = Neo4jVector.from_existing_index(
            embedding=embeddings,
            url=graph_url,
            username=graph_username,
            password=graph_password,
            database="neo4j",  # neo4j by default
            index_name="top_questions",  # vector by default
            text_node_property="body",  # text by default
            retrieval_query=retrieval_query
        )

        # k = number of vector matches, k=2 means 2 nearest documents would be returned (It is KNN)
        return RetrievalQAWithSourcesChain(
            combine_documents_chain=qa_chain,
            retriever=knowledge_graph.as_retriever(search_kwargs={"k": 2}),
            reduce_k_below_max_tokens=False,
            max_tokens_limit=3375 # This is a difference of 4096 - len(general template) I guess, can make this dynamic
        )
    
    cache = {}

    def cacher(site: str) -> RetrievalQAWithSourcesChain:
        if not site in cache:
            chain = factory(site)
            cache[site] = chain

        return cache[site]
    
    return cacher