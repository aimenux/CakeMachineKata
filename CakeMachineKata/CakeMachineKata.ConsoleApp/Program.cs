using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CakeMachineKata.UsingDataflow;
using CakeMachineKata.UsingDataflow.Settings;
using Microsoft.Extensions.Configuration;

namespace CakeMachineKata.ConsoleApp
{
    public static class Program
    {
        private static async Task Main()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            var total = configuration.GetValue<int>("Stock");
            var stock = new Stock(total);

            var durationType = configuration.GetValue<DurationType>("DurationType");
            var cakeMachine = CakeMachineFactory.GetCakeMachine(durationType);

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            await cakeMachine.RunAsync(stock);
            stopWatch.Stop();
            Console.WriteLine($"Elapsed time: {stopWatch.Elapsed}");
            Console.WriteLine("Press any key to exit ..");
            Console.ReadKey();
        }
    }
}
