<script lang="ts">
    import { createEventDispatcher } from 'svelte';
	import type { Question } from "$lib/models/models";
	import { ProgressBar } from "@skeletonlabs/skeleton";
	import CorgiMarkdown from "$lib/components/ui/CorgiMarkdown.svelte";

    export let question: Question;

    const dispatch = createEventDispatcher();
</script>

<div class="w-full flex flex-col">
    {#if question.loaded}
        <CorgiMarkdown source={question.text}/>
        <hr class="my-4">
        <div class="flex flex-col gap-2">
            {#each question.options as option, index}
                <div class="flex items-center space-x-2 bg-surface-900 rounded-container-token p-2">
                    <input class="radio ml-2" type="radio" name="radio-direct" 
                        on:change={() => dispatch('answer', {ans: option.correct, ans_index: index})}
                    />
                    <CorgiMarkdown source={option.text}/>
                </div>
            {/each}
        </div>
    {:else}
        <ProgressBar meter="bg-primary-500" track="bg-primary-500/30" />
    {/if}
</div>


