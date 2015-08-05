using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SPOEmulators.Tests
{
    [TestClass]
    public class UrlUtilityTest
    {
        [TestMethod]
        public void IsUrlRootWeb_returns_true_for_webapp_rootweb()
        {
            var url = new Uri("http://www.someurl.com");

            Assert.IsTrue(UrlUtility.IsUrlRootWeb(url));
        }

        [TestMethod]
        public void IsUrlRootWeb_returns_false_for_webapp_subweb()
        {
            var url = new Uri("http://www.someurl.com/subweb");

            Assert.IsFalse(UrlUtility.IsUrlRootWeb(url));
        }

        [TestMethod]
        public void IsUrlRootWeb_returns_true_for_sites()
        {
            foreach (var managedPath in new[] { "sites", "teams", "portals" })
            {
                var url = new Uri("http://www.someurl.com/" + managedPath + "/root");
                Assert.IsTrue(UrlUtility.IsUrlRootWeb(url));
            }           
        }

        [TestMethod]
        public void IsUrlRootWeb_returns_false_for_sites_subsites()
        {
            foreach (var managedPath in new[] { "sites", "teams", "portals" })
            {
                var url = new Uri("http://www.someurl.com/" + managedPath + "/root/subweb");
                Assert.IsFalse(UrlUtility.IsUrlRootWeb(url));
            }
        }

        [TestMethod]
        public void GetRootWebUri_gets_root_for_webapp()
        {
            var url = new Uri("http://www.someurl.com/subweb");
            var result = UrlUtility.GetRootWebUri(url);

            Assert.AreEqual("http://www.someurl.com/", result.AbsoluteUri);
        }

        [TestMethod]
        public void GetRootWebUri_gets_root_for_site()
        {
            foreach (var managedPath in new[] { "sites", "teams", "portals" })
            {
                var url = new Uri("http://www.someurl.com/" + managedPath + "/root/subweb");
                var result = UrlUtility.GetRootWebUri(url);

                Assert.AreEqual("http://www.someurl.com/" + managedPath + "/root/", result.AbsoluteUri);
            }
        }
    }
}
