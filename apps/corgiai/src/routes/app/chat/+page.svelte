<script lang="ts">
	import Chat from "$lib/components/chat/Chat.svelte";
	import UserInput from "$lib/components/chat/UserInput.svelte";


</script>

<div class="h-full flex flex-col">
    <div class="flex-1 pt-8 overflow-y-auto">
        <Chat />
    </div>
    <div class="w-full max-w-7xl mx-auto p-8 space-y-10">
        <UserInput />
    </div>
</div>