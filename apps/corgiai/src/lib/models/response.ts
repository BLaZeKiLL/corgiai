import type { Tag } from "./models";

export interface ChatResponse {
    status: string;
    model: string;
    chain_duration: number;
    duration: number;
    text: string;
}

export interface TagsResponse {
    tags: Tag[];
}