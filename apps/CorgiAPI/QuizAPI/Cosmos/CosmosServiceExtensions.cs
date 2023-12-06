using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;

namespace QuizAPI.Cosmos;

public static class CosmosServiceExtensions
{
    public static IServiceCollection AddCosmosDb(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<CosmosClient>(_ =>
        {
            CosmosSerializationOptions options = new()
            {
                PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
            };

            return new CosmosClientBuilder(
                config.GetValue<string>("Cosmos:Endpoint"),
                config.GetValue<string>("Cosmos:Key")
            ).WithSerializerOptions(options).Build();
        });
        
        return services;
    }
}