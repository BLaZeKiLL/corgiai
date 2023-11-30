using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QuizAPI.Kernels.TextKernel;
using QuizAPI.Models;

namespace QuizAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PromptController(TextKernel _TextKernel, IConfiguration _Config) : ControllerBase
{
    private readonly Stopwatch _Stopwatch = new();
    private readonly string _Model = _Config.GetValue<string>("Ollama:Model");
    
    [HttpPost("summarize")]
    public async Task<ActionResult<TextResponse>> SummarizePrompt(TextRequest request)
    {
        _Stopwatch.Restart();
        
        var result = await _TextKernel.Summarize(request.Text);
        
        _Stopwatch.Stop();
        
        return Ok(new TextResponse
        {
            Text = result,
            Model = _Model,
            Duration = _Stopwatch.Elapsed.TotalSeconds
        });
    }
}