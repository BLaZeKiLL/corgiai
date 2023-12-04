using Microsoft.SemanticKernel;
using SemanticKernel.Ollama;

namespace QuizAPI.Kernels;

public static class SemanticKernelServiceExtensions
{
    public static IServiceCollection AddSemanticKernel(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton(provider => { 
            var http = provider.GetRequiredService<IHttpClientFactory>();
            var log_factory = provider.GetRequiredService<ILoggerFactory>();

            var builder = new KernelBuilder();
            
            var model = config.GetValue<string>("Ollama:Model") ?? throw new InvalidDataException("Ollama model not specified");
            var baseUrlCompletion = config.GetValue<string>("Ollama:BaseUrlCompletion") ?? throw new InvalidDataException("Ollama embeddings url not specified");
            var baseUrlEmbeddings = config.GetValue<string>("Ollama:BaseUrlEmbeddings") ?? throw new InvalidDataException("Ollama embeddings url not specified");

            builder.WithLoggerFactory(log_factory);
            builder.WithOllamaTextCompletionService(model, baseUrlCompletion, http.CreateClient());
            builder.WithOllamaTextEmbeddingGeneration(model, baseUrlEmbeddings, http.CreateClient());

            return builder.Build();
        });

        return services;
    }

    public static IServiceCollection AddQuizAPIKernels(this IServiceCollection services)
    {
        services.AddSingleton<QuizKernel.QuizKernel>();

        return services;
    }
}