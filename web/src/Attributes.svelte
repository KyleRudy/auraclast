<script>
    import Rating from "./Rating.svelte"
    export let values = 
        {   Strength: {name: "Strength", value: 1, type: "_", validTypes: ["_", "c", "f"], minimum: 1, maximum: 5}, 
            Dexterity: {name: "Dexterity", value: 1, type: "_", validTypes: ["_", "c", "f"], minimum: 1, maximum: 5}, 
            Stamina: {name: "Stamina", value: 1, type: "_", validTypes: ["_", "c", "f"], minimum: 1, maximum: 5},
            Charisma: {name: "Charisma", value: 1, type: "_", validTypes: ["_", "c", "f"], minimum: 1, maximum: 5},
            Manipulation: {name: "Manipulation", value: 1, type: "_", validTypes: ["_", "c", "f"], minimum: 1, maximum: 5},
            Appearance: {name: "Appearance", value: 1, type: "_", validTypes: ["_", "c", "f"], minimum: 1, maximum: 5},
            Intelligence: {name: "Intelligence", value: 1, type: "_", validTypes: ["_", "c", "f"], minimum: 1, maximum: 5},
            Wits: {name: "Wits", value: 1, type: "_", validTypes: ["_", "c", "f"], minimum: 1, maximum: 5},
            Perception: {name: "Perception", value: 1, type: "_", validTypes: ["_", "c", "f"], minimum: 1, maximum: 5}
        };
    
</script>

<ul class="attributes">
    <li class="physical">
        <ul>
            <Rating rating={values.Strength} />
            <Rating rating={values.Dexterity} />
            <Rating rating={values.Stamina} />
        </ul>
    </li>
    <li class="mental">
        <ul>
            <Rating rating={values.Charisma} />
            <Rating rating={values.Manipulation} />
            <Rating rating={values.Appearance} />
        </ul>
    </li>
    <li class="social">
        <ul>
            <Rating rating={values.Intelligence} />
            <Rating rating={values.Wits} />
            <Rating rating={values.Perception} />
        </ul>
    </li>
</ul>

<style>
    ul.attributes {
		display: grid;
		grid-template-columns: repeat(3, 1fr);
        grid-column-gap: 10px;
        list-style: none;
    }
    ul {
        padding-left: 0;
        padding-inline-start: 0;
    }
    li {
        list-style: none;
    }
    @media (max-width: 810px) {
		ul.attributes li {
			grid-column: 1 / 3;
		}
	}
</style>