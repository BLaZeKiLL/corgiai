import axios from 'axios';

import { json } from '@sveltejs/kit';

import { env } from '$env/dynamic/private';
import { proxy } from '$lib/infra/proxy';

import type { ChatRequest } from '$lib/models/request';
import type { ChatResponse } from '$lib/models/response';


export async function POST({ request }) {
    const body: ChatRequest = await request.json();

    console.log(`Chat API : ${env.API_PY_CHAT}`);

    const response = await axios.post(`${env.API_PY_CHAT}/query`, body, { proxy: proxy });

    const result: ChatResponse = response.data;

    console.log(result);

    return json(result, {status: 201});
}