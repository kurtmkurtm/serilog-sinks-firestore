using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Serilog.Sinks.Firestore.Enums;
using Serilog.Sinks.Firestore.Interfaces;
using Serilog.Sinks.Firestore.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Serilog.Sinks.Firestore
{
    /// <summary>
    /// Client wrapper for FirestoreDb
    /// </summary>
    internal class FirestoreApiClient : IFirestoreApiClient
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
            var clientBuilder = new FirestoreClientBuilder();

            switch (configuration.CredentialType)
            {
                case CredentialType.Default:
                    break;
                case CredentialType.CredentialsPath:
                    clientBuilder.CredentialsPath = configuration.CredentialValue;
                    break;
                case CredentialType.JsonCredentials:
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
        public async Task WriteAsync(IEnumerable<IReadOnlyDictionary<string, object>> messages, CancellationToken cancellationToken = default(CancellationToken))
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
