// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using DataLoader.Services;
using Microsoft.Extensions.Configuration;
using Spectre.Console;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

AnsiConsole.Write(new FigletText($"{config["Name"]!}").Color(Color.Green));
AnsiConsole.WriteLine("");

CosmosDBService cosmos = null;
KernelService kernel = null;

AnsiConsole.Status().Start("Initializing...", ctx =>
{
    ctx.Spinner(Spinner.Known.Star);
    ctx.SpinnerStyle(Style.Parse("green"));

    cosmos = new CosmosDBService(config);
    kernel = new KernelService(config);

    ctx.Status("Initialized");
});

const string import_stack = "1.\tImport data from stack exchange";
const string import_neo = "2.\tImport data from neo4j";
const string prompt = "3.\tPrompt kernel";
const string delete = "4.\tDelete all data";
const string exit = "5.\tExit";

Run();

return;

void Run()
{
    while (true)
    {
        var option = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select an option")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                .AddChoices(import_stack, import_neo, prompt, delete, exit)
        );

        switch (option)
        {
            case import_stack:
                break;
            case import_neo:
                Import().GetAwaiter().GetResult();
                break;
            case prompt:
                Prompt().GetAwaiter().GetResult();
                break;
            case delete:
                Delete().GetAwaiter().GetResult();
                break;
            case exit:
                return;
        }
    }
}

async Task Delete()
{
    await cosmos.CleanAllData();

    AnsiConsole.WriteLine("Data cleaned, Indexes need to be cleared manually");
}

async Task Import()
{
    var neo = new Neo4jService(config);

    var count = await neo.GetTotalQuestionCount();
    var size = int.Parse(config["BatchSize"]);
    var batches = count / size + 1;
    var inc = 100.0 / batches;

    AnsiConsole.WriteLine($"Batches : {batches}");

    await AnsiConsole.Progress()
        .Columns(
            new TaskDescriptionColumn(),
            new ProgressBarColumn(),
            new PercentageColumn(),
            new RemainingTimeColumn(),
            new SpinnerColumn()
        )
        .StartAsync(async ctx =>
        {
            var task = ctx.AddTask("[green]Importing Data[/]");
            var etask = ctx.AddTask("[yellow]Embedding Progress[/]");

            for (var i = 0; i < batches; i++)
            {
                var start = i * size;
                var limit = Math.Min((i + 1) * size, count) - start;

                var questions = await neo.GetQuestions(start, limit);

                // Add questions
                var response = await cosmos.AddQuestionsAsync(questions);

                if (response.Failures.Count > 0)
                {
                    AnsiConsole.Markup("[red]Cosmos Error[/]");
                    AnsiConsole.WriteLine("");
                    await Delete();
                    return;
                }

                // Add embeddings
                etask.Value = 0;
                var einc = 100.0 / limit;

                foreach (var question in questions)
                {
                    await kernel.SaveEmbeddingsAsync($"{question.Title}\n{question.Body}", $"{question.Id}", question.Site);
                    etask.Increment(einc);
                }

                task.Increment(inc);
            }
        });
}

async Task Prompt()
{
    var chatCompletion = string.Empty;

    var site = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Select a site")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
            .AddChoices("stackoverflow", "math", "physics")
    );
    var prompt = AnsiConsole.Prompt(new TextPrompt<string>("Type a question...").PromptStyle("teal"));

    await AnsiConsole.Status().StartAsync("Processing...", async ctx =>
    {
        ctx.Spinner(Spinner.Known.Star);
        ctx.SpinnerStyle(Style.Parse("green"));

        ctx.Status("Performing Vector Search..");
        var ids = kernel.SearchEmbeddingsAsync(site, prompt).GetAwaiter().GetResult();

        ctx.Status("Retrieving questions from Cosmos");
        var questions = cosmos.GetQuestionsAsync(ids).GetAwaiter().GetResult();

        ctx.Status($"Processing {questions.Count} questions to generate Chat Response");
        chatCompletion = await kernel.GenerateCompletionAsync(prompt, site, questions);
    });

    AnsiConsole.WriteLine("");
    AnsiConsole.Write(new Rule($"[silver]AI Assistant Response[/]") { Justification = Justify.Center });
    AnsiConsole.WriteLine(chatCompletion);
    AnsiConsole.WriteLine("");
    AnsiConsole.WriteLine("");
    AnsiConsole.Write(new Rule($"[yellow]****[/]") { Justification = Justify.Center });
    AnsiConsole.WriteLine("");
}