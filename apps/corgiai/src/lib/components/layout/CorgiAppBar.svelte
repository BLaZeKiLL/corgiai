<script lang="ts">
    import { AppBar, LightSwitch } from '@skeletonlabs/skeleton';
	import { Github, PawPrint, Youtube } from 'lucide-svelte';
</script>

<AppBar shadow="shadow-2xl">
    <svelte:fragment slot="lead">
        <PawPrint size={48} />
    </svelte:fragment>
    <h2 class="h2">Corgi AI</h2>
    <svelte:fragment slot="trail">
        <section class="hidden sm:flex flex-row place-content-center items-center space-x-1">
            <LightSwitch />
			<a class="btn-icon hover:variant-soft-primary" href="https://github.com/BLaZeKiLL/corgiai" target="_blank" rel="noreferrer">
				<Github />
			</a>
			<a class="btn-icon hover:variant-soft-primary" href="https://www.youtube.com/channel/UC_qfPIYfXOvg0SDAc8Z68WA" target="_blank" rel="noreferrer">
				<Youtube />
			</a>
		</section>
    </svelte:fragment>
</AppBar>