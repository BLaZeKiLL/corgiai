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
            var baseUrl = config.GetValue<string>("Ollama:BaseUrl") ?? throw new InvalidDataException("Ollama url not specified");

            builder.WithOllamaTextCompletionService(model, baseUrl, http.CreateClient());

            return builder.Build();
        });

        return services;
    }

    public static IServiceCollection AddCorgiKernels(this IServiceCollection services)
    {
        services.AddSingleton<TextKernel.TextKernel>();

        return services;
    }
}