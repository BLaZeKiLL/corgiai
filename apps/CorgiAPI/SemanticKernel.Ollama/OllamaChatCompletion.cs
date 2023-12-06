using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.SemanticKernel.AI;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.AI.TextCompletion;

namespace SemanticKernel.Ollama;

public class OllamaChatCompletion : IChatCompletion, ITextCompletion
{
        public IReadOnlyDictionary<string, string> Attributes => _attributes;

    private Dictionary<string, string> _attributes = new();
    private readonly HttpClient _httpClient;
    private readonly ILogger<OllamaTextCompletion> _logger;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="model_id">Ollama model to use</param>
    /// <param name="base_url">Ollama endpoint</param>
    /// <param name="httpClient">Http client used internally to query ollama api</param>
    /// <param name="loggerFactory">Logger</param>
    public OllamaChatCompletion(string model_id, string base_url, HttpClient httpClient, ILoggerFactory loggerFactory)
    {
        _attributes.Add("model_id", model_id);
        _attributes.Add("base_url", base_url);

        _httpClient = httpClient;
        _logger = loggerFactory is not null ? loggerFactory.CreateLogger<OllamaTextCompletion>() : NullLogger<OllamaTextCompletion>.Instance;
    }


    /// <summary>
    /// Generate response using Ollama api
    /// </summary>
    /// <param name="text">Prompt</param>
    /// <param name="requestSettings">Llama2 Settings can be passed as extension data</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IReadOnlyList<ITextResult>> GetCompletionsAsync(string text, AIRequestSettings? requestSettings = null, CancellationToken cancellationToken = default)
    {
        var data = new
        {
            model = Attributes["model_id"],
            prompt = text,
            stream = false,
            options = requestSettings?.ExtensionData,
        };

        var response = await _httpClient.PostAsJsonAsync($"{Attributes["base_url"]}/api/generate", data, cancellationToken);

        response.EnsureSuccessStatusCode();

        var json = JsonSerializer.Deserialize<JsonNode>(await response.Content.ReadAsStringAsync(cancellationToken));

        return new List<ITextResult> { new OllamaTextResult(json!["response"]!.GetValue<string>()) };
    }


    /// <summary>
    /// !NOTE : Haven't tested, Not sure if this works
    /// </summary>
    /// <param name="text">Prompt</param>
    /// <param name="requestSettings">Llama2 Settings can be passed as extension data</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async IAsyncEnumerable<ITextStreamingResult> GetStreamingCompletionsAsync(string text, AIRequestSettings? requestSettings = null, CancellationToken cancellationToken = default)
    {
        var data = new
        {
            model = Attributes["model_id"],
            prompt = text,
            stream = true,
            options = requestSettings?.ExtensionData,
        };

        var response = await _httpClient.PostAsJsonAsync($"{Attributes["base_url"]}/api/generate", data, cancellationToken);

        var stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);

        await using (stream)
        {
            using var reader = new StreamReader(stream);

            var done = false;

            while (!done)
            {
                var json = JsonSerializer.Deserialize<JsonNode>(
                    await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false)
                );

                done = json!["done"]!.GetValue<bool>();

                yield return new OllamaTextStreamingResult(json!["response"]!.GetValue<string>());
            }
        }
    }

    public ChatHistory CreateNewChat(string? instructions = null)
    {
        var history = new ChatHistory();

        if (!string.IsNullOrEmpty(instructions)) history.AddSystemMessage(instructions);
        
        return history;
    }

    public async Task<IReadOnlyList<IChatResult>> GetChatCompletionsAsync(ChatHistory chat, AIRequestSettings? requestSettings = null, CancellationToken cancellationToken = new())
    {
        var system = string.Join("\n", chat.Where(x => x.Role == AuthorRole.System).Select(x => x.Content));
        var user = chat.Last(x => x.Role == AuthorRole.User);
        
        var data = new
        {
            model = Attributes["model_id"],
            prompt = user.Content,
            system,
            stream = false,
            options = requestSettings?.ExtensionData,
        };

        var response = await _httpClient.PostAsJsonAsync($"{Attributes["base_url"]}/api/generate", data, cancellationToken);

        response.EnsureSuccessStatusCode();

        var json = JsonSerializer.Deserialize<JsonNode>(await response.Content.ReadAsStringAsync(cancellationToken));

        return new List<IChatResult> { new OllamaChatResult(json!["response"]!.GetValue<string>()) };
    }

    public IAsyncEnumerable<IChatStreamingResult> GetStreamingChatCompletionsAsync(ChatHistory chat, AIRequestSettings? requestSettings = null,
        CancellationToken cancellationToken = new())
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Pings ollama to see if the required model is running.
    /// </summary>
    /// <returns></returns>
    private async Task PingOllama()
    {
        var data = new
        {
            name = Attributes["model_id"]
        };

        var response = await _httpClient.PostAsJsonAsync($"{Attributes["base_url"]}/api/show", data);

        response.EnsureSuccessStatusCode();

        _logger.LogInformation($"Connected to Ollama at {Attributes["base_url"]} with model {Attributes["model_id"]}");
    }
}