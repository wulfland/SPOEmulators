namespace SPOEmulators.EmulatedTypes
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.QualityTools.Testing.Fakes.Instances;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint.Client;
    using Microsoft.SharePoint.Client.Fakes;

    internal class SimWeb : Isolator<Web, ShimWeb>, IInstanced<Web>, IInstanced
    {

        private Guid? id;
        private string title;
        private string url;

        public User CurrentUser
        {
            get;
            set;
        }

        public WebInformation ParentWeb
        {
            get;
            set;
        }


        public string Url
        {
            get
            {
                return this.url;
            }
            private set
            {
                this.url = value;
            }
        }

        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
            }
        }

        public Guid ID
        {
            get
            {
                if (!this.id.HasValue)
                {
                    this.id = new Guid?(Guid.NewGuid());
                }
                return this.id.Value;
            }
            set
            {
                this.id = new Guid?(value);
            }
        }

        public new ShimWeb Fake
        {
            get;
            private set;
        }

        public new Web Instance
        {
            get
            {
                return (Web)base.Instance;
            }
        }

        public SimWeb()
            : this(ShimRuntime.CreateUninitializedInstance<Web>())
        {
        }

        public SimWeb(Web instance)
            : base(instance)
        {

            var shimWeb = new ShimWeb(instance);
            shimWeb.IdGet = (() => this.ID);
            shimWeb.UrlGet = (() => this.Url);
            shimWeb.TitleGet = (() => this.Title);
            shimWeb.TitleSetString = ((s) => this.title = s);
            shimWeb.Update = () => { };

            this.Fake = shimWeb;
        }

        public static SimWeb FromInstance(Web instance)
        {
            return InstancedPool.CastAsInstanced<Web, SimWeb>(instance);
        }

        internal static void Initialize()
        {
            ShimWeb.BehaveAsNotImplemented();
        }
    }
}
