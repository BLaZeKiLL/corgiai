<script lang="ts">
  import Chat from './lib/Chat.svelte';
import Title from './lib/Title.svelte';

  let chat_start = false;
  let chat_ready = false;

  const hide = (event: KeyboardEvent) => {
    if (event.code === 'Enter') {
      chat_start = true;
    }
  }
</script>

<main class="flex w-screen flex-col place-content-center items-center gap-y-4">
  <span class="grow mt-16">
    {#if !chat_start}
      <Title on:transitionend={() => chat_ready = true}/>
    {/if}
  </span>

  <Chat on:keyup={hide} ready={chat_ready}/>

</main>
