using Google.Cloud.Firestore;
using Shared;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestConsoleReader
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var firestoreDatabase = FirestoreDb.Create(Settings.ProjectId);
            var collectionQuery = firestoreDatabase.Collection(Settings.CollectionName).OrderByDescending("Timestamp").Limit(5);

            collectionQuery.Listen(WriteToConsole);

            Console.ReadKey();
        }

        private static Task WriteToConsole(QuerySnapshot snapshot, CancellationToken cancellationToken)
        {
            foreach (var latestDocument in snapshot.Documents)
            {
                var content = latestDocument.TryGetValue<string>("Message", out var message) ? message : "could not find";
                var time = latestDocument.TryGetValue<DateTimeOffset>("Timestamp", out var dateTime) ? dateTime.ToLocalTime().ToString() : "could not find";

                Console.WriteLine($"{time}-{latestDocument.Id}-{content}");
            }
            return Task.CompletedTask;
        }
    }
}
