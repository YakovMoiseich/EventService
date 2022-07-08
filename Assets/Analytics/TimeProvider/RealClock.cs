using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
