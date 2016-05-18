namespace AzureActiveDirectorySearcher
{
    /// <summary>
    /// Contains needed configuration values for authenticating against Azure AD
    /// </summary>
    public class ActiveDirectoryConfigurationValues
    {
        public ActiveDirectoryConfigurationValues()
        {
            
        }

        /// <summary>
        /// Set required configuration values
        /// </summary>
        /// <param name="tenantName">Tenant name in the form of [tenant].onmicrosoft.com</param>
        /// <param name="tenantId">Tenant Id</param>
        /// <param name="clientId">Client Id of the application</param>
        /// <param name="clientSecret">Client Secret (aka 'Key')</param>
        public ActiveDirectoryConfigurationValues(string tenantName, string tenantId, string clientId, string clientSecret)
        {
            //Set defaults
            AzureLoginUrl = ActiveDirectoryDefaults.AzureLoginUrl;
            ResourceUrl = ActiveDirectoryDefaults.AzureGraphResourceUrl;

            //set user values
            TenantName = tenantName;
            TenantId = tenantId;

            ClientId = clientId;
            ClientSecret = clientSecret;
        }

        public string AzureLoginUrl { get; set; }
        public string ResourceUrl { get; set; }
        public string AuthString => "https://login.microsoftonline.com/" + TenantName;

        public string TenantName { get; set; }
        public string TenantId { get; set; }

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}