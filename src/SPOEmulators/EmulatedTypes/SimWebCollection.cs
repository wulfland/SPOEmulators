namespace SPOEmulators.EmulatedTypes
{
    using System;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint.Client;
    using Microsoft.SharePoint.Client.Fakes;

    internal class SimWebCollection : CollectionIsolator<Web, WebCollection, ShimWebCollection>
    {
        public SimWeb Parent { get; private set; }

        public SimWebCollection(SimWeb parent)
            : this(ShimRuntime.CreateUninitializedInstance<WebCollection>(), parent)
        {
        }

        public SimWebCollection(WebCollection instance, SimWeb parent)
            : base(instance)
        {
            Parent = parent;

            this.Fake.AddWebCreationInformation = (options) => AddWeb(options).Instance;

            new SimClientObjectCollection(this.Instance);
        }

        public SimWeb AddWeb(WebCreationInformation options)
        {
            var url = new Uri(Parent.Url.TrimEnd('/') + '/' + options.Url);

            var simWeb = new SimWeb
            {
                Title = options.Title,
                Url = url.AbsoluteUri.TrimEnd('/'),
                ServerRelativeUrl = url.AbsolutePath,
                Description = options.Description
            };

            // todo: add other properties
            this.Add(simWeb.Instance);

            return simWeb;
        }
    }
}
