using System.Text.Json;
using DataLoader.Models;
using Microsoft.Extensions.Configuration;
using Neo4j.Driver;
using Spectre.Console;

namespace DataLoader.Services;

public class Neo4jService(IConfiguration config)
{
    private readonly IDriver _Driver = GraphDatabase.Driver(
        config["Neo4j:Url"]!,
        AuthTokens.Basic(
            config["Neo4j:Username"]!,
            config["Neo4j:Password"]!
        )
    );

    public async Task<int> GetTotalQuestionCount()
    {
        const string query = "MATCH (q:Question) RETURN COUNT(q) as count";

        await using var session = _Driver.AsyncSession();

        return await session.ExecuteReadAsync(async transaction =>
        {
            var cursor = await transaction.RunAsync(query);

            return await cursor.SingleAsync(record => record["count"].As<int>());
        });
    }

    public async Task<IList<Question>> GetQuestions(int skip, int limit)
    {
        const string query = """
            MATCH (q:Question) <-[:ANSWERS]- (a:Answer)
            MATCH (s:Site) <-[:SITE]- (q)
            WITH q, a, s
            ORDER BY a.score DESC
            WITH collect({score: a.score, body: a.body, accepted: a.is_accepted}) as ans, q, s
            WITH {id: q.id, site: s.name, title: q.title, body: q.body, link: q.link, score: q.score, answers: ans} as question
            RETURN question
            SKIP $skip LIMIT $limit
        """;
        
        await using var session = _Driver.AsyncSession();

        return await session.ExecuteReadAsync(async transaction =>
        {
            var cursor = await transaction.RunAsync(query, new { skip, limit });

            return await cursor.ToListAsync(record =>
            {
                var question = record["question"].As<IDictionary<string, object>>();
                
                var result = new Question
                {
                    Id = Guid.NewGuid().ToString("D"),
                    Site = question["site"].As<string>(),
                    Title = question["title"].As<string>(),
                    Body = question["body"].As<string>(),
                    Link = question["link"].As<string>(),
                    Score = question["score"].As<int>(),
                    Answers = question["answers"].As<IList<IDictionary<string, object>>>().Select(answer => new Answer
                    {
                        Body = answer["body"].As<string>(),
                        IsAccepted = answer["accepted"].As<bool>(),
                        Score = answer["score"].As<int>(),
                    })
                };
                
                return result;
            });
        });
    }
}