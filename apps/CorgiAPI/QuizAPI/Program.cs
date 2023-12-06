using QuizAPI.Cosmos;
using QuizAPI.Neo4j;
using QuizAPI.Kernels;
using QuizAPI.Services;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration.AddJsonFile("appsettings.Secret.json").Build();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddLogging(logging =>
    {
        logging.AddFile(config.GetSection("Logging"));
    });
}

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpClient();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCosmosDb(config);
builder.Services.AddNeo4j(config);
builder.Services.AddSemanticKernel(config);

builder.Services.AddQuizApiRepositories();
builder.Services.AddQuizApiServices();
builder.Services.AddQuizAPIKernels();

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

logger.LogInformation("Swagger running on: http://localhost:5000/swagger");

app.Run();
