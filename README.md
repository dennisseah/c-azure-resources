# Sample Code to for Azure in C#

## Setup

Create a file, `config.properties` with this content

```
subscription=<subscription id>
client=<service-principal-id>
tenant=<tenant-id>
key=<service-principal-secret>
managementURI=https\://management.core.windows.net/
baseURL=https\://management.azure.com/
authURL=https\://login.microsoftonline.com/
graphURL=https\://graph.windows.net/
```

export

```
AZURE_AUTH_LOCATION=path where the config.properties is located
SUBSCRIPTION_ID=subscription id
```
