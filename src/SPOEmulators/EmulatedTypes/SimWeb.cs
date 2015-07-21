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
        private readonly SimListCollection _lists;
        
        
        private Guid? _id;
        private string _title;
        private string _url;

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
                return this._url;
            }
            private set
            {
                this._url = value;
            }
        }

        public string Title
        {
            get
            {
                return this._title;
            }
            set
            {
                this._title = value;
            }
        }

        public Guid ID
        {
            get
            {
                if (!this._id.HasValue)
                {
                    this._id = new Guid?(Guid.NewGuid());
                }
                return this._id.Value;
            }
            set
            {
                this._id = new Guid?(value);
            }
        }

        public SimListCollection Lists
        {
            get
            {
                return _lists;
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
            _lists = new SimListCollection();

            var shimWeb = new ShimWeb(instance);
            shimWeb.IdGet = (() => this.ID);
            shimWeb.UrlGet = (() => this.Url);
            shimWeb.TitleGet = (() => this.Title);
            shimWeb.TitleSetString = ((s) => this._title = s);
            shimWeb.Update = () => { };
            shimWeb.ListsGet = () => _lists.Instance;

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
