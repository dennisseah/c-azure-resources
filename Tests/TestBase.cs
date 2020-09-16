using CSharp_Azure_API.Common;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using NUnit.Framework;

namespace CSharp_Azure_API.Tests
{
    [TestFixture]
    abstract class TestBase
    {
        protected string subscriptionId;
        protected AzureCredentials credentials;
        protected Region region = Region.USWest;

        public void init()
        {
            this.subscriptionId = EnvironmentVals.GetSubscription();
            this.credentials = Authentication.GetCredential();
        }
    }
}