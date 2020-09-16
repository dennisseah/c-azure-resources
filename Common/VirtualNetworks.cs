
using Microsoft.Azure.Management.Network.Fluent;
using Microsoft.Azure.Management.Network.Fluent.Models;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSharp_Azure_API.Common
{
    /// <summary>
    /// Resource Groups API
    /// </summary>
    public class VirtualNetworks : ResourceBase
    {
        private NetworkManagementClient client;

        /// Constructor
        /// </summary>
        /// <param name="credentials">Credential to authenticate to a subscription</param>
        /// <param name="subscriptionId">Subscription Id</param>
        public VirtualNetworks(AzureCredentials credentials, string subscriptionId) : base(credentials, subscriptionId)
        {
            var restClient = RestClient.Configure()
                    .WithEnvironment(AzureEnvironment.AzureGlobalCloud)
                    .WithCredentials(this.credentials)
                    .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                    .Build();
            this.client = new NetworkManagementClient(restClient);
            this.client.SubscriptionId = subscriptionId;
        }

        /// <summary>
        /// Create a virtual network.
        /// </summary>
        /// <param name="name">Name of the virtual network.</param>
        /// <param name="resourceGroup">Name of the resource group.</param>
        /// <returns>a async Task</returns>
        public async Task<VirtualNetworkInner> Create(
            string name,
            string resourceGroup,
            List<string> ipAddresses,
            List<string> dnsServers)
        {
            VirtualNetworkInner vnet = new VirtualNetworkInner()
            {
                Location = Region.USWest.ToString(),
                AddressSpace = new AddressSpace()
                {
                    AddressPrefixes = ipAddresses
                },
                DhcpOptions = new DhcpOptions()
                {
                    DnsServers = dnsServers
                }
            };

            return await this.client.VirtualNetworks.CreateOrUpdateAsync(resourceGroup, name, vnet);
        }

        /// <summary>
        /// Deletes a virtual network.
        /// </summary>
        /// <param name="name">Name of the virtual network.</param>
        /// <param name="resourceGroup">Name of the resource group.</param>
        /// <returns>a async Task</returns>
        public async Task Del(string name, string resourceGroup)
        {
            await this.client.VirtualNetworks.DeleteAsync(resourceGroup, name);
        }

        /// <summary>
        /// Returns a referene to virtual network.
        /// </summary>
        /// <param name="name">Name of the virtual network.</param>
        /// <param name="resourceGroup">Name of the resource group.</param>
        /// <returns>a referene to virtual network</returns>
        public async Task<VirtualNetworkInner> Get(string name, string resourceGroup)
        {
            return await this.client.VirtualNetworks.GetAsync(resourceGroup, name);
        }
    }
}