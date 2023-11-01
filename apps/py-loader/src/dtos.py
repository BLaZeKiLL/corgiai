from typing import List
from pydantic import BaseModel


class ImportConfig(BaseModel):
    tags: List[str]