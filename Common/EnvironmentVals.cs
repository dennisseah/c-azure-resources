using System;

namespace CSharp_Azure_API.Common
{
    /// <summary>
    /// Helper function to get Environment Values
    /// </summary>
    public static class EnvironmentVals
    {

        private static string KEY_SUBSCRIPTION = "API_SUBSCRIPTION_ID";
        private static string KEY_CLIENT = "API_CLIENT";
        private static string KEY_TENANT = "API_TENANT";
        private static string KEY_CLIENT_KEY = "API_CLIENT_KEY";

        /// <summary>Returns Subscription environment variable value
        /// </summary>
        public static string GetSubscription()
        {
            return Environment.GetEnvironmentVariable(KEY_SUBSCRIPTION);
        }

        /// <summary>Returns Client environment variable value
        /// </summary>
        public static string GetClient()
        {
            return Environment.GetEnvironmentVariable(KEY_CLIENT);
        }

        /// <summary>Returns Client Key environment variable value
        /// </summary>
        public static string GetClientKey()
        {
            return Environment.GetEnvironmentVariable(KEY_CLIENT_KEY);
        }

        /// <summary>Returns Tenant environment variable value
        /// </summary>
        public static string GetTenant()
        {
            return Environment.GetEnvironmentVariable(KEY_TENANT);
        }
    }
}