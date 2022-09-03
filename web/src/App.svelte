<script>
import CharacterSheet from "./CharacterSheet.svelte";

	export let name;
</script>

<div id="container">
	<header>Auraclast Prototype</header>
	<aside>Menu blah blah</aside>
	<main>
		<h1>Hello {name}!</h1>
		<p>Blah blah, I'm boilerplate</p>
		<CharacterSheet></CharacterSheet>
	</main>
	<footer>Do we ever really want a footer?</footer>
</div>


<style>
	#container {
		width: 100%;
		height: 100vh;
		display: grid;
		grid-template-rows: 2rem 1fr 2rem;
		grid-template-columns: 150px 1fr;
	}
	header {
		grid-column: 1 / 3;
		grid-row: 1 / 2;
		background: blue;
	}
	aside {
		grid-column: 1 / 2;
		grid-row: 2 / 3;
		background: green;
	}
	main {
		grid-column: 2 / 3;
		grid-row: 2 / 3;
		background: white;
	}
	footer {
		grid-column: 1 / 3;
		grid-row: 3 / 4;
		background: yellow;
	}

	h1 {
		color: #ff3e00;
		text-transform: uppercase;
		font-size: 4em;
		font-weight: 100;
	}

	@media (max-width: 960px) {
		main {
			grid-column: 1 / 3;
		}
	}
</style>