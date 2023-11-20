using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.TextCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticKernel.Ollama
{
    public static class OllamaKernelBuilderExtensions
    {
        public static KernelBuilder WithOllamaTextCompletionService(
            this KernelBuilder builder,
            string modelId,
            string baseUrl,
            HttpClient httpClient,
            string? serviceId = null
        )
        {
            builder.WithAIService<ITextCompletion>(serviceId, loggerFactory =>
            {
                return new OllamaTextCompletion(modelId, baseUrl, httpClient, loggerFactory);
            });

            return builder;
        }
    }
}
