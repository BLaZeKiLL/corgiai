<script lang="ts">
    import { tick } from 'svelte';
    import SvelteMarkdown from 'svelte-markdown';

    export let source: string = '';

    const mathjax = async () => {
        await tick();

        MathJax.typeset(); // TODO : check only div
    }
</script>

<article class="max-w-full prose dark:prose-invert">
    <SvelteMarkdown
        {source}
        on:parsed={mathjax}
    />
</article>