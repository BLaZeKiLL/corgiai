namespace QuizAPI.Models;

public class Question
{
    public string Text { get; set; }
    public Option[] Options { get; set; }
    public string Source { get; set; }
    public string Topic { get; set; }
}

public class Option
{
    public string Text { get; set; }
    public bool Correct { get; set; }
}

public class QuizQuestion
{
    public string Question { get; set; }
    public string Answer { get; set; }
    public string Source { get; set; }
}

public class QuizQuestionResponse : LLMResponseBase
{
    public Question Result { get; set; }
}