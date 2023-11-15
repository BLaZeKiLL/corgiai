import { writable } from 'svelte/store';

export const QUESTIONS = [{
    question: `You pick up a can of soda off of the countertop.
    The countertop underneath the can feels colder than the rest of the counter. 
    Which explanation do you think is the best?`,
    answers: [{
        id: 'A',
        text: `The cold has been transferred from the soda to the counter.`,
        correct: false
    }, {
        id: 'B',
        text: `There is no heat energy left in the counter beneath the can.`,
        correct: false
    }, {
        id: 'C',
        text: `Some heat has been transferred from the counter to the soda.`,
        correct: true
    }, {
        id: 'D',
        text: `The heat beneath the can moves away into other parts of the countertop.`,
        correct: false
    }]
}, {
    question: `The Zeroth Law of Thermodynamics deals with which of the following?`,
    answers: [{
        id: 'A',
        text: `Heat`,
        correct: false
    }, {
        id: 'B',
        text: `Thermal equilibrium`,
        correct: true
    }, {
        id: 'C',
        text: `Thermal energy`,
        correct: false
    }, {
        id: 'D',
        text: `Kinetic energy`,
        correct: false
    }]
}]

export interface QuizStoreState {
    currentQuestion: number;
    totalQuestions: number;
    userScore: number;
}

function createQuizStore() {
    const { subscribe, update } = writable<QuizStoreState>({ 
        currentQuestion: 0,
        totalQuestions: 0,
        userScore: 0
    });

    const nextQuestion = (correct: boolean) => {
        update((state: QuizStoreState) => {
            const currentQuestion = (state.currentQuestion + 1) % QUESTIONS.length;
            const totalQuestions = state.totalQuestions + 1;
            const userScore = state.userScore + (correct ? 1 : 0);

            return {currentQuestion, totalQuestions, userScore};
        });
    }

    return {
        subscribe,
        nextQuestion
    }
}

const store = createQuizStore()

export const quizStore = store;