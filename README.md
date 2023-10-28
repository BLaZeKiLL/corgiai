# Corgi AI

## Dev Setup

- ### Compose

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

