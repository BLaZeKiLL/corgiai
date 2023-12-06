using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.TextCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.AI.Embeddings;

namespace SemanticKernel.Ollama;

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
        builder.WithAIService<ITextCompletion>(serviceId, loggerFactory => new OllamaTextCompletion(modelId, baseUrl, httpClient, loggerFactory));

        return builder;
    }
    
    public static KernelBuilder WithOllamaChatCompletionService(
        this KernelBuilder builder,
        string modelId,
        string baseUrl,
        HttpClient httpClient,
        string? serviceId = null
    )
    {
        builder.WithAIService<IChatCompletion>(serviceId, loggerFactory => new OllamaChatCompletion(modelId, baseUrl, httpClient, loggerFactory));

        return builder;
    }

    public static KernelBuilder WithOllamaTextEmbeddingGeneration(
        this KernelBuilder builder,
        string modelId,
        string baseUrl,
        HttpClient httpClient,
        string? serviceId = null
    )
    {
        builder.WithAIService<ITextEmbeddingGeneration>(serviceId, loggerFactory => new OllamaTextEmbeddingGeneration(modelId, baseUrl, httpClient, loggerFactory));

        return builder;
    }
}