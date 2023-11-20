import axios from 'axios';

import { json } from '@sveltejs/kit';

import type { ChatRequest } from '$lib/models/request';
import type { ChatResponse } from '$lib/models/response';

import { env } from '$env/dynamic/private';

export async function POST({ request }) {
    const body: ChatRequest = await request.json();

    console.log(`Chat API : ${env.API_PY_CHAT}`);

    const response = await axios.post(`${env.API_PY_CHAT}/query`, body, {
        proxy: {
            host: 'localhost',
            port: 1055
        }
    });

    const result: ChatResponse = response.data;

    console.log(result);

    return json(result, {status: 201});
}