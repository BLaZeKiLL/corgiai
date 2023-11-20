using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.SemanticKernel.AI;
using Microsoft.SemanticKernel.AI.TextCompletion;

using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;


namespace SemanticKernel.Ollama
{
    public class OllamaTextCompletion : ITextCompletion
    {
        public IReadOnlyDictionary<string, string> Attributes => _attributes;

        private Dictionary<string, string> _attributes = [];
        private readonly HttpClient _httpClient;
        private readonly ILogger<OllamaTextCompletion> _logger;

        public OllamaTextCompletion(string model_id, string base_url, HttpClient httpClient, ILoggerFactory loggerFactory)
        {
            _attributes.Add("model_id", model_id);
            _attributes.Add("base_url", base_url);

            _httpClient = httpClient;
            _logger = loggerFactory is not null ? loggerFactory.CreateLogger<OllamaTextCompletion>() : NullLogger<OllamaTextCompletion>.Instance;

            PingOllama().Wait();
        }


        public async Task<IReadOnlyList<ITextResult>> GetCompletionsAsync(string text, AIRequestSettings? requestSettings = null, CancellationToken cancellationToken = default)
        {
            var data = new
            {
                model = Attributes["model_id"],
                prompt = text,
                stream = false
            };

            var response = await _httpClient.PostAsJsonAsync($"{Attributes["base_url"]}/api/generate", data, cancellationToken);

            response.EnsureSuccessStatusCode();

            var json = JsonSerializer.Deserialize<JsonNode>(await response.Content.ReadAsStringAsync(cancellationToken));

            return new List<ITextResult> { new OllamaTextResult(json!["response"]!.GetValue<string>()) };
        }


        public IAsyncEnumerable<ITextStreamingResult> GetStreamingCompletionsAsync(string text, AIRequestSettings? requestSettings = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }


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
}
