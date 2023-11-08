import time
import os
import requests

import streamlit as st

from dotenv import load_dotenv
# from fastapi import FastAPI

from neo import (
    connect_neo_graph,
    create_constraints,
    create_vector_index,
    insert_data
)
from dtos import ImportConfig
from chains import load_embeddings


SE_BASE_URL = 'https://api.stackexchange.com/2.3/search/advanced'

load_dotenv(".env")

url = os.getenv("NEO4J_URI")
username = os.getenv("NEO4J_USERNAME")
password = os.getenv("NEO4J_PASSWORD")
ollama_base_url = os.getenv("OLLAMA_BASE_URL")
embedding_model_name = os.getenv("EMBEDDING_MODEL")

# Remapping for Langchain Neo4j integration
os.environ["NEO4J_URL"] = url

neo_graph = connect_neo_graph(url, username, password)

embeddings, dimension = load_embeddings(ollama_base_url, embedding_model_name)

create_constraints(neo_graph)
create_vector_index(neo_graph, dimension)

# app = FastAPI()

# @app.post("/import/{site}/")
def import_data(config: ImportConfig, site: str, page: int = 1, pages: int = 10):
    start_time = time.time()

    tags = ';'.join(config.tags)

    bar = st.progress(0, text=f"Fetching data from {site}")

    items = []

    for i in range(0, pages):
        # filter is generated using - https://api.stackexchange.com/docs/filters
        parameters = (
            f"?pagesize=100&page={page + i}&order=desc&sort=votes&answers=1&accepted=True&tagged={tags}"
            f"&site={site}&filter=!*236eb_eL9rai)MOSNZ-6D3Q6ZKb0buI*IVotWaTb"
        )

        response = requests.get(SE_BASE_URL + parameters).json()

        if "items" in response:
            items += response["items"]
            bar.progress((i + 1) / pages, f'Fetching page : {page + i} from {site}, Got {len(response["items"])} Questions.')
        else:
            st.write(response)
            bar.progress((i + 1) / pages, f'Fetching page : {page + i} from {site}, Got Nothing.')
            
        time.sleep(5) # to avoid stack exchange rate limit

    data = {
        "site": site,
        "items": items
    }

    if len(data["items"]) > 0:
        try:
            result = insert_data(data, embeddings, neo_graph)
        except Exception as e:
            return {
                "status": "failed",
                "description": "insert data failed",
                "site": site,
                "duration": time.time() - start_time,
                "size": len(data["items"]),
                "neo": result,
                "error": e
            }

    return {
        "status": "sucess",
        "site": site,
        "size": len(data["items"]),
        "duration": time.time() - start_time,
        "neo": result
    }

# Streamlit

def get_site() -> str:
    input_text = st.text_input(
        "From which site do you want to import data?", value="math"
    )

    return input_text


def get_tag() -> str:
    input_text = st.text_input(
        "For which tag do you want to import data?", "quadratics"
    )

    return input_text


def get_pages() -> (int, int):
    col1, col2 = st.columns(2)

    with col1:
        start_page = st.number_input(
            "Start page", step=1, min_value=1
        )

    with col2:
        num_pages = st.number_input(
            "Number of pages (100 questions per page)", step=1, min_value=1, value=25
        )
    
    st.caption("Only questions with answers will be imported.")

    return (start_page, num_pages)


def render_page():
    st.header("Corgi Loader")
    st.subheader("Choose StackExchange sites and tags to load into Neo4j")
    st.caption("Go to http://[::]:7474/ to explore the graph.")

    site = get_site()
    tag = get_tag()

    start_page, num_pages = get_pages()

    if st.button("Import", type="primary"):
        with st.spinner("Loading... This might take a minute or two."):
            try:
                config = ImportConfig(tags=[tag])
                result = import_data(config, site, page=start_page, pages=num_pages)

                st.success(f"Imported {result['size']} questions in {result['duration']}", icon="✅")
                
                st.write(result)
            except Exception as e:
                st.error(f"Error: {e}", icon="🚨")


render_page()