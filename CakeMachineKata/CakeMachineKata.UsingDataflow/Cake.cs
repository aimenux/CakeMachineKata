using System;

namespace CakeMachineKata.UsingDataflow
{
    public class Cake
    {
        public string Id { get; set; }
        public CakeStatus Status { get; set; }
        public Recipe Recipe { get; }
        public DateTime CreationDate { get; set; }
        public DateTime DeliveryDate { get; set; }

        public Cake(Recipe recipe)
        {
            Recipe = recipe;
            Status = CakeStatus.Prepared;
            Id = Guid.NewGuid().ToString();
            CreationDate = DateTime.Now;
        }

        public override string ToString()
        {
            var creationDate = CreationDate.ToString("hh:mm:ss.f");
            var deliveryDate = DeliveryDate.ToString("hh:mm:ss.f");
            var spendingTime = Math.Round((DeliveryDate - CreationDate).TotalSeconds, 3).ToString("F3");
            return $"Cake '{Id}' tooks '{spendingTime}' seconds [created '{creationDate}' delivered '{deliveryDate}']";
        }
    }
}
