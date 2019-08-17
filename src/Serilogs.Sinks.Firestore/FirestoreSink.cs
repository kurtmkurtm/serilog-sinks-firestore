using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Serilog.Formatting.Compact;
using Serilog.Sinks.Firestore.Models;
using Serilog.Formatting.Json;

namespace Serilog.Sinks.Firestore
{
    /// <summary>
    /// Implementation of <see cref="PeriodicBatchingSink"/> that batch writes logs to Google Firestore 
    /// </summary>
    internal class FirestoreSink : PeriodicBatchingSink
    {
        private readonly IFormatProvider _formatProvider;
        private readonly FirestoreApiClient _firestoreClient;
        private readonly JsonFormatter _formatter;

        /// <summary>
        /// Initialises a new instance of <see cref="FirestoreSink"/> 
        /// </summary>
        /// <param name="formatProvider">Message formatter</param>
        /// <param name="configuration"></param>
        /// <param name="batchInterval">The time to wait between checking for event batches</param>
        /// <param name="batchSizeLimit">The maximum number of events to include in a single batch, Firestore limit is 500</param>
        internal FirestoreSink(IFormatProvider formatProvider, FirestoreConfiguration configuration, TimeSpan batchInterval, int batchSizeLimit)
            : base(batchSizeLimit, batchInterval)
        {
            _formatProvider = formatProvider;
            _firestoreClient = new FirestoreApiClient(configuration);
            _formatter = new JsonFormatter();
        }

        /// <summary>
        /// Emit a batch of log events, running asynchronously.
        /// </summary>
        /// <param name="events">The events to emit</param>
        /// <returns>A task that can be awaited</returns> 
        protected override async Task EmitBatchAsync(IEnumerable<LogEvent> events)
        {
            var messages = events.Select(ConvertLog);
            await _firestoreClient.WriteAsync(messages);
        }

        /// <summary>
        /// Convert log event to dictionary to Firestore
        /// </summary> 
        /// <param name="logEvent">A log event</param>
        /// <returns>A read only dictionary containing </returns>
        internal IReadOnlyDictionary<string, object> ConvertLog(LogEvent logEvent)
        {
            return new Dictionary<string, object>
            {
                { "EventIdHash", EventIdHash.Compute(logEvent.MessageTemplate.Text) },
                { "Timestamp", logEvent.Timestamp.ToUniversalTime() },
                { "Level", logEvent.Level.ToString() },
                { "Message", logEvent.RenderMessage(_formatProvider) },
                { "Properties", SerialiseData(logEvent.Properties)},
                { "Exception", logEvent.Exception?.ToString() }
            };
        }

        /// <summary>
        /// Serialises property values to strings
        /// </summary>
        /// <param name="properties">Properties from a log event</param>
        /// <returns>A dictionary containing serialised properties</returns>
        internal Dictionary<string, string> SerialiseData(IReadOnlyDictionary<string, LogEventPropertyValue> properties)
        {
           return properties.ToDictionary(x => x.Key, v => v.Value.ToString());
        }
    }
}
