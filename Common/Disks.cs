
using Microsoft.Azure.Management.Compute.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using System.Threading;

namespace CSharp_Azure_API.Common
{
    /// <summary>
    /// Resource Groups API
    /// </summary>
    public class Disks : ResourceBase
    {
        /// Constructor
        /// </summary>
        /// <param name="credentials">Credential to authenticate to a subscription</param>
        /// <param name="subscriptionId">Subscription Id</param>
        public Disks(AzureCredentials credentials, string subscriptionId) : base(credentials, subscriptionId) { }

        /// <summary>
        /// Returns the fully qualified name.
        /// </summary>
        /// <param name="name">Name of the disk</param>
        /// <param name="resourceGroup">Name of resource group</param>
        /// <returns>fully qualified name.</returns>
        private string getId(string name, string resourceGroup)
        {
            return $"/subscriptions/{this.subscriptionId}/resourceGroups/{resourceGroup}/providers/Microsoft.Compute/disks/{name}";
        }

        /// <summary>
        /// Creates a disk
        /// </summary>
        /// <param name="name">Name of the disk</param>
        /// <param name="resourceGroup">Name of resource group</param>
        /// <param name="region">Region where the disk resides</param>
        /// <param name="gbytes">Disk size in giga bytes</param>
        /// <returns></returns>
        public IDisk Create(
            string name,
            string resourceGroup,
            Region region,
            int gbytes
        )
        {
            IDisk d = azure.Disks.Define(name).WithRegion(region).WithExistingResourceGroup(resourceGroup).WithData().WithSizeInGB(gbytes).Create();
            Thread.Sleep(60000);
            return d;
        }

        /// <summary>
        /// Deletes a Disk
        /// </summary>
        /// <param name="id">Identifier of a Disk</param>
        public void Del(string id)
        {
            azure.Disks.DeleteById(id);
        }


        /// <summary>
        /// Deletes a Disk
        /// </summary>
        /// <param name="name">Name of the disk</param>
        /// <param name="resourceGroup">Name of resource group</param>
        public void Del(string name, string resourceGroup)
        {
            azure.Disks.DeleteById(this.getId(name, resourceGroup));
        }

        /// <summary>
        /// Returns a reference a disk
        /// </summary>
        /// <param name="id">Identifier of a Disk</param>
        /// <returns>a reference of a disk</returns>
        public IDisk Get(string id)
        {
            return azure.Disks.GetById(id);
        }

        /// <summary>
        /// Returns a reference a disk
        /// </summary>
        /// <param name="name">Name of the disk</param>
        /// <param name="resourceGroup">Name of resource group</param>
        /// <returns>a reference of a disk</returns>
        public IDisk Get(string name, string resourceGroup)
        {
            return azure.Disks.GetById(this.getId(name, resourceGroup));
        }
    }
}