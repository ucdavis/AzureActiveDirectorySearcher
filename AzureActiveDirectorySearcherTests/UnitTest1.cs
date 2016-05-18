using System;
using AzureActiveDirectorySearcher;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AzureActiveDirectorySearcherTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var searcher = new SearchClient(new ActiveDirectoryConfigurationValues(TestValues.TenantName, TestValues.TenantId, TestValues.ClientId, TestValues.ClientSecret));
            
            Assert.IsNotNull(searcher.GetSingleByKerberosOrEmail("postit", "jpknoll@ucdavis.edu").Result);
        }
    }
}
