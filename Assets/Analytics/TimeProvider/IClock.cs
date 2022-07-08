using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
