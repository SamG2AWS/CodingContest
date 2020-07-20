using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Scenario.Configuration;
using Scenario.FileOperations;
using Scenario.Models;
using System;

namespace Scenario
{
    class Program
    {
        static void Main(string[] args)
        {
            //Set up DI

            var serviceProvider = new ServiceCollection()
                .AddLogging(option =>
                {
                    option.AddConsole();
                })
                .AddTransient<IJsonReader, JsonReader>()
                .AddTransient<IAppSettings, AppSettings>()
                .AddTransient<ICargoLoader, CargoLoader>()
                .BuildServiceProvider();

            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();

            IScheduler scheduler = new Scheduler(serviceProvider.GetService<ICargoLoader>());
            try
            {
                scheduler.BeginProcessing();
            }
            catch(Exception e)
            {
                logger.LogError(e, "Scenario.Program: [Main] Error Occured");
            }

            Console.ReadKey();
        }
    }
}
