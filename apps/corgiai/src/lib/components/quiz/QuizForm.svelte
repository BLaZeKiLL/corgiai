<script lang="ts">
	import { InputChip, Autocomplete, type AutocompleteOption, popup, type PopupSettings } from "@skeletonlabs/skeleton";
    import { topicsStore } from '$lib/store/topics.store';

    export let value: {count: number, topics: string[]} = {
        count: 5,
        topics: []
    }

    let topicsChip = '';

    // Gotta be reactive I guess
    $: topics = $topicsStore.topics.map(x => x.value);
    $: flavorOptions = topics.map(x => ({label: x, value: x})) as AutocompleteOption<string, string>[];

    const onInputChipSelect = (e: CustomEvent<AutocompleteOption<string, string>>) => {
		if (value.topics.includes(e.detail.value) === false) {
			value.topics = [...value.topics, e.detail.value];
			topicsChip = '';
		}
    }

    const popupSettings: PopupSettings = {
        event: 'focus-click',
        target: 'popupAutocomplete',
        placement: 'bottom',
    };
</script>

<div class="w-full h-full max-w-7xl mx-auto px-4 flex flex-row place-content-center items-center">
    <div class="p-4 card w-1/2 border border-surface-500">
        <header class="card-header text-center mb-4">
            <h2 class="h2">Quiz Settings</h2>
        </header>
        <form class="w-full flex flex-col gap-4">
            <span class="w-full label flex flex-col">
                <span class="mb-2">Number of questions</span>
                <input 
                    name="questionCount"
                    class="input" 
                    type="number" 
                    placeholder="Number of questions in the quiz" 
                    bind:value={value.count}
                />
            </span>

            <span class="w-full label flex flex-col">
                <span class="mb-2">Topics</span>
                <span use:popup={popupSettings}>
                    <InputChip 
                        name="topics"
                        bind:input={topicsChip} 
                        bind:value={value.topics} 
                        chips="variant-filled-primary"  
                        whitelist={topics} 
                    />
                </span>
                <div data-popup="popupAutocomplete" class="card border border-primary-500 w-full max-w-xl max-h-48 p-4 overflow-y-auto" tabindex="-1">
                    <Autocomplete
                        bind:input={topicsChip}
                        options={flavorOptions}
                        denylist={value.topics}
                        on:selection={onInputChipSelect}
                    />
                </div>
            </span>
        </form>
    </div>
</div>