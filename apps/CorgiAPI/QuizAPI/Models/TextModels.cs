namespace QuizAPI.Models;

public class TextRequest
{
    public string Text { get; set; }
}

public class TextResponse : LLMResponseBase
{
    public string Text { get; set; }
}

public class ChatRequest
{
    public string Text { get; set; }
    public string Site { get; set; }
}