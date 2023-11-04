<script lang="ts">
    import ChatView from "./ChatView.svelte";

    export let ready = false;
</script>

{#if ready}
    <ChatView/>
{/if}

<form
  on:submit|preventDefault
  class="my-16 w-1/2 flex flex-col place-content-center items-center gap-y-4"
>
  <div class="w-full flex flex-row">
    <select
      class="rounded-md bg-slate-100 dark:bg-slate-900 text-slate-900 dark:text-white border-slate-200 dark:border-slate-950"
    >
      <option>Math</option>
      <option>Physics</option>
    </select>

    <span class="grow text-center text-slate-900 dark:text-white" />

    <select
      class="rounded-md bg-slate-100 dark:bg-slate-900 text-slate-900 dark:text-white border-slate-200 dark:border-slate-950"
    >
      <option>Learn</option>
      <option>Quiz</option>
    </select>
  </div>

  <input
    on:keyup|preventDefault
    class="w-full rounded-md bg-slate-100 dark:bg-slate-900 text-slate-900 dark:text-white border-slate-200 dark:border-slate-950"
  />
</form>

<style>
</style>
