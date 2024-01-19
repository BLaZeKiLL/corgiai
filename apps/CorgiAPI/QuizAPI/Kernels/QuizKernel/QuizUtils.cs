using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

using QuizAPI.Models;

namespace QuizAPI.Kernels.QuizKernel;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class QuizUtils
{
    private static readonly string[] ANSWER_PREFIXES = {"ANS1:", "ANS2:", "ANS3:"};
    private static readonly Random rng = new();

    // [SKFunction, Description("Renames the input variable to question")]
    // public static SKContext RenameQuestionOutput(SKContext context)
    // {
    //     context.Variables["question"] = context.Variables["input"];
    //
    //     return context;
    // }
    //
    // [SKFunction, Description("Renames the input variable to answer")]
    // public static SKContext RenameAnswerOutput(SKContext context)
    // {
    //     context.Variables["answer"] = context.Variables["input"];
    //
    //     return context;
    // }
    //
    // [SKFunction, Description("Creates a json output for a quiz question")]
    // public static SKContext QuizJsonBuilder(SKContext context)
    // {
    //     var question = context.Variables["question"];
    //     var answer = new Option {Text = context.Variables["answer"], Correct = true};
    //     var wrong = context.Variables["input"]
    //         .Split("\n")
    //         .Where(x => ANSWER_PREFIXES.Any(x.StartsWith))
    //         .Select(x => new Option { Text = x[6..], Correct = false })
    //         .GetEnumerator();
    //
    //     var options = new Option[4];
    //     var ans_index = rng.Next(0, 4);
    //
    //     for (var i = 0; i < 4; i++) // rng shuffle wasn't giving good results
    //     {
    //         if (i == ans_index) options[i] = answer;
    //         else
    //         {
    //             wrong.MoveNext();
    //             options[i] = wrong.Current;
    //         }
    //     }
    //     
    //     var result = new Question
    //     {
    //         Text = question,
    //         Options = options
    //     };
    //
    //     context.Variables["input"] = JsonSerializer.Serialize(result);
    //
    //     return context;
    // }
}