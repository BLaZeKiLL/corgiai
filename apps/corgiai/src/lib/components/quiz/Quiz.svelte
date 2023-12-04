<script lang="ts">
	import { Stepper, Step, SlideToggle } from "@skeletonlabs/skeleton";
    import { quizStore } from "$lib/store/quiz.store";
	import QuizQuestion from "./QuizQuestion.svelte";
</script>

<div class="w-full h-full max-w-7xl mx-auto px-4">
    <Stepper buttonComplete="hidden" regionContent="stepper-max-height" stepTerm="Question">
        <div class="card w-full h-full p-4">
            {#each $quizStore.questions as question, index (question.id) }
                <Step regionContent="grow overflow-y-auto">
                    <svelte:fragment slot="header">Question {index+1}</svelte:fragment>
                    <QuizQuestion {question}/>
                </Step>
            {/each}
        </div>
    </Stepper>
</div>

<style>

</style>