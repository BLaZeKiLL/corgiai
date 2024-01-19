using Neo4j.Driver;
using QuizAPI.Models;

namespace QuizAPI.Neo4j.Repositories;

public interface IQuestionRepository
{
    public Task<QuizQuestion> GetRandomQuestionForTopic(string topic);
    public Task<List<ChatMemory>> GetChatQuestions(List<string> ids);
}

public class QuestionRepository(IDriver Driver) : IQuestionRepository
{
    private readonly Random rng = new();
    
    public async Task<QuizQuestion> GetRandomQuestionForTopic(string topic)
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

            return await cursor.SingleAsync(record => new QuizQuestion
            {
                Question = record["question"].As<string>(),
                Answer = record["answer"].As<string>(),
                Source = record["source"].As<string>()
            });
        });
    }

    public async Task<List<ChatMemory>> GetChatQuestions(List<string> ids)
    {
        await using var session = Driver.AsyncSession();

        return await session.ExecuteReadAsync(async transaction =>
        {
            var cursor = await transaction.RunAsync(
                """
                    UNWIND $ids as id
                    MATCH (question:Question {id: id})
                    CALL {
                        WITH question
                        MATCH (question) <-[:ANSWERS]- (answer)
                        WITH answer
                        ORDER BY answer.is_accepted DESC, answer.score DESC
                        WITH collect(answer)[..2] AS answers
                        RETURN reduce(str='', a IN answers | str +
                            '\n### Answer (Accepted: ' + a.is_accepted +
                            ' Score: ' + a.score+ '): '+  a.body + '\n') as answerTexts
                    }
                    RETURN '##Question: ' + question.title + '\n' + question.body + '\n' 
                    + answerTexts AS text, question.link as source, question.title as title
                """, new { ids = ids.Select(int.Parse).ToArray() });

            return await cursor.ToListAsync(record => new ChatMemory
            {
                Text = record["text"].As<string>(),
                Title = record["title"].As<string>(),
                Source = record["source"].As<string>()
            }).ConfigureAwait(false);
        });
    }
}