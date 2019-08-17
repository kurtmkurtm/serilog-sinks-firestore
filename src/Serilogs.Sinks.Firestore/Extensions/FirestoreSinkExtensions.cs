using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.Firestore;
using Serilog.Sinks.Firestore.Enums;
using Serilog.Sinks.Firestore.Models;
using System;

namespace Serilog.Sinks
{
    /// <summary>
    /// Extension method for creating Firestore sink
    /// </summary>
    public static class FirestoreSinkExtensions
    {
        /// <summary>
        /// Configuration for logging events to Firestore Sink
        /// </summary>
        /// <param name="loggerConfiguration">Serilog specific configuration</param>
        /// <param name="projectId">ID of the Firestore project to use</param>
        /// <param name="collectionName">Name of the Firestore collection to use, will be create if does not exist</param>
        /// <param name="restrictedToMinimumLevel">The minimum level for events passed through the sink. Ignored when levelSwitch is specified.</param>
        /// <param name="levelSwitch">A switch allowing the pass-through minimum level to be changed at runtime</param>
        /// <param name="batchIntervalInSeconds">The time to wait between checking for event batches</param>
        /// <param name="batchSizeLimit">he maximum number of events to include in a single batch, Firestore limit is 500</param>
        /// <param name="formatProvider">Message formatter</param>
        /// <param name="credentialType"></param>
        /// <param name="credentialValue"></param>
        /// <returns>Configuration</returns>
        public static LoggerConfiguration Firestore(
            this LoggerSinkConfiguration loggerConfiguration,
            string projectId,
            string collectionName,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            LoggingLevelSwitch levelSwitch = null,
            int batchIntervalInSeconds = 10,
            int batchSizeLimit = 100,
            IFormatProvider formatProvider = null,
            CredentialType credentialType = CredentialType.Default,
            string credentialValue = null)
            => Firestore(loggerConfiguration,
                new FirestoreConfiguration(projectId, collectionName, credentialType, credentialValue),
                restrictedToMinimumLevel,
                levelSwitch,
                batchIntervalInSeconds,
                batchSizeLimit,
                formatProvider);

        /// <summary>
        /// Configuration for logging events to Firestore Sink
        /// </summary>
        /// <param name="loggerConfiguration">Serilog specific configuration</param>
        /// <param name="firestoreConfiguration">Firestore specific configuration</param>
        /// <param name="restrictedToMinimumLevel">The minimum level for events passed through the sink. Ignored when levelSwitch is specified.</param>
        /// <param name="levelSwitch">A switch allowing the pass-through minimum level to be changed at runtime</param>
        /// <param name="batchIntervalInSeconds">The time to wait between checking for event batches</param>
        /// <param name="batchSizeLimit">The maximum number of events to include in a single batch, Firestore limit is 500</param>
        /// <param name="formatProvider">Message formatter</param>
        /// <returns>Configuration</returns>
        public static LoggerConfiguration Firestore(
            this LoggerSinkConfiguration loggerConfiguration,
            FirestoreConfiguration firestoreConfiguration,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            LoggingLevelSwitch levelSwitch = null,
            int batchIntervalInSeconds = 10,
            int batchSizeLimit = 100,
            IFormatProvider formatProvider = null)
        {
            if (batchSizeLimit > 500 || batchSizeLimit <= 0)
                throw new ArgumentOutOfRangeException(nameof(batchSizeLimit), batchSizeLimit, $"Batch size must be greater than zero and less than 500, a Firestore batched write can contain up to 500 operations");

            var batchInterval = TimeSpan.FromSeconds(batchIntervalInSeconds);
            if (batchInterval <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(batchInterval), batchInterval, "Batch period must be longer than zero");

            return loggerConfiguration.Sink(new FirestoreSink(formatProvider, firestoreConfiguration, batchInterval, batchSizeLimit), restrictedToMinimumLevel, levelSwitch);
        }
    }
}
