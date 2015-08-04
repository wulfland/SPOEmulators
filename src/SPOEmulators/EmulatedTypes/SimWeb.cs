﻿namespace SPOEmulators.EmulatedTypes
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

    internal class SimWeb : Isolator<Web, ShimWeb>
    {
        private readonly SimListCollection _lists;
        
        
        private Guid? _id;
        private string _title;
        private string _url;

        public ShimSite Site { get; set; }

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
            set
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

        public string Description { get; set; }

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

        public SimWeb()
            : this(ShimRuntime.CreateUninitializedInstance<Web>())
        {
        }

        public SimWeb(Web instance)
            : base(instance)
        {
            _lists = new SimListCollection();

            this.Fake.IdGet = (() => this.ID);
            this.Fake.UrlGet = (() => this.Url);
            this.Fake.TitleGet = (() => this.Title);
            this.Fake.TitleSetString = ((s) => this._title = s);
            this.Fake.DescriptionGet = () => this.Description;
            this.Fake.DescriptionSetString = (s) => this.Description = s;
            this.Fake.Update = () => { };
            this.Fake.ListsGet = () => _lists.Instance;

            this.Site = new ShimSite();
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
