using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Serilog.Sinks.Firestore.Models;

namespace Serilog.Sinks.Firestore
{
    /// <summary>
    /// Client wrapper for FirestoreDb
    /// </summary>
    internal class FirestoreApiClient
    {
        private readonly FirestoreDb _firestoreDatabase;
        private readonly CollectionReference _collection;

        /// <summary>
        /// Initialises a new instance of <see cref="FirestoreApiClient"/> 
        /// creates a document collection in the project, if it does not exist
        /// </summary>
        /// <param name="configuration">Configuration options for Firestore usage</param>
        internal FirestoreApiClient(FirestoreConfiguration configuration)
        {
            if (configuration.ProjectId is null) throw new ArgumentNullException(nameof(configuration.ProjectId));
            if (configuration.CollectionName is null) throw new ArgumentNullException(nameof(configuration.CollectionName));

            var clientBuilder = new FirestoreClientBuilder();

            switch (configuration.CredentialType)
            {
                case Enums.CredentialType.Default:
                    break;
                case Enums.CredentialType.CredentialsPath:
                    clientBuilder.CredentialsPath = configuration.CredentialValue;
                    break;
                case Enums.CredentialType.JsonCredentials:
                    clientBuilder.JsonCredentials = configuration.CredentialValue;
                    break;
            }

            _firestoreDatabase = FirestoreDb.Create(configuration.ProjectId, clientBuilder.Build());
            _collection = _firestoreDatabase.Collection(configuration.CollectionName);
        }

        /// <summary>
        /// Batch add log data to a Firestore collection
        /// </summary>
        /// <param name="messages">Formatted log item</param>
        /// <param name="cancellationToken">Cancellation token for server commit</param>
        /// <returns>A task that can be awaited</returns> 
        internal async Task WriteAsync(IEnumerable<IReadOnlyDictionary<string, object>> messages, CancellationToken cancellationToken = default(CancellationToken))
        {           
            var batch = _firestoreDatabase.StartBatch();

            foreach (var message in messages)
            {
                var logRef = _collection.Document();
                batch.Set(logRef, message);
            }     

            _ = await batch.CommitAsync(cancellationToken);
        }
    }
}
