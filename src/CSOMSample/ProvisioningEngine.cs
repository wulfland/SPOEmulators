using Microsoft.SharePoint.Client;

namespace CSOMSample
{
    /// <summary>
    /// A fake business logic object that depends on CSOM objects.
    /// </summary>
    public class ProvisioningEngine
    {
        public void SetDepartmentTitle(ClientContext context, Web web)
        {
            web.Title = "Department A";
            web.Update();
            context.ExecuteQuery();
        }

        public void CreateDefaultList(ClientContext context, Web web)
        {
            // Create a list
            var listInfo = new ListCreationInformation
            {
                Title = "Default List",
                TemplateType = (int)ListTemplateType.GenericList
            };
            var list = web.Lists.Add(listInfo);
            list.Description = "A default list that is provisioned.";
            list.EnableVersioning = true;
            list.Update();
            context.ExecuteQuery();

            // Add a field
            var field = list.Fields.AddFieldAsXml("<Field DisplayName='My Number1' Type='Number' />", true, AddFieldOptions.DefaultValue);
            var numberField = context.CastTo<FieldNumber>(field);
            numberField.MaximumValue = 1000;
            numberField.MinimumValue = 10;
            numberField.Update();

            // Add a second field
            var field2 = list.Fields.AddFieldAsXml("<Field DisplayName='My Number2' Type='Number' />", true, AddFieldOptions.DefaultValue);

            context.ExecuteQuery();
        }

        public void AddDeaultData(ClientContext context, List list)
        {
            for (int i = 1; i <= 5; i++)
            {
                // Add a list item
                var itemInfo = new ListItemCreationInformation
                {
                    LeafName = string.Format("List Item {0}", i)
                };
                var item = list.AddItem(itemInfo);
                item["My_x0020_Number1"] = 100 + i;
                item["My_x0020_Number2"] = i;
                item.Update();
            }

            context.ExecuteQuery();
        }
    }
}
