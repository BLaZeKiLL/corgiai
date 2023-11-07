<script lang="ts">
  import SvelteMarkdown from 'svelte-markdown';

  import MdLink from './MdLink.svelte';
  import MdText from './MdText.svelte';
  import { chatStore } from '../stores/chat.store';
</script>

<div class="grow mt-16 w-1/2 flex flex-col gap-4 text-start overflow-y-auto">
  {#each $chatStore.data as message (message.id)}
    <div class="p-6 bg-white border border-gray-200 rounded-lg shadow dark:bg-gray-800 dark:border-gray-700">
      {#if message.model && message.time}
        <span class="text-sm text-gray-500">
          Model : {message.model}, Response Time : {message.time.toFixed(3)} sec
        </span>
      {/if}
      <article class="prose max-w-full">
        <SvelteMarkdown
          source={message.text}
          renderers={{ link: MdLink, text: MdText }}
        />
      </article>
    </div>
  {/each}
</div>

<style>
  /* Markdown Overides */
  /* ol {
    padding: 1em;
    list-style: decimal;
  } */
</style>
