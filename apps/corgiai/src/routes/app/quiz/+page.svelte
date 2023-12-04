<script lang="ts">
    import Quiz from "$lib/components/quiz/Quiz.svelte";
    import QuizForm from "$lib/components/quiz/QuizForm.svelte";
    import QuizReport from "$lib/components/quiz/QuizReport.svelte";
    import QuizControls from "$lib/components/quiz/QuizControls.svelte";

    import { quizStore } from "$lib/store/quiz.store";

	import { QuizMode } from "$lib/models/models";

    let mode: QuizMode = QuizMode.START;

    let quiz: { count: number, topics: string[] } = { count: 5, topics: [] };

    const mode_change = (e: CustomEvent<{mode: QuizMode}>) => {
        let next = e.detail.mode;

        if (next === QuizMode.QUIZ)
        {
            quizStore.startQuiz(quiz.count, quiz.topics);
        }

        mode = next;
    }
</script>

<div class="h-full flex flex-col">
    <div class="flex-1 pt-8 overflow-y-auto">
        {#if mode === QuizMode.START}
            <QuizForm bind:value={quiz} />
        {:else if mode === QuizMode.QUIZ}
            <Quiz />
        {:else}
            <QuizReport />
        {/if}
    </div>
    <div class="w-full max-w-7xl mx-auto p-8 mb-8 space-y-10">
        <QuizControls on:mode={mode_change} disable_main={quiz.topics.length === 0}/>
    </div>
</div>