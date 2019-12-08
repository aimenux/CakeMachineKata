using System;
using System.Threading.Tasks;
using CakeMachineKata.UsingDataflow;

namespace CakeMachineKata.ConsoleApp
{
    public static class Program
    {
        private static async Task Main()
        {
            var stock = new Stock(50);
            var cakeMachine = new CakeMachine();
            await cakeMachine.RunAsync(stock);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
