<script lang="ts">
  import { createEventDispatcher } from 'svelte';

	import { AppMode } from '$lib/models/enums';
	import Spinner from '$lib/ui/Spinner.svelte';
	import { chatStore, ChatState } from '$lib/store/chat.store';

  const dispatch = createEventDispatcher();

	let question = 'How can I help you ?';

	let site: string;
	let mode: string;

	const send = () => {
		if (mode === 'chat') {
			chatStore.send(question, site);
		}

		question = '';
	};

  const changemode = () => {
    switch (mode) {
      case 'chat':
        dispatch('changemode', AppMode.CHAT);
        break;
      case 'quiz':
        dispatch('changemode', AppMode.QUIZ);
        break;
    }
  }
</script>

<form
	on:submit|preventDefault={send}
	class="mt-8 w-11/12 sm:w-8/12 lg:w-7/12 xl:w-6/12 flex flex-col place-content-center items-center gap-y-4"
>
	<div class="w-full flex flex-row">
		<select
			bind:value={site}
			class="rounded-md bg-slate-100 dark:bg-slate-900 text-slate-900 dark:text-white border-slate-200 dark:border-slate-950"
		>
			<option value="stackoverflow">Programming</option>
			<option value="math">Math</option>
			<option value="physics">Physics</option>
		</select>

		<span
			class="grow text-center flex flex-row place-content-center text-slate-900 dark:text-white"
		>
			{#if $chatStore.state === ChatState.RECEIVING}
				<Spinner />
			{/if}
		</span>

		<select
			bind:value={mode}
      on:change={changemode}
			class="rounded-md bg-slate-100 dark:bg-slate-900 text-slate-900 dark:text-white border-slate-200 dark:border-slate-950"
		>
			<option value="chat">Chat</option>
			<option value="quiz">Quiz</option>
		</select>
	</div>

	<input
		bind:value={question}
		on:keyup|preventDefault
		class="w-full rounded-md bg-slate-100 dark:bg-slate-900 text-slate-900 dark:text-white border-slate-200 dark:border-slate-950"
	/>
</form>
