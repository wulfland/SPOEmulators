namespace SPOEmulators.EmulatedTypes
{
    using Microsoft.QualityTools.Testing.Fakes.Instances;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint.Client;
    using Microsoft.SharePoint.Client.Fakes;

    internal class SimList : Isolator<List, ShimList>
    {
        readonly SimFieldCollection _fields = new SimFieldCollection();

        public string Description { get; set; }

        public string Title { get; set; }

        public bool EnableVersioning { get; set; }

        public SimFieldCollection Fields
        {
            get { return _fields; }
        }

        public SimList()
            : this(ShimRuntime.CreateUninitializedInstance<List>())
        {
        }

        protected SimList(List instance)
            : base(instance)
        {
            new ShimList(instance)
            {
                TitleGet = () => this.Title,
                TitleSetString = (s) => this.Title = s,
                DescriptionGet = () => this.Description,
                DescriptionSetString = (s) => this.Description = s,
                EnableVersioningGet = () => this.EnableVersioning,
                EnableVersioningSetBoolean = (b) => this.EnableVersioning = b,
                Update = () => { },
                FieldsGet = () => this.Fields.Instance,

            };
        }



        public static SimList FromInstance(List instance)
        {
            return InstancedPool.CastAsInstanced<List, SimList>(instance);
        }

        internal static void Initialize()
        {
            ShimList.BehaveAsNotImplemented();
        }
    }
}
