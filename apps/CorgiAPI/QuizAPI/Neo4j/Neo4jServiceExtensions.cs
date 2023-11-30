using Neo4j.Driver;

namespace QuizAPI.Neo4j;

public static class Neo4jServiceExtensions
{
    public static IServiceCollection AddNeo4j(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IDriver>(_ => GraphDatabase.Driver(
            config.GetValue<string>("Neo4j:Url"), 
            AuthTokens.Basic(
                config.GetValue<string>("Neo4j:Username"), 
                config.GetValue<string>("Neo4j:Password")
            )
        ));

        return services;
    }
}