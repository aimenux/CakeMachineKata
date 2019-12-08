namespace CakeMachineKata.UsingDataflow
{
    public class Ingredient
    {
        public IngredientType IngredientType { get; }

        public Ingredient(IngredientType ingredientType)
        {
            IngredientType = ingredientType;
        }
    }
}