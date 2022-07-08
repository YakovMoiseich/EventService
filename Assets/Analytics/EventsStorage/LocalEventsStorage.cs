using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Assets.Analytics.EventsStorage
{
    internal class LocalEventsStorage : IEventsStorage
    {
        private readonly Dictionary<string, AnalyticEvent> _events;
        private readonly string _eventsDataFile;

        public LocalEventsStorage(string eventsFile)
        {
            _eventsDataFile = eventsFile;
            _events = LoadNotSentEvents();
        }

        public void AddEvent(AnalyticEvent evt)
        {
            Add(evt);
            SaveEvents();
        }

        IEnumerable<AnalyticEvent> IEventsStorage.GetNotSentEvents()
        {
            var notSentEvents = _events.Values.Where(x => x.Status == Status.NotSent);
            foreach (var analyticEvent in notSentEvents)
            {
                yield return analyticEvent;
            }
        }

        public void SetEventsSent(IEnumerable<string> ids)
        {
            foreach (var id in ids)
            {
                var evt = GetEvent(id);
                if (evt != null)
                    evt.Status = Status.Sent;
            }
        }

        public void SetEventsSending(IEnumerable<string> ids)
        {
            foreach (var id in ids)
            {
                var evt = GetEvent(id);
                if (evt != null)
                    evt.Status = Status.Sending;
            }
        }

        public void SetEventsNotSent(IEnumerable<string> ids)
        {
            foreach (var id in ids)
            {
                var evt = GetEvent(id);
                if (evt != null)
                    evt.Status = Status.NotSent;
            }
        }

        private AnalyticEvent GetEvent(string id)
        {
            _events.TryGetValue(id, out var evt);
            return evt;
        }

        private void Add(AnalyticEvent evt)
        {
            if (!_events.ContainsKey(evt.Id))
                _events.Add(evt.Id, evt);
        }

        private Dictionary<string, AnalyticEvent> LoadNotSentEvents()
        {
            if (!File.Exists(_eventsDataFile))
            {
                return new Dictionary<string, AnalyticEvent>();
            }

            string fileContents = File.ReadAllText(_eventsDataFile);
            var events = JsonUtility.FromJson<StorageAnalyticEventsBatch>(fileContents);
            var eventsDictionary = new Dictionary<string, AnalyticEvent>();
            foreach (var evt in events.Events)
            {
                if (evt.Status == Status.Sent)
                {
                    continue;
                }

                eventsDictionary.Add(evt.Id, new AnalyticEvent(evt.Id, evt.Type, evt.Data, evt.Status));
            }

            return eventsDictionary;
        }

        private void SaveEvents()
        {
            if (_events == null)
            {
                return;
            }

            var storageEventsBatch = new StorageAnalyticEventsBatch
            {
                Events = new List<StorageAnalyticEvent>()
            };

            foreach (var evt in _events)
            {
                var storageEvent = new StorageAnalyticEvent(evt.Value);
                if (storageEvent.Status == Status.Sent)
                    continue;

                if (storageEvent.Status == Status.Sending)
                    storageEvent.Status = Status.NotSent;

                storageEventsBatch.Events.Add(storageEvent);
            }

            File.WriteAllText(_eventsDataFile, JsonUtility.ToJson(storageEventsBatch));
        }
    }
}
