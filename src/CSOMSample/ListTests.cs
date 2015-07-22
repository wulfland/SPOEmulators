using System;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPOEmulators;

namespace CSOMSample
{
    [TestClass]
    public class ListTests
    {
        [TestMethod]
        public void SimWeb_can_work_with_list_fake()
        {
            using (var context = new SPOEmulationContext(IsolationLevel.Fake))
            {
                var web = context.ClientContext.Web;

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

                // caml queries are not yet supported
                context.SetQueryResultsForFakeList(list, item);

                var items = list.GetItems(query);
                context.ClientContext.Load(items);
                context.ClientContext.ExecuteQuery();

                Assert.AreEqual(1, items.Count);

                // Clean up and delete list
                var listToDelete = web.Lists.GetByTitle("A custom list");
                listToDelete.DeleteObject();
                context.ClientContext.ExecuteQuery();
            }
        }
    }
}
