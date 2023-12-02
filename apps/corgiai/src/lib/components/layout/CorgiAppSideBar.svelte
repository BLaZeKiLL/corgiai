<script lang="ts">
	import { AppRail, AppRailAnchor, } from "@skeletonlabs/skeleton";
	import { BookOpenCheck, Database, Home, MessagesSquare } from "lucide-svelte";

</script>

<div class="h-full bg-surface-50-900-token border-r border-surface-500/30">
	<AppRail background="bg-transparent" border="border-r border-surface-500/30">
		<AppRailAnchor href="/app">
			<svelte:fragment slot="lead">
				<span class="flex flex-row place-content-center items-center"><Home size={32} /></span>
			</svelte:fragment>
			<span>Home</span>
		</AppRailAnchor>
		<AppRailAnchor href="/app/chat">
			<svelte:fragment slot="lead">
				<span class="flex flex-row place-content-center items-center"><MessagesSquare size={32} /></span>
			</svelte:fragment>
			<span>Chat</span>
		</AppRailAnchor>
		<AppRailAnchor href="/app/quiz">
			<svelte:fragment slot="lead">
				<span class="flex flex-row place-content-center items-center"><BookOpenCheck size={32}/></span>
			</svelte:fragment>
			<span>Quiz</span>
		</AppRailAnchor>
		<AppRailAnchor href="/app/knowledge">
			<svelte:fragment slot="lead">
				<span class="flex flex-row place-content-center items-center"><Database size={32}/></span>
			</svelte:fragment>
			<span>Knowledge</span>
		</AppRailAnchor>
	</AppRail>
</div>

