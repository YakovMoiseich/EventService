using System;

namespace Assets.Analytics.TimeProvider
{
    public interface IClock
    {
        /// <summary>
        /// Current time
        /// </summary>
        DateTime Now();
    }
}
