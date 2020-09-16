using System.IO;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;

namespace CSharp_Azure_API.Common
{
    /// <summary>
    /// Authentication Helper
    /// </summary>
    public static class Authentication
    {
        /// <summary>
        /// Returns Azure credential object by call 
        /// SdkContext.AzureCredentialsFactory.FromFile function
        /// which takes a file path to authentication information.
        /// This file is created during realtime by reading the 
        /// environment values of API_SUBSCRIPTION_ID, API_TENANT,
        /// API_CLIENT, and API_CLIENT_KEY.
        /// This file will be removed after the function is called.
        /// </summary>
        /// <returns>Azure credential object</returns>
        public static AzureCredentials GetCredential()
        {
            string dir = $"{Directory.GetCurrentDirectory()}/config.properties";
            ConfigBuilder.build(dir);
            var credentials = SdkContext.AzureCredentialsFactory.FromFile(dir);
            File.Delete(dir);
            return credentials;
        }
    }
}