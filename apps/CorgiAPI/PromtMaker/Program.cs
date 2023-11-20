using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using PromtMaker;
using SemanticKernel.Ollama;

var builder = Host.CreateDefaultBuilder();

builder.ConfigureServices(services =>
{
    services.AddHttpClient();

    services.AddSingleton(provider => { 
        var http = provider.GetRequiredService<HttpClient>();
        var log_factory = provider.GetRequiredService<ILoggerFactory>();

        var builder = new KernelBuilder();
            
        builder.WithOllamaTextCompletionService("llama2:7b-chat-q4_K_M", "http://100.77.129.101:11434", http);

        return builder.Build();
    });

    services.AddHostedService<PromptMaker>();
});

var app = builder.Build();

app.Run();
