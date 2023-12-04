<script lang="ts">
	import { Stepper, Step } from "@skeletonlabs/skeleton";
    import { quizStore } from "$lib/store/quiz.store";
	import QuizQuestion from "./QuizQuestion.svelte";
    import { variant_hash } from '$lib/utils/theme';

    let variant_class = variant_hash();
</script>

<div class="w-full h-full max-w-7xl mx-auto px-4">
    <Stepper buttonComplete="hidden" regionContent="stepper-max-height" stepTerm="Question">
        <div class="card w-full h-full p-4">
            {#each $quizStore.questions as question, index (question.id) }
                <Step regionContent="grow flex flex-col py-4 overflow-y-auto">
                    <svelte:fragment slot="header">
                        <div class="flex flex-row">
                            <span>Question {index+1}</span>
                            <span class="grow"></span>
                            {#if question.topic.length > 0}
                                <span class="badge {variant_class(question.topic)}">{question.topic}</span>
                            {/if}
                        </div>
                    </svelte:fragment>
                    
                    <QuizQuestion {question} on:answer={e => quizStore.submitAnswer(index, e.detail.ans, e.detail.ans_index)}/>
                    <svelte:fragment slot="navigation">
                        <button class="hidden">Back</button>
                    </svelte:fragment>
                </Step>
            {/each}
        </div>
    </Stepper>
</div>

<style>

</style>