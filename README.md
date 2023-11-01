# Corgi AI

## Access

The app will be hosteed on codeblaze-server which should be accisible using tailscale

## Dev Setup

- ### Env setup
    - Default .env file
    ```cmd
    OLLAMA_BASE_URL=http://llm:11434
    NEO4J_URI=neo4j://neo:7687
    NEO4J_USERNAME=neo4j
    NEO4J_PASSWORD=password
    LLM=llama2
    EMBEDDING_MODEL=sentence_transformer
    ```

- ### Compose
    - Start cluster with the following command
    ```cmd
    docker compose up
    ```

- ### Manual
    - Start a neo4j instance

    ```cmd
    docker run \
        --name neo4j \
        -p 7474:7474 -p 7687:7687 \
        -d \
        -e NEO4J_AUTH=neo4j/pleaseletmein \
        -e NEO4J_PLUGINS=\[\"apoc\"\]  \
        neo4j:latest
    ```

