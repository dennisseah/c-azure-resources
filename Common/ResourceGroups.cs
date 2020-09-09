using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.Management.ResourceManager.Fluent.Models;
using Microsoft.Rest.Azure;

namespace C_azure_resources.Common
{
    public class ResourceGroups
    {
        private AzureCredentials credentials;
        private string subscriptionId;
        private RestClient restClient;

        private IAzure azure;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="credentials">Credential to authenticate to a subscription</param>
        /// <param name="subscriptionId">Subscription Id</param>
        public ResourceGroups(AzureCredentials credentials, string subscriptionId)
        {
            this.credentials = credentials;
            this.subscriptionId = subscriptionId;

            this.azure = Microsoft.Azure.Management.Fluent.Azure
                .Configure()
                .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                .Authenticate(credentials)
                .WithDefaultSubscription();
        }

        /// <summary>
        /// Create a new resource group
        /// </summary>
        /// <param name="name">Name of Resource Group.</param>
        /// <param name="region">Region where resource group should be created.</param>
        /// <returns>Resource Group</returns>
        public IResourceGroup Create(string name, Region region)
        {
            if (this.IsExist(name))
            {
                return this.azure.ResourceGroups.GetByName(name);
            }
            return this.azure.ResourceGroups
                    .Define(name)
                    .WithRegion(region)
                    .Create();
        }

        /// <summary>
        /// Returns true if resource group exists.
        /// </summary>
        /// <param name="name">Name of Resource Group.</param>
        /// <returns>true if resource group exists</returns>
        public bool IsExist(string name)
        {
            return this.azure.ResourceGroups.Contain(name);
        }

        /// <summary>
        /// Delete resource group. Caution, this may take a while.
        /// </summary>
        /// <param name="name">Name of Resource Group.</param>
        public void Del(string name)
        {
            this.azure.ResourceGroups.DeleteByName(name);
        }

        /// <summary>
        /// Returns an enum of IResourceGroup
        /// </summary>
        /// <returns>an enum of IResourceGroup</returns>
        public IEnumerable<IResourceGroup> List()
        {
            return this.azure.ResourceGroups.List();
        }

        /// <summary>
        /// Return a list of resources in a resource group
        /// </summary>
        /// <param name="name">Name of resource group</param>
        /// <returns>a list of resources in a resource group</returns>
        public async Task<IPage<GenericResourceInner>> GetResources(string name)
        {
            if (restClient == null)
            {
                restClient = RestClient.Configure()
                    .WithEnvironment(AzureEnvironment.AzureGlobalCloud)
                    .WithCredentials(this.credentials)
                    .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                    .Build();
            }
            ResourceManagementClient client = new ResourceManagementClient(restClient);
            client.SubscriptionId = this.subscriptionId;
            return await client.Resources.ListByResourceGroupAsync(name);
        }
    }
}