using Neo4j.Driver;
using QuizAPI.Models;

namespace QuizAPI.Neo4j.Repositories;

public interface IQuestionRepository
{
    public Task<QuizQuestionNeo> GetRandomQuestionForTopic(string topic);
}

public class QuestionRepository(IDriver Driver) : IQuestionRepository
{
    private readonly Random rng = new();
    
    public async Task<QuizQuestionNeo> GetRandomQuestionForTopic(string topic)
    {
        await using var session = Driver.AsyncSession();

        return await session.ExecuteReadAsync(async transaction =>
        {
            var cursor = await transaction.RunAsync(
                """
                MATCH (t:Tag {name:$topic}) <-[:TAGGED]- (q:Question)
                RETURN COUNT(q) as qcount
                """,
                new {topic}
            );

            var count = await cursor.SingleAsync(record => record["qcount"].As<int>());

            cursor = await transaction.RunAsync(
                """
                MATCH (question:Question) -[r:TAGGED]-> (t:Tag {name:$topic})
                WITH COUNT(question) as qcount, question
                CALL  {
                    WITH question
                    MATCH (question)<-[:ANSWERS]-(answer)
                    WITH answer
                    ORDER BY answer.is_accepted DESC, answer.score DESC
                    WITH collect(answer)[0] as a
                    RETURN a.body as atext
                }
                RETURN question.title + '\n' + question.body as question, atext as answer, question.link as source
                SKIP $offset LIMIT 1
                """,
                new { topic, offset = rng.Next(0, count)});

            return await cursor.SingleAsync(record => new QuizQuestionNeo
            {
                Question = record["question"].As<string>(),
                Answer = record["answer"].As<string>(),
                Source = record["source"].As<string>()
            });
        });
    }
}