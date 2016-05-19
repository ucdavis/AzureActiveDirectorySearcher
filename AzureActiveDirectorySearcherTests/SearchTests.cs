﻿using System;
using AzureActiveDirectorySearcher;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AzureActiveDirectorySearcherTests
{
    [TestClass]
    public class SearchTests
    {
        [TestInitialize]
        public void SetupActiveDirectorySearcher()
        {
            if (Searcher == null)
            {
                Searcher =
                    new ActiveDirectorySearchClient(new ActiveDirectoryConfigurationValues(TestValues.TenantName,
                        TestValues.TenantId, TestValues.ClientId, TestValues.ClientSecret));
            }
        }

        public ActiveDirectorySearchClient Searcher { get; set; }

        [TestMethod]
        public void TestGetSingleByEmail()
        {
            Assert.IsNotNull(Searcher.GetSingleByEmail("jpknoll@ucdavis.edu").Result);
        }

        [TestMethod]
        public void TestGetSingleByKerb()
        {
            Assert.IsNotNull(Searcher.GetSingleByKerberos("jpknoll").Result);
        }

        [TestMethod]
        public void TestFindByLastName()
        {
            var result = Searcher.FindByLastName("kirkl").Result;
            Assert.IsNotNull(result.CurrentPage.Count > 0);
        }
        [TestMethod]
        public void TestFindByEmail()
        {
            var result = Searcher.FindByEmail("srkirkland").Result;
            Assert.IsNotNull(result.CurrentPage.Count > 0);
        }
        [TestMethod]
        public void TestFindByFirstAndLast()
        {
            var result = Searcher.FindByFirstAndLastName("scott", "kirkland").Result;
            Assert.IsNotNull(result.CurrentPage.Count > 0);
        }
        [TestMethod]
        public void TestFindByKerberos()
        {
            var result = Searcher.FindByKerberos("jpkno").Result;
            Assert.IsNotNull(result.CurrentPage.Count > 0);
        }
        [TestMethod]
        public void TestFindByKerberosOrEmail()
        {
            var result = Searcher.FindByKerberosOrEmail("jpkno", "jpknoll").Result;
            Assert.IsNotNull(result.CurrentPage.Count > 0);
        }
        [TestMethod]
        public void TestFindByUserInfo()
        {
            var result = Searcher.FindByUserInfo("jpkno", "jpknoll", "john", "knoll").Result;
            Assert.IsNotNull(result.CurrentPage.Count > 0);
        }
    }
}