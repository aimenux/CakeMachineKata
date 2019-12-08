using System;
using System.Collections.Generic;

namespace CakeMachineKata.UsingDataflow
{
    public class Recipe : List<Ingredient>
    {
        public Recipe()
        {
            AddRange(GetRandomIngredients(5));
        }

        private static IEnumerable<Ingredient> GetRandomIngredients(int total)
        {
            var random = new Random();
            var types = Enum.GetValues(typeof(IngredientType));
            for(var index = 0; index < total; index++)
            {
                var randomIndex = random.Next(types.Length);
                var randomType = (IngredientType) types.GetValue(randomIndex);
                yield return new Ingredient(randomType);
            }
        }
    }
}