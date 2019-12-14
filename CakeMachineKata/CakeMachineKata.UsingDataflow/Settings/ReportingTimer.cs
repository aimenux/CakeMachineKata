using System.Timers;

namespace CakeMachineKata.UsingDataflow.Settings
{
    public class ReportingTimer
    {
        private bool _isStopped;
        private readonly Timer _timer;
        private readonly ElapsedEventHandler _eventHandler;

        public ReportingTimer(int interval, ElapsedEventHandler handler)
        {
            _timer = new Timer();
            _timer.Elapsed += OnElapsedTimeEvent;
            _timer.Interval = interval;
            _timer.AutoReset = false;
            _eventHandler = handler;
        }

        public void Start()
        {
            _timer?.Start();
        }

        public void Stop()
        {
            _isStopped = true;
            _timer?.Stop();
            _timer?.Dispose();
        }

        private void OnElapsedTimeEvent(object sender, ElapsedEventArgs e)
        {
            _eventHandler(sender, e);
            if (_isStopped) return;
            Start();
        }
    }
}
