using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.SemanticKernel;
using SemanticKernel.Ollama;

namespace PromtMaker;

internal class PromptMaker(IKernel kernel, ILoggerFactory? loggerFactory) : IHostedService
{
    private readonly ILogger<PromptMaker> _logger = loggerFactory is not null
        ? loggerFactory.CreateLogger<PromptMaker>()
        : NullLogger<PromptMaker>.Instance;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Prompt Maker Started");

        await BasicPrompt();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Prompt Maker Done");

        return Task.CompletedTask;
    }

    private async Task BasicPrompt()
    {
        const string prompt = """
        Bot: How can I help you?
        User: {{$input}}
                              
        ---------------------------------------------
                              
        The intent of the user in 5 words or less:
        """;

        var function = kernel.CreateSemanticFunction(prompt, functionName: "GetIntent", pluginName: "Basic");

        var result = await kernel.RunAsync(
            "I want to send an email to the marketing team celebrating their recent milestone.",
            function
        );

        _logger.LogInformation(result.ToString());
    }
}