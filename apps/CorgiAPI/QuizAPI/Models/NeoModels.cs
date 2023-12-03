namespace QuizAPI.Models;

public class Topic
{
    public string Value { get; set; }
    public int Count { get; set; }
}

public class TopicsResponse
{
    public IEnumerable<Topic> Topics { get; set; }
}