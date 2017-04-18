using AzureActiveDirectorySearcher;
using Xunit;

namespace AzureActiveDirectorySearcherTests
{
    public class SearchTests
    {
        public SearchTests()
        {
            if (Searcher == null)
            {
                Searcher =
                    new GraphSearchClient(new ActiveDirectoryConfigurationValues(TestValues.TenantName,
                        TestValues.TenantId, TestValues.ClientId, TestValues.ClientSecret));
            }
        }

        public IGraphSearchClient Searcher { get; set; }

        [Fact]
        public void TestGetSingleByEmail()
        {
            Assert.NotNull(Searcher.GetUserByEmail("jpknoll@ucdavis.edu").Result);
        }

        [Fact]
        public void TestGetSingleByKerb()
        {
            Assert.NotNull(Searcher.GetUserByKerberos("jpknoll").Result);
        }

        [Fact]
        public void TestFindByKerberosOrEmail()
        {
            var result = Searcher.FindByEmailOrKerberos("jpknoll", "jsylvestre@ucdavis.edu").Result;
            Assert.NotNull(result);
            Assert.Equal(2, result.Length);
        }
    }
}
