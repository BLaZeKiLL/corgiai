using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QuizAPI.Kernels.ChatKernel;
using QuizAPI.Kernels.QuizKernel;
using QuizAPI.Models;
using QuizAPI.Neo4j.Repositories;

namespace QuizAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PromptController(
    QuizKernel _QuizKernel, 
    ChatKernel _ChatKernel,
    IQuestionRepository _QuestionRepository, 
    IConfiguration _Config
) : ControllerBase
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

    [HttpGet("question/{topic}")]
    public async Task<ActionResult<QuizQuestionResponse>> QuestionPrompt([FromRoute] string topic)
    {
        var stopwatch = new Stopwatch();
        
        stopwatch.Start();

        var question = await _QuestionRepository.GetRandomQuestionForTopic(topic);

        var result = await _QuizKernel.Question(question.Question, question.Answer);

        result.Source = question.Source;
        result.Topic = topic;
        
        stopwatch.Stop();

        return Ok(new QuizQuestionResponse
        {
            Result = result,
            Model = _Model,
            Duration = stopwatch.Elapsed.TotalSeconds
        });
    }

    [HttpPost("chat")]
    public async Task<ActionResult<TextResponse>> ChatPrompt([FromBody] ChatRequest chat)
    {
        var stopwatch = new Stopwatch();
        
        stopwatch.Start();
        
        var result = await _ChatKernel.UserChat(chat.Site, chat.Text);
        
        stopwatch.Stop();
        
        return Ok(new TextResponse
        {
            Text = result,
            Model = _Model,
            Duration = stopwatch.Elapsed.TotalSeconds
        });
    }
}