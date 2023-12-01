<script lang="ts">
    import { AppShell, AppBar } from '@skeletonlabs/skeleton';
</script>

<AppShell>
	<svelte:fragment slot="header">
		<AppBar>Skeleton</AppBar>
	</svelte:fragment>

    <svelte:fragment slot="sidebarLeft">Sidebar Left</svelte:fragment>

    <svelte:fragment slot="pageHeader">Page Header</svelte:fragment>

    <slot />

    <svelte:fragment slot="pageFooter">Page Footer</svelte:fragment>

</AppShell>