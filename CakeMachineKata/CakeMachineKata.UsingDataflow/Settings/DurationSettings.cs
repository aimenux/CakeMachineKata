namespace CakeMachineKata.UsingDataflow.Settings
{
    public class DurationSettings
    {
        public DurationSettings(Duration prepareDuration, Duration cookDuration, Duration packageDuration, Duration deliveryDuration = null)
        {
            PrepareDuration = prepareDuration ?? GetDefaultDuration();
            CookDuration = cookDuration ?? GetDefaultDuration();
            PackageDuration = packageDuration ?? GetDefaultDuration();
            DeliveryDuration = deliveryDuration ?? GetDefaultDuration();
        }
        
        public Duration PrepareDuration { get; }
        public Duration CookDuration { get; }
        public Duration PackageDuration { get; }
        public Duration DeliveryDuration { get; }

        private static Duration GetDefaultDuration()
        {
            return new Duration(10, 20, DurationType.Milliseconds);
        }
    }
}