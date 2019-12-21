using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Timers;
using CakeMachineKata.UsingDataflow.Settings;

namespace CakeMachineKata.UsingDataflow
{
    public class CakeMachine
    {
        private readonly ConcurrentBag<Cake> _cakes;
        private readonly ReportingTimer _reportingTimer;
        private readonly DataflowLinkOptions _linkOptions;
        private readonly ExecutionDataflowBlockOptions _prepareOptions;
        private readonly ExecutionDataflowBlockOptions _cookOptions;
        private readonly ExecutionDataflowBlockOptions _packageOptions;
        private readonly Duration _prepareDuration;
        private readonly Duration _cookDuration;
        private readonly Duration _packageDuration;
        private readonly Duration _deliveryDuration;

        public CakeMachine(CakeMachineSettings settings)
        {
            _cakes = new ConcurrentBag<Cake>();
            var durationSettings = settings.DurationSettings;
            _prepareDuration = durationSettings.PrepareDuration;
            _cookDuration = durationSettings.CookDuration;
            _packageDuration = durationSettings.PackageDuration;
            _deliveryDuration = durationSettings.DeliveryDuration;
            var prepareMaxDegree = settings.ParallelismSettings.PrepareMaxDegree;
            var cookMaxDegree = settings.ParallelismSettings.CookMaxDegree;
            var packageMaxDegree = settings.ParallelismSettings.PackageMaxDegree;
            _linkOptions = new DataflowLinkOptions { PropagateCompletion = true };
            _prepareOptions = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = prepareMaxDegree };
            _cookOptions = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = cookMaxDegree };
            _packageOptions = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = packageMaxDegree };

            var reportingSettings = settings.ReportingSettings;
            if (!(reportingSettings?.IsEnabled ?? false)) return;
            _reportingTimer = new ReportingTimer(reportingSettings.ReportInterval, OnReportingTimerEvent);
        }

        public async Task RunAsync(Stock stock)
        {
            _reportingTimer?.Start();

            var prepareStep = new TransformBlock<Recipe,Cake>(async recipe => await PrepareCakeAsync(recipe), _prepareOptions);
            var cookStep = new TransformBlock<Cake,Cake>(async cake => await CookCakeAsync(cake), _cookOptions);
            var packageStep = new TransformBlock<Cake,Cake>(async cake => await PackageCakeAsync(cake), _packageOptions);
            var deliveryStep = new ActionBlock<Cake>(async cake => await DeliveryCakeAsync(cake));

            prepareStep.LinkTo(cookStep, _linkOptions);
            cookStep.LinkTo(packageStep, _linkOptions);
            packageStep.LinkTo(deliveryStep, _linkOptions);

            while (true)
            {
                var recipe = await stock.GetNextRecipeAsync();
                if (recipe == null)
                {
                    break;
                }

                await prepareStep.SendAsync(recipe);
            }

            prepareStep.Complete();
            await prepareStep.Completion;
            await cookStep.Completion;
            await packageStep.Completion;

             _reportingTimer?.Stop();
        }

        private async Task<Cake> PrepareCakeAsync(Recipe recipe)
        {
            var creationDate = DateTime.Now;
            await Task.Delay(_prepareDuration);
            var cake = new Cake(recipe)
            {
                CreationDate = creationDate
            };
            _cakes.Add(cake);
            return cake;
        }

        private async Task<Cake> CookCakeAsync(Cake cake)
        {
            await Task.Delay(_cookDuration);
            cake.Status = CakeStatus.Cooked;
            return cake;
        }

        private async Task<Cake> PackageCakeAsync(Cake cake)
        {
            await Task.Delay(_packageDuration);
            cake.Status = CakeStatus.Packaged;
            return cake;
        }

        private async Task DeliveryCakeAsync(Cake cake)
        {
            await Task.Delay(_deliveryDuration);
            cake.Status = CakeStatus.Delivered;
            cake.DeliveryDate = DateTime.Now;
        }

        private void OnReportingTimerEvent(object sender, ElapsedEventArgs e)
        {
            var prepared = _cakes.Count(x => x.Status == CakeStatus.Prepared);
            var cooked = _cakes.Count(x => x.Status == CakeStatus.Cooked);
            var packaged = _cakes.Count(x => x.Status == CakeStatus.Packaged);
            var delivered = _cakes.Count(x => x.Status == CakeStatus.Delivered);
            Console.WriteLine($"Current cake machine status at: {DateTime.Now}");
            Console.WriteLine($"Total finished cakes: {delivered}");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"- Prepared: {prepared}");
            Console.WriteLine($"- Cooked: {cooked}");
            Console.WriteLine($"- Packaged: {packaged}\n");
            Console.ResetColor();
        }
    }
}