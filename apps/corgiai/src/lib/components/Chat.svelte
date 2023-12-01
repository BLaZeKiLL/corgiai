<script lang="ts">
    import { tick } from 'svelte';
  
    import SvelteMarkdown from 'svelte-markdown';
    import { chatStore } from '$lib/store/chat.store';
  
    const mathjax = async () => {
      await tick();
  
      MathJax.typeset(); // check only div
    }
  </script>
  
  <div class="grow mt-0 w-11/12 sm:w-8/12 lg:w-7/12 xl:w-6/12 flex flex-col gap-4 text-start overflow-y-auto">
    {#each $chatStore.data as message (message.id)}
      <div class="p-6 card">
        {#if message.model && message.time}
          <span class="text-sm text-gray-500">
            Model : {message.model}, Response Time : {message.time.toFixed(3)} sec
          </span>
        {/if}
        <article class="max-w-full prose dark:prose-invert prose-a:text-blue-500">
          <SvelteMarkdown
            source={message.text}
            on:parsed={mathjax}
          />
        </article>
      </div>
    {/each}
  </div>
  
  <style>
  </style>
  