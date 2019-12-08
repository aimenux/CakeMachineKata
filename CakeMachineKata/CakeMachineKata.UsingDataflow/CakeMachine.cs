using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace CakeMachineKata.UsingDataflow
{
    public class CakeMachine : ConcurrentBag<Cake>
    {
        private static readonly Random Random = new Random();

        private readonly DataflowLinkOptions _linkOptions;
        private readonly ExecutionDataflowBlockOptions _prepareOptions;
        private readonly ExecutionDataflowBlockOptions _cookOptions;
        private readonly ExecutionDataflowBlockOptions _packageOptions;

        public CakeMachine()
        {
            _linkOptions = new DataflowLinkOptions { PropagateCompletion = true };
            _prepareOptions = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 3 };
            _cookOptions = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 5 };
            _packageOptions = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 2 };
        }

        public async Task RunAsync(Stock stock)
        {
            var prepareStep = new TransformBlock<Recipe,Cake>(async recipe => await PrepareCakeAsync(recipe), _prepareOptions);
            var cookStep = new TransformBlock<Cake,Cake>(async cake => await CookCakeAsync(cake), _cookOptions);
            var packageStep = new TransformBlock<Cake,Cake>(async cake => await PackageCakeAsync(cake), _packageOptions);
            var reportStep = new ActionBlock<Cake>(async cake => await DeliveryCakeAsync(cake));

            prepareStep.LinkTo(cookStep, _linkOptions);
            cookStep.LinkTo(packageStep, _linkOptions);
            packageStep.LinkTo(reportStep, _linkOptions);

            foreach (var recipes in stock)
            {
                await prepareStep.SendAsync(recipes);
            }
	
            prepareStep.Complete();
            await reportStep.Completion;
        }

        private async Task<Cake> PrepareCakeAsync(Recipe recipe)
        {
            var creationDate = DateTime.Now;
            var durationInSeconds = Random.Next(5,8);
            var prepareCakeDuration = 1000 * durationInSeconds;
            await Task.Delay(prepareCakeDuration);
            var cake = new Cake(recipe)
            {
                CreationDate = creationDate
            };
            Add(cake);
            return cake;
        }

        private async Task<Cake> CookCakeAsync(Cake cake)
        {
            const int cookCakeDuration = 10_000;
            await Task.Delay(cookCakeDuration);
            cake.Status = CakeStatus.Cooked;
            return cake;
        }

        private async Task<Cake> PackageCakeAsync(Cake cake)
        {
            const int packageCakeDuration = 2_000;
            await Task.Delay(packageCakeDuration);
            cake.Status = CakeStatus.Packaged;
            return cake;
        }

        private Task DeliveryCakeAsync(Cake cake)
        {
            cake.Status = CakeStatus.Delivered;
            cake.DeliveryDate = DateTime.Now;
            Console.WriteLine(cake);
            return Task.CompletedTask;
        }
    }
}