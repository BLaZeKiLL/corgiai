from typing import List, Dict, Union

import os
import requests

from dotenv import load_dotenv
from fastapi import FastAPI
from pydantic import BaseModel

from neo import (
    connect_neo_graph,
    create_constraints,
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

app = FastAPI()

@app.get("/")
def read_root():
    return {"Hello": "Corgi"}


@app.post("/import/{site}/")
def import_data(config: ImportConfig, site: str, count: int = 100, page: int = 1):
    tags = ';'.join(config.tags)

    parameters = (
        f"?pagesize=100&page={page}&order=desc&sort=creation&answers=1&tagged={tags}"
        f"&site={site}&filter=!*236eb_eL9rai)MOSNZ-6D3Q6ZKb0buI*IVotWaTb"
    )

    print(f'Importing data from : {site}')

    data = requests.get(SE_BASE_URL + parameters).json()
    data['site'] = site
    data['count'] = count
    data['page'] = page

    try:
        insert_data(data, embeddings, neo_graph)
    except:
        return {
            "status": "failed",
            "description": "insert data failed",
            "site": site,
            "count": len(data["items"]),
            "page": page
        }

    return {
        "status": "sucess",
        "site": site,
        "count": len(data["items"]),
        "page": page
    }
