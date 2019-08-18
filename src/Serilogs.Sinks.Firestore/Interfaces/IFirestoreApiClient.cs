using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Serilog.Sinks.Firestore.Interfaces
{
    /// <summary>
    /// Firestore database client
    /// </summary>
    internal interface IFirestoreApiClient
    {
        /// <summary>
        /// Batch add log data to a Firestore collection
        /// </summary>
        /// <param name="messages">Formatted log item</param>
        /// <param name="cancellationToken">Cancellation token for server commit</param>
        /// <returns>A task that can be awaited</returns> 
        Task WriteAsync(IEnumerable<IReadOnlyDictionary<string, object>> messages, CancellationToken cancellationToken = default(CancellationToken));
    }
}