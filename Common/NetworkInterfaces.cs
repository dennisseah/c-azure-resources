
using Microsoft.Azure.Management.Network.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSharp_Azure_API.Common
{
    /// <summary>
    /// Resource Groups API
    /// </summary>
    public class NetworkInterfaces : ResourceBase
    {
        /// Constructor
        /// </summary>
        /// <param name="credentials">Credential to authenticate to a subscription</param>
        /// <param name="subscriptionId">Subscription Id</param>
        public NetworkInterfaces(AzureCredentials credentials, string subscriptionId) : base(credentials, subscriptionId) { }

        public INetworkInterface Create(
            string name,
            string resourceGroup,
            Region region,
            string primaryNetwork,
            string subnet)
        {
            return azure.NetworkInterfaces.Define(name)
                        .WithRegion(region)
                        .WithExistingResourceGroup(resourceGroup)
                        .WithNewPrimaryNetwork(primaryNetwork)
                        .WithPrimaryPrivateIPAddressDynamic()
                        .WithIPForwarding()
                        .Create();
        }

        /// <summary>
        /// Returns the fully qualified identifier.
        /// </summary>
        /// <param name="name">Name of Network Interface</param>
        /// <param name="resourceGroup">Name of Resource Group</param>
        /// <returns>fully qualified identifier</returns>
        private string GetId(string name, string resourceGroup)
        {
            return $"/subscriptions/{this.subscriptionId}/resourceGroups/{resourceGroup}/providers/Microsoft.Network/networkInterfaces/{name}";
        }


        /// <summary>
        /// Deletes a Network Interface
        /// </summary>
        /// <param name="name">Name of the network interface</param>
        /// <param name="resourceGroup">Name of the resource group</param>
        /// <param name="deleteAll">true to remove all the resources associated with the network interface.</param>
        public async Task Del(string name, string resourceGroup, bool deleteAll = false)
        {
            INetworkInterface reference = this.Get(name, resourceGroup);
            List<string> virtualNetworkNames = new List<string>();
            var ipconfig = reference.IPConfigurations;
            foreach (var k in ipconfig.Keys)
            {
                virtualNetworkNames.Add(ipconfig[k].GetNetwork().Name);
            }

            azure.NetworkInterfaces.DeleteById(this.GetId(name, resourceGroup));

            if (deleteAll)
            {
                var virtualNetworks = new VirtualNetworks(this.credentials, this.subscriptionId);
                foreach (var n in virtualNetworkNames)
                {
                    await virtualNetworks.Del(n, resourceGroup);
                }
            }
        }

        /// <summary>
        /// Deletes a Network Interface
        /// </summary>
        /// <param name="id">Identifier of the network interface</param>
        public void Del(string id)
        {
            azure.NetworkInterfaces.DeleteById(id);
        }

        /// <summary>
        /// Returns a reference to Network Interface.
        /// </summary>
        /// <param name="id">Identifier of the network interface</param>
        /// <param name="resourceGroup">Name of the resource group</param>
        /// <returns>a reference to Network Interface.</returns>
        public INetworkInterface Get(string name, string resourceGroup)
        {
            return azure.NetworkInterfaces.GetById(this.GetId(name, resourceGroup));
        }

        /// <summary>
        /// Returns a reference to Network Interface.
        /// </summary>
        /// <param name="id">Identifier of the network interface</param>
        /// <returns>a reference to Network Interface.</returns>
        public INetworkInterface Get(string id)
        {
            return azure.NetworkInterfaces.GetById(id);
        }
    }
}