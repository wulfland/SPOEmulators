namespace SPOEmulators.EmulatedTypes
{
    using Microsoft.QualityTools.Testing.Fakes.Instances;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint.Client;
    using Microsoft.SharePoint.Client.Fakes;

    internal class SimClientRuntimeContext : Isolator<ClientRuntimeContext, ShimClientRuntimeContext>, IInstanced<ClientRuntimeContext>, IInstanced
    {
        public SimClientRuntimeContext()
            : this(ShimRuntime.CreateUninitializedInstance<ClientRuntimeContext>())
        {
        }

        public SimClientRuntimeContext(ClientRuntimeContext instance)
            : base(instance)
        {

            SetDefaultLoadFor<Site>(this.Fake);
            SetDefaultLoadFor<Web>(this.Fake);
            SetDefaultLoadFor<List>(this.Fake);
            SetDefaultLoadFor<User>(this.Fake);
            SetDefaultLoadFor<Field>(this.Fake);
            SetDefaultLoadFor<FieldCollection>(this.Fake);
            SetDefaultLoadFor<ListItem>(this.Fake);
            SetDefaultLoadFor<ListItemCollection>(this.Fake);
            SetDefaultLoadFor<FieldNumber>(this.Fake);

            this.Fake.CastToOf1ClientObject<FieldNumber>((i) => new SimFieldNumber(i).Instance);

            this.Fake.CredentialsGet = () =>
            { 
                return this.Instance.Credentials; 
            };
            this.Fake.CredentialsSetICredentials = (credential) =>
            {
            };

            this.Fake.Dispose = () => { };
            this.Fake.DisposeBoolean = (b) => { };
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
