import { json } from '@sveltejs/kit';

import type { ChatRequest } from '$lib/models/request';
import type { ChatResponse } from '$lib/models/response';

export async function POST({ request }) {
    const body: ChatRequest = await request.json();

    const response: ChatResponse = {
        text: body.text,
        model: 'llama2:7b',
        status: 'Success',
        chain_duration: 10.00,
        duration: 10.05
    }

    return json(response, {status: 201});
}