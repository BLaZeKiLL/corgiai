import axios from 'axios';

import { json } from '@sveltejs/kit';

import { env } from '$env/dynamic/private';
import { TAG_QUESTION_THRESHOLD } from '$env/static/private';

import { proxy } from '$lib/infra/proxy';
import type { TagsResponse } from '$lib/models/response';


export async function GET() {
    console.log(`Quiz Net API : ${env.API_QUIZ_NET}`);

    const response = await axios.get(`${env.API_QUIZ_NET}/topics`, { proxy: proxy, params: { threshold: TAG_QUESTION_THRESHOLD } });

    const result: TagsResponse = response.data;

    console.log(result);

    return json(result, {status: 201});
}