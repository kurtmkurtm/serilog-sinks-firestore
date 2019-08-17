namespace Serilog.Sinks.Firestore.Enums
{
    /// <summary>
    /// Credentials to use to connect to a Firestore database
    /// </summary>
    public enum CredentialType
    {
        /// <summary>
        /// Default: credentials from Environment Variable
        /// </summary>
        Default = 0,
        /// <summary>
        /// CredentialsPath: the path to a JSON file containing service account credentials
        /// </summary>
        CredentialsPath = 1,
        /// <summary>
        /// JsonCredentials: a string (in JSON format) containing service account credentials
        /// </summary>
        JsonCredentials = 2
    }
}
