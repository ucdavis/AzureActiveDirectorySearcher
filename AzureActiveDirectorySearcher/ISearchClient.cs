using System.Threading.Tasks;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.Azure.ActiveDirectory.GraphClient.Extensions;

namespace AzureActiveDirectorySearcher
{
    public interface ISearchClient
    {
        ActiveDirectoryClient ActiveDirectoryClient { get; set; }
        Task<IPagedCollection<IUser>> FindByLastName(string last);
        Task<IPagedCollection<IUser>> FindByFirstAndLastName(string first, string last);
        Task<IPagedCollection<IUser>> FindByEmail(string email);
        Task<IPagedCollection<IUser>> FindByKerberos(string kerb);
        Task<IPagedCollection<IUser>> FindByKerberosOrEmail(string kerb, string email);
        Task<IPagedCollection<IUser>> FindByUserInfo(string kerb, string email, string first, string last);
        Task<IUser> GetSingleByKerberos(string kerb);
        Task<IUser> GetSingleByEmail(string email);
    }
}