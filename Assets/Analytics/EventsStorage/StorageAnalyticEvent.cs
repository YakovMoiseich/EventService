using System;
using UnityEngine;

namespace Assets.Analytics.EventsStorage
{
    [Serializable]
    internal class StorageAnalyticEvent
    {
        [SerializeField] public string Type;
        [SerializeField] public string Data;

        [SerializeField] public string Id;
        [SerializeField] public Status Status;

        internal StorageAnalyticEvent(string id, string type, string data, Status status)
        {
            Id = id;
            Type = type;
            Data = data;
            Status = status;
        }

        internal StorageAnalyticEvent(AnalyticEvent evt)
        {
            Id = evt.Id;
            Type = evt.Type;
            Data = evt.Data;
            Status = evt.Status;
        }
    }
}
