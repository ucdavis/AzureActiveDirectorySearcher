using System;
using System.Threading.Tasks;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace AzureActiveDirectorySearcher
{
    public class ActiveDirectoryConfigurationValues
    {
        public string AuthString { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string ResourceUrl { get; set; }
        public string TenantId { get; set; }
    }

    internal class AuthenticationHelper
    {
        public static string TokenForUser;

        /// <summary>
        /// Async task to acquire token for Application.
        /// </summary>
        /// <returns>Async Token for application.</returns>
        public static async Task<string> AcquireTokenAsyncForApplication(ActiveDirectoryConfigurationValues config)
        {
            return GetTokenForApplication(config);
        }

        /// <summary>
        /// Get Token for Application.
        /// </summary>
        /// <returns>Token for application.</returns>
        public static string GetTokenForApplication(ActiveDirectoryConfigurationValues config)
        {
            AuthenticationContext authenticationContext = new AuthenticationContext(config.AuthString, false);
            // Config for OAuth client credentials 
            ClientCredential clientCred = new ClientCredential(config.ClientId, config.ClientSecret);
            AuthenticationResult authenticationResult = authenticationContext.AcquireTokenAsync(config.ResourceUrl, clientCred).Result;
            string token = authenticationResult.AccessToken;
            return token;
        }

        /// <summary>
        /// Get Active Directory Client for Application.
        /// </summary>
        /// <returns>ActiveDirectoryClient for Application.</returns>
        public static ActiveDirectoryClient GetActiveDirectoryClientAsApplication(ActiveDirectoryConfigurationValues config)
        {
            Uri servicePointUri = new Uri(config.ResourceUrl);
            Uri serviceRoot = new Uri(servicePointUri, config.TenantId);
            ActiveDirectoryClient activeDirectoryClient = new ActiveDirectoryClient(serviceRoot,
                async () => await AcquireTokenAsyncForApplication(config));
            return activeDirectoryClient;
        }
    }
}