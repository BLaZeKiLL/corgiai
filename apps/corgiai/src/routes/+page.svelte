<script lang="ts">
    import { fade } from 'svelte/transition';

    import { AppMode } from '$lib/models/enums';

	import Chat from '$lib/components/Chat.svelte';
    import Title from '$lib/components/Title.svelte';
    import UserInput from '$lib/components/UserInput.svelte';

    let start = false;
    let ready = false;

    let mode: AppMode = AppMode.CHAT;

    const hide = (event: KeyboardEvent) => {
        if (event.code === 'Enter') {
            start = true;
        }
    }

    const changemode = (event: CustomEvent<AppMode>) => {
        mode = event.detail;
        console.log(mode);
    }
</script>

<div class="w-full h-full flex flex-col place-content-center items-center">
    {#if !start}
        <span transition:fade on:outroend={() => ready = true}>
            <Title />
        </span>
    {/if}

    {#if ready && mode === AppMode.CHAT}
        <Chat />
    {/if}



    <span class="grow"></span>

    <UserInput on:keyup={hide} on:changemode={changemode}/>
</div>
