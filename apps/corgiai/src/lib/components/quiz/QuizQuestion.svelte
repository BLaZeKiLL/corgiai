<script lang="ts">
	import type { Question } from "$lib/models/models";
	import { ProgressBar } from "@skeletonlabs/skeleton";

    export let question: Question;
</script>

<div class="w-full flex flex-col">
    {#if question.loaded}
        <p>{question.text}</p>
    {:else}
        <ProgressBar meter="bg-primary-500" track="bg-primary-500/30" />
    {/if}
</div>


