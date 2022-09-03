<script>
    import { createEventDispatcher } from 'svelte';
    import Dots from "./Dots.svelte"
    export let rating = { name: "Unknown", type: "_", value: 1, validTypes: ["_", "c", "f"], minimum: 0, maximum: 5 };
    export let colorPast = 5;
    const dispatch = createEventDispatcher();

    let innerValue = rating.value; // we can't give the Dots component direct access to our model, sadly
    let innerMinimum = rating.minimum;
    let innerMaximum = rating.maximum;

    function changeType() {
        if(rating.validTypes.length > 1) {
            var index = rating.validTypes.indexOf(rating.type);
            rating.type = rating.validTypes[(index + 1) % rating.validTypes.length];
            dispatch('typeChanged', {rating: rating});
        }
        else return;
    }
    function changeValue(event) {
        rating.value = innerValue;
        dispatch('valueChanged', {rating: rating});
    }
    console.log("Hello, " + rating.name + " with minimum " + rating.minimum.toString());
</script>

<li>
{#if (rating.validTypes.length == 0)}
    <span class="rating-type"></span>
    <span class="rating-label">{rating.name}</span> 
{:else}
    <span on:click={changeType} class="rating-type clickable">{rating.type}</span>
    <span class="rating-label">{rating.name}</span> 
{/if}
<Dots value={innerValue} minimum={innerMinimum} maximum={innerMaximum} colorPast={colorPast} on:valueChanged={changeValue}/>
</li>

<style>
    li {
        list-style: none;
    }
    .rating-type {
        width: 1em;
        display: inline-block;
    }
    .rating-label {
        width: 9em;
        display: inline-block;
    }
    .clickable {
        cursor:pointer;
    }
</style>