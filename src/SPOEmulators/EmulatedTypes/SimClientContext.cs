namespace SPOEmulators.EmulatedTypes
{
    using System;
    using Microsoft.QualityTools.Testing.Fakes.Instances;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint.Client;
    using Microsoft.SharePoint.Client.Fakes;

    internal class SimClientContext : Isolator<ClientContext, ShimClientContext>, IInstanced<ClientContext>, IInstanced
    {
        public static SimClientContext _current;
        
        public Web Web { get; set; }

        public Site Site { get; set; }

        public Version ServerVersion { get; set; }

        public RequestResources RequestResources { get; set; }

        public SimClientContext(Uri url)
            : this(ShimRuntime.CreateUninitializedInstance<ClientContext>(), url)
        {
        }

        public SimClientContext(ClientContext instance, Uri url)
            : base(instance)
        {            
            this.Fake.ExecuteQuery = () => { };

            this.Fake.SiteGet = () => this.Site;
            this.Fake.WebGet = () => this.Web;

            this.Fake.ServerVersionGet = () => this.ServerVersion;
            this.Fake.RequestResourcesGet = () => this.RequestResources;

            var shimRuntimeClientContext = new SimClientRuntimeContext(this.Instance);

            if (_current == null)
            {
                var simSite = new SimSite(url);
                this.Web = simSite.CurrentWeb.Instance;
                this.Site = simSite.Instance;
            }
            else
            {
                this.Site = _current.Site;

                var simSite = new SimSite(_current.Site, url);
                // check if the context has changed to a known web
                var newWeb = simSite.OpenWeb(x => x.ServerRelativeUrl == url.AbsolutePath);
                this.Web = newWeb ?? _current.Web;
            }
        }

        public static SimClientContext FromInstance(ClientContext instance)
        {
            return InstancedPool.CastAsInstanced<ClientContext, SimClientContext>(instance);
        }

        internal static ClientContext Initialize(Uri siteUrl)
        {
            ShimClientContext.BehaveAsNotImplemented();

            ShimClientContext.ConstructorString = (context, url) =>
            {
                new SimClientContext(context, new Uri(url));
            };
            ShimClientContext.ConstructorUri = (context, url) =>
            {
                new SimClientContext(context, url);
            };

            _current = new SimClientContext(siteUrl);

            return _current.Instance;
        }
    }
}
