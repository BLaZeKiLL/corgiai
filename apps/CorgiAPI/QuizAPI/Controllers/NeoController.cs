using Microsoft.AspNetCore.Mvc;
using QuizAPI.Models;
using QuizAPI.Neo4j.Repositories;

namespace QuizAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NeoController(ITopicRepository TopicRepository, IQuestionRepository QuestionRepository) : ControllerBase
{
    [HttpGet("/topics")]
    public async Task<ActionResult<TagResponse>> GetAllTopics([FromQuery] int threshold = 100)
    {
        return Ok(new TagResponse
        {
            Tags = await TopicRepository.GetAllTags(threshold)
        });
    }

    [HttpGet("/question:{topic}")]
    public async Task<ActionResult<QuizQuestionRequest>> GetRandomQuestionForTopic([FromRoute] string topic)
    {
        return Ok(await QuestionRepository.GetRandomQuestionForTopic(topic));
    }
}