export interface ChatResponse {
    status: string;
    model: string;
    chain_duration: number;
    duration: number;
    text: string;
}