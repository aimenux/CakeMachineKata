using System.Collections.Generic;

namespace CakeMachineKata.UsingDataflow
{
    public class Stock : List<Recipe>
    {
        public Stock(int total)
        {
            for(var index = 0; index < total; index++)
            {
                Add(new Recipe());
            }
        }
    }
}