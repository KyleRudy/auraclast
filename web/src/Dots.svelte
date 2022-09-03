<script>
    import { createEventDispatcher } from 'svelte';
    export let value = 1;
    export let minimum = 1;
    export let maximum = 5;
    export let colorPast = 5;
    const filled_character = '⬤';
    const empty_character = '○';
    const dispatch = createEventDispatcher();
    function setValue(num) {
		if(num >= minimum && num <= maximum) {
            if(value == num && value > minimum) {
                value--;
            } else {
                value = num;
            }
            dispatch('valueChanged', {});
        }
	}
    console.log("Entered dots with " + minimum.toString() + " minimum.");
</script>

<span class="dots">
<!-- we don't have a proper for loop in svelte, it iterates through a collection -->
<!-- So, we create an array of (maximum) elements with undefined values, and iterate through their keys -->
<!-- this is functionally equivalent to for(index = 0; index < maximum; index++)-->
{#each Array(maximum) as _, index (index) }
    {#if index < value}
        <span on:click={setValue(index + 1)} class="{index >= colorPast ? 'bonus' : ''}">{filled_character}</span>
	{:else}
        <span on:click={setValue(index + 1)}>{empty_character}</span>
    {/if}
{/each}
</span>

<style>
    span.dots 
    {
        text-align: right;
    }
	span.dots span 
    {
		cursor:pointer;
		font-family:-apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen-Sans, Ubuntu, Cantarell, 'Helvetica Neue', sans-serif;
    }
    .bonus 
    {
        color:green;
    }
</style>