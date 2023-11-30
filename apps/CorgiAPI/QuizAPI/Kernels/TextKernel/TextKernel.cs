using Microsoft.SemanticKernel;

namespace QuizAPI.Kernels.TextKernel;

public class TextKernel
{
    private readonly IKernel _Kernel;
    private readonly IDictionary<string, ISKFunction> _Functions;

    public TextKernel(IKernel kernel)
    {
        _Kernel = kernel;

        var path = Path.Combine(AppContext.BaseDirectory, "Kernels", "TextKernel");

        _Functions = _Kernel.ImportSemanticFunctionsFromDirectory(path, "Skills");
    }

    public async Task<string> Summarize(string text)
    {
        var result = await _Kernel.RunAsync(text, _Functions["Summarize"]);

        return result.GetValue<string>();
    }

    public async Task<string> Question(string question)
    {
        var result = await _Kernel.RunAsync(question, _Functions["Question"]);

        return result.GetValue<string>();
    }
}