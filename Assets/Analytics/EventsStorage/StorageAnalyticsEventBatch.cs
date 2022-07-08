using System;
using System.Collections.Generic;
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
