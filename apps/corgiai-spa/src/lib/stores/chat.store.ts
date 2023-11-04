import { writable } from 'svelte/store';

const PY_CHAT = import.meta.env.VITE_PY_CHAT;

export interface Message {
    id: string;
    from: string;
    text: string;
    model?: string;
    time?: number;
}

export enum ChatState {
    IDLE,
    RECEIVING
}

export interface ChatStoreState {
    state: ChatState;
    data: Message[];
}

interface ChatResponse {
    status: string;
    model: string;
    chain_duration: number;
    duration: number;
    text: string;
}

function createChatStore() {
    const { subscribe, update } = writable<ChatStoreState>({ state: ChatState.IDLE, data: [] });

    const addMessage = (from: string, text: string) => {
        const id = Math.random().toString(36).substring(2, 9);

        update((state: ChatStoreState) => {
            state.data.push({id, from, text});

            return state;
        });

        return id;
    }

    const updateMessage = (existingId: string, text: string, model?: string, time?: number) => {
        if (!existingId) return;

        update((state: ChatStoreState) => {
            const messages = state.data;
            const existingIdIndex = messages.findIndex(m => m.id === existingId);

            if (existingIdIndex === -1) return state;

            messages[existingIdIndex].text += text;
            messages[existingIdIndex].model = model;
            messages[existingIdIndex].time = time;

            return {...state, data: messages } as ChatStoreState;
        })
    } // streaming

    const send = async (question: string, site: string) => {
        if (!question.trim().length) return;

        update((state) => ({ ...state, state: ChatState.RECEIVING }));

        addMessage('Me', question);
        const messageId = addMessage('Corgi', '');

        try {
            const response = await fetch(`${PY_CHAT}/query`, {
                method: 'POST',
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    text: question,
                    site: site
                })
            });

            const result: ChatResponse = await response.json();

            console.log(result);

            updateMessage(messageId, result.text, result.model, result.duration);

            update((state: ChatStoreState) => ({...state, state: ChatState.IDLE}));
        } catch (error) {
            updateMessage(messageId, 'Corgi Bamboozled');

            update((state: ChatStoreState) => ({...state, state: ChatState.IDLE}));
        }
    }

    return {
        subscribe,
        send
    };
}

const store = createChatStore();

export const chatStore = store; // single global store