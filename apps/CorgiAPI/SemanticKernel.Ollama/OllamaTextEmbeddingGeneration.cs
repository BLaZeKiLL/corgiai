using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.SemanticKernel.AI.Embeddings;

namespace SemanticKernel.Ollama;

public class OllamaTextEmbeddingGeneration : ITextEmbeddingGeneration
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
    public OllamaTextEmbeddingGeneration(string model_id, string base_url, HttpClient httpClient, ILoggerFactory loggerFactory)
    {
        _attributes.Add("model_id", model_id);
        _attributes.Add("base_url", base_url);

        _httpClient = httpClient;
        
        _logger = loggerFactory is not null ? loggerFactory.CreateLogger<OllamaTextCompletion>() : NullLogger<OllamaTextCompletion>.Instance;
    }
    
    
    public async Task<IList<ReadOnlyMemory<float>>> GenerateEmbeddingsAsync(IList<string> data, CancellationToken cancellationToken = new())
    {
        var result = new List<ReadOnlyMemory<float>>(data.Count);
        
        foreach (var text in data)
        {
            var request = new
            {
                model = Attributes["model_id"],
                prompt = text
            };

            var response = await _httpClient.PostAsJsonAsync($"{Attributes["base_url"]}/api/embeddings", request, cancellationToken);

            response.EnsureSuccessStatusCode();

            var json = JsonSerializer.Deserialize<JsonNode>(await response.Content.ReadAsStringAsync(cancellationToken));

            var embedding = new ReadOnlyMemory<float>(json!["embedding"].GetValue<float[]>());
            
            result.Add(embedding);
        }

        return result;
    }
}