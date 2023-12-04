using System.Text.Json;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;
using QuizAPI.Models;

namespace QuizAPI.Kernels.QuizKernel;

public class QuizKernel
{
    private readonly IKernel _Kernel;
    
    private readonly IDictionary<string, ISKFunction> _Skills;
    private readonly IDictionary<string, ISKFunction> _Utils;

    public QuizKernel(IKernel kernel)
    {
        _Kernel = kernel;

        var path = Path.Combine(AppContext.BaseDirectory, "Kernels", "QuizKernel");

        _Skills = _Kernel.ImportSemanticFunctionsFromDirectory(path, "Skills");
        _Utils = _Kernel.ImportFunctions(new QuizUtils(), "Utils");
    }

    public async Task<string> Summarize(string text)
    {
        var result = await _Kernel.RunAsync(text, _Skills["Summarize"]);

        return result.GetValue<string>();
    }

    public async Task<Question> Question(string question, string answer)
    {
        var context = new ContextVariables
        {
            ["question_original"] = question,
            ["answer_original"] = answer
        };

        var result = await _Kernel.RunAsync(
            context,
            _Skills["Question"],
            _Utils["RenameQuestionOutput"],
            _Skills["Answer"],
            _Utils["RenameAnswerOutput"],
            _Skills["Options"],
            _Utils["QuizJsonBuilder"]
        );

        return JsonSerializer.Deserialize<Question>(result.GetValue<string>());
    }
}