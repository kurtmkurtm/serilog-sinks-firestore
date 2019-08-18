# Serilog.Sinks.Firestore

Serilog.Sinks.Firestore is a Serilog sink that writes logs to a Google Cloud Firestore document database.

[![Build Status](https://dev.azure.com/kurt0233/Serilog.Sinks.Firestore/_apis/build/status/kurtmkurtm.serilog-sinks-firestore?branchName=master)](https://dev.azure.com/kurt0233/Serilog.Sinks.Firestore/_build/latest?definitionId=3&branchName=master) [![Nuget](https://img.shields.io/nuget/v/Serilog.Sinks.Firestore.svg)](https://www.nuget.org/packages/Serilog.Sinks.Firestore/)

<img src="https://firebase.google.com/downloads/brand-guidelines/PNG/logo-built_white.png" width="256"> 

## Installation

Using the dotnet cli:

```bash
dotnet add package Serilog.Sinks.Firestore
```

## Usage

To use with default settings, add the Firestore Sink using one of the following methods:

```c#
var project = "myProject";
var collection = "myLogs";

var log = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Firestore(project, collection)
    .CreateLogger();

// or

var firestoreConfiguration = new FirestoreConfiguration(project, collection);

var log = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Firestore(firestoreConfiguration)
    .CreateLogger();
```



#### Credential types

By default if no credentials are specified the default environment variables credentials for Google Cloud in the following structure`GOOGLE_APPLICATION_CREDENTIALS ` -  `path/credentials.json`

In order to explicitly set other credentials, the optional parameters of both sink configurations provide a `credentialType` and `credentialValue` parameter. 

##### Examples

**JsonCredentials** - a string (in JSON format) containing service account credentials

```c#
var firestoreApiClient = new FirestoreConfiguration(CredentialType.JsonCredentials, "project", "logs", "{ \"creds\" : \"value\"}");
```

**CredentialsPath** - a path to a JSON file containing service account credentials

```c#
var firestoreApiClient = new FirestoreConfiguration(CredentialType.CredentialsPath, "project", "logs", @"C:\creds.json");
```

**Default** - null/not used (Credentials loaded from Environment Variable)

```c#
var firestoreConfiguration = new FirestoreConfiguration("project", "logs");
```



#### Batching

This sink utilises `PeriodicBatchingSink` to write logs in batches to Firestore, because batch jobs have a limit of 500 operations, this limit is enforced in the configuration. 

## Contributing

If you find a bug or have a feature request, please report them in this repository's issues section. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License
Serilog.Sinks.Firestore is under [MIT](https://choosealicense.com/licenses/mit/) License.

This library utilises the following libraries under the Apache 2.0 license, which can be obtained from http://www.apache.org/licenses/LICENSE-2.0.

- [Google.Cloud.Firestore](https://github.com/googleapis/google-cloud-dotnet/blob/master/LICENSE)
- [Serilog](https://github.com/serilog/serilog/blob/dev/LICENSE)
- [Serilog.Formatting.Compact](https://github.com/serilog/serilog-formatting-compact/blob/dev/LICENSE)
- [Serilog.Sinks.PeriodicBatching](https://github.com/serilog/serilog-sinks-periodicbatching/blob/dev/LICENSE)