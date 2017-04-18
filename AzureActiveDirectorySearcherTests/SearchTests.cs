using AzureActiveDirectorySearcher;

namespace AzureActiveDirectorySearcherTests
{
    public class SearchTests
    {
        public void SetupSearcher()
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
            Assert.IsNotNull(Searcher.GetUserByEmail("jpknoll@ucdavis.edu").Result);
        }

        [Fact]
        public void TestGetSingleByKerb()
        {
            Assert.IsNotNull(Searcher.GetUserByKerberos("jpknoll").Result);
        }

        [Fact]
        public void TestFindByKerberosOrEmail()
        {
            var result = Searcher.FindByEmailOrKerberos("jpknoll", "jsylvestre@ucdavis.edu").Result;
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
        }
    }
}
