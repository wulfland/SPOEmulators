namespace SPOEmulators.EmulatedTypes
{
    using Microsoft.QualityTools.Testing.Fakes.Instances;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint.Client;
    using Microsoft.SharePoint.Client.Fakes;

    internal class SimClientContext : Isolator<ClientContext, ShimClientContext>, IInstanced<ClientContext>, IInstanced
    {
        Web _web;

        public Web Web
        {
            get
            {
                if (this._web == null)
                {
                    this._web = SimClientContext.CreateWeb();
                }

                return this._web;
            }

            set
            {
                this._web = value;
            }
        }

        public new ShimClientContext Fake
        {
            get;
            private set;
        }

        public new ClientContext Instance
        {
            get
            {
                return (ClientContext)base.Instance;
            }
        }

        public SimClientContext()
            : this(ShimRuntime.CreateUninitializedInstance<ClientContext>())
        {
        }

        public SimClientContext(ClientContext instance)
            : base(instance)
        {

            var shimClientContext = new ShimClientContext(instance);
            shimClientContext.ExecuteQuery = () => { };
            shimClientContext.WebGet = () => this.Web;

            var shimRuntimeClientContext = new SimClientRuntimeContext(this.Instance);

            this.Fake = shimClientContext;
        }

        public static SimClientContext FromInstance(ClientContext instance)
        {
            return InstancedPool.CastAsInstanced<ClientContext, SimClientContext>(instance);
        }

        internal static void Initialize()
        {
            ShimClientContext.BehaveAsNotImplemented();
        }

        static Web CreateWeb()
        {
            // todo: create site and return rootweb
            return new SimWeb().Instance;
        }
    }
}
