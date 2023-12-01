namespace QuizAPI.Models;

public class QuizQuestion
{
    public string Question { get; set; }
    public string Answer { get; set; }
    public string[] Options { get; set; }
}

public class QuizQuestionRequest
{
    public string Question { get; set; }
    public string Answer { get; set; }
}

public class QuizQuestionResponse : LLMResponseBase
{
    public QuizQuestion Result { get; set; }
}