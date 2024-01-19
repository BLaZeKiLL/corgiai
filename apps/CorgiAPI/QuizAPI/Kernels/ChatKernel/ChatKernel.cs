using Codeblaze.SemanticKernel.Connectors.Memory.Neo4j;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Memory;

using QuizAPI.Models;
using QuizAPI.Neo4j.Repositories;

namespace QuizAPI.Kernels.ChatKernel;

#pragma warning disable SKEXP0003
public class ChatKernel(Kernel Kernel, ISemanticTextMemory memory, INeo4jQueryFactory qf, IQuestionRepository questionRepository)
#pragma warning restore SKEXP0003
{
    private readonly IDictionary<string, string> AssistantDomain = new Dictionary<string, string>
    {
        { "stackoverflow", "programming" },
        { "math", "math" },
        { "physics", "physics" },
    };

    public async Task<string> UserChat(string site, string prompt)
    {
        var ids = await SearchQuestions(prompt);

        if (ids.Count == 0) return "Sorry I don't have any knowledge about your question.";
        
        var questions = await questionRepository.GetChatQuestions(ids);

        return await GenerateCompletionAsync(prompt, site, questions);
    }
    
    
    private async Task<string> GenerateCompletionAsync(string prompt, string site, IEnumerable<ChatMemory> questions)
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

        var context = string.Join("\n", questions.Select(x => x.Text));
        
        var chatCompletion = Kernel.Services.GetService<IChatCompletionService>();

        var chatHistory = new ChatHistory();

        // add instructions
        chatHistory.AddSystemMessage(system);
        
        // add shortlisted questions as system message
        chatHistory.AddSystemMessage(context);

        // send user prompt as user message
        chatHistory.AddUserMessage(prompt);

        // Finally, get the response from AI
        var answer = await chatCompletion.GetChatMessageContentAsync(chatHistory);
        
        // Add sources
        var sources = string.Join("", questions.Select(x => $"\n- [{x.Title}]({x.Source})"));
        
        return answer.Content + "\n \nSources:" + sources;
    }
    
    private async Task<List<string>> SearchQuestions(string query)
    {
        var memories = memory.SearchAsync(qf.IndexName, query, 5, 0.5);

        var result = new List<string>();
        
        await foreach (var memory in memories)
        {
            result.Add(memory.Metadata.Id);
        }
        
        return result;
    }
}