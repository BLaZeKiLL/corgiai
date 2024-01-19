using System.Text.Json;
using Microsoft.SemanticKernel;
using QuizAPI.Models;

namespace QuizAPI.Kernels.QuizKernel;

public class QuizKernel
{
    private readonly Kernel _Kernel;
    
    private readonly KernelPlugin _Skills;
    //private readonly KernelPlugin _Utils;

    public QuizKernel(Kernel kernel)
    {
        _Kernel = kernel;

        var path = Path.Combine(AppContext.BaseDirectory, "Kernels", "QuizKernel", "Skills");

        _Skills = _Kernel.ImportPluginFromPromptDirectory(path, "Skills");
        //_Utils = _Kernel.ImportPluginFromObject(new QuizUtils(), "Utils");
    }

    public async Task<string> Summarize(string text)
    {
        var result = await _Kernel.InvokeAsync(_Skills.Name, "Summarize", new KernelArguments
        {
            {"input", text}
        });

        return result.GetValue<string>();
    }

    public async Task<Question> Question(string question, string answer)
    {
        // var context = new ContextVariables
        // {
        //     ["question_original"] = question,
        //     ["answer_original"] = answer
        // };
        //
        // var result = await _Kernel.RunAsync(
        //     context,
        //     _Skills["Question"],
        //     _Utils["RenameQuestionOutput"],
        //     _Skills["Answer"],
        //     _Utils["RenameAnswerOutput"],
        //     _Skills["Options"],
        //     _Utils["QuizJsonBuilder"]
        // );

        return JsonSerializer.Deserialize<Question>("");
    }
}