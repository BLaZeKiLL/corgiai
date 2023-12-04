// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

Console.WriteLine($"App : {config["Name"]!}");