namespace QuizAPI.Models;

public class QuestionCosmos
{
    public string Id { get; set; }
    public string Site { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public string Link { get; set; }
    public int Score { get; set; }
    public IEnumerable<AnswerCosmos> Answers { get; set; }

    public QuestionCosmos()
    {
        Id = "";
        Site = "";
        Title = "";
        Body = "";
        Link = "";
        Score = 0;
        Answers = new List<AnswerCosmos>();
    }
}

public class AnswerCosmos
{
    public string Body { get; set; }
    public bool IsAccepted { get; set; }
    public int Score { get; set; }

    public AnswerCosmos()
    {
        Body = "";
        IsAccepted = false;
        Score = 0;
    }
}