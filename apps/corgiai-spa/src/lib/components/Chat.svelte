<script lang="ts">
    import ChatView from "./ChatView.svelte";
    import { ChatState, chatStore } from "../stores/chat.store";
    import Spinner from "./Spinner.svelte";
    import Quiz from "./Quiz.svelte";

    export let ready = false;

    let question = 'How can I help you ?';
    let site: string;
    let mode: string;

    let select: HTMLSelectElement;
    let input: HTMLInputElement;


    const send = () => {
      if (mode === 'learn') {
        chatStore.send(question, site);
      } else {

      }

      question = '';
    }
</script>

{#if ready && mode === 'learn'}
  <ChatView/>
{/if}

{#if ready && mode === 'quiz'}
  <Quiz/>
{/if}

<form
  on:submit|preventDefault={send}
  class="my-16 w-1/2 flex flex-col place-content-center items-center gap-y-4"
>
  <div class="w-full flex flex-row">
    <select
      bind:value={site}
      bind:this={select}
      class="rounded-md bg-slate-100 dark:bg-slate-900 text-slate-900 dark:text-white border-slate-200 dark:border-slate-950"
    >
      <option value="math">Math</option>
      <option value="physics">Physics</option>
    </select>

    <span class="grow text-center flex flex-row place-content-center text-slate-900 dark:text-white">
      {#if $chatStore.state === ChatState.RECEIVING}
        <Spinner />
      {/if}
    </span>

    <select
      bind:value={mode}
      class="rounded-md bg-slate-100 dark:bg-slate-900 text-slate-900 dark:text-white border-slate-200 dark:border-slate-950"
    >
      <option value="learn">Learn</option>
      <option value="quiz">Quiz</option>
    </select>
  </div>

  <input
    bind:value={question}
    bind:this={input}
    on:keyup|preventDefault
    class="w-full rounded-md bg-slate-100 dark:bg-slate-900 text-slate-900 dark:text-white border-slate-200 dark:border-slate-950"
  />
</form>

<style>
</style>
