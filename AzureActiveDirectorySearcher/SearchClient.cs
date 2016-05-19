using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.Azure.ActiveDirectory.GraphClient.Extensions;

namespace AzureActiveDirectorySearcher
{
    public class ActiveDirectorySearchClient : ISearchClient
    {
        public ActiveDirectoryClient ActiveDirectoryClient { get; set; }

        public ActiveDirectorySearchClient(ActiveDirectoryConfigurationValues config)
        {
            ActiveDirectoryClient = AuthenticationHelper.GetActiveDirectoryClientAsApplication(config);
        }

        public async Task<IPagedCollection<IUser>> FindByLastName(string last)
        {
            return
                await
                    ActiveDirectoryClient.Users.Where(x => x.Surname.Equals(last, StringComparison.OrdinalIgnoreCase))
                        .ExecuteAsync();
        }

        public async Task<IPagedCollection<IUser>> FindByFirstAndLastName(string first, string last)
        {
            return
                await
                    ActiveDirectoryClient.Users.Where(
                        x =>
                            x.GivenName.Equals(first, StringComparison.OrdinalIgnoreCase) &&
                            x.Surname.Equals(last, StringComparison.OrdinalIgnoreCase))
                        .ExecuteAsync();
        }

        public async Task<IPagedCollection<IUser>> FindByEmail(string email)
        {
            return
                await
                    ActiveDirectoryClient.Users.Where(x => x.Mail.Equals(email, StringComparison.OrdinalIgnoreCase))
                        .ExecuteAsync();
        }

        public async Task<IPagedCollection<IUser>> FindByKerberos(string kerb)
        {
            return
                await
                    ActiveDirectoryClient.Users.Where(
                        x => x.ProxyAddresses.Any(a => a.Equals(GetKerberosStringForActiveDirectory(kerb))))
                        .ExecuteAsync();
        }
        
        public async Task<IPagedCollection<IUser>> FindByKerberosOrEmail(string kerb, string email)
        {
            return
                await
                    ActiveDirectoryClient.Users.Where(
                        x => x.Mail.Equals(email, StringComparison.OrdinalIgnoreCase) ||
                             x.ProxyAddresses.Any(
                                 a =>
                                     a.Equals(GetKerberosStringForActiveDirectory(kerb),
                                         StringComparison.OrdinalIgnoreCase)))
                        .ExecuteAsync();
        }

        public async Task<IPagedCollection<IUser>> FindByUserInfo(string kerb, string email, string first, string last)
        {
            return
                await
                    ActiveDirectoryClient.Users.Where(
                        x => x.Mail.Equals(email, StringComparison.OrdinalIgnoreCase) ||
                             x.GivenName.Equals(first, StringComparison.OrdinalIgnoreCase) ||
                             x.Surname.Equals(last, StringComparison.OrdinalIgnoreCase) ||
                             x.ProxyAddresses.Any(
                                 a =>
                                     a.Equals(GetKerberosStringForActiveDirectory(kerb),
                                         StringComparison.OrdinalIgnoreCase)))
                        .ExecuteAsync();
        }

        public async Task<IUser> GetSingleByKerberos(string kerb)
        {
            var result =
                await
                    ActiveDirectoryClient.Users.Where(
                        x => x.ProxyAddresses.Any(
                                 a =>
                                     a.Equals(GetKerberosStringForActiveDirectory(kerb),
                                         StringComparison.OrdinalIgnoreCase)))
                        .ExecuteAsync();

            return result.CurrentPage.SingleOrDefault();
        }

        public async Task<IUser> GetSingleByEmail(string email)
        {
            var result =
                await
                    ActiveDirectoryClient.Users.Where(
                        x => x.Mail.Equals(email, StringComparison.OrdinalIgnoreCase))
                        .ExecuteAsync();

            return result.CurrentPage.SingleOrDefault();
        }

        /// <summary>
        /// Active directory doesn't have the kerb directly but we can turn it into a proxy address and search there
        /// </summary>
        /// <param name="kerb">kerberos ID</param>
        /// <returns></returns>
        private string GetKerberosStringForActiveDirectory(string kerb)
        {
            return string.Format("smtp:{0}@ad3.ucdavis.edu", kerb);
        }
    }
}
