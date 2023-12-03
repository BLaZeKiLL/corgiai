export interface Topic
{
    id?: string; // UI use only
    value: string;
    count: number;
}

export enum QuizMode
{
    START,
    QUIZ,
    REPORT
}