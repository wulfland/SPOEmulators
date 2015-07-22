namespace SPOEmulators.EmulatedTypes
{
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint.Client;
    using Microsoft.SharePoint.Client.Fakes;

    internal class SimClientObjectCollection : CollectionIsolator<ClientObject, ClientObjectCollection, ShimClientObjectCollection>
    {
        public SimClientObjectCollection()
            : this(ShimRuntime.CreateUninitializedInstance<ClientObjectCollection>())
        {
        }

        public SimClientObjectCollection(ClientObjectCollection instance)
            : base(instance)
        {
            this.Fake.CountGet = () => this.Count;
        }
    }
}
