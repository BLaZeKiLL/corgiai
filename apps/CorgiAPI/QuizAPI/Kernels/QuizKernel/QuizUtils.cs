using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;
using QuizAPI.Models;

namespace QuizAPI.Kernels.QuizKernel;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class QuizUtils
{
    [SKFunction, Description("Renames the input variable to question")]
    public static SKContext RenameQuestionOutput(SKContext context)
    {
        context.Variables["question"] = context.Variables["input"];

        return context;
    }

    [SKFunction, Description("Renames the input variable to answer")]
    public static SKContext RenameAnswerOutput(SKContext context)
    {
        context.Variables["answer"] = context.Variables["input"];

        return context;
    }

    [SKFunction, Description("Creates a json output for a quiz question")]
    public static SKContext QuizJsonBuilder(SKContext context)
    {
        var question = context.Variables["question"];
        var answer = context.Variables["answer"];
        var options = context.Variables["input"];

        var result = new QuizQuestion
        {
            Question = question,
            Answer = answer,
            Options = options.Split("\n")
        };

        context.Variables["input"] = JsonSerializer.Serialize(result);

        return context;
    }
}