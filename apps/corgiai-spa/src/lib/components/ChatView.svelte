<script lang="ts">
  import SvelteMarkdown from 'svelte-markdown';

  import MdLink from './MdLink.svelte';
  import MdText from './MdText.svelte';
  import { chatStore } from '../stores/chat.store';
</script>

<div class="grow mt-16 w-1/2 flex flex-col gap-4 text-start overflow-y-auto">
  {#each $chatStore.data as message (message.id)}
    <div class="p-6 bg-white border border-gray-200 rounded-lg shadow dark:bg-gray-800 dark:border-gray-700">
      <SvelteMarkdown
        source={message.text}
        renderers={{ link: MdLink, text: MdText }}
      />
    </div>
  {/each}
</div>

<style>
</style>
