using System;
using Microsoft.SharePoint.Client;
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
        public void SimClientContext_creates_Web_for_integration_0365()
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

        [TestMethod]
        public void SimWeb_can_work_with_list_o365()
        {
            using (var context = new SPOEmulationContext(IsolationLevel.Integration, _connectionInformation))
            {
                var web = context.ClientContext.Web;

                try
                {
                    // Create a list
                    var listInfo = new ListCreationInformation
                    {
                        Title = "A custom list",
                        TemplateType = (int)ListTemplateType.GenericList
                    };
                    var list = web.Lists.Add(listInfo);
                    list.Description = "A custom description...";
                    list.EnableVersioning = true;
                    list.Update();
                    context.ClientContext.ExecuteQuery();

                    // Add a field
                    var field = list.Fields.AddFieldAsXml("<Field DisplayName='My Number' Type='Number' />", true, AddFieldOptions.DefaultValue);
                    var numberField = context.ClientContext.CastTo<FieldNumber>(field);
                    numberField.MaximumValue = 1000;
                    numberField.MinimumValue = 10;
                    numberField.Update();
                    context.ClientContext.Load(field, f => f.InternalName);
                    context.ClientContext.ExecuteQuery();

                    // Add a list item
                    var itemInfo = new ListItemCreationInformation
                    {
                        LeafName = "List Item 1"
                    };
                    var item = list.AddItem(itemInfo);
                    item[field.InternalName] = 100;
                    item.Update();
                    context.ClientContext.ExecuteQuery();

                    // Query list and retrieve item
                    var query = new CamlQuery
                    {
                        ViewXml = @"<View>
 <Query>
  <Where>
   <Eq>
    <FieldRef Name='Title' />
    <Value Type='Text'>List Item 1</Value>
   </Eq>
  </Where>
 </Query>
</View>"
                    };
                    var items = list.GetItems(query);
                    context.ClientContext.Load(items);
                    context.ClientContext.ExecuteQuery();

                    Assert.AreEqual(1, items.Count);
                }
                finally
                {
                    // Clean up and delete list
                    var listToDelete = web.Lists.GetByTitle("A custom list");
                    listToDelete.DeleteObject();
                    context.ClientContext.ExecuteQuery();
                }
            }
        }
    }
}
