namespace CakeMachineKata.UsingDataflow.Settings
{
    public class DurationSettings
    {
        public DurationSettings(Duration prepareDuration, Duration cookDuration, Duration packageDuration)
        {
            PrepareDuration = prepareDuration;
            CookDuration = cookDuration;
            PackageDuration = packageDuration;
        }
        
        public Duration PrepareDuration { get; }
        public Duration CookDuration { get; }
        public Duration PackageDuration { get; }
    }
}