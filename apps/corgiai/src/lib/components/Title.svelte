<script lang="ts">
  import ollamaLogo from '$lib/assets/ollama-dark.png';
  import dockerLogo from '$lib/assets/docker.svg';
  import corgiaiLogo from '$lib/assets/corgi.svg';
  import neoLogo from '$lib/assets/neo4j.svg';
  import pythonLogo from '$lib/assets/python.svg';

  const mode = import.meta.env.MODE;
</script>

<div class="flex flex-col">
  <div class="flex flex-col sm:flex-row place-content-center items-center gap-12 md:gap-14 lg:gap-20 xl:gap-24">
    <img src={ollamaLogo} class="w-16 md:w-24 lg:w-30 xl:w-36 logo ollama" alt="Ollama Logo" />
    <img src={dockerLogo} class="w-16 md:w-24 lg:w-30 xl:w-36 logo docker" alt="Docker Logo" />
    <img src={corgiaiLogo} class="w-24 md:w-36 lg:w-48 xl:w-64 logo corgi" alt="Corgi AI Logo" />
    <img src={neoLogo} class="w-16 md:w-24 lg:w-30 xl:w-36 logo neo" alt="Neo4j Logo" />
    <img src={pythonLogo} class="w-16 md:w-24 lg:w-30 xl:w-36 logo python" alt="Python Logo" />
  </div>
  
  <div class="mt-8 sm:mt-32 flex flex-col place-content-center items-center gap-y-4">
    <h1 class="h1">Corgi AI</h1>
    <p class="text-surface-500 dark:text-surface-400">
      {mode}
    </p>
  </div>
</div>

<style>
  .logo {
    will-change: filter;
    transition: filter 300ms;
  }
  .logo.ollama:hover {
    filter: drop-shadow(0 0 2em #ffffffaa);
  }
  .logo.docker:hover {
    filter: drop-shadow(0 0 2em #3596edaa);
  }
  .logo.corgi:hover {
    filter: drop-shadow(0 0 2em #f3b15baa);
  }
  .logo.neo:hover {
    filter: drop-shadow(0 0 2em #66b245aa);
  }
  .logo.python:hover {
    filter: drop-shadow(0 0 2em #fbd546aa);
  }
</style>