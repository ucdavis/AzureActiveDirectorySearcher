using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace AzureActiveDirectorySearcher
{
    internal class AuthenticationHelper
    {
        /// <summary>
        /// Async task to acquire token for Application.
        /// </summary>
        /// <returns>Async Token for application.</returns>
        public static async Task<string> AcquireTokenAsyncForApplication(ActiveDirectoryConfigurationValues config)
        {
            return await GetTokenForApplication(config);
        }

        /// <summary>
        /// Get Token for Application.
        /// </summary>
        /// <returns>Token for application.</returns>
        public static async Task<string> GetTokenForApplication(ActiveDirectoryConfigurationValues config)
        {
            AuthenticationContext authenticationContext = new AuthenticationContext(config.AuthString, false);
            // Config for OAuth client credentials 
            ClientCredential clientCred = new ClientCredential(config.ClientId, config.ClientSecret);
            AuthenticationResult authenticationResult = await authenticationContext.AcquireTokenAsync(config.ResourceUrl, clientCred);
            string token = authenticationResult.AccessToken;
            return token;
        }
    }
}