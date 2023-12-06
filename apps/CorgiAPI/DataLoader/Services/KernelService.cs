using DataLoader.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Memory.AzureCognitiveSearch;
using SemanticKernel.Ollama;

namespace DataLoader.Services;

public class KernelService
{
    private readonly IKernel _Kernel;
    
    private readonly IDictionary<string, string> MemoryCollectionName;
    private readonly IDictionary<string, string> AssistantDomain;
    
    public KernelService(IConfiguration config)
    {
        var http = new HttpClient();
        
        _Kernel = new KernelBuilder()
            .WithOllamaChatCompletionService(config["Ollama:Model"]!, config["Ollama:BaseUrlCompletion"]!, http)
            .WithOllamaTextEmbeddingGeneration(config["Ollama:Model"]!, config["Ollama:BaseUrlEmbeddings"]!, http)
            .WithMemoryStorage(new AzureCognitiveSearchMemoryStore(config["Search:Endpoint"]!, config["Search:Key"]!))
            .Build();

        MemoryCollectionName = new Dictionary<string, string>
        {
            { "stackoverflow", "sk_stackoverflow" },
            { "math", "sk_math" },
            { "physics", "sk_physics" },
        };
        
        AssistantDomain = new Dictionary<string, string>
        {
            { "stackoverflow", "programming" },
            { "math", "math" },
            { "physics", "physics" },
        };
    }
    
    public async Task SaveEmbeddingsAsync(string data, string id, string site)
    {
        try
        {
            await _Kernel.Memory.SaveReferenceAsync(
                collection: MemoryCollectionName[site],
                externalSourceName: "Question",
                externalId: id,
                description: data,
                text: data
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());   
        }
    }

    public async Task<List<string>> SearchEmbeddingsAsync(string site, string query)
    {
        var memories = _Kernel.Memory.SearchAsync(MemoryCollectionName[site], query, limit: 2, minRelevanceScore: 0.5);

        var result = new List<string>();
        
        await foreach (var memory in memories)
        {
            result.Add(memory.Metadata.Id);
        }
        
        return result;
    }
    
    public async Task<string> GenerateCompletionAsync(string prompt, string site, IEnumerable<QuestionCosmos> questions)
    {
        var system = $"""
            You are a teaching assistant for {AssistantDomain[site]}.
            Use the following pieces of question-answer pairs to answer the question at the end.
            
            INSTRUCTIONS
            - Assume the user is not an expert in {AssistantDomain[site]}.
            - You should prefer information from more upvoted answers.
            - Make sure to rely on information from the answers and not on questions to provide accurate responses.
            - Your response should be complete.
            - If you don't know the answer, just say that you don't know, don't try to make up an answer.
        """;

        var context = string.Join("\n",
            questions.Select(q =>
                $"## Question: {q.Title}\n{q.Body}\n{string.Join("\n", 
                    q.Answers.Select(a => $"### Answer (Accepted: {a.IsAccepted} Score: {a.Score})\n{a.Body}")
                )}"
            )
        );
        
        var chatCompletion = _Kernel.GetService<IChatCompletion>();

        var chatHistory = chatCompletion.CreateNewChat(system);

        // add shortlisted recipes as system message
        chatHistory.AddSystemMessage(context);

        // send user prompt as user message
        chatHistory.AddUserMessage(prompt);

        // Finally, get the response from AI
        var answer = await chatCompletion.GenerateMessageAsync(chatHistory);
        
        return answer;
    }
}