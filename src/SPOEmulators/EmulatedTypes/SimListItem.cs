namespace SPOEmulators.EmulatedTypes
{
    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.QualityTools.Testing.Fakes.Shims;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Fakes;

    internal class SimListItem : Isolator<ListItem, ShimListItem>
    {
        private readonly Dictionary<string, object> fieldValues = new Dictionary<string, object>();

       

        public SimListItem()
            : this(ShimRuntime.CreateUninitializedInstance<ListItem>())
        {
        }

        public SimListItem(ListItem instance)
            : base(instance)
        {
            this.Fake.DeleteObject = () => this.Delete();
            this.Fake.DisplayNameGet = () => this.DisplayName;
            this.Fake.IdGet = () => this.Id;
            this.Fake.ItemGetString = (string field) =>
            {
                return fieldValues[field];
            };
            this.Fake.ItemSetStringObject = (key, value) => 
            {
                fieldValues[key] = value;
            };
            this.Fake.SetFieldValueStringObject = (key, value) =>
            {
                fieldValues[key] = value;
            };
            this.Fake.Update = () => { };
        }

        private void Delete()
        {
            ListItems.Remove(this.Instance);
        }


        internal static void Initialize()
        {
            ShimListItem.BehaveAsNotImplemented();
        }

        public SimListItemCollection ListItems { get; set; }

        public string DisplayName { get; set; }

        public int Id { get; set; }
    }
}
