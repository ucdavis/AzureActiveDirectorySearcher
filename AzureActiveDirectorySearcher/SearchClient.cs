using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
                    ActiveDirectoryClient.Users.Where(x => x.Surname.StartsWith(last))
                        .ExecuteAsync();
        }

        public async Task<IPagedCollection<IUser>> FindByFirstAndLastName(string first, string last)
        {
            return
                await
                    ActiveDirectoryClient.Users.Where(
                        x =>
                            x.GivenName.StartsWith(first) &&
                            x.Surname.StartsWith(last))
                        .ExecuteAsync();
        }

        public async Task<IPagedCollection<IUser>> FindByEmail(string email)
        {
            return
                await
                    ActiveDirectoryClient.Users.Where(x => x.Mail.StartsWith(email))
                        .ExecuteAsync();
        }

        public async Task<IPagedCollection<IUser>> FindByKerberos(string kerb)
        {
            return
                await
                    ActiveDirectoryClient.Users.Where(
                        x => x.ProxyAddresses.Any(a => a.StartsWith(KerberosHelpers.GetKerberosStringForActiveDirectory(kerb))))
                        .ExecuteAsync();
        }
        
        public async Task<IPagedCollection<IUser>> FindByKerberosOrEmail(string kerb, string email)
        {
            return
                await
                    ActiveDirectoryClient.Users.Where(
                        x => x.Mail.StartsWith(email) ||
                             x.ProxyAddresses.Any(
                                 a =>
                                     a.StartsWith(KerberosHelpers.GetKerberosStringForActiveDirectory(kerb))))
                        .ExecuteAsync();
        }

        public async Task<IPagedCollection<IUser>> FindByUserInfo(string kerb, string email, string first, string last)
        {
            return
                await
                    ActiveDirectoryClient.Users.Where(
                        x => x.Mail.StartsWith(email) ||
                             x.GivenName.StartsWith(first) ||
                             x.Surname.StartsWith(last) ||
                             x.ProxyAddresses.Any(
                                 a =>
                                     a.StartsWith(KerberosHelpers.GetKerberosStringForActiveDirectory(kerb))))
                        .ExecuteAsync();
        }

        public async Task<IUser> GetSingleByKerberos(string kerb)
        {
            var result =
                await
                    ActiveDirectoryClient.Users.Where(
                        x => x.ProxyAddresses.Any(
                                 a =>
                                     a.Equals(KerberosHelpers.GetKerberosStringForActiveDirectory(kerb),
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
    }

    public static class KerberosHelpers
    {
        public static string GetKerberos(this IUser user, string proxyBase = "@ad3.ucdavis.edu")
        {
            if (user == null || user.ProxyAddresses == null) return null;

            var kerberosProxy = user.ProxyAddresses.FirstOrDefault(x => x.EndsWith(proxyBase));

            if (string.IsNullOrWhiteSpace(kerberosProxy)) return null;

            var matches = Regex.Match(kerberosProxy, string.Format(@"smtp:(\w+){0}", proxyBase), RegexOptions.IgnoreCase);

            return matches.Success ? matches.Groups[1].Value : null;
        }

        /// <summary>
        /// Active directory doesn't have the kerb directly but we can turn it into a proxy address and search there
        /// </summary>
        /// <param name="kerb">kerberos ID</param>
        /// <returns></returns>
        public static string GetKerberosStringForActiveDirectory(string kerb)
        {
            return string.Format("smtp:{0}@ad3.ucdavis.edu", kerb);
        }
    }
}
