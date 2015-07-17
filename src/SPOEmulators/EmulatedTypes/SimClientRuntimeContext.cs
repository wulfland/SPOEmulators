namespace SPOEmulators.EmulatedTypes
{
    using Microsoft.QualityTools.Testing.Fakes.Instances;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint.Client;
    using Microsoft.SharePoint.Client.Fakes;

    internal class SimClientRuntimeContext : Isolator<ClientRuntimeContext, ShimClientRuntimeContext>, IInstanced<ClientRuntimeContext>, IInstanced
    {
        public new ShimClientRuntimeContext Fake
        {
            get;
            private set;
        }

        public new ClientRuntimeContext Instance
        {
            get
            {
                return (ClientRuntimeContext)base.Instance;
            }
        }

        public SimClientRuntimeContext()
            : this(ShimRuntime.CreateUninitializedInstance<ClientRuntimeContext>())
        {
        }

        public SimClientRuntimeContext(ClientRuntimeContext instance)
            : base(instance)
        {

            // http://sharepoint.stackexchange.com/questions/73538/mocking-client-object-models-clientcontext-with-moles
            var shimClientRuntimeContext = new ShimClientRuntimeContext(instance);
            shimClientRuntimeContext.LoadOf1M0ExpressionOfFuncOfM0ObjectArray<Web>((a, b) => { });

            this.Fake = shimClientRuntimeContext;
        }

        public static SimClientRuntimeContext FromInstance(ClientRuntimeContext instance)
        {
            return InstancedPool.CastAsInstanced<ClientRuntimeContext, SimClientRuntimeContext>(instance);
        }

        internal static void Initialize()
        {
            ShimClientRuntimeContext.BehaveAsNotImplemented();
        }
    }
}
