using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPOEmulators.Tests.Properties;

namespace SPOEmulators.Tests
{
    [TestClass]
    public class EmulatedTypesSPOTest
    {
        ConnectionInformation _connectionInformation = new ConnectionInformation
        {
            Url = new Uri(Settings.Default.O365Url),
            UserName = Settings.Default.O365User,
        };

        public EmulatedTypesSPOTest()
        {
            _connectionInformation.SetPassword(Settings.Default.O365Password);
        }

        [TestMethod]
        public void SimClientContext_creates_Web_for_fake()
        {
            using (var context = new SPOEmulationContext(IsolationLevel.Integration, _connectionInformation))
            {
                Assert.IsNotNull(context.ClientContext);
                Assert.IsNotNull(context.ClientContext.Web);

                context.ClientContext.ExecuteQuery();
            }
        }

        [TestMethod]
        public void SimWeb_can_change_web_title_o365()
        {
            using (var context = new SPOEmulationContext(IsolationLevel.Integration, _connectionInformation))
            {
                // Get title
                context.ClientContext.Load(context.ClientContext.Web, w => w.Title);
                context.ClientContext.ExecuteQuery();
                var originalTitle = context.ClientContext.Web.Title;
                Assert.IsNotNull(originalTitle);

                // set title to something different
                context.ClientContext.Web.Title = "A new Title that is applied";
                context.ClientContext.Web.Update();
                context.ClientContext.ExecuteQuery();

                context.ClientContext.Load(context.ClientContext.Web, w => w.Title);
                context.ClientContext.ExecuteQuery();
                Assert.AreEqual("A new Title that is applied", context.ClientContext.Web.Title);

                // set title back
                context.ClientContext.Web.Title = originalTitle;
                context.ClientContext.Web.Update();
                context.ClientContext.ExecuteQuery();
            }
        }
    }
}
