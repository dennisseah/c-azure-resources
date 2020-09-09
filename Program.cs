using System;
using System.Threading.Tasks;
using C_azure_resources.Common;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;

namespace C_azure_resources
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            string rgName = SdkContext.RandomResourceName("rgRSMA", 24);
            string blobAccountName = SdkContext.RandomResourceName("rgRSMA", 24);
            try
            {
                var subscriptionId = Environment.GetEnvironmentVariable("SUBSCRIPTION_ID");
                AzureCredentials credentials = Authentication.GetCredential();
                ResourceGroups oResourceGroups = new ResourceGroups(credentials, subscriptionId);
                oResourceGroups.Create(rgName, Region.USWest);
                Console.WriteLine(oResourceGroups.IsExist(rgName));

                StorageBlobs oStorageBlobs = new StorageBlobs(credentials, subscriptionId);
                oStorageBlobs.Create(blobAccountName, Region.USWest, rgName);

                var resources = await oResourceGroups.GetResources(rgName);

                foreach (var resource in resources)
                {
                    if (resource.Kind == StorageBlobs.KIND)
                    {
                        Console.WriteLine(oStorageBlobs.IsExist(rgName, resource.Name));
                    }
                }

                oResourceGroups.Del(rgName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return;
        }
    }
}