using Neo4j.Driver;
using QuizAPI.Models;

namespace QuizAPI.Neo4j.Repositories;

public interface ITopicRepository
{
    public Task<IEnumerable<Tag>> GetAllTags(int threshold);
}

public class TopicRepository(IDriver Driver) : ITopicRepository
{
    public async Task<IEnumerable<Tag>> GetAllTags(int threshold)
    {
        await using var session = Driver.AsyncSession();

        return await session.ExecuteReadAsync(async transaction =>
        {
            var cursor = await transaction.RunAsync(
                """
                MATCH (t:Tag) <-[:TAGGED]- (q:Question)
                WITH COUNT(q) as qcount, t as tag
                WHERE qcount >= $threshold
                RETURN qcount, tag.name
                ORDER BY qcount DESC
                """,
                new {threshold}
            );

            return await cursor.ToListAsync(record => new Tag
            {
                Value = record["tag.name"].As<string>(),
                Count = record["qcount"].As<int>()
            });
        });
    }
}