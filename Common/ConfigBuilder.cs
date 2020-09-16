using System.Collections.Generic;
using System.IO;

namespace CSharp_Azure_API.Common
{
    /// <summary>
    /// Builder for config.properties file for authentication
    /// </summary>
    public static class ConfigBuilder
    {
        private static string KEY_SUBSCRIPTION = "subscription";
        private static string KEY_CLIENT = "client";
        private static string KEY_TENANT = "tenant";
        private static string KEY_CLIENT_KEY = "key";

        private static string MANAGEMENT_URI = "managementURI=https\\://management.core.windows.net/";
        private static string BASE_URL = "baseURL=https\\://management.azure.com/";
        private static string AUTH_URL = "authURL=https\\://login.microsoftonline.com/";
        private static string GRAPH_URL = "graphURL=https\\://graph.windows.net/";


        /// <summary>
        /// Create config.properties file. This file is for the consumption
        /// of SdkContext.AzureCredentialsFactory.FromFile function
        /// </summary>
        /// <param name="fileName">Absolute path of config.properties.</param>
        public static void build(string fileName)
        {
            string subscription = EnvironmentVals.GetSubscription();
            string tenant = EnvironmentVals.GetTenant();
            string client = EnvironmentVals.GetClient();
            string client_key = EnvironmentVals.GetClientKey();

            List<string> buffer = new List<string>();
            buffer.Add($"{KEY_SUBSCRIPTION}={subscription}");
            buffer.Add($"{KEY_CLIENT}={client}");
            buffer.Add($"{KEY_TENANT}={tenant}");
            buffer.Add($"{KEY_CLIENT_KEY}={client_key}");

            buffer.Add(MANAGEMENT_URI);
            buffer.Add(BASE_URL);
            buffer.Add(AUTH_URL);
            buffer.Add(GRAPH_URL);

            File.WriteAllText(fileName, string.Join<string>("\n", buffer));
        }
    }
}