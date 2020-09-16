using System.Collections.Generic;
using System.Threading.Tasks;
using CSharp_Azure_API.Common;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using NUnit.Framework;

namespace CSharp_Azure_API.Tests
{
    [TestFixture]
    class Disk : TestBase
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
            string diskName = SdkContext.RandomResourceName("disk", 24);

            ResourceGroups oResourceGroups = new ResourceGroups(this.credentials, this.subscriptionId);
            oResourceGroups.Create(rgName, this.region);

            var disk = new Disks(credentials, subscriptionId);
            disk.Create(diskName, rgName, this.region, 50);

            var resources = await oResourceGroups.GetResources(rgName);
            Assert.AreEqual(Formatter.CountPageable(resources), 1);

            disk.Del(diskName, rgName);

            var resourcesAfterDel = await oResourceGroups.GetResources(rgName);

            Assert.LessOrEqual(Formatter.CountPageable(resourcesAfterDel), 0);

            oResourceGroups.Del(rgName);
        }
    }
}