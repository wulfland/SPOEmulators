using System;
using CSOMSample.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPOEmulators;

namespace CSOMSample
{
    [TestClass]
    public class WebTests
    {
        IsolationLevel _isolationLevel = Settings.Default.IsolationLevel;
        ConnectionInformation _connectionInformation = new ConnectionInformation
        {
            Url = new Uri(Settings.Default.Url)
        };

        public WebTests()
        {
            if (_isolationLevel != IsolationLevel.Fake)
            {
                _connectionInformation.UserName = Settings.Default.User;
                _connectionInformation.SetPassword(Settings.Default.Password);
            }
        }

        [TestMethod]
        public void ProvisioningEngine_sets_web_title_to_department_name()
        {
            using (var context = new SPOEmulationContext(_isolationLevel, _connectionInformation))
            {
                // Get title
                context.ClientContext.Load(context.ClientContext.Web, w => w.Title);
                context.ClientContext.ExecuteQuery();
                var originalTitle = context.ClientContext.Web.Title;

                var sut = new ProvisioningEngine();
                
                // set title of web to department name
                sut.SetDepartmentTitle(context.ClientContext, context.ClientContext.Web);
                
                context.ClientContext.Load(context.ClientContext.Web, w => w.Title);
                context.ClientContext.ExecuteQuery();
                Assert.AreEqual("Department A", context.ClientContext.Web.Title);

                // Clean up title for integration testing
                if (_isolationLevel != IsolationLevel.Fake)
                {
                    context.ClientContext.Web.Title = originalTitle;
                    context.ClientContext.Web.Update();
                    context.ClientContext.ExecuteQuery();
                }
            }
        }
    }
}
