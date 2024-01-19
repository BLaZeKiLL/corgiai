using Microsoft.SemanticKernel;
using QuizAPI.Models;

namespace QuizAPI.Kernels.QuizKernel;

public class QuizKernel
{
    private readonly Kernel _Kernel;
    
    private readonly KernelPlugin _Skills;

    public QuizKernel(Kernel kernel)
    {
        _Kernel = kernel;

        var path = Path.Combine(AppContext.BaseDirectory, "Kernels", "QuizKernel", "Skills");

        _Skills = _Kernel.ImportPluginFromPromptDirectory(path, "Skills");
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
        var summarizedQuestion = await _Kernel.InvokeAsync<string>(
            _Skills.Name,
            "Question",
            new KernelArguments
            {
                {"question_original", question}
            }
        );

        var summarizedAnswer = await _Kernel.InvokeAsync<string>(
            _Skills.Name,
            "Answer",
            new KernelArguments
            {
                {"question", summarizedQuestion},
                {"answer_original", answer}
            }
        );

        var options = await _Kernel.InvokeAsync<string>(
            _Skills.Name,
            "Options",
            new KernelArguments
            {
                {"question", summarizedQuestion},
                {"answer", summarizedAnswer}
            }
        );

        return QuizUtils.QuestionBuilder(summarizedQuestion, summarizedAnswer, options);
    }
}