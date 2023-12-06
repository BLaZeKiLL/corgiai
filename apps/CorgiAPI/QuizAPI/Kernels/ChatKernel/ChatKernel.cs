using Microsoft.Azure.Cosmos;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using QuizAPI.Models;

namespace QuizAPI.Kernels.ChatKernel;

public class ChatKernel(IKernel Kernel, CosmosClient cosmos, IConfiguration config)
{
    private readonly Container _Container = cosmos.GetContainer(
        config.GetValue<string>("Cosmos:DatabaseName"), 
        config.GetValue<string>("Cosmos:ContainerName")
    );
    
    private readonly IDictionary<string, string> MemoryCollectionName = new Dictionary<string, string>
    {
        { "stackoverflow", "sk_stackoverflow" },
        { "math", "sk_math" },
        { "physics", "sk_physics" },
    };
    
    private readonly IDictionary<string, string> AssistantDomain = new Dictionary<string, string>
    {
        { "stackoverflow", "programming" },
        { "math", "math" },
        { "physics", "physics" },
    };

    public async Task<string> UserChat(string site, string prompt)
    {
        var ids = await GetEmbeddings(site, prompt);

        if (ids.Count == 0) return "Sorry I don't have any knowledge about your question.";
        
        var questions = await GetQuestions(ids);

        return await GenerateCompletionAsync(prompt, site, questions);
    }
    
    
    private async Task<string> GenerateCompletionAsync(string prompt, string site, IEnumerable<QuestionCosmos> questions)
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
        
        var chatCompletion = Kernel.GetService<IChatCompletion>();
        var chatHistory = chatCompletion.CreateNewChat(system);

        // add shortlisted recipes as system message
        chatHistory.AddSystemMessage(context);

        // send user prompt as user message
        chatHistory.AddUserMessage(prompt);

        // Finally, get the response from AI
        var answer = await chatCompletion.GenerateMessageAsync(chatHistory);
        
        // Add sources
        var sources = string.Join("", questions.Select(x => $"\n- [{x.Title}]({x.Link})"));
        
        return answer + "\n \nSources:" + sources;
    }
    
    private async Task<List<string>> GetEmbeddings(string site, string query)
    {
        var memories = Kernel.Memory.SearchAsync(MemoryCollectionName[site], query, limit: 2, minRelevanceScore: 0.5);

        var result = new List<string>();
        
        await foreach (var memory in memories)
        {
            result.Add(memory.Metadata.Id);
        }
        
        return result;
    }
    
    public async Task<List<QuestionCosmos>> GetQuestions(IEnumerable<string> ids)
    {
        var querystring= "SELECT * FROM c WHERE c.id IN(" + string.Join(",", ids.Select(id => $"'{id}'")) + ")";

        var query = new QueryDefinition(querystring);

        var results = _Container.GetItemQueryIterator<QuestionCosmos>(query);

        List<QuestionCosmos> output = new();
        
        while (results.HasMoreResults)
        {
            var response = await results.ReadNextAsync();
            
            output.AddRange(response);
        }

        foreach (var question in output)
        {
            var answers = question.Answers.ToList();
            
            answers.Sort((x, y) => y.Score - x.Score);

            if (answers.Count > 2) question.Answers = answers[..2];
        }
        
        return output;
    }
}