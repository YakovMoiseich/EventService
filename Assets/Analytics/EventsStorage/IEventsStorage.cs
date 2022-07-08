using System.Collections.Generic;

namespace Assets.Analytics.EventsStorage
{
    /// <summary>
    /// Storage for events aggregation (could be different for target platforms)
    /// </summary>
    public interface IEventsStorage
    {
        /// <summary>
        /// Add event for cache on disk or other options
        /// </summary>
        /// <param name="evt">Event to cache</param>
        void AddEvent(AnalyticEvent evt);

        /// <summary>
        /// Return all events with status not sent
        /// <see cref="Status"/>
        /// </summary>
        /// <returns>All evnts marked NotSent</returns>
        IEnumerable<AnalyticEvent> GetNotSentEvents();

        /// <summary>
        /// Update events status after succes sent to server
        /// </summary>
        /// <param name="ids">Ids of events to mark</param>
        void SetEventsSent(IEnumerable<string> ids);

        /// <summary>
        /// Mark events as sending
        /// </summary>
        /// <param name="ids">Ids of events to mark</param>
        void SetEventsSending(IEnumerable<string> ids);

        /// <summary>
        /// Mark events as failed sent
        /// </summary>
        /// <param name="ids">Ids of events to mark</param>
        void SetEventsNotSent(IEnumerable<string> ids);
    }
}
