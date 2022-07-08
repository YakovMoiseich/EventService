using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Analytics.TimeProvider
{
    public class MockClock : IClock
    {
        private DateTime _time;

        public MockClock(DateTime time)
        {
            _time = time;
        }

        public void SetTime(DateTime time)
        {
            _time = time;
        }

        public DateTime Now()
        {
            return _time;
        }

        public void Offset(TimeSpan offset)
        {
            _time += offset;
        }
    }
}
