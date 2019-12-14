using System.Collections.Concurrent;
using System.Threading.Tasks;
using CakeMachineKata.UsingDataflow.Settings;

namespace CakeMachineKata.UsingDataflow
{
    public class Stock
    {
        private readonly ConcurrentQueue<Recipe> _recipes;

        public Duration DurationForNextRecipe { get; set; } = GetDurationForNextRecipe();

        public Stock(int total)
        {
            _recipes = new ConcurrentQueue<Recipe>();

            for(var index = 0; index < total; index++)
            {
                _recipes.Enqueue(new Recipe());
            }
        }

        public async Task<Recipe> GetNextRecipeAsync()
        {
            await Task.Delay(DurationForNextRecipe);
            return _recipes.TryDequeue(out var recipe) ? recipe : null;
        }

        private static Duration GetDurationForNextRecipe()
        {
            const DurationType durationType = DurationType.Milliseconds;
            return new Duration(1, 10, durationType);
        }
    }
}