using CSharp_Azure_API.Common;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using NUnit.Framework;

namespace CSharp_Azure_API.Tests
{
    [TestFixture]
    class BlobStorageTest : TestBase
    {
        [OneTimeSetUp]
        new public void init()
        {
            base.init();
        }

        [TestCase]
        public void SanityTest()
        {
            string rgName = SdkContext.RandomResourceName("rgRSMA", 24);
            string blobAccountName = SdkContext.RandomResourceName("rgRSMA", 24);
            string containerName = "dummycontainer";

            ResourceGroups oResourceGroups = new ResourceGroups(this.credentials, this.subscriptionId);
            oResourceGroups.Create(rgName, this.region);

            StorageBlobs oStorageBlobs = new StorageBlobs(credentials, subscriptionId, rgName);
            oStorageBlobs.Create(blobAccountName, this.region);

            Assert.AreEqual(Formatter.CountPageable(oStorageBlobs.GetContainers(blobAccountName)), 0);
            oStorageBlobs.CreateBlobContainer(blobAccountName, containerName);
            Assert.AreEqual(Formatter.CountPageable(oStorageBlobs.GetContainers(blobAccountName)), 1);
            oStorageBlobs.DelBlobContainer(blobAccountName, containerName);
            Assert.AreEqual(Formatter.CountPageable(oStorageBlobs.GetContainers(blobAccountName)), 0);

            // Blob Storage Account shall be deleted once resource group is deleted.
            oResourceGroups.Del(rgName);
        }
    }
}