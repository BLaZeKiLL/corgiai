<script lang="ts">
    import { AppShell } from '@skeletonlabs/skeleton';

	import CorgiAppBar from '$lib/components/layout/CorgiAppBar.svelte';
	import CorgiSideBar from '$lib/components/layout/CorgiAppSideBar.svelte';
	import CorgiAppFooter from '$lib/components/layout/CorgiAppFooter.svelte';
</script>

<AppShell>
	<svelte:fragment slot="header">
		<CorgiAppBar />
	</svelte:fragment>

    <svelte:fragment slot="sidebarLeft">
        <CorgiSideBar />
    </svelte:fragment>

    <!-- <svelte:fragment slot="pageHeader">Page Header</svelte:fragment> -->

    <!-- <div class="w-full h-full max-w-7xl mx-auto p-4 py-16 space-y-10"> -->
        <slot />
    <!-- </div> -->

    <!-- <svelte:fragment slot="pageFooter">
        <CorgiAppFooter />
    </svelte:fragment> -->

</AppShell>

<style>

</style>