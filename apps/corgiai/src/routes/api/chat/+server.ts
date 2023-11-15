import { json } from '@sveltejs/kit';

import type { ChatRequest } from '$lib/models/request';
import type { ChatResponse } from '$lib/models/response';

import { API_PY_CHAT } from '$env/static/private';

export async function POST({ request }) {
    const body: ChatRequest = await request.json();

    const response = await fetch(`${API_PY_CHAT}/query`, {
        method: 'POST',
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(body)
    });

    const result: ChatResponse = await response.json();

    console.log(result);

    return json(result, {status: 201});
}