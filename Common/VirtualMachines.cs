using Microsoft.Azure.Management.Compute.Fluent;
using Microsoft.Azure.Management.Compute.Fluent.Models;
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
    public class VirtualMachines : ResourceBase
    {
        /// Constructor
        /// </summary>
        /// <param name="credentials">Credential to authenticate to a subscription</param>
        /// <param name="subscriptionId">Subscription Id</param>
        public VirtualMachines(AzureCredentials credentials, string subscriptionId) : base(credentials, subscriptionId) { }

        /// <summary>
        /// Creates a Virtual Machine
        /// </summary>
        /// <param name="name">name of Virtual Machine</param>
        /// <param name="resourceGroup">Name of Resource Group</param>
        /// <param name="region">Region where Virtual Machine resides</param>
        /// <param name="primaryNetwork">Primary Network</param>
        /// <param name="primaryPublicIPAddress">Primary Public IP Address</param>
        /// <param name="image">Image Type (Windows/Linux)</param>
        /// <param name="adminUserName">Administrator Name</param>
        /// <param name="adminUserPwd">Administrator Password</param>
        /// <param name="vmSize">Size of the Virtual Machine</param>
        /// <returns></returns>
        public IVirtualMachine CreateWindowsVM(
            string name,
            string resourceGroup,
            Region region,
            string primaryNetwork,
            string primaryPublicIPAddress,
            KnownWindowsVirtualMachineImage image,
            string adminUserName,
            string adminUserPwd,
            VirtualMachineSizeTypes vmSize
        )
        {
            return azure.VirtualMachines.Define(name)
                .WithRegion(region)
                .WithExistingResourceGroup(resourceGroup)
                .WithNewPrimaryNetwork(primaryNetwork)
                .WithPrimaryPrivateIPAddressDynamic()
                .WithNewPrimaryPublicIPAddress(primaryPublicIPAddress)
                .WithPopularWindowsImage(image)
                .WithAdminUsername(adminUserName)
                .WithAdminPassword(adminUserPwd)
                .WithSize(vmSize)
                .Create();
        }

        /// <summary>
        /// Returns fully qualified identifier of virtual machine.
        /// </summary>
        /// <param name="name">Name of virtual machine</param>
        /// <param name="resourceGroup">Name of the resource group.</param>
        /// <returns></returns>
        private string getId(string name, string resourceGroup)
        {
            return $"/subscriptions/{this.subscriptionId}/resourceGroups/{resourceGroup}/providers/Microsoft.Compute/virtualMachines/{name}";
        }

        /// <summary>
        /// Deletes virtual machine.
        /// </summary>
        /// <param name="name">Name of the virtual machine</param>
        /// <param name="resourceGroup">Name of the resource group.</param>
        /// <param name="deleteAll">true to remove all the resources associated with the virtual machine.</param>
        /// <returns></returns>
        public async Task Del(string name, string resourceGroup, bool deleteAll = false)
        {
            IVirtualMachine vmInstance = this.Get(name, resourceGroup);

            string osDiskId = vmInstance.OSDiskId;
            string priNetworkInterface = vmInstance.GetPrimaryNetworkInterface().Id;
            string publicIPAddress = vmInstance.GetPrimaryPublicIPAddressId();
            List<string> virtualNetworkNames = new List<string>();

            var networkInterface = new NetworkInterfaces(this.credentials, this.subscriptionId);
            INetworkInterface nwInterface = networkInterface.Get(priNetworkInterface);
            var ipConfigs = nwInterface.IPConfigurations;
            foreach (var k in ipConfigs.Keys)
            {
                virtualNetworkNames.Add(ipConfigs[k].GetNetwork().Name);
            }

            azure.VirtualMachines.DeleteById(this.getId(name, resourceGroup));

            if (deleteAll)
            {
                Disks disks = new Disks(this.credentials, this.subscriptionId);
                disks.Del(osDiskId);

                var oNetworkInterface = new NetworkInterfaces(this.credentials, this.subscriptionId);
                oNetworkInterface.Del(priNetworkInterface);

                var oPublicIPAddress = new PublicIPAddresses(this.credentials, this.subscriptionId);
                oPublicIPAddress.Del(publicIPAddress);

                var virtualNetworks = new VirtualNetworks(this.credentials, this.subscriptionId);

                foreach (var n in virtualNetworkNames)
                {
                    await virtualNetworks.Del(n, resourceGroup);
                }
            }
        }

        /// <summary>
        /// Returns a reference to a virtual machine.
        /// </summary>
        /// <param name="name">Name of the virtual machine.</param>
        /// <param name="resourceGroup">Name of the resource group.</param>
        /// <returns></returns>
        public IVirtualMachine Get(string name, string resourceGroup)
        {
            return azure.VirtualMachines.GetById(this.getId(name, resourceGroup));
        }
    }
}