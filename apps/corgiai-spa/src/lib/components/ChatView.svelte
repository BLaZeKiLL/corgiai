<script lang="ts">
  import { tick } from 'svelte';

  import SvelteMarkdown from 'svelte-markdown';
  import MdLink from './MdLink.svelte';
  import MdText from './MdText.svelte';
  import { chatStore } from '../stores/chat.store';

  const mathjax = async () => {
    await tick();

    MathJax.typeset(); // check only div
  }
</script>

<div class="grow mt-0 w-11/12 sm:w-8/12 lg:w-7/12 xl:w-6/12 flex flex-col gap-4 text-start overflow-y-auto">
  {#each $chatStore.data as message (message.id)}
    <div class="p-6 bg-white border border-gray-200 rounded-lg shadow dark:bg-gray-800 dark:border-gray-700">
      {#if message.model && message.time}
        <span class="text-sm text-gray-500">
          Model : {message.model}, Response Time : {message.time.toFixed(3)} sec
        </span>
      {/if}
      <article class="prose dark:prose-invert max-w-full">
        <SvelteMarkdown
          source={message.text}
          renderers={{ link: MdLink }}
          on:parsed={mathjax}
        />
      </article>
    </div>
  {/each}
</div>

<style>
</style>
