import { writable } from 'svelte/store';

import type { Question } from '$lib/models/models';
import type { QuestionResponse } from '$lib/models/response';

export interface QuizStoreState {
    totalQuestions: number;
    questions: Question[];
    report: {ans: boolean, index: number}[];
    topics: string[];
}

function createQuizStore() {
    const { subscribe, update, set } = writable<QuizStoreState>({ 
        totalQuestions: 0,
        questions: [],
        report: [],
        topics: []
    });

    const loadQuestion = async (id: string, topic: string) => {
        try {
            const response = await fetch("/api/question?" + new URLSearchParams({topic}), {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                },
            });

            const result: QuestionResponse = await response.json();

            update(state => {
                const questions = state.questions;

                const existingIdIndex = questions.findIndex(m => m.id === id);

                if (existingIdIndex === -1) return state;

                questions[existingIdIndex].loaded = true;
                questions[existingIdIndex].model = result.model;
                questions[existingIdIndex].duration = result.duration;
                questions[existingIdIndex].text = result.result.text;
                questions[existingIdIndex].options = result.result.options;
                questions[existingIdIndex].source = result.result.source;
                questions[existingIdIndex].topic = result.result.topic;

                return {...state, questions};
            });
        } catch (error) {
            update(state => {
                const questions = state.questions;

                const existingIdIndex = questions.findIndex(m => m.id === id);

                if (existingIdIndex === -1) return state;

                questions[existingIdIndex].model = 'ERROR'; // better handle

                return {...state, questions};
            })

            console.error(error);
        }
    }

    const loadQuiz = async (quiz: QuizStoreState) => {
        for (let index = 0; index < quiz.questions.length; index++) {
            const topic = quiz.topics[Math.floor(Math.random() * quiz.topics.length)];

            await loadQuestion(quiz.questions[index].id!, topic); 
        }
    }

    const startQuiz = (count: number, topics: string[]) => {
        const quiz: QuizStoreState = {
            topics: topics,
            totalQuestions: count,
            report: [...Array(count).keys()].map(() => ({ans: false, index: -1})),
            questions: [...Array(count).keys()].map(() => ({
                loaded: false,
                id: Math.random().toString(36).substring(2, 9),
                // required
                text: '',
                source: '',
                options: [],
                topic: ''
            }))
        };

        loadQuiz(quiz); // stream load

        set(quiz);
    }

    const submitAnswer = (index: number, ans: boolean, ans_index: number) => update(state => {
        const report = state.report;

        report[index] = {ans, index: ans_index};

        return {...state, report};
    });

    return {
        subscribe,
        startQuiz,
        submitAnswer
    }
}

const store = createQuizStore();

export const quizStore = store;