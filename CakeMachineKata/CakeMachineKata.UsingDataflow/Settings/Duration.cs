using System;

namespace CakeMachineKata.UsingDataflow.Settings
{
    public class Duration
    {
        private static readonly Random Random = new Random();

        private readonly int _durationMin;
        private readonly int _durationMax;

        public DurationType DurationType { get; }

        public Duration(int duration, DurationType durationType)
        {
            _durationMin = duration;
            _durationMax = duration;
            DurationType = durationType;
        }

        public Duration(int durationMin, int durationMax, DurationType durationType)
        {
            _durationMin = durationMin;
            _durationMax = durationMax;
            DurationType = durationType;
        }

        public static implicit operator int(Duration duration)
        {
            return duration?.Value ?? 0;
        }

        public int Value
        {
            get
            {
                var duration = Random.Next(_durationMin, _durationMax);

                if (DurationType == DurationType.Seconds)
                {
                    return duration * 1000;
                }

                return duration;
            }
        }
    }
}
