using AzureActiveDirectorySearcher;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AzureActiveDirectorySearcherTests
{
    [TestClass]
    public class SearchTests
    {
        [TestInitialize]
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

        [TestMethod]
        public void TestGetSingleByEmail()
        {
            Assert.IsNotNull(Searcher.GetUserByEmail("jpknoll@ucdavis.edu").Result);
        }

        [TestMethod]
        public void TestGetSingleByKerb()
        {
            Assert.IsNotNull(Searcher.GetUserByKerberos("jpknoll").Result);
        }

        [TestMethod]
        public void TestFindByKerberosOrEmail()
        {
            var result = Searcher.FindByEmailOrKerberos("jpknoll", "jsylvestre@ucdavis.edu").Result;
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
        }
    }
}
