<script lang="ts">
  import { tick } from 'svelte';

  import SvelteMarkdown from 'svelte-markdown';
  
  import { chatStore } from '$lib/store/chat.store';
	import { ProgressBar } from '@skeletonlabs/skeleton';

  const mathjax = async () => {
    await tick();

    MathJax.typeset(); // check only div
  }
</script>

<div class="flex flex-col gap-4 text-start scrollable-content">
  {#each $chatStore.data as message (message.id)}
    <div class="w-full max-w-7xl mx-auto px-4">
      <div class="p-4 card border border-surface-500">
        {#if message.model && message.time}
          <span class="text-sm text-surface-500">
            Model : {message.model}, Response Time : {message.time.toFixed(3)} sec
          </span>
        {/if}
        <article class="max-w-full prose dark:prose-invert">
          {#if message.text.length > 0}
            <SvelteMarkdown
              source={message.text}
              on:parsed={mathjax}
            />
          {:else}
            <ProgressBar meter="bg-primary-500" track="bg-primary-500/30" />
          {/if}
        </article>
      </div>
    </div>
  {/each}
</div>

<style>
  /* Fade effect when reaching the top/bottom of scroll */
  /* You might need to adjust the gradient and its positioning */
  /* to achieve the desired fade effect */
  .scrollable-content::before,
  .scrollable-content::after {
    content: '';
    position: absolute;
    top: 0;
    width: 100%;
    height: 20px;
    pointer-events: none;
  }
  
  /* Fade at top */
  .scrollable-content::before {
    background: linear-gradient(transparent, white);
  }
  
  /* Fade at bottom */
  .scrollable-content::after {
    bottom: 0;
    background: linear-gradient(white, transparent);
  }
  </style>
  