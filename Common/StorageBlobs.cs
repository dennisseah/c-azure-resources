using System;
using System.Threading;

using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;

using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.Management.Storage.Fluent;
using Microsoft.Azure.Management.Storage.Fluent.Models;

namespace CSharp_Azure_API.Common
{
    /// <summary>
    /// Azure Blob Storage API
    /// </summary>
    public class StorageBlobs : ResourceBase
    {
        public static string KIND = "BlobStorage";

        private string resourceGroup;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="credentials">Credentials for the subscription</param>
        /// <param name="subscriptionId">Subscription Id</param>
        /// <param name="resourceGroup">Name of resource group</param>
        public StorageBlobs(AzureCredentials credentials, string subscriptionId, string resourceGroup) : base(credentials, subscriptionId)
        {
            this.resourceGroup = resourceGroup;
        }

        private string getPrimaryStorageKey(string name)
        {
            IStorageAccount acct = this.GetAccount(name);
            return acct.GetKeys()[0].Value;
        }

        /// <summary>
        /// Returns the unique identifier of a storage account.
        /// </summary>
        /// <param name="name">name of the storage account</param>
        /// <returns>the unique identifier of a storage account</returns>
        private string GetAccountId(string name)
        {
            return $"/subscriptions/{this.subscriptionId}/resourceGroups/{this.resourceGroup}/providers/Microsoft.Storage/storageAccounts/{name}";
        }

        /// <summary>
        /// Creates a new Blob Storage Account
        /// </summary>
        /// <param name="name">Name of the new account</param>
        /// <param name="region">Region where it resides</param>
        /// <returns>Storage Account object</returns>
        public IStorageAccount Create(string name, Region region)
        {
            this.azure.StorageAccounts
                .Define(name)
                .WithRegion(region)
                .WithExistingResourceGroup(this.resourceGroup)
                .WithBlobEncryption()
                .WithOnlyHttpsTraffic()
                .WithBlobStorageAccountKind()
                .WithAccessTier(AccessTier.Hot)
                .Create();
            Thread.Sleep(30000); // There is a delay in blog account creation
            return this.GetAccount(name);
        }

        /// <summary>
        /// Delete a blog storage account.
        /// </summary>
        /// <param name="name">Name of the new account</param>
        public void Del(string name)
        {
            this.azure.StorageAccounts.DeleteById(this.GetAccountId(name));
            Thread.Sleep(30000); // There is a delay in blog account creation
        }

        /// <summary>
        /// Returns a reference to a storage account
        /// </summary>
        /// <param name="name">name of account name</param>
        /// <returns></returns>
        public IStorageAccount GetAccount(string name)
        {
            return this.azure.StorageAccounts.GetById(this.GetAccountId(name));
        }

        /// <summary>
        /// Returns true if blob storage account exists.
        /// </summary>
        /// <param name="name">Name of blob storage account</param>
        /// <returns>true if blob storage account exists.</returns>
        public bool IsExist(string name)
        {
            try
            {
                return this.GetAccount(name) != null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        private BlobServiceClient getBlobServiceClient(string name)
        {
            string key = this.getPrimaryStorageKey(name);
            StorageSharedKeyCredential storageCredentials = new StorageSharedKeyCredential(name, key);
            return new BlobServiceClient(new Uri($"https://{name}.blob.core.windows.net"), storageCredentials);
        }

        /// <summary>
        /// Creates blob container.
        /// </summary>
        /// <param name="name">Name of Blob Storage Account</param>
        /// <param name="containerName">Name of new container</param>
        public void CreateBlobContainer(string name, string containerName)
        {
            var oBlobServiceClient = this.getBlobServiceClient(name);
            oBlobServiceClient.CreateBlobContainer(containerName);
        }

        /// <summary>
        /// Deletes Blob Container.
        /// </summary>
        /// <param name="name">Name of Blob Storage Account</param>
        /// <param name="containerName">Name of Blob Container</param>
        public void DelBlobContainer(string name, string containerName)
        {
            var oBlobServiceClient = this.getBlobServiceClient(name);
            oBlobServiceClient.DeleteBlobContainer(containerName);
        }

        /// <summary>
        /// Returns an enumeration of blob containers
        /// </summary>
        /// <param name="name">Name of Blob Storage Account</param>
        /// <returns>an enumeration of blob containers</returns>
        public Pageable<Azure.Storage.Blobs.Models.BlobContainerItem> GetContainers(string name)
        {
            var oBlobServiceClient = this.getBlobServiceClient(name);
            return oBlobServiceClient.GetBlobContainers();
        }
    }
}