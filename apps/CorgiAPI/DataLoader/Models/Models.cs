using Newtonsoft.Json;

namespace DataLoader.Models;

public class QuestionNeo
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public string Link { get; set; }
    public int Score { get; set; }
    public IList<float> Embedding { get; set; }
}

public class AnswerNeo
{
    public string Body { get; set; }
    public bool IsAccepted { get; set; }
    public int Score { get; set; }
    public IList<float> Embedding { get; set; }
}

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
    
    public QuestionCosmos(Question question)
    {
        Id = question.Id;
        Site = question.Site;
        Title = question.Title;
        Body = question.Body;
        Link = question.Link;
        Score = question.Score;
        Answers = question.Answers.Select(x => new AnswerCosmos(x));
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
    
    public AnswerCosmos(Answer answer)
    {
        Body = answer.Body;
        IsAccepted = answer.IsAccepted;
        Score = answer.Score;
    }
}

public class Question
{
    public string Id { get; set; }
    public string Site { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public string Link { get; set; }
    public int Score { get; set; }
    public IList<double> Embedding { get; set; }
    public IEnumerable<Answer> Answers { get; set; }
}

public class Answer
{
    public string Body { get; set; }
    public bool IsAccepted { get; set; }
    public int Score { get; set; }
    public IList<double> Embedding { get; set; }
}