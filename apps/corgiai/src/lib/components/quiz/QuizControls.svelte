<script lang="ts">
    import { createEventDispatcher } from 'svelte';

	import { QuizMode } from "$lib/models/models";

    const dispatch = createEventDispatcher();

    export let disable_main: boolean = false;

    let mode: QuizMode = QuizMode.START;

    const get_main_btn_text = () => {
        switch (mode) {
            case QuizMode.START: return 'Start';
            case QuizMode.QUIZ: return 'Submit';
            case QuizMode.REPORT: return 'Restart';
        }
    }

    const get_second_btn_text = () => {
        switch (mode) {
            case QuizMode.QUIZ: return 'Quit';
            case QuizMode.REPORT: return 'Reset';
        }
    }

    const main_btn_click = () => {
        switch (mode) {
            case QuizMode.START:
                mode = QuizMode.QUIZ;
                break;
            case QuizMode.QUIZ:
                mode = QuizMode.REPORT;
                break;
            case QuizMode.REPORT:
                mode = QuizMode.QUIZ;
                break;
        }

        main_btn_text = get_main_btn_text();
        second_btn_text = get_second_btn_text();

        dispatch('mode', {mode});
    }

    const second_btn_click = () => {
        switch (mode) {
            case QuizMode.QUIZ:
                mode = QuizMode.START;
                break;
            case QuizMode.REPORT:
                mode = QuizMode.START;
                break;
        }

        main_btn_text = get_main_btn_text();
        second_btn_text = get_second_btn_text();

        dispatch('mode', {mode});
    }

    let main_btn_text = get_main_btn_text();
    let second_btn_text = get_second_btn_text();
</script>

<div class="w-full flex flex-row">
    {#if mode !== QuizMode.START}
        <button on:click={second_btn_click} class="btn variant-filled-warning">
            {second_btn_text}
        </button>
    {/if}
    <span class="grow"></span>
    <button on:click={main_btn_click} class="btn variant-filled-primary" disabled={disable_main}>
        {main_btn_text}
    </button>
</div>