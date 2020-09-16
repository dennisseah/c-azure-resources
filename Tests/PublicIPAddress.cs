using System.Collections.Generic;
using System.Threading.Tasks;
using CSharp_Azure_API.Common;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using NUnit.Framework;

namespace CSharp_Azure_API.Tests
{
    [TestFixture]
    class PublicIPAddress : TestBase
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
            string publicIPAddrName = SdkContext.RandomResourceName("pubIP", 24);

            ResourceGroups oResourceGroups = new ResourceGroups(this.credentials, this.subscriptionId);
            oResourceGroups.Create(rgName, this.region);

            var publicIPAddr = new PublicIPAddresses(credentials, subscriptionId);
            publicIPAddr.Create(publicIPAddrName, rgName, this.region);

            var resources = await oResourceGroups.GetResources(rgName);
            Assert.AreEqual(Formatter.CountPageable(resources), 1);

            publicIPAddr.Del(publicIPAddrName, rgName);

            var resourcesAfterDel = await oResourceGroups.GetResources(rgName);

            // there is this thing called, smartDetectorAlertRules/Failure Anomalies
            Assert.LessOrEqual(Formatter.CountPageable(resourcesAfterDel), 1);

            oResourceGroups.Del(rgName);
        }
    }
}