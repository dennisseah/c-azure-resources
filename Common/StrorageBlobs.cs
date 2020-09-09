using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.Storage.Fluent;
using Microsoft.Azure.Management.Storage.Fluent.Models;
using System;

namespace C_azure_resources.Common
{
    public class StorageBlobs
    {
        public static string KIND = "BlobStorage";

        private string subscriptionId;
        private IAzure azure;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="credentials">Credentials for the subscription</param>
        /// <param name="subscriptionId">Subscription Id</param>
        public StorageBlobs(AzureCredentials credentials, string subscriptionId)
        {
            this.subscriptionId = subscriptionId;
            this.azure = Microsoft.Azure.Management.Fluent.Azure
                .Configure()
                .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                .Authenticate(credentials)
                .WithDefaultSubscription();
        }

        /// <summary>
        /// Returns the unique identifier of a storage account.
        /// </summary>
        /// <param name="resourceGroup">name of the resource group</param>
        /// <param name="name">name of the storage account</param>
        /// <returns>the unique identifier of a storage account</returns>
        private string GetAccountId(string resourceGroup, string name)
        {
            return $"/subscriptions/{this.subscriptionId}/resourceGroups/{resourceGroup}/providers/Microsoft.Storage/storageAccounts/{name}";
        }

        /// <summary>
        /// Creates a new Blob Storage Account
        /// </summary>
        /// <param name="name">Name of the new account</param>
        /// <param name="region">Region where it resides</param>
        /// <param name="resourceGroup">name of resource group</param>
        /// <returns>Storage Account object</returns>
        public IStorageAccount Create(string name, Region region, string resourceGroup)
        {
            this.azure.StorageAccounts
                .Define(name)
                .WithRegion(region)
                .WithExistingResourceGroup(resourceGroup)
                .WithBlobEncryption()
                .WithOnlyHttpsTraffic()
                .WithBlobStorageAccountKind()
                .WithAccessTier(AccessTier.Hot)
                .Create();
            return this.GetAccount(resourceGroup, name);
        }

        /// <summary>
        /// Returns a reference to a storage account
        /// </summary>
        /// <param name="resourceGroup">name of the resource group</param>
        /// <param name="name">name of account name</param>
        /// <returns></returns>
        public IStorageAccount GetAccount(string resourceGroup, string name)
        {
            return this.azure.StorageAccounts.GetById(this.GetAccountId(resourceGroup, name));
        }

        public bool IsExist(string resourceGroup, string name)
        {
            try
            {
                return this.GetAccount(resourceGroup, name) != null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}