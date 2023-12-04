import axios from 'axios';

import { json } from '@sveltejs/kit';

import { env } from '$env/dynamic/private';
import { proxy } from '$lib/infra/proxy';

import type { QuestionResponse } from '$lib/models/response';


export async function GET({ url }) {
    const topic = url.searchParams.get('topic')!;

    console.log(`Question API : ${env.API_QUIZ}`);

    const response = await axios.get(`${env.API_QUIZ}/api/Prompt/question/${topic}`, { proxy: proxy });

    const result: QuestionResponse = response.data;

    console.log(result);

    return json(result, {status: 201});
}