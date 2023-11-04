<script lang="ts">
  import SvelteMarkdown from 'svelte-markdown';

  import MdLink from './MdLink.svelte';
  import MdText from './MdText.svelte';
  import { chatStore } from '../stores/chat.store';
</script>

<div class="grow mt-16 w-1/2 text-start overflow-y-auto">
  {#each $chatStore.data as message (message.id)}
    <SvelteMarkdown
      source={message.text}
      renderers={{ link: MdLink, text: MdText }}
    />
  {/each}
</div>

<style>
</style>
