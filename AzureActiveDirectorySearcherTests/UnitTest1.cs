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
            var searcher = new ActiveDirectorySearchClient(new ActiveDirectoryConfigurationValues(TestValues.TenantName, TestValues.TenantId, TestValues.ClientId, TestValues.ClientSecret));
            
            Assert.IsNotNull(searcher.GetSingleByEmail("jpknoll@ucdavis.edu").Result);
        }
    }
}
