using System.Diagnostics;
using System.Threading.Tasks;
using CSharp_Azure_API.Common;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using NUnit.Framework;

namespace CSharp_Azure_API.Tests
{
    [TestFixture]
    class MainTest : TestBase
    {

        [OneTimeSetUp]
        new public void init()
        {
            base.init();
        }

        [TestCase]
        public async Task End2EndTest()
        {
            string rgName = SdkContext.RandomResourceName("rgRSMA", 24);
            string blobAccountName = SdkContext.RandomResourceName("rgRSMA", 24);

            ResourceGroups oResourceGroups = new ResourceGroups(this.credentials, this.subscriptionId);
            oResourceGroups.Create(rgName, this.region);
            Assert.True(oResourceGroups.IsExist(rgName));

            StorageBlobs oStorageBlobs = new StorageBlobs(credentials, subscriptionId, rgName);
            oStorageBlobs.Create(blobAccountName, this.region);

            var resources = await oResourceGroups.GetResources(rgName);
            int count = 0;

            foreach (var resource in resources)
            {
                if (resource.Kind == StorageBlobs.KIND)
                {
                    Assert.True(oStorageBlobs.IsExist(resource.Name));
                }
                else
                {
                    Assert.True(false);
                }
                count++;
            }

            Assert.AreEqual(count, 1);

            oStorageBlobs.Del(blobAccountName);
            Trace.WriteLine("Delete Storage Account");
            Assert.False(oStorageBlobs.IsExist(blobAccountName));

            oResourceGroups.Del(rgName);
            Assert.False(oResourceGroups.IsExist(rgName));
        }
    }
}