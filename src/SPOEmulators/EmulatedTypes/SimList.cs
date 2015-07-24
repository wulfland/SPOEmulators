namespace SPOEmulators.EmulatedTypes
{
    using System;
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.QualityTools.Testing.Fakes.Instances;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint.Client;
    using Microsoft.SharePoint.Client.Fakes;

    internal class SimList : Isolator<List, ShimList>
    {
        readonly SimFieldCollection _fields = new SimFieldCollection();
        readonly SimListItemCollection _items = new SimListItemCollection();

        public string Description { get; set; }

        public string Title { get; set; }

        public bool EnableVersioning { get; set; }

        public SimFieldCollection Fields
        {
            get { return _fields; }
        }

        public SimListItemCollection Items
        {
            get { return _items; }
        }

        public SimList()
            : this(ShimRuntime.CreateUninitializedInstance<List>())
        {
        }

        protected SimList(List instance)
            : base(instance)
        {

            this.Fake.TitleGet = () => this.Title;
            this.Fake.TitleSetString = (s) => this.Title = s;
            this.Fake.DescriptionGet = () => this.Description;
            this.Fake.DescriptionSetString = (s) => this.Description = s;
            this.Fake.EnableVersioningGet = () => this.EnableVersioning;
            this.Fake.EnableVersioningSetBoolean = (b) => this.EnableVersioning = b;
            this.Fake.Update = () => { };
            this.Fake.FieldsGet = () => this.Fields.Instance;
            this.Fake.AddItemListItemCreationInformation = (ListItemCreationInformation properties) => 
                {
                    var item = Items.CreateItem();
                    item.DisplayName = properties.LeafName;
                    item.Id = _items.Count + 1;

                    return item.Instance;
                };
            this.Fake.DeleteObject = () => { };
            this.Fake.GetItemsCamlQuery = (q) => 
            {
                if (q.ViewXml == CamlQuery.CreateAllItemsQuery().ViewXml)
                {
                    return this.Items.Instance;
                }
                else
                {
                    return new ShimListItemCollection().Instance;
                }
            };
        }

        public void Delete()
        {
            // remove from parent
        }

        public static void SetQueryResults(List instance, Func<CamlQuery, ListItemCollection> query)
        {
            var simList = FromInstance(instance);
            simList.Fake.GetItemsCamlQuery = new FakesDelegates.Func<CamlQuery, ListItemCollection>(query);
        }

        public static void SetQueryResults(List instance, params ListItem[] items)
        {
            var simList = FromInstance(instance);
            simList.Fake.GetItemsCamlQuery = (query) =>
            {
                var result = new SimListItemCollection();

                if (items != null)
                {
                    foreach (var item in items)
                    {
                        result.Add(item);
                    }
                }

                return result.Instance;
            };
        }


        public static SimList FromInstance(List instance)
        {
            return InstancedPool.CastAsInstanced<List, SimList>(instance);
        }

        internal static void Initialize()
        {
            ShimList.BehaveAsNotImplemented();
        }
    }
}
