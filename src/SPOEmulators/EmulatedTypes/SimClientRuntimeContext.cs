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

            var shimClientRuntimeContext = new ShimClientRuntimeContext(instance);

            SetDefaultLoadFor<Site>(shimClientRuntimeContext);
            SetDefaultLoadFor<Web>(shimClientRuntimeContext);
            SetDefaultLoadFor<List>(shimClientRuntimeContext);
            SetDefaultLoadFor<User>(shimClientRuntimeContext);
            SetDefaultLoadFor<Field>(shimClientRuntimeContext);
            SetDefaultLoadFor<FieldCollection>(shimClientRuntimeContext);
            SetDefaultLoadFor<ListItem>(shimClientRuntimeContext);
            SetDefaultLoadFor<ListItemCollection>(shimClientRuntimeContext);


            this.Fake = shimClientRuntimeContext;

            this.Fake.CastToOf1ClientObject<FieldNumber>((i) => new SimFieldNumber(i).Instance);
        }

        private static void SetDefaultLoadFor<T>(ShimClientRuntimeContext shimClientRuntimeContext) where T : ClientObject
        {
            shimClientRuntimeContext.LoadOf1M0ExpressionOfFuncOfM0ObjectArray<T>((a, b) =>
            {
            });
            shimClientRuntimeContext.LoadQueryOf1ClientObjectCollectionOfM0<T>(delegate
            {
                return null;
            });
            shimClientRuntimeContext.LoadQueryOf1IQueryableOfM0<T>(delegate
            {
                return null;
            });
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
