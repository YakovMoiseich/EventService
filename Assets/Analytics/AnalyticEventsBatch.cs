using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Analytics
{
    [Serializable]
    internal struct AnalyticEventsBatch
    {
        [SerializeField]
        public List<AnalyticEvent> Events;
    }
}
