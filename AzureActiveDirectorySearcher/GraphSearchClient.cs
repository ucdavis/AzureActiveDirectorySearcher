using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AzureActiveDirectorySearcher
{
    public interface IGraphSearchClient
    {
        Task<GraphUser[]> FindByEmailOrKerberos(string kerberos, string email);
        Task<GraphUser> GetUserByKerberos(string kerberos);
        Task<GraphUser> GetUserByEmail(string email);
    }

    /// <summary>
    /// Manually query the graph API
    /// </summary>
    public class GraphSearchClient : IGraphSearchClient
    {
        private readonly ActiveDirectoryConfigurationValues _config;
        private readonly string _baseUri;

        public GraphSearchClient(ActiveDirectoryConfigurationValues config)
        {
            _config = config;
            _baseUri = ActiveDirectoryDefaults.AzureGraphResourceUrl + config.TenantName;
        }

        private string UserQueryUri => _baseUri + "/users?api-version=" + ActiveDirectoryDefaults.ApiVersion;

        private async Task<HttpClient> GetAuthenticatedClient()
        {
            var token = await AuthenticationHelper.AcquireTokenAsyncForApplication(_config);

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", token);

            return client;
        }

        private async Task<GraphQueryResult> PerformGraphQuery(string filter)
        {
            var filterUri = $"{UserQueryUri}&$filter={filter}";

            // now make a request
            var client = await GetAuthenticatedClient();
            var result = await client.GetStringAsync(filterUri);
            return JsonConvert.DeserializeObject<GraphQueryResult>(result);
        }

        public async Task<GraphUser[]> FindByEmailOrKerberos(string kerberos, string email)
        {
            var filter = $"{GraphUser.GetExtensionForKerberos()} eq '{kerberos}' or mail eq '{email}'";

            // now make a request
            var graphResult = await PerformGraphQuery(filter);
            return graphResult.Value;
        }

        public async Task<GraphUser> GetUserByKerberos(string kerberos)
        {
            var filter = $"{GraphUser.GetExtensionForKerberos()} eq '{kerberos}'";

            // now make a request
            var graphResult = await PerformGraphQuery(filter);
            return graphResult.Value.SingleOrDefault();
        }

        public async Task<GraphUser> GetUserByEmail(string email)
        {
            var filter = $"mail eq '{email}'";

            // now make a request
            var graphResult = await PerformGraphQuery(filter);
            return graphResult.Value.SingleOrDefault();
        }
    }
}