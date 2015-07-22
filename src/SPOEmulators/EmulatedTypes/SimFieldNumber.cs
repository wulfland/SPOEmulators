namespace SPOEmulators.EmulatedTypes
{
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint.Client;
    using Microsoft.SharePoint.Client.Fakes;

    internal class SimFieldNumber : Isolator<FieldNumber, ShimFieldNumber>
    {

        SimField Field { get; set; }

        public SimFieldNumber()
            : this(ShimRuntime.CreateUninitializedInstance<FieldNumber>())
        {
        }

        public SimFieldNumber(ClientObject clientObject)
            : this(ShimRuntime.CreateUninitializedInstance<FieldNumber>())
        {
            this.Field = SimField.FromInstance((Field)clientObject);
        }

        public SimFieldNumber(FieldNumber instance)
            : base(instance)
        {
            this.Fake.MaximumValueGet = () => this.MaximumValue;
            this.Fake.MaximumValueSetDouble = (d) => this.MaximumValue = d;
            this.Fake.MinimumValueGet = () => this.MinimumValue;
            this.Fake.MinimumValueSetDouble = (d) => this.MinimumValue = d;

            new ShimField(instance)
            {
                Update = () => { }
            };
            
        }



        public double MaximumValue { get; set; }

        public double MinimumValue { get; set; }
    }
}
