using System;
using CSOMSample.Properties;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPOEmulators;

namespace CSOMSample
{
    [TestClass]
    public class ListTests
    {
        IsolationLevel _isolationLevel = Settings.Default.IsolationLevel;
        ConnectionInformation _connectionInformation = new ConnectionInformation
        {
            Url = new Uri(Settings.Default.Url)
        };

        public ListTests()
        {
            if (_isolationLevel != IsolationLevel.Fake)
            {
                _connectionInformation.UserName = Settings.Default.User;
                _connectionInformation.SetPassword(Settings.Default.Password);
            }
        }

        [TestMethod]
        public void ProvisioningEngine_creates_default_list()
        {
            using (var context = new SPOEmulationContext(_isolationLevel, _connectionInformation))
            {
                var sut = new ProvisioningEngine();
                sut.CreateDefaultList(context.ClientContext, context.ClientContext.Web);

                var result = context.ClientContext.Web.Lists.GetByTitle("Default List");
                context.ClientContext.Load(result);
                context.ClientContext.ExecuteQuery();

                Assert.IsNotNull(result);
                Assert.AreEqual("A default list that is provisioned.", result.Description);
                Assert.IsTrue(result.EnableVersioning);


                // Delete list if we do integration testing
                if (_isolationLevel != IsolationLevel.Fake)
                {
                    result.DeleteObject();
                    context.ClientContext.ExecuteQuery();
                }
            }
        }

        [TestMethod]
        public void ProvisioningEngine_default_list_contains_fields()
        {
            using (var context = new SPOEmulationContext(_isolationLevel, _connectionInformation))
            {
                var sut = new ProvisioningEngine();
                sut.CreateDefaultList(context.ClientContext, context.ClientContext.Web);

                var result = context.ClientContext.Web.Lists.GetByTitle("Default List");
                context.ClientContext.Load(result);
                context.ClientContext.ExecuteQuery();

                // Check that the list contains fields.
                var field1 = result.Fields.GetByTitle("My Number1");
                var field2 = result.Fields.GetByTitle("My Number2");
                context.ClientContext.Load(field1);
                context.ClientContext.Load(field2);
                context.ClientContext.ExecuteQuery();
                Assert.IsNotNull(field1);
                Assert.IsNotNull(field2);

                // Delete list if we do integration testing
                if (_isolationLevel != IsolationLevel.Fake)
                {
                    result.DeleteObject();
                    context.ClientContext.ExecuteQuery();
                }
            }
        }

        [TestMethod]
        public void ProvisioningEngine_adds_default_data()
        {
            using (var context = new SPOEmulationContext(_isolationLevel, _connectionInformation))
            {
                var sut = new ProvisioningEngine();
                // ensure the list
                sut.CreateDefaultList(context.ClientContext, context.ClientContext.Web);
                var list = context.ClientContext.Web.Lists.GetByTitle("Default List");
                context.ClientContext.Load(list);

                // add items to list
                sut.AddDeaultData(context.ClientContext, list);

                // reload items validate
                var query = CamlQuery.CreateAllItemsQuery();
                var items = list.GetItems(query);
                context.ClientContext.Load(items);
                context.ClientContext.ExecuteQuery();

                Assert.AreEqual(5, items.Count);

                // Delete list if we do integration testing
                if (_isolationLevel != IsolationLevel.Fake)
                {
                    list.DeleteObject();
                    context.ClientContext.ExecuteQuery();
                }
            }
        }
    }
}
