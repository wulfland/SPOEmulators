using System;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPOEmulators.Tests.Properties;

namespace SPOEmulators.Tests
{
    [TestClass]
    public class SitesTest
    {
        ConnectionInformation _connectionInformation = new ConnectionInformation
        {
            Url = new Uri(Settings.Default.O365Url),
            UserName = Settings.Default.O365User,
        };

        public SitesTest()
        {
            _connectionInformation.SetPassword(Settings.Default.O365Password);
        }

        [TestMethod]
        public void Site_can_add_and_remove_Webs_Fake()
        {
            var isolationLevel = IsolationLevel.Fake;

            // create a unique name for a subweb
            var uniqueName = Guid.NewGuid().ToString("N");

            using (var context = new SPOEmulationContext(isolationLevel, _connectionInformation))
            {
                using(var clientContext = new ClientContext(_connectionInformation.Url))
                {
                    clientContext.Credentials = new SharePointOnlineCredentials(_connectionInformation.UserName, _connectionInformation.Password);
                    var site = clientContext.Site;
                    clientContext.Load(site);

                    // The rootweb should exit...
                    var rootWeb = site.RootWeb;
                    clientContext.Load(rootWeb, w => w.Url);
                    clientContext.Load(rootWeb, w => w.ServerRelativeUrl);
                    clientContext.ExecuteQuery();
                    Assert.IsNotNull(site.RootWeb);
                    Assert.AreEqual(_connectionInformation.Url.AbsoluteUri.TrimEnd('/'), site.RootWeb.Url);
                    Assert.AreEqual(MakeServerRelative(_connectionInformation.Url), site.RootWeb.ServerRelativeUrl);

                    // add a new web
                    var subweb = site.RootWeb.Webs.Add(new WebCreationInformation
                    { 
                        Url = uniqueName, 
                        Title = uniqueName, 
                        UseSamePermissionsAsParentSite=true, 
                        WebTemplate = "STS#0"
                    });

                    clientContext.Load(subweb, w => w.Title);
                    clientContext.Load(subweb, w => w.Url);
                    clientContext.Load(subweb, w => w.ServerRelativeUrl);
                    clientContext.ExecuteQuery();

                    Assert.AreEqual(uniqueName, subweb.Title);
                    Assert.AreEqual(_connectionInformation.Url.AbsoluteUri + uniqueName, subweb.Url);
                    Assert.AreEqual(MakeServerRelative(_connectionInformation.Url) + uniqueName, subweb.ServerRelativeUrl);
                }
                

                // Directly connect and delete web
                using (var clientContext = new ClientContext(_connectionInformation.Url.AbsoluteUri + uniqueName))
                {
                    clientContext.Credentials = new SharePointOnlineCredentials(_connectionInformation.UserName, _connectionInformation.Password);
                    var web = clientContext.Web;
                    clientContext.Load(web);
                    clientContext.ExecuteQuery();

                    Assert.AreEqual(uniqueName, web.Title);
                    Assert.AreEqual(_connectionInformation.Url.AbsoluteUri + uniqueName, web.Url);
                    Assert.AreEqual(MakeServerRelative(_connectionInformation.Url) + uniqueName, web.ServerRelativeUrl);

                    web.DeleteObject();
                    clientContext.ExecuteQuery();
                }

            }
        }

        [TestMethod]
        public void Site_can_add_and_remove_Webs_O365()
        {
            var isolationLevel = IsolationLevel.Integration;

            // create a unique name for a subweb
            var uniqueName = Guid.NewGuid().ToString("N");

            using (var context = new SPOEmulationContext(isolationLevel, _connectionInformation))
            {
                using (var clientContext = new ClientContext(_connectionInformation.Url))
                {
                    clientContext.Credentials = new SharePointOnlineCredentials(_connectionInformation.UserName, _connectionInformation.Password);
                    var site = clientContext.Site;
                    clientContext.Load(site);

                    // The rootweb should exit...
                    clientContext.Load(site.RootWeb, w => w.Url);
                    clientContext.Load(site.RootWeb, w => w.ServerRelativeUrl);
                    clientContext.ExecuteQuery();
                    Assert.IsNotNull(site.RootWeb);
                    Assert.AreEqual(_connectionInformation.Url.AbsoluteUri.TrimEnd('/'), site.RootWeb.Url);
                    Assert.AreEqual(MakeServerRelative(_connectionInformation.Url), site.RootWeb.ServerRelativeUrl);

                    // add a new web
                    var subweb = site.RootWeb.Webs.Add(new WebCreationInformation
                    {
                        Url = uniqueName,
                        Title = uniqueName,
                        UseSamePermissionsAsParentSite = true,
                        WebTemplate = "STS#0"
                    });

                    clientContext.Load(subweb, w => w.Title);
                    clientContext.Load(subweb, w => w.Url);
                    clientContext.Load(subweb, w => w.ServerRelativeUrl);
                    clientContext.ExecuteQuery();

                    Assert.AreEqual(uniqueName, subweb.Title);
                    Assert.AreEqual(_connectionInformation.Url.AbsoluteUri + uniqueName, subweb.Url);
                    Assert.AreEqual(MakeServerRelative(_connectionInformation.Url) + uniqueName, subweb.ServerRelativeUrl);
                }


                // Directly connect and delete web
                using (var clientContext = new ClientContext(_connectionInformation.Url.AbsoluteUri + uniqueName))
                {
                    clientContext.Credentials = new SharePointOnlineCredentials(_connectionInformation.UserName, _connectionInformation.Password);
                    var web = clientContext.Web;
                    clientContext.Load(web);
                    clientContext.ExecuteQuery();

                    Assert.AreEqual(uniqueName, web.Title);
                    Assert.AreEqual(_connectionInformation.Url.AbsoluteUri + uniqueName, web.Url);
                    Assert.AreEqual(MakeServerRelative(_connectionInformation.Url) + uniqueName, web.ServerRelativeUrl);

                    web.DeleteObject();
                    clientContext.ExecuteQuery();
                }

            }
        }

        private string MakeServerRelative(Uri uri)
        {
            return uri.AbsolutePath;
        }
    }
}
