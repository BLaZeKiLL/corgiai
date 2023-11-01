from langchain.embeddings import (
    OllamaEmbeddings
)

from langchain.schema.embeddings import Embeddings

# Should use $ENV:EMBEDDING_MODEL to choose an embedding model
def load_embeddings(base_url: str, model: str) -> tuple[Embeddings, int]:
    embeddings = OllamaEmbeddings(
        base_url=base_url,
        model=model
    )

    return embeddings, 4096