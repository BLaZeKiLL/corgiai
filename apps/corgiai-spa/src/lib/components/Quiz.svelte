<script lang="ts">
  import SvelteMarkdown from 'svelte-markdown';
  import corgiaiLogo from '/corgi.svg';
  import MdLink from './MdLink.svelte';
  import MdText from './MdText.svelte';
  import { quizStore, QUESTIONS } from '../stores/quiz.store';

  let ans_elem: HTMLInputElement[] = [];
  let result = false;

  $: current = QUESTIONS[$quizStore.currentQuestion];

  const next_question = () => {
    let correct = true;

    for (let index = 0; index < ans_elem.length; index++) {
      const element = ans_elem[index];
      const ans = current.answers[index];

      correct = correct && element.checked === ans.correct;

      element.checked = false;
    }

    quizStore.nextQuestion(correct);
  };

  const end_quiz = () => {
    next_question();
    result = true;
  };
</script>

<div class="grow mt-16 w-1/2 text-start overflow-y-auto">
  {#if result}
    <div class="w-full h-full flex flex-col gap-4 place-content-center items-center">
      <img src={corgiaiLogo} class="w-64 logo corgi" alt="Corgi AI Logo" />
      <h1 class="text-slate-900 dark:text-white text-xl">Your Score:</h1>
      <h1 class="text-slate-900 dark:text-white text-2xl">
        {$quizStore.userScore}/{$quizStore.totalQuestions}
      </h1>
    </div>
  {/if}

  {#if !result}
    <div
      class="flex flex-col gap-4 w-full p-6 bg-white border border-gray-200 rounded-lg shadow dark:bg-gray-800 dark:border-gray-700"
    >
      <span class="mb-8 flex flex-col gap-2">
        <span class="text-slate-900 dark:text-white font-bold">Question:</span>
        <SvelteMarkdown
          source={current.question}
          renderers={{ link: MdLink, text: MdText }}
        />
      </span>

      {#each current.answers as ans, index (ans.id)}
        <span class="flex flex-row gap-2">
          <input
            type="checkbox"
            bind:this={ans_elem[index]}
            class="w-4 h-4 text-blue-600 bg-gray-100 border-gray-300 rounded focus:ring-blue-500 dark:focus:ring-blue-600 dark:ring-offset-gray-800 focus:ring-2 dark:bg-gray-700 dark:border-gray-600"
          />
          <span class="text-slate-900 dark:text-white font-bold">{ans.id}:</span
          >
          <SvelteMarkdown
            source={ans.text}
            renderers={{ link: MdLink, text: MdText }}
          />
        </span>
      {/each}

      <div class="self-end w-full flex flex-grow place-content-center">
        <button
          type="button"
          on:click={next_question}
          class="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 mr-2 mb-2 dark:bg-blue-600 dark:hover:bg-blue-700 focus:outline-none dark:focus:ring-blue-800"
          >Next Question</button
        >
        <span class="flex-grow" />
        <button
          type="button"
          on:click={end_quiz}
          class="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 mr-2 mb-2 dark:bg-blue-600 dark:hover:bg-blue-700 focus:outline-none dark:focus:ring-blue-800"
          >End Quiz</button
        >
      </div>
    </div>
  {/if}
</div>

<style>
  .logo {
    will-change: filter;
    transition: filter 300ms;
  }
  .logo.corgi:hover {
    filter: drop-shadow(0 0 2em #f3b15baa);
  }
</style>
