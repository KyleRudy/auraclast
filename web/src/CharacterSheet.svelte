<script>
	import Abilities from './Abilities.svelte';
import Attributes from './Attributes.svelte'
</script>

<div class="character-sheet">
    <h3 style="grid-column:1/4;grid-row:1/span 1;">Attributes</h3>
    <div style="grid-column:1/4;grid-row:2/span 1;">
        <Attributes/>
    </div>
    <h3 style="grid-column:1/1;grid-row:3/3">Abilities</h3>
    <h3 style="grid-column:2/2;grid-row:3/3">Backgrounds</h3>
    <h3 style="grid-column:3/3;grid-row:3/3">Intimacies</h3>
    <div style="grid-column:1/1;grid-row:4/span 4">
        <Abilities/>
    </div>
    
</div>

<style>
    div.character-sheet {
		width: 810px;
        user-select: none;
        -moz-user-select: none;
        -webkit-user-select: none;
        -ms-user-select: none;
        display: grid;
		grid-template-columns: repeat(3, 1fr);
        grid-column-gap: 10px;
    }
    div.character-sheet div {
        padding-left: 10px;
    }
    h3 {
        position: relative;
        font-size: 20px;
        z-index: 1;
        overflow: hidden;
        text-align: center;
        margin-block-start: 0px;
        margin-block-end: 0px;
    }
    h3:before, h3:after {
        position: absolute;
        top: 51%;
        overflow: hidden;
        width: 50%;
        height: 1px;
        content: '\a0';
        background-color: black;
    }
    h3:before {
        margin-left: -50%;
        text-align: right;
    }
    @media (max-width: 810px) {
		div.character-sheet {
            width: 100%;
        }
	}
</style>