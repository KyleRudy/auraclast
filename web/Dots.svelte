<script>
    export let value = 1;
    export let maximum = 5;
	const filled_character = '⬤';
	const empty_character = '○';
    function setValue(num) {
		value = num;
	}
</script>

{#each Array(maximum) as _, index (index) }
	<span on:click={setValue(index + 1)}>
    {#if index < value}
		{filled_character}
	{:else}
		{empty_character}
    {/if}
	</span>
{/each}

<style>
	span 
    {
		cursor:pointer;
		font-family:-apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen-Sans, Ubuntu, Cantarell, 'Helvetica Neue', sans-serif;
	}
</style>