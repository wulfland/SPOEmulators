using System;
using Microsoft.QualityTools.Testing.Fakes.Instances;
using Microsoft.QualityTools.Testing.Fakes.Shims;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Fakes;

namespace SPOEmulators.EmulatedTypes
{
    internal class SimUser : SimPrincipal, ICanIsolate<User, ShimUser>
    {
        public new ShimUser Fake
        {
            get;
            private set;
        }

        public new User Instance
        {
            get
            {
                return (User)base.Instance;
            }
        }

        public SimUser()
            : this(ShimRuntime.CreateUninitializedInstance<User>())
        {
        }

        public SimUser(User instance)
            : base(instance)
        {
            this.Fake = new ShimUser(instance)
            {
                EmailGet = () => this.Email,
                EmailSetString = (mail) => this.Email = mail,
                InitFromCreationInformationUserCreationInformation = (userCreationInformation) => InitFromCreationInformationUser(userCreationInformation),
                IsShareByEmailGuestUserGet = () => this.IsShareByEmailGuestUser,
                IsSiteAdminGet = () => this.IsSiteAdmin,
                IsSiteAdminSetBoolean = (b) => this.IsSiteAdmin = b,
                Update = () => { },
                UserIdGet = () => this.UserId
            };

            // todo: groupcollection

        }

        private void InitFromCreationInformationUser(UserCreationInformation userCreationInformation)
        {
            this.Email = userCreationInformation.Email;

            var principal = SimPrincipal.FromInstance(this.Instance);
            principal.LoginName = userCreationInformation.LoginName;
            principal.Title = userCreationInformation.Title;
        }

        public string Email { get; private set; }
        public bool IsShareByEmailGuestUser { get; private set; }
        public bool IsSiteAdmin { get; private set; }
        public UserIdInfo UserId { get; private set; }

        internal new static void Initialize()
        {
            SimPrincipal.Initialize();
            ShimUser.BehaveAsNotImplemented();
        }
    }
}
