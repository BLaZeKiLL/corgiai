using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QuizAPI.Kernels.TextKernel;
using QuizAPI.Models;

namespace QuizAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PromptController(TextKernel _TextKernel, IConfiguration _Config) : ControllerBase
{
    private readonly string _Model = _Config.GetValue<string>("Ollama:Model");
    
    [HttpPost("summarize")]
    public async Task<ActionResult<TextResponse>> SummarizePrompt(TextRequest request)
    {
        var stopwatch = new Stopwatch();
        
        stopwatch.Start();
        
        var result = await _TextKernel.Summarize(request.Text);
        
        stopwatch.Stop();
        
        return Ok(new TextResponse
        {
            Text = result,
            Model = _Model,
            Duration = stopwatch.Elapsed.TotalSeconds
        });
    }

    [HttpPost("question")]
    public async Task<ActionResult<TextResponse>> QuestionPrompt(TextRequest request)
    {
        var stopwatch = new Stopwatch();
        
        stopwatch.Start();

        var result = await _TextKernel.Question(request.Text);
        
        stopwatch.Stop();

        return Ok(new TextResponse
        {
            Text = result,
            Model = _Model,
            Duration = stopwatch.Elapsed.TotalSeconds
        });
    }
}