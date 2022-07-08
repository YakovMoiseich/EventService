using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Analytics.EventsStorage;
using Assets.Analytics.TimeProvider;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Analytics
{
    public class EventService : MonoBehaviour
    {
        private IEventsStorage _storage;
        private ILogger _logger;
        private IClock _clock;

        /// <summary>
        /// Analytics server address
        /// </summary>
        public string ServerEndpoint;

        /// <summary>
        /// Period for accumulate events for sending events batches
        /// </summary>
        public float CooldownBeforeSendSeconds = 3f;

        private string _userSessionPrefix;
        private int _eventsCounter;

        public static EventService Instance { get; private set; }

        private EventService() { }

        void Awake()
        {
            if (Instance == this)
            {
                Destroy(gameObject);
            }

            if (Instance == null)
            {
                Instance = this;
            }

            DontDestroyOnLoad(gameObject);

            _logger = Debug.unityLogger;
            _userSessionPrefix = DateTime.Now.ToString("dd’-‘MM’-‘yyyy’T’HH’:’mm’:’ss.fffffff");
            _clock = new RealClock();
            _storage = new LocalEventsStorage(Application.persistentDataPath + "/eventsData.data");

            StartCoroutine(MonitorEvents());
            TrackEvent("ls", "cd");
        }

        /// <summary>
        /// Send event on analytics server
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data"></param>
        public void TrackEvent(string type, string data)
        {
            var analyticEvent = new AnalyticEvent($"{_userSessionPrefix}_{_eventsCounter++}", type, data, Status.NotSent);
            _storage.AddEvent(analyticEvent);
            _logger.Log("Got event to send", analyticEvent);
        }

        private IEnumerator MonitorEvents()
        {
            WaitForSeconds waitTime = new WaitForSeconds(CooldownBeforeSendSeconds);
            while (true)
            {
                SendBatch();
                yield return waitTime;
            }
        }

        private void SendBatch()
        {
            try
            {
                var toSend = _storage.GetNotSentEvents();
                if (!toSend.Any())
                    return;

                _logger.Log("Prepare to send events num {count}", toSend.Count());
                StartCoroutine(SendEvents(toSend));
            }
            catch (Exception exc)
            {
                _logger.LogError("SendEvents exception", exc);
            }
        }

        private IEnumerator SendEvents(IEnumerable<AnalyticEvent> events)
        {
            using (UnityWebRequest request = PrepareJson(ServerEndpoint, events))
            {
                _logger.Log("SendEvents", $"Request {request}");
                var ids = events.Select(evt => evt.Id).ToList();
                var count = ids.Count;
                _storage.SetEventsSending(ids);
                yield return request.SendWebRequest();
                _logger.Log("SendEvents", $"Result {request.result}");

                var result = request.responseCode == 200;
                if (result)
                {
                    _storage.SetEventsSent(ids);
                    _logger.Log("SendEvents",$"Successful sent events num {count}");
                }
                else
                {
                    StartCoroutine(SendEvents(events));
                    _logger.Log("SendEvents", $"Failed to send events num {count}");
                }
            }
        }

        private UnityWebRequest PrepareJson(string serverUrl, IEnumerable<AnalyticEvent> toSend)
        {
            var batch = new AnalyticEventsBatch
            {
                Events = toSend.ToList()
            };
            var json = JsonUtility.ToJson(batch);
            _logger.Log("PrepareJson", $"Prepared events to send {json}");
            Debug.Log(json);
            UnityWebRequest request = UnityWebRequest.Post(serverUrl, json);
            request.SetRequestHeader("Authorization", "Basic Og==");
            request.SetRequestHeader("Content-Type", "application/json");
            return request;
        }

        /// <summary>
        /// Set time perion for batching events in milliseconds
        /// </summary>
        /// <returns></returns>
        public void SetSendingCooldownMs(int cooldownSeconds)
        {
            CooldownBeforeSendSeconds = cooldownSeconds;
        }

        /// <summary>
        /// Set server url to sent analytic events
        /// </summary>
        /// <param name="serverUrl"></param>
        public void SetAnalyticServerEndpoint(string serverUrl)
        {
            ServerEndpoint = serverUrl;
        }

        /// <summary>
        /// Set storage for saving events to send them in case of connection problems with analytics service
        /// </summary>
        /// <param name="storage"><see cref="IEventsStorage"/></param>
        /// <returns>This instance of EventService</returns>
        public EventService InitStorage(IEventsStorage storage)
        {
            if (storage != null)
                _logger.LogError(GetType().Name, "Storage already set");

            _storage = storage;
            return Instance;
        }

        /// <summary>
        /// Set logger for further usage by class
        /// </summary>
        /// <param name="logger"><see cref="ILogger"/></param>
        /// <returns>This instance of EventService</returns>
        public EventService InitLogger(ILogger logger)
        {
            if (_logger != null)
                _logger.LogError(GetType().Name, "Logger already set");

            _logger = logger;
            return Instance;
        }

        /// <summary>
        /// Set time provider for further usage by class
        /// </summary>
        /// <param name="clock"><see cref="IClock"/></param>
        /// <returns>This instance of EventService</returns>
        public EventService InitClock(IClock clock)
        {
            if (_clock != null)
                _logger.LogError(GetType().Name, "Clock already set");

            _clock = clock;
            return Instance;
        }
    }
}