<script lang="ts">
	import { SendHorizontal } from 'lucide-svelte';

	import { chatStore } from '$lib/store/chat.store';

	let question = '';

	let site: string;

	const send = () => {
		chatStore.send(question, site);

		question = '';
	};
</script>

<form
	on:submit|preventDefault={send}
	class="w-full flex flex-col place-content-center items-center gap-y-4"
>
	<div class="w-full flex flex-row">
		<select
			bind:value={site}
			name="site"
			class="select w-auto left-border-radius text-center"
		>
			<option value="math">Math</option>
			<option value="physics">Physics</option>
			<option value="stackoverflow">Programming</option>
		</select>

		<span class="grow relative">
			<input
				bind:value={question}
				name="prompt"
				type="text"
				placeholder="How can I help you?"
				class="block w-full input right-border-radius"
			/>
	
			<button type="submit" class="absolute top-0 end-0 p-2.5 h-full">
				<SendHorizontal />
			</button>
		</span>
	</div>

</form>

<style>
	.left-border-radius {
		border-top-left-radius: var(--theme-rounded-base);
		border-bottom-left-radius: var(--theme-rounded-base);
		border-top-right-radius: 0;
		border-bottom-right-radius: 0;
	}

	.right-border-radius {
		border-top-left-radius: 0;
		border-bottom-left-radius: 0;
		border-top-right-radius: var(--theme-rounded-base);
		border-bottom-right-radius: var(--theme-rounded-base);
	}
</style>