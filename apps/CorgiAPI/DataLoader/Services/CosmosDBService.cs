using System.Diagnostics;
using DataLoader.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;
using Spectre.Console;

namespace DataLoader.Services;

public class CosmosDBService
{
    private Container _Container;
    private readonly Database _Database;
    private readonly IConfiguration _Config;
    
    public CosmosDBService(IConfiguration config)
    {
        _Config = config;
        
        CosmosSerializationOptions options = new()
        {
            PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
        };

        var client = new CosmosClientBuilder(
                config["Cosmos:Endpoint"]!, 
                config["Cosmos:Key"]!
            )
            .WithSerializerOptions(options)
            .Build();

        _Database = client?.GetDatabase(config["Cosmos:DatabaseName"]!);
        var container = _Database?.GetContainer(config["Cosmos:ContainerName"]!);

        _Container = container ?? throw new ArgumentException("Unable to connect to existing Azure Cosmos DB container or database.");
    }

    public async Task CleanAllData()
    {
        await _Container.DeleteContainerAsync();

        await _Database.CreateContainerAsync(new ContainerProperties
        {
            Id = _Config["Cosmos:ContainerName"]!,
            PartitionKeyPath = "/id",
        });
        
        var container = _Database?.GetContainer(_Config["Cosmos:ContainerName"]!);
        
        _Container = container ?? throw new ArgumentException("Unable to connect to existing Azure Cosmos DB container or database.");
    }
    
    public async Task<List<QuestionCosmos>> GetQuestionsAsync(IEnumerable<string> ids)
    {
        var querystring= "SELECT * FROM c WHERE c.id IN(" + string.Join(",", ids.Select(id => $"'{id}'")) + ")";

        var query = new QueryDefinition(querystring);

        var results = _Container.GetItemQueryIterator<QuestionCosmos>(query);

        List<QuestionCosmos> output = new();
        
        while (results.HasMoreResults)
        {
            var response = await results.ReadNextAsync();
            
            output.AddRange(response);
        }

        foreach (var question in output)
        {
            var answers = question.Answers.ToList();
            
            answers.Sort((x, y) => y.Score - x.Score);

            question.Answers = answers[..2];
        }
        
        return output;
    }
    
    public async Task<BulkOperationResponse<QuestionCosmos>> AddQuestionsAsync(IList<Question> questions)
    {
        var bulkOperations = new BulkOperations<QuestionCosmos>(questions.Count);
        
        foreach (var question in questions)
        {
            var cosmos_question = new QuestionCosmos(question);
            
            bulkOperations.Tasks.Add(CaptureOperationResponse(_Container.CreateItemAsync(
                cosmos_question), cosmos_question
            ));
        }
        
        return await bulkOperations.ExecuteAsync();
    }
    
    private class BulkOperations<T>(int operationCount)
    {
        public readonly List<Task<OperationResponse<T>>> Tasks = new(operationCount);

        private readonly Stopwatch stopwatch = Stopwatch.StartNew();

        public async Task<BulkOperationResponse<T>> ExecuteAsync()
        {
            await Task.WhenAll(Tasks);
            stopwatch.Stop();
            return new BulkOperationResponse<T>
            {
                TotalTimeTaken = stopwatch.Elapsed,
                TotalRequestUnitsConsumed = Tasks.Sum(task => task.Result.RequestUnitsConsumed),
                SuccessfulDocuments = Tasks.Count(task => task.Result.IsSuccessful),
                Failures = Tasks.Where(task => !task.Result.IsSuccessful).Select(task => (task.Result.Item, task.Result.CosmosException)).ToList()
            };
        }
    }
    
    public class BulkOperationResponse<T>
    {
        public TimeSpan TotalTimeTaken { get; set; }
        public int SuccessfulDocuments { get; set; } = 0;
        public double TotalRequestUnitsConsumed { get; set; } = 0;

        public IReadOnlyList<(T, Exception)>? Failures { get; set; }
    }

    public class OperationResponse<T>
    {
        public T? Item { get; set; }
        public double RequestUnitsConsumed { get; set; } = 0;
        public bool IsSuccessful { get; set; }
        public Exception? CosmosException { get; set; }
    }

    private static async Task<OperationResponse<T>> CaptureOperationResponse<T>(Task<ItemResponse<T>> task, T item)
    {
        try
        {
            await task;

            return new OperationResponse<T>
            {
                Item = item,
                IsSuccessful = true,
                RequestUnitsConsumed = task.Result.RequestCharge
            };
        }
        catch (Exception ex)
        {
            if (ex is CosmosException cosmosException)
            {
                AnsiConsole.WriteException(cosmosException);
                
                return new OperationResponse<T>
                {
                    Item = item,
                    RequestUnitsConsumed = cosmosException.RequestCharge,
                    IsSuccessful = false,
                    CosmosException = cosmosException
                };
            }
            
            AnsiConsole.WriteException(ex);

            return new OperationResponse<T>
            {
                Item = item,
                IsSuccessful = false,
                CosmosException = ex
            };
        }
    }
}