<script lang="ts">
    import { tick } from 'svelte';
    import SvelteMarkdown from 'svelte-markdown';
	import CorgiAnchorTag from './CorgiAnchorTag.svelte';

    export let source: string = '';

    const mathjax = async () => {
        await tick();

        MathJax.typeset(); // TODO : check only div
    }
</script>

<article class="max-w-full prose dark:prose-invert">
    <SvelteMarkdown
        {source}
        renderers={{link: CorgiAnchorTag}}
        on:parsed={mathjax}
    />
</article>