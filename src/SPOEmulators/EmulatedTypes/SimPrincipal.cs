namespace SPOEmulators.EmulatedTypes
{
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint.Client;
    using Microsoft.SharePoint.Client.Fakes;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.SharePoint.Client.Utilities;
    using Microsoft.QualityTools.Testing.Fakes.Instances;

    internal class SimPrincipal : Isolator<Principal, ShimPrincipal>
    {
        public SimPrincipal()
            : this(ShimRuntime.CreateUninitializedInstance<Principal>())
        {
        }

        public SimPrincipal(Principal instance)
            : base(instance)
        {
            this.Fake.IdGet = () => this.Id;
            this.Fake.IsHiddenInUIGet = () => this.IsHiddenInUI;
            this.Fake.LoginNameGet = () => this.LoginName;
            this.Fake.PrincipalTypeGet = () => this.PrincipalType;
            this.Fake.TitleGet = () => this.Title;
            this.Fake.TitleSetString = (title) => this.Title = title;
        }

        public int Id { get; set; }
        public bool IsHiddenInUI { get; set; }
        public string LoginName { get; set; }
        public PrincipalType PrincipalType { get; set; }
        public string Title { get; set; }

        public static SimPrincipal FromInstance(Principal instance)
        {
            return InstancedPool.CastAsInstanced<Principal, SimPrincipal>(instance);
        }

        internal static void Initialize()
        {
            ShimPrincipal.BehaveAsNotImplemented();
        }
    }
}
