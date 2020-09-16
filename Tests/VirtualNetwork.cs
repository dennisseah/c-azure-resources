using System.Collections.Generic;
using System.Threading.Tasks;
using CSharp_Azure_API.Common;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using NUnit.Framework;

namespace CSharp_Azure_API.Tests
{
    [TestFixture]
    class VirtualNetwork : TestBase
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
            string virtualNetworkName = SdkContext.RandomResourceName("vnet", 24);

            ResourceGroups oResourceGroups = new ResourceGroups(this.credentials, this.subscriptionId);
            oResourceGroups.Create(rgName, this.region);

            var virtualNetwork = new VirtualNetworks(credentials, subscriptionId);
            await virtualNetwork.Create(virtualNetworkName, rgName, new List<string>() {
                "10.0.0.0/28"
            }, new List<string>() { "1.1.1.1", "1.1.2.4" });

            var resources = await oResourceGroups.GetResources(rgName);
            Assert.AreEqual(Formatter.CountPageable(resources), 1);

            await virtualNetwork.Del(virtualNetworkName, rgName);

            var resourcesAfterDel = await oResourceGroups.GetResources(rgName);

            Assert.LessOrEqual(Formatter.CountPageable(resourcesAfterDel), 0);

            oResourceGroups.Del(rgName);
        }
    }
}