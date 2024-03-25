# Corgi AI
### [HackSC X by MLH Winner](https://devpost.com/software/corgi-ai-community-retrieval-augmented-generation) (Best Use of AI for Education), [Microsoft Semantic Kernel Hackathon Winner](https://www.linkedin.com/posts/devashish-lal-096868176_semantic-kernel-v10-hackathon-finalists-activity-7141824995655057408-8vfB?utm_source=share&utm_medium=member_desktop)

CorgiAI is a community-based hub powered by AI to help anyone learn and practice by interacting with ever-expanding shared knowledge.

[DEMO](https://youtu.be/oMmJ1X1lASo)

## Inspiration
- During exam season, when the crunch begins. Having fast access to doubt-solving and practice questions is crucial.
- However, with a limited number of sample tests, we felt underprepared
 -Moreover, getting your doubts cleared about the topic and receiving timely feedback could be a hassle (TA not responding)
- Information regarding niche topics requires a lot of searching effort
- Educators and Content Creators can’t give a personalized learning experience to each student

## Solution
- Custom practice quizzes for a specific topic to test knowledge and gain confidence
- Chat interface where students can ask questions and receive timely feedback personalized to their strengths and weaknesses
- A community platform for creating public/private knowledge graphs and interacting with them
- Personalized content according to the user’s weaknesses and strengths

## What it does
Corgi is a community-based hub that uses AI to help students study, learn, and share what they have learned with others in a one-stop solution. Corgi can generate custom quiz based on chosen topics. Users can take the quiz and review the answers. Users can also ask Corgi specific questions (free text) about a specific topic in a user-friendly chat interface, much like a Tutor for a class. Corgi Tutor will provide answers, relevant links, and extra study materials, such as Wikipedia and YouTube links. Quizzes and answers are highly topic-specific because we mainly reference the Stack exchange networks. 

## How we built it
We used Stackoverflow and the various websites in the Stack Exchange network as our reference data. Data is stored partially in neo4j, and the embeddings are in CosmosDB and AzureAI service. APIs are implemented in ASP.NET 8, and llm orchestration is powered by Semantic Kernel. Docker plays a vital role in easing the deployment of this composite application.

![image](https://github.com/BLaZeKiLL/corgiai/assets/33104478/3f0dbd79-494b-4155-a069-94990bb5bab0)

## Challenges we ran into
Chat responses are slow. We weren’t able to run the 70 billion parameter model. We used the 7 billion parameter model instead. It is also self-hosted on our local server to save on costs. Connecting the self-hosted cluster to the azure app presented a unique challenge which we solved by using Tailscale VPN and setting up reverse proxies in azure containers allowing us to securely access the cluster without exposing port over the internet

## Accomplishments that we're proud of
We are proud to create a working demo of the app. The app can take user input and output a custom quiz or help clear doubts. We are also proud to brainstorm the potential of the app as well as overcome the unique challenges while developing the app. Finally we are most proud of the open source contributions that resulted from this app - [Ollama bindings for semantic kernel](https://github.com/microsoft/semantic-kernel/pull/3603) and other bug reports and analysis around the dotnet ecosystem

Infrastructure components developed during the creation of CorgiAI are now available as separate packages [here](https://github.com/BLaZeKiLL/Codeblaze.SemanticKernel)

## What we learned
- Neo4j and Graph Databases
- Cypher Language
- LLMs (Ollama)
- Generating Embeddings
- Docker as orchestrating different components of system
- Streamlit to easily create data-backed Uis
- ASP.NET 8 and minimal API's
- Semantic Kernel
- Azure, Azure AI Services, Azure Cosmos DB
- GitHub Actions

## What's next for Corgi AI - Community Retrieval Augmented Generation
- We want to integrate a powerful analytics system that would help educators and us better understand their students and enable personalized content and greater feedback on LLM outputs to tune them further.
- Public/Invite Only/Private Knowledge graphs with full data governance and a robust multi-faceted ingestion system allowing users to create and maintain ever-expanding knowledge graphs which other users (students) can interact which learning niche topics and receiving personalized content

# Development

## Access

The app will be hosteed on codeblaze-server which should be accisible using tailscale

## Neo4j Queries

- ### Delete All
    ```sql
    MATCH (node) DETACH DELETE node
    ```

- ### View All
    ```sql
    MATCH (node) RETURN node
    ```
    - Filtering can be done using (node:LABEL)
    - Multiple nodes can be returned using multiple match
    - specific properties can be returned from the node using RETURN node.property

## Dev Setup

- ### Env setup
    - Default .env file, place it in the config folder
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

