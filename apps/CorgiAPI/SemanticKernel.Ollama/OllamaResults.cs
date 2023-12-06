using Microsoft.SemanticKernel.AI.TextCompletion;
using Microsoft.SemanticKernel.Orchestration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SemanticKernel.AI.ChatCompletion;

namespace SemanticKernel.Ollama;

internal class OllamaTextResult(string result) : ITextResult
{
    public ModelResult ModelResult => new(result);

    public Task<string> GetCompletionAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(result);
    }
}

internal class OllamaTextStreamingResult(string result) : ITextStreamingResult
{
    public ModelResult ModelResult => new(result);

    public async IAsyncEnumerable<string> GetCompletionStreamingAsync(CancellationToken cancellationToken = default)
    {
        yield return result;
    }
}

internal class OllamaChatResult(string result) : IChatResult
{
    public ModelResult ModelResult => new(result);
    public Task<ChatMessage> GetChatMessageAsync(CancellationToken cancellationToken = new())
    {
        return Task.FromResult(new ChatMessage(AuthorRole.Assistant, result));
    }
}