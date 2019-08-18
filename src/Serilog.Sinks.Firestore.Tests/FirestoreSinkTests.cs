using NSubstitute;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.Firestore.Interfaces;
using Serilog.Sinks.Firestore.Models;
using Serilog.Sinks.TestCorrelator;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Serilog.Sinks.Firestore.Tests
{
    public class FirestoreSinkTests : IDisposable
    {
        private readonly IFormatProvider _subFormatProvider;
        private readonly IFirestoreApiClient _subFirestoreApiClient;
        private readonly Logger _testLogger;
        private readonly ITestCorrelatorContext _testContext;

        public FirestoreSinkTests()
        {
            _subFormatProvider = Substitute.For<IFormatProvider>();
            _subFirestoreApiClient = Substitute.For<IFirestoreApiClient>();
            _testLogger = new LoggerConfiguration().WriteTo.TestCorrelator().CreateLogger();
            _testContext = TestCorrelator.TestCorrelator.CreateContext();
        }

        private FirestoreSink CreateFirestoreSink()
        {
            return new FirestoreSink(_subFormatProvider, _subFirestoreApiClient, TimeSpan.FromSeconds(1), 1);
        }

        [Fact]
        public void ConvertLog_WithFlatLogProperty_ReturnsDictionary()
        {
            // Arrange
            var config = new FirestoreConfiguration("project", "logs");
            _testLogger.Error("messageTemplate");

            // Act
            var formatted = CreateFirestoreSink().ConvertLog(GetLastLogEvent());

            // Assert
            Assert.NotNull(formatted);
        }


        [Fact]
        public void ConvertLog_WithNestedLogProperty_ReturnsDictionaryContainingExpectedValue()
        {
            // Arrange
            var config = new FirestoreConfiguration("project", "logs");
            var nestedId = Guid.NewGuid();
            var nestedObject = new { Name = "test", Nested = new { Bytes = new byte[8], Output = uint.MaxValue, Nested = new { Nested = new { Id = nestedId } }  } };
            _testLogger.Error("Processed {@nestedObject}", nestedObject);

            // Act
            var formatted = CreateFirestoreSink().ConvertLog(GetLastLogEvent());
            var propertiesDictionary = formatted["Properties"] as IDictionary<string, string>;

            // Assert
            Assert.Contains(nestedId.ToString(), propertiesDictionary["nestedObject"].ToString());
        }

        [Fact]
        public void ConvertLog_WithNullLogProperty_ReturnsDictionary()
        {
            // Arrange
            var config = new FirestoreConfiguration("project", "logs");
            _testLogger.Error("Processed {@nestedObject}", null);

            // Act
            var formatted = CreateFirestoreSink().ConvertLog(GetLastLogEvent());

            // Assert
            Assert.NotNull(formatted);
        }

        private LogEvent GetLastLogEvent()
        {
            return TestCorrelator.TestCorrelator.GetLogEventsFromContextGuid(_testContext.Guid).First();
        }

        public void Dispose()
        {
            _testContext.Dispose();
        }
    }
}
