namespace SPOEmulators.EmulatedTypes
{
    using System;
    using Microsoft.QualityTools.Testing.Fakes.Instances;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint.Client;
    using Microsoft.SharePoint.Client.Fakes;

    internal class SimClientContext : Isolator<ClientContext, ShimClientContext>, IInstanced<ClientContext>, IInstanced
    {
        public Web Web { get; set; }

        public Site Site { get; set; }

        public SimClientContext(Uri url)
            : this(ShimRuntime.CreateUninitializedInstance<ClientContext>(), url)
        {
        }

        public SimClientContext(ClientContext instance, Uri url)
            : base(instance)
        {            
            var simSite = new SimSite(url);
            this.Web = simSite.CurrentWeb.Instance;
            this.Site = simSite.Instance;


            this.Fake.ExecuteQuery = () => { };
            this.Fake.WebGet = () => this.Web;
            this.Fake.SiteGet = () => this.Site;

            var shimRuntimeClientContext = new SimClientRuntimeContext(this.Instance);
        }

        public static SimClientContext FromInstance(ClientContext instance)
        {
            return InstancedPool.CastAsInstanced<ClientContext, SimClientContext>(instance);
        }

        internal static void Initialize()
        {
            ShimClientContext.BehaveAsNotImplemented();

            ShimClientContext.ConstructorString = (context, url) =>
            {
                new SimClientContext(context, new Uri(url));
            };
            ShimClientContext.ConstructorUri = (context, uri) =>
            {
                new SimClientContext(context, uri);
            };
        }

        static Web CreateWeb()
        {
            // todo: create site and return rootweb
            return new SimWeb().Instance;
        }
    }
}
