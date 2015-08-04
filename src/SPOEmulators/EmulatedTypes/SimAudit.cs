namespace SPOEmulators.EmulatedTypes
{
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint.Client;
    using Microsoft.SharePoint.Client.Fakes;

    internal class SimAudit : Isolator<Audit, ShimAudit>
    {
        public SimAudit()
            : this(ShimRuntime.CreateUninitializedInstance<Audit>())
        {
        }

        public SimAudit(Audit instance)
            : base(instance)
        {
            this.Fake.AuditFlagsGet = () => this.AuditFlags;
            this.Fake.AuditFlagsSetAuditMaskType = (f) => this.AuditFlags = f;
            this.Fake.Update = () => { };
        }

        public AuditMaskType AuditFlags { get; set; }
    }
}
