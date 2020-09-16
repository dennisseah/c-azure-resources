using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;

namespace CSharp_Azure_API.Common {
    /// <summary>
    /// Resource Base Class
    /// </summary>
    public class ResourceBase {
        /// <summary>
        /// Credential to authenticate to a subscription
        /// </summary>
        protected AzureCredentials credentials;

        /// <summary>
        /// Subscription Id
        /// </summary>
        protected string subscriptionId;

        protected IAzure azure;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="credentials">Credential to authenticate to a subscription</param>
        /// <param name="subscriptionId">Subscription Id</param>
        protected ResourceBase (AzureCredentials credentials, string subscriptionId) {
            this.credentials = credentials;
            this.subscriptionId = subscriptionId;

            this.azure = Microsoft.Azure.Management.Fluent.Azure
                .Configure ()
                .WithLogLevel (HttpLoggingDelegatingHandler.Level.Basic)
                .Authenticate (credentials)
                .WithDefaultSubscription ();
        }
    }
}