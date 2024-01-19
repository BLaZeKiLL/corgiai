using System.Text.Json;

using QuizAPI.Models;

namespace QuizAPI.Kernels.QuizKernel;

public class QuizUtils
{
    private static readonly string[] ANSWER_PREFIXES = {"ANS1:", "ANS2:", "ANS3:"};
    private static readonly Random rng = new();
    
    public static Question QuestionBuilder(string question, string answer, string wrongOptions)
    {
        var correct = new Option {Text = answer, Correct = true};
        var wrong = wrongOptions
            .Split("\n")
            .Where(x => ANSWER_PREFIXES.Any(x.StartsWith))
            .Select(x => new Option { Text = x[6..], Correct = false })
            .GetEnumerator();
    
        var options = new Option[4];
        var ans_index = rng.Next(0, 4);
    
        for (var i = 0; i < 4; i++) // rng shuffle wasn't giving good results
        {
            if (i == ans_index) options[i] = correct;
            else
            {
                wrong.MoveNext();
                options[i] = wrong.Current;
            }
        }
        
        var result = new Question
        {
            Text = question,
            Options = options
        };
        
        return result;
    }
}