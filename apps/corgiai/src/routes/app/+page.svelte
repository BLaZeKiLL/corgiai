<script lang="ts">
    import { topicsStore } from '$lib/store/topics.store';
	import { BookOpenCheck, Database, MessagesSquare } from 'lucide-svelte';
    import { variant_hash } from '$lib/utils/theme';

    let variant_class = variant_hash();
</script>

<div class="w-full h-full max-w-7xl mx-auto px-4 py-32">
    <div class="flex h-full flex-col gap-4 place-content-center items-center">
        <h1 class="h1 gradient-heading">What We Learning?</h1>

        <div class="grow flex flex-row gap-4 flex-wrap place-content-center items-center">
            {#each $topicsStore.topics as topic (topic.id)}
                <span class="flex flex-row">
                    <span class="badge {variant_class(topic.value)} left-border-radius">
                        {topic.value}
                    </span>
                    <span class="badge variant-filled-surface right-border-radius">
                        {topic.count}
                    </span>
                </span>
            {/each}
        </div>

        <div class="flex flex-row gap-8 place-content-center items-center">
            <a href="/app/chat" class="btn btn-l gap-2 variant-filled-primary">
                <MessagesSquare />
                Chat
            </a>
            <a href="/app/quiz" class="btn btn-l gap-2 variant-filled-primary">
                <BookOpenCheck />
                Quiz
            </a>
            <a href="/app/knowledge" class="btn btn-l gap-2 variant-filled-primary">
                <Database />
                Knowledge
            </a>
        </div>
    </div>
</div>

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