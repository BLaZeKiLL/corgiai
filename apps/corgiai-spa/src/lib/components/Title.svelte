<script lang="ts">
  import ollamaLogo from '../../assets/ollama-dark.png';
  import dockerLogo from '../../assets/docker.svg';
  import corgiaiLogo from '/corgi.svg';
  import neoLogo from '../../assets/neo4j.svg';
  import pythonLogo from '../../assets/python.svg';

  const mode = import.meta.env.MODE;
</script>

<div class="flex flex-row place-content-center items-center gap-x-32">
  <img src={ollamaLogo} class="w-36 logo ollama" alt="Ollama Logo" />
  <img src={dockerLogo} class="w-36 logo docker" alt="Docker Logo" />
  <img src={corgiaiLogo} class="w-64 logo corgi" alt="Corgi AI Logo" />
  <img src={neoLogo} class="w-36 logo neo" alt="Neo4j Logo" />
  <img src={pythonLogo} class="w-36 logo python" alt="Python Logo" />
</div>

<div class="mt-32 flex flex-col place-content-center items-center gap-y-4">
  <h1 class="text-4xl text-slate-900 dark:text-white">Corgi AI</h1>
  <p class="text-slate-500 dark:text-slate-400">
    {mode}
  </p>
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