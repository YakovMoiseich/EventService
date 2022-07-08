using System;

namespace Assets.Analytics.TimeProvider
{
    public class RealClock : IClock
    {
        public DateTime Now()
        {
            return DateTime.Now;
        }
    }
}
