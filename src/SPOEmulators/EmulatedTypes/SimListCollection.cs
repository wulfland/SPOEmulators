namespace SPOEmulators.EmulatedTypes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint.Client;
    using Microsoft.SharePoint.Client.Fakes;

    internal class SimListCollection : CollectionIsolator<List, ListCollection, ShimListCollection>
    {
        private Guid assetListId;
        private Guid pagesListId;

        public SimListCollection()
            : this(ShimRuntime.CreateUninitializedInstance<ListCollection>())
        {
        }

        public SimListCollection(ListCollection instance) 
            : base(instance)
        {
            new Microsoft.SharePoint.Client.Fakes.ShimClientObjectCollection(base.Fake)
            {
            };

            base.Fake.Bind(this);
            base.Fake.GetByIdGuid = (id) => 
            { 
                return this.GetList((list) => list.Id == id).Instance; 
            };
            base.Fake.GetByTitleString = (title) => 
            {
                return this.GetList((list) => list.Title == title).Instance; 
            };
            base.Fake.AddListCreationInformation = (parameters) =>
            {
                return this.Add(parameters).Instance;
            };
            base.Fake.EnsureSiteAssetsLibrary = () =>
            {
                if (this.assetListId == Guid.Empty)
                {
                    var result = this.Add("Assets", "List designed as a default asset location for images.");
                    this.assetListId = result.Instance.Id;
                }

                return GetList(x => x.Id == assetListId).Instance;
            };
            base.Fake.EnsureSitePagesLibrary = () =>
            {
                if (this.pagesListId == Guid.Empty)
                {
                    var result = this.Add("Pages", "List designed as a default asset location for wiki pages.");
                    this.pagesListId = result.Instance.Id;
                }

                return GetList(x => x.Id == pagesListId).Instance;
            };
        }

        public SimList GetList(Func<List, bool> predicate)
        {
            return SimList.FromInstance(this.First(predicate));
        }

        public SimList Add(ListCreationInformation parameters)
        {
            var list = new SimList();
            list.Description = parameters.Description;
            list.Title = parameters.Title;

            base.Add(list.Instance);

            return list;
        }

        public SimList Add(string title, string description)
        {
            var list = new SimList();
            list.Description = description;
            list.Title = title;

            base.Add(list.Instance);

            return list;
        }
    }
}
