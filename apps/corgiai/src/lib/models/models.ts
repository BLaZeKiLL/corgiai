export interface Topic {
    id?: string; // UI use only
    value: string;
    count: number;
}

export interface Question {
    id?: string; // UI use only
    loaded?: boolean; // UI use only
    model?: string;
    duration?: number;
    text: string;
    options: {text: string, correct: boolean}[];
    source: string;
    topic: string;
}

export enum QuizMode {
    START,
    QUIZ,
    REPORT
}