using NSubstitute;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.Firestore.Enums;
using Serilog.Sinks.Firestore.Models;
using System;
using Xunit;

namespace Serilog.Sinks.Firestore.Tests.Extensions
{
    public class FirestoreSinkExtensionsTests
    { 
        [Theory]
        [Trait(Trait.Category,Category.IntegrationTest)]
        [InlineData(CredentialType.Default, "project", "logs", null, 1, 1)]
        [InlineData(CredentialType.Default, "project", "logs", null, 500, 500)]
        [InlineData(CredentialType.Default, "project", "logs", null, 100, 100)]
        public void Firestore_LoggerConfigurationWithValidSettings_ReturnsConfig(CredentialType credentialType, string projectId, string collectionName, string credentialValue, int batchSize, int intervalSeconds)
        {
            // Arrange
            var loggerConfiguration = new LoggerConfiguration();
            var firestoreConfiguration = new FirestoreConfiguration(projectId, collectionName, credentialType, credentialValue);

            // Act
            var result = FirestoreSinkExtensions.Firestore(loggerConfiguration.WriteTo, firestoreConfiguration, batchIntervalInSeconds: intervalSeconds, batchSizeLimit: batchSize);

            // Assert
            Assert.NotNull(result);
        }

        [Theory]
        [Trait(Trait.Category, Category.IntegrationTest)]
        [InlineData(CredentialType.Default, "project", "logs", null, -1, 10)]
        [InlineData(CredentialType.Default, "project", "logs", null, 0, 10)]
        [InlineData(CredentialType.Default, "project", "logs", null, 501, 10)]
        [InlineData(CredentialType.Default, "project", "logs", null, 100, -1)]
        [InlineData(CredentialType.Default, "project", "logs", null, 100, 0)]
        public void Firestore_LoggerConfigurationWithInvalidSettings_Throws(CredentialType credentialType, string projectId, string collectionName, string credentialValue, int batchSize, int intervalSeconds)
        {
            // Arrange
            var loggerConfiguration = new LoggerConfiguration();
            var firestoreConfiguration = new FirestoreConfiguration(projectId, collectionName, credentialType, credentialValue);

            // Act Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => FirestoreSinkExtensions.Firestore(loggerConfiguration.WriteTo, firestoreConfiguration, batchIntervalInSeconds: intervalSeconds, batchSizeLimit: batchSize));
        }
    }
}
