using System.Threading;
using System.Threading.Tasks;
using CSharp_Azure_API.Common;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using NUnit.Framework;

namespace CSharp_Azure_API.Tests
{
    [TestFixture]
    class NetworkInterface : TestBase
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
            string networkInterfaceName = SdkContext.RandomResourceName("netinf", 24);
            string ipLabel = SdkContext.RandomResourceName("rgRSMA", 12);

            ResourceGroups oResourceGroups = new ResourceGroups(this.credentials, this.subscriptionId);
            oResourceGroups.Create(rgName, this.region);

            var networkInferface = new NetworkInterfaces(credentials, subscriptionId);
            networkInferface.Create(networkInterfaceName, rgName, this.region, "10.0.0.0/28", "test");

            var resources = await oResourceGroups.GetResources(rgName);
            Assert.AreEqual(Formatter.CountPageable(resources), 2);

            await networkInferface.Del(networkInterfaceName, rgName, true);
            Thread.Sleep(20000);

            var resourcesAfterDel = await oResourceGroups.GetResources(rgName);

            // there is this thing called, smartDetectorAlertRules/Failure Anomalies
            Assert.LessOrEqual(Formatter.CountPageable(resourcesAfterDel), 1);

            oResourceGroups.Del(rgName);
        }
    }
}