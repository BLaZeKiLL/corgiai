using Microsoft.SemanticKernel;
using SemanticKernel.Ollama;

namespace QuizAPI.Plugins
{
    public static class SemanticKernelServiceExtensions
    {
        public static IServiceCollection AddSemanticKernel(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton(provider => { 
                var http = provider.GetRequiredService<HttpClient>();
                var log_factory = provider.GetRequiredService<ILoggerFactory>();

                var builder = new KernelBuilder();

                var model = config.GetValue<string>("Ollama:Model") ?? throw new InvalidDataException("Ollama model not specified");
                var base_url = config.GetValue<string>("Ollama:BaseUrl") ?? throw new InvalidDataException("Ollama url not specified");

                builder.WithOllamaTextCompletionService(model, base_url, http);

                return builder.Build();
            });

            return services;
        }
    }
}
