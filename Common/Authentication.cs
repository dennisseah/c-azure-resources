using System;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;

namespace C_azure_resources.Common
{
    public static class Authentication
    {
        /// <summary>Returns Azure credentials object
        /// </summary>
        public static AzureCredentials GetCredential()
        {
            var env = Environment.GetEnvironmentVariable("AZURE_AUTH_LOCATION");
            return SdkContext.AzureCredentialsFactory.FromFile(env);
        }
    }
}