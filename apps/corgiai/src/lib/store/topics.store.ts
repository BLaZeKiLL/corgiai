import { writable } from 'svelte/store';

import type { Topic } from '$lib/models/models';
import type { TopicsResponse } from '$lib/models/response';

export interface TopicsStoreState {
    topics: Topic[]
}

function createTopicsStore() {
    const { subscribe, update } = writable<TopicsStoreState>({ topics: [] });

    const load_topics = async () => {
        try {
            const response = await fetch("/api/neo/topics", {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                },
            });

            const result: TopicsResponse = await response.json();

            update((state: TopicsStoreState) => ({...state, topics: result.topics.map(x => ({...x, id: Math.random().toString(36).substring(2, 9)}))}));
        } catch (error) {
            console.error(error);

            update((state: TopicsStoreState) => ({...state, topics: [] }));
        }
    }

    load_topics(); // load on creation

    return {
        subscribe
    }
}

const store = createTopicsStore()

export const topicsStore = store;