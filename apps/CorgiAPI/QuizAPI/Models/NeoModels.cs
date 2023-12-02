namespace QuizAPI.Models;

public class Tag
{
    public string Value { get; set; }
    public int Count { get; set; }
}

public class TagResponse
{
    public IEnumerable<Tag> Tags { get; set; }
}