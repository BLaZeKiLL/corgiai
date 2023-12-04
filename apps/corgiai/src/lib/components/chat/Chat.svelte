<script lang="ts">
	import { ProgressBar } from '@skeletonlabs/skeleton';
  import { chatStore } from '$lib/store/chat.store';
	import CorgiMarkdown from '$lib/components/ui/CorgiMarkdown.svelte';
</script>

<div class="flex flex-col gap-4 text-start">
  {#each $chatStore.data as message (message.id)}
    <div class="w-full max-w-7xl mx-auto px-4">
      <div class="p-4 card border border-surface-500">
        {#if message.model && message.time}
          <span class="text-sm text-surface-500">
            Model : {message.model}, Response Time : {message.time.toFixed(3)} sec
          </span>
        {/if}
        <div class="w-full">
          {#if message.text.length > 0}
            <CorgiMarkdown source={message.text}/>
          {:else}
            <ProgressBar meter="bg-primary-500" track="bg-primary-500/30" />
          {/if}
          </div>
      </div>
    </div>
  {/each}
</div>
  