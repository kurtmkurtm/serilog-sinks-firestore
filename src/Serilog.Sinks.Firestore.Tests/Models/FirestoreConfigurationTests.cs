using Serilog.Sinks.Firestore.Enums;
using Serilog.Sinks.Firestore.Models;
using System;
using Xunit;

namespace Serilog.Sinks.Firestore.Tests.Models
{
    public class FirestoreConfigurationTests
    {
        [Theory]
        [InlineData(CredentialType.Default, null, "logs", null)]
        [InlineData(CredentialType.Default, "project", null, null)]
        [InlineData(CredentialType.CredentialsPath, "project", "logs", null)]
        [InlineData(CredentialType.JsonCredentials, "project", "logs", null)]
        public void CreateFirestoreConfiguration_MissingValues_Throws(CredentialType credentialType, string projectId, string collectionName, string credentialValue)
        {
            // Act
            Func<FirestoreConfiguration> firestoreApiClient = () => new FirestoreConfiguration(projectId, collectionName, credentialType, credentialValue);

            // Assert
            Assert.Throws<ArgumentNullException>(firestoreApiClient);
        }

        [Theory]
        [InlineData(CredentialType.JsonCredentials, "project", "logs", "{ \"creds\" : \"value\"}")]
        [InlineData(CredentialType.CredentialsPath, "project", "logs", @"C:\mycreds.json")]
        [InlineData(CredentialType.Default, "project", "logs", null)]
        public void CreateFirestoreConfiguration_ValidValues_CreatesConfig(CredentialType credentialType, string projectId, string collectionName, string credentialValue)
        {
            // Act
            var firestoreApiClient = new FirestoreConfiguration(projectId, collectionName, credentialType, credentialValue);

            // Assert
            Assert.IsAssignableFrom<FirestoreConfiguration>(firestoreApiClient);
        }
    }
}
