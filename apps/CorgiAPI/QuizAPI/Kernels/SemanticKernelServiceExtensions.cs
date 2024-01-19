using Codeblaze.SemanticKernel.Connectors.Memory.Neo4j;
using Codeblaze.SemanticKernel.Connectors.Ollama;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Memory;

using ILoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;

namespace QuizAPI.Kernels;

public static class SemanticKernelServiceExtensions
{
    // TODO : Kernel segregation move kernels and memories into individual services like QuizKernel, ChatKernel
    public static IServiceCollection AddSemanticKernel(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton(_ => { 
            var builder = Kernel.CreateBuilder();
            
            var model = config.GetValue<string>("Ollama:Model") ?? throw new InvalidDataException("Ollama model not specified");
            var baseUrlCompletion = config.GetValue<string>("Ollama:BaseUrlCompletion") ?? throw new InvalidDataException("Ollama completion url not specified");
            var baseUrlEmbeddings = config.GetValue<string>("Ollama:BaseUrlEmbeddings") ?? throw new InvalidDataException("Ollama embeddings url not specified");

            builder.Services.AddTransient<HttpClient>();
            
            builder.AddOllamaChatCompletion(model, baseUrlCompletion);
            builder.AddOllamaTextGeneration(model, baseUrlCompletion);
            builder.AddOllamaTextEmbeddingGeneration(model, baseUrlEmbeddings);

            return builder.Build();
        });

        return services;
    }

    public static IServiceCollection AddSemanticMemory(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<INeo4jQueryFactory>(_ => new Neo4jVectorIndexQueryFactory(
            "top_questions",
            "Question",
            "embedding",
            "title",
            384
        ));
        
        services.AddSingleton(provider =>
        {
            var http = provider.GetRequiredService<IHttpClientFactory>();
            var log_factory = provider.GetRequiredService<ILoggerFactory>();
            
            var model = config.GetValue<string>("Ollama:Model") ?? throw new InvalidDataException("Ollama model not specified");
            var baseUrlEmbeddings = config.GetValue<string>("Ollama:BaseUrlEmbeddings") ?? throw new InvalidDataException("Ollama embeddings url not specified");
            
            #pragma warning disable SKEXP0003
            var builder = new MemoryBuilder();
            #pragma warning restore SKEXP0003

            builder.WithLoggerFactory(log_factory);
            builder.WithHttpClient(http.CreateClient());

            builder.WithOllamaTextEmbeddingGeneration(model, baseUrlEmbeddings);
            builder.WithNeo4jMemoryStore(
                config.GetValue<string>("Neo4j:Url"),
                config.GetValue<string>("Neo4j:Username"),
                config.GetValue<string>("Neo4j:Password"),
                provider.GetRequiredService<INeo4jQueryFactory>()
            );

            return builder.Build();
        });

        return services;
    }

    public static IServiceCollection AddQuizAPIKernels(this IServiceCollection services)
    {
        services.AddSingleton<QuizKernel.QuizKernel>();
        services.AddScoped<ChatKernel.ChatKernel>();

        return services;
    }
}