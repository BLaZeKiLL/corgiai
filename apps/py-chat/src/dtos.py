from pydantic import BaseModel


class Question(BaseModel):
    text: str
    site: str


class Quiz(BaseModel):
    text: str
    topic: str