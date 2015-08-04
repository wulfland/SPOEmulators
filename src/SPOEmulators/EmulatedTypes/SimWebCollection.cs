namespace SPOEmulators.EmulatedTypes
{
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint.Client;
    using Microsoft.SharePoint.Client.Fakes;

    internal class SimWebCollection : CollectionIsolator<Web, WebCollection, ShimWebCollection>
    {
        public SimWebCollection()
            : this(ShimRuntime.CreateUninitializedInstance<WebCollection>())
        {
        }

        public SimWebCollection(WebCollection instance)
            : base(instance)
        {
            this.Fake.AddWebCreationInformation = (options) => AddWeb(options).Instance;

            new SimClientObjectCollection(this.Instance);
        }

        public SimWeb AddWeb(WebCreationInformation options)
        {
            var simWeb = new SimWeb
            {
                Title = options.Title,
                Url = options.Url,
                Description = options.Description
            };

            // todo: add other properties
            this.Add(simWeb.Instance);

            return simWeb;
        }
    }
}
