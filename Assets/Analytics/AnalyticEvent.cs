using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Analytics
{
    [Serializable]
    public class AnalyticEvent
    {
        [SerializeField] public string Type;
        [SerializeField] public string Data;

        [System.NonSerialized] public string Id;
        [System.NonSerialized] public Status Status;
        

        internal AnalyticEvent(string id, string type, string data, Status status)
        {
            Id = id;
            Type = type;
            Data = data;
            Status = status;
        }

        internal string ToAnalyticJson()
        {
            return JsonUtility.ToJson(this);
        }

        public override string ToString()
        {
            return JsonUtility.ToJson(this);
        }
    }

    public enum Status
    {
        NotSent,
        Sending,
        Sent
    }
}
