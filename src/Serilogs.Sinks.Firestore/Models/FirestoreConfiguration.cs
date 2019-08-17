using Serilog.Sinks.Firestore.Enums;
using System;

namespace Serilog.Sinks.Firestore.Models
{
    /// <summary>
    /// Firestore specific configuration
    /// </summary>
    public class FirestoreConfiguration
    {
        /// <summary>
        /// Initialises a new instance of <see cref="FirestoreConfiguration"/> 
        /// </summary>
        /// <param name="projectId">ID of the Firestore project to use</param>
        /// <param name="collectionName">Name of the Firestore collection to use, will be created if one does not exist</param>
        /// <param name="credentialType">Type of credentials to use</param>
        /// <param name="credentialValue">Credential value, either path to credentials or JSON string</param>
        public FirestoreConfiguration(string projectId, string collectionName, CredentialType credentialType = CredentialType.Default, string credentialValue = null)
        {
            CredentialValue = (credentialType == CredentialType.Default || credentialValue != null) ? credentialValue : throw new ArgumentNullException(nameof(credentialValue)); // Value not required for default because it is read from the env var
            CredentialType = credentialType;
            ProjectId = (!string.IsNullOrEmpty(projectId)) ? projectId : throw new ArgumentNullException(nameof(projectId));
            CollectionName = (!string.IsNullOrEmpty(collectionName)) ? collectionName : throw new ArgumentNullException(nameof(collectionName));
        }

        /// <summary>
        /// Type of credentials to use
        /// </summary>
        public CredentialType CredentialType { get; }

        /// <summary>
        /// Credential value, either path to credentials or JSON string
        /// </summary>
        public string CredentialValue { get; }

        /// <summary>
        /// ID of the Firestore project to use
        /// </summary>
        public string ProjectId { get; }

        /// <summary>
        /// Name of the Firestore collection to use, will be created if one does not exist
        /// </summary>        
        public string CollectionName { get; }
    }
}
