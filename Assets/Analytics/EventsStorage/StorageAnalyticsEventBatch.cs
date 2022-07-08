using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Analytics.EventsStorage
{
    [Serializable]
    internal struct StorageAnalyticEventsBatch
    {
        [SerializeField]
        public List<StorageAnalyticEvent> Events;
    }
}
