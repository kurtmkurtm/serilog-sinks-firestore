using Serilog;
using Serilog.Sinks;
using Shared;
using System;

namespace TestConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var log = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Firestore(Settings.ProjectId, Settings.CollectionName)
                    .CreateLogger();


            var position = new { Latitude = 25, Longitude = 134, Nested = new { Bytes = new byte[8], Output = uint.MaxValue } };
            var exception = new InvalidProgramException();
            var elapsedMs = 34;


            while (true)
            {
                Console.WriteLine("Writing log item");
                log.Information("Processed {@Position} in {Elapsed:000} ms.", position, elapsedMs);
                log.Error(exception, "Invalid Program Exception");
                if (Console.ReadKey(true).Key == ConsoleKey.Escape) break;
                Console.WriteLine("Press any key to send again, or ESC to exit");
            }
        }
    }
}
