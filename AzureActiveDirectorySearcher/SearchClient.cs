using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.Azure.ActiveDirectory.GraphClient.Extensions;

namespace AzureActiveDirectorySearcher
{
    public class SearchClient
    {
        public ActiveDirectoryClient ActiveDirectoryClient { get; set; }

        public SearchClient(ActiveDirectoryConfigurationValues config)
        {
            ActiveDirectoryClient = AuthenticationHelper.GetActiveDirectoryClientAsApplication(config);
        }

        public async Task<IPagedCollection<IUser>> GetBySurname(string surname)
        {
            return await ActiveDirectoryClient.Users.Where(x => x.Surname.Equals("Kirkland"))
                .ExecuteAsync();
        }
    }
}
