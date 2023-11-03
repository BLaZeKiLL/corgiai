<script lang="ts">
  import svelteLogo from './assets/svelte.svg'
  import corgiaiLogo from './assets/corgi.svg'
  import viteLogo from '/vite.svg'
  import Counter from './lib/Counter.svelte'

  const mode = import.meta.env.MODE;
</script>

<main>
  <div>
    <a href="https://vitejs.dev" target="_blank" rel="noreferrer">
      <img src={viteLogo} class="logo" alt="Vite Logo" />
    </a>
    <img src={corgiaiLogo} class="logo corgi" alt="Corgi AI Logo" />
    <a href="https://svelte.dev" target="_blank" rel="noreferrer">
      <img src={svelteLogo} class="logo svelte" alt="Svelte Logo" />
    </a>
  </div>
  <h1>Corgi AI</h1>

  <div class="card">
    <Counter />
  </div>

  <p>
    May the boops be with you ❤️
  </p>

  <p class="read-the-docs">
    MODE : {mode}
  </p>
</main>

<style>
  .logo {
    height: 6em;
    padding: 1.5em;
    will-change: filter;
    transition: filter 300ms;
  }
  .logo:hover {
    filter: drop-shadow(0 0 2em #646cffaa);
  }
  .logo.corgi:hover {
    filter: drop-shadow(0 0 2em #F3B15Baa);
  }
  .logo.svelte:hover {
    filter: drop-shadow(0 0 2em #ff3e00aa);
  }
</style>
