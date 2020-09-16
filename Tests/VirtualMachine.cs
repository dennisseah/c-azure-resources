using System.Threading.Tasks;
using CSharp_Azure_API.Common;
using Microsoft.Azure.Management.Compute.Fluent;
using Microsoft.Azure.Management.Compute.Fluent.Models;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using NUnit.Framework;

namespace CSharp_Azure_API.Tests
{
    [TestFixture]
    class VirtualMachine : TestBase
    {
        [OneTimeSetUp]
        new public void init()
        {
            base.init();
        }

        [TestCase]
        public async Task MainTest()
        {
            string rgName = SdkContext.RandomResourceName("rgRSMA", 24);
            string vmName = SdkContext.RandomResourceName("rgRSMA", 24);
            string ipLabel = SdkContext.RandomResourceName("rgRSMA", 12);

            ResourceGroups oResourceGroups = new ResourceGroups(this.credentials, this.subscriptionId);
            oResourceGroups.Create(rgName, this.region);

            var vm = new VirtualMachines(credentials, subscriptionId);
            vm.CreateWindowsVM(vmName, rgName, this.region, "10.0.0.0/28", ipLabel, KnownWindowsVirtualMachineImage.WindowsServer2008R2_SP1, "adminUser", "Password@123", VirtualMachineSizeTypes.StandardA0);

            var resources = await oResourceGroups.GetResources(rgName);
            Assert.AreEqual(Formatter.CountPageable(resources), 5);

            await vm.Del(vmName, rgName, true);

            var resourcesAfterDel = await oResourceGroups.GetResources(rgName);
            Assert.AreEqual(Formatter.CountPageable(resourcesAfterDel), 0);

            oResourceGroups.Del(rgName);
        }
    }
}