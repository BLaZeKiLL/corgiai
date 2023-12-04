<script lang="ts">
    import { quizStore } from '$lib/store/quiz.store';
	import { ProgressRadial } from '@skeletonlabs/skeleton';
    import { variant_hash } from '$lib/utils/theme';
	import CorgiMarkdown from '$lib/components/ui/CorgiMarkdown.svelte';
	import type { Question } from '$lib/models/models';

    let variant_class = variant_hash();

    const correct_ans = (question: Question) => {
        return question.options.find(option => option.correct)?.text;
    }

    $: correct = $quizStore.report.reduce((p, c) => p + (c.ans ? 1 : 0), 0);
    $: value = (correct/ $quizStore.totalQuestions) * 100;
    $: score = `${correct} / ${$quizStore.totalQuestions}`;
</script>

<div class="w-full h-full max-w-7xl mx-auto px-4 flex flex-row place-content-center items-center">
    <div class="h-full p-4 flex flex-col card border border-surface-500">
        <header class="flex flex-row place-content-center items-center gap-4">
            <ProgressRadial width="w-16" value={value} stroke={60} meter="stroke-primary-500" track="stroke-primary-500/30" strokeLinecap="butt"/>
            <span class="grow"></span>
            <div class="flex flex-col gap-2">
                <h3 class="h3 text-right">Score : {score}</h3>
                <div class="flex flex-row-reverse gap-2">
                    {#each $quizStore.topics as topic}
                        <span class="badge {variant_class(topic)}">
                            {topic}
                        </span>
                    {/each}
                </div>
            </div>
        </header>

        <div class="grow flex flex-col gap-4 mt-4 overflow-y-auto">
            {#each $quizStore.questions as question, index (question.id) }
                <!-- If question is answered -->
                {#if $quizStore.report[index].index !== -1}
                    <hr>
                    <div class="flex flex-col gap-2">
                        <div class="flex flex-row place-content-center items-center gap-2">
                            <h4>Question {index + 1}</h4>
                            <span class="grow"></span>
                            <span class="badge {variant_class(question.topic)}">{question.topic}</span>
                            <a href={question.source} class="badge variant-filled-secondary" target="_blank" rel="noopener noreferrer">Source</a>
                        </div>

                        <CorgiMarkdown source={question.text}/>

                        {#if $quizStore.report[index].ans}
                            <div class="card variant-ghost-success p-4">
                                <CorgiMarkdown source={question.options[$quizStore.report[index].index].text}/>
                            </div>
                        {:else}
                            <div class="flex flex-row gap-4">
                                <div class="flex-1 card variant-ghost-error p-4">
                                    <CorgiMarkdown source={question.options[$quizStore.report[index].index].text}/>
                                </div>

                                <div class="flex-1 card variant-ghost-success p-4">
                                    <CorgiMarkdown source={correct_ans(question)}/>
                                </div>
                            </div>
                        {/if}
                    </div>
                {/if}
            {/each}
        </div>
    </div>
</div>