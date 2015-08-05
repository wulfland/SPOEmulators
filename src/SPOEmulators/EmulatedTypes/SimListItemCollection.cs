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

    internal class SimListItemCollection : CollectionIsolator<ListItem, ListItemCollection, ShimListItemCollection>
    {
        public SimListItemCollection()
            : this(ShimRuntime.CreateUninitializedInstance<ListItemCollection>())
        {
        }

        public SimListItemCollection(ListItemCollection instance)
            : base(instance)
        {
            this.Fake.GetByIdInt32 = (id) => Get(i => i.Id == id);
            this.Fake.GetByIdString = (s) => Get(i => i.Id.ToString() == s);
            this.Fake.GetByStringIdString = (sid) => Get(i => i.Id.ToString() == sid);

            new ShimClientObjectCollection(instance)
            {
                CountGet = () => this.Count
            };
        }

        public SimListItem CreateItem()
        {
            var item = new SimListItem
            {
                ListItems = this
            };

            this.Add(item.Instance);

            return item;
        }

        private ListItem Get(Func<ListItem, bool> predicate)
        {
            return this.First(predicate);
        }

        public static void Initialize()
        {
            ShimListCollection.BehaveAsNotImplemented();
        }
    }
}
