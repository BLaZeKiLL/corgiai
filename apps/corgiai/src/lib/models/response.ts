import type { Question, Topic } from "./models";

export interface ChatResponse {
    model: string;
    duration: number;
    text: string;
}

export interface TopicsResponse {
    topics: Topic[];
}

export interface QuestionResponse {
    model: string;
    duration: number;
    result: Question;
}