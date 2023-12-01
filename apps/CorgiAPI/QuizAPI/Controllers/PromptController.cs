using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QuizAPI.Kernels.QuizKernel;
using QuizAPI.Models;

namespace QuizAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PromptController(QuizKernel _QuizKernel, IConfiguration _Config) : ControllerBase
{
    private readonly string _Model = _Config.GetValue<string>("Ollama:Model");
    
    [HttpPost("summarize")]
    public async Task<ActionResult<TextResponse>> SummarizePrompt(TextRequest request)
    {
        var stopwatch = new Stopwatch();
        
        stopwatch.Start();
        
        var result = await _QuizKernel.Summarize(request.Text);
        
        stopwatch.Stop();
        
        return Ok(new TextResponse
        {
            Text = result,
            Model = _Model,
            Duration = stopwatch.Elapsed.TotalSeconds
        });
    }

    [HttpPost("question")]
    public async Task<ActionResult<QuizQuestionResponse>> QuestionPrompt(QuizQuestionRequest request)
    {
        var stopwatch = new Stopwatch();
        
        stopwatch.Start();

        var result = await _QuizKernel.Question(request.Question, request.Answer);
        
        stopwatch.Stop();

        return Ok(new QuizQuestionResponse
        {
            Result = result,
            Model = _Model,
            Duration = stopwatch.Elapsed.TotalSeconds
        });
    }
}