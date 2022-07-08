using System;
using System.Collections.Generic;
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
