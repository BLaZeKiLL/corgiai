import axios from 'axios';

import { json } from '@sveltejs/kit';

import { env } from '$env/dynamic/private';
import { TOPIC_QUESTION_THRESHOLD } from '$env/static/private';

import { proxy } from '$lib/infra/proxy';
import type { TopicsResponse } from '$lib/models/response';


export async function GET() {
    console.log(`Neo API : ${env.API_QUIZ}`);

    const response = await axios.get(`${env.API_QUIZ}/api/Neo/topics`, { proxy: proxy, params: { threshold: TOPIC_QUESTION_THRESHOLD } });

    const result: TopicsResponse = response.data;

    console.log(result);

    return json(result, {status: 201});
}