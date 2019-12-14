namespace CakeMachineKata.UsingDataflow.Settings
{
    public class ReportingSettings
    {
        public bool IsEnabled { get; }
        public Duration ReportInterval { get; }

        public ReportingSettings(Duration reportInterval, bool isEnabled)
        {
            ReportInterval = reportInterval;
            IsEnabled = isEnabled;
        }
    }
}
