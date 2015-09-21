using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPOEmulators.Tests.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPOEmulators.Tests
{
    [TestClass]
    public class UserPrincipalTest
    {
        ConnectionInformation _connectionInformation = new ConnectionInformation
        {
            Url = new Uri(Settings.Default.O365Url),
            UserName = Settings.Default.O365User,
        };

        public UserPrincipalTest()
        {
            _connectionInformation.SetPassword(Settings.Default.O365Password);
        }

        [TestMethod]
        public void Can_set_principal_title_on_web_user()
        {
            var isolationLevel = IsolationLevel.Fake;
            var uniqueName = Guid.NewGuid().ToString("N");

            using (var context = new SPOEmulationContext(isolationLevel, _connectionInformation))
            {
                context.ClientContext.Web.CurrentUser.Title = uniqueName;

                string actual = context.ClientContext.Web.CurrentUser.Title;

                Assert.AreEqual(uniqueName, actual);
            }
        }
    }
}
