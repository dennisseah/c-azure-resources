
using Microsoft.Azure.Management.Network.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;

namespace CSharp_Azure_API.Common
{
    /// <summary>
    /// Resource Groups API
    /// </summary>
    public class PublicIPAddresses : ResourceBase
    {
        /// Constructor
        /// </summary>
        /// <param name="credentials">Credential to authenticate to a subscription</param>
        /// <param name="subscriptionId">Subscription Id</param>
        public PublicIPAddresses(AzureCredentials credentials, string subscriptionId) : base(credentials, subscriptionId) { }

        /// <summary>
        /// Creates Public IP Address
        /// </summary>
        /// <param name="name">Name of the Public IP Address</param>
        /// <param name="resourceGroup">Name of the resource group</param>
        /// <param name="region">Region where Public IP Address resides</param>
        /// <returns>a reference to the newly created Public IP Address</returns>
        public IPublicIPAddress Create(string name, string resourceGroup, Region region)
        {
            return azure.PublicIPAddresses.Define(
                    name
                ).WithRegion(
                    region
                ).WithExistingResourceGroup(
                    resourceGroup
                ).WithDynamicIP().Create();
        }

        /// <summary>
        /// Returns fully qualified name.
        /// </summary>
        /// <param name="name">Name of the Public IP Address</param>
        /// <param name="resourceGroup">Name of the resource group</param>
        /// <returns>fully qualified name</returns>
        private string getId(string name, string resourceGroup)
        {
            return $"/subscriptions/{this.subscriptionId}/resourceGroups/{resourceGroup}/providers/Microsoft.Network/publicIPAddresses/{name}";
        }

        /// <summary>
        /// Deletes Public IP Address by Id.
        /// </summary>
        /// <param name="id">Identifier of Public IP Address</param>
        public void Del(string id)
        {
            azure.PublicIPAddresses.DeleteById(id);
        }


        /// <summary>
        /// Deletes Public IP Address by Id.
        /// </summary>
        /// <param name="name">Name of the Public IP Address</param>
        /// <param name="resourceGroup">Name of the resource group</param>
        public void Del(string name, string resourceGroup)
        {
            azure.PublicIPAddresses.DeleteById(this.getId(name, resourceGroup));
        }

        /// <summary>
        /// Returns a reference to Public IP Address by Id.
        /// </summary>
        /// <param name="id">Identifier of Public IP Address</param>
        /// <returns>a reference to Public IP Address by Id.</returns>
        public IPublicIPAddress Get(string id)
        {
            return azure.PublicIPAddresses.GetById(id);
        }

        /// <summary>
        /// Returns a reference to Public IP Address by Id.
        /// </summary>
        /// <param name="name">Name of the Public IP Address</param>
        /// <param name="resourceGroup">Name of the resource group</param>
        /// <returns>a reference to Public IP Address by Id.</returns>
        public IPublicIPAddress Get(string name, string resourceGroup)
        {
            return azure.PublicIPAddresses.GetById(this.getId(name, resourceGroup));
        }
    }
}