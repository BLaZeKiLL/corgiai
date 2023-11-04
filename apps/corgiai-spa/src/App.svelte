<script lang="ts">
  import { fade } from 'svelte/transition';

  import Chat from './lib/components/Chat.svelte';
  import Title from './lib/components/Title.svelte';

  let chat_start = false;
  let chat_ready = false;

  const hide = (event: KeyboardEvent) => {
    if (event.code === 'Enter') {
      chat_start = true;
    }
  }
</script>

<main class="flex w-screen flex-col place-content-center items-center gap-y-4">
  
  {#if !chat_start}
    <span transition:fade on:outroend={() => chat_ready = true} class="grow mt-16">
      <Title/>
    </span>
  {/if}

  <Chat on:keyup={hide} ready={chat_ready}/>

</main>
