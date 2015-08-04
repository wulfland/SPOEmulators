namespace SPOEmulators.EmulatedTypes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint.Client;
    using Microsoft.SharePoint.Client.Fakes;

    internal class SimSite : Isolator<Site, ShimSite>
    {
        SimAudit _audit;
        SimWebCollection _webs = new SimWebCollection();
        // EventReceivers
        // Features
        // CustomActions

        public SimWeb RootWeb { get; set; }

        public SimWeb CurrentWeb { get; set; }

        public SimSite(Uri siteUrl)
            : this(ShimRuntime.CreateUninitializedInstance<Site>(), siteUrl)
        {
        }

        internal SimSite(Site instance, Uri siteUrl)
            : base(instance)
        {
            Fake.AllowCreateDeclarativeWorkflowGet = () => AllowCreateDeclarativeWorkflow;
            Fake.AllowCreateDeclarativeWorkflowSetBoolean = (b) => AllowCreateDeclarativeWorkflow = b;
            Fake.AllowDesignerGet = () => AllowDesigner;
            Fake.AllowDesignerSetBoolean = (b) => AllowDesigner = b;
            Fake.AllowMasterPageEditingGet = () => AllowMasterPageEditing;
            Fake.AllowMasterPageEditingSetBoolean = (b) => AllowMasterPageEditing = b;
            Fake.AllowRevertFromTemplateGet = () => AllowRevertFromTemplate;
            Fake.AllowRevertFromTemplateSetBoolean = (b) => AllowRevertFromTemplate = b;
            Fake.AllowSaveDeclarativeWorkflowAsTemplateGet = () => AllowSaveDeclarativeWorkflowAsTemplate;
            Fake.AllowSaveDeclarativeWorkflowAsTemplateSetBoolean = (b) => AllowSaveDeclarativeWorkflowAsTemplate = b;
            Fake.AllowSavePublishDeclarativeWorkflowGet = () => AllowSavePublishDeclarativeWorkflow;
            Fake.AllowSavePublishDeclarativeWorkflowSetBoolean = (b) => AllowSavePublishDeclarativeWorkflow = b;
            Fake.AllowSelfServiceUpgradeEvaluationGet = () => AllowSelfServiceUpgradeEvaluation;
            Fake.AllowSelfServiceUpgradeEvaluationSetBoolean = (b) => AllowSelfServiceUpgradeEvaluation = b;
            Fake.AllowSelfServiceUpgradeGet = () => AllowSelfServiceUpgrade;
            Fake.AllowSelfServiceUpgradeSetBoolean = (b) => AllowSelfServiceUpgrade = b;
            Fake.AuditGet = () => this.Audit;
            Fake.AuditLogTrimmingRetentionGet = () => AuditLogTrimmingRetention;
            Fake.AuditLogTrimmingRetentionSetInt32 = (i) => AuditLogTrimmingRetention = i;
            Fake.CanUpgradeGet = () => CanUpgrade;
            Fake.CompatibilityLevelGet = () => CompatibilityLevel;
            Fake.EventReceiversGet = () => this.EventReceivers;
            Fake.FeaturesGet = () => this.Features;
            Fake.GetCatalogInt32 = (typeCatalog) => this.GetCatalog(typeCatalog);
            Fake.IdGet = () => Id;
            Fake.OpenWebByIdGuid = (id) =>
            {
                return OpenWeb((w) => w.Id == id);
            };
            Fake.OpenWebString = (url) =>
            {
                // todo: check url format
                return OpenWeb((w) => w.Url == url);
            };
            Fake.OwnerGet = () => Owner;
            Fake.OwnerSetUser = (u) => this.Owner = u;
            Fake.PrimaryUriGet = () => PrimaryUri;
            Fake.RootWebGet = () => RootWeb.Instance;
            //Fake.ServerRelativeUrlGet = () => ServerRelativeUrl;
            //Fake.UrlGet = () => Url;
            Fake.UserCustomActionsGet = () => UserCustomActions;

            this.RootWeb = new SimWeb 
            {
                Site = Fake, 
                Title = "Team Site"
            };

            _webs.Add(RootWeb.Instance);

            this.CurrentWeb = new SimWeb
            {
                Site = Fake,
                Title = "Team Site"
            };

            _webs.Add(CurrentWeb.Instance);
        }

        public bool AllowCreateDeclarativeWorkflow { get; set; }

        public bool AllowDesigner { get; set; }

        public bool AllowMasterPageEditing { get; set; }

        public bool AllowRevertFromTemplate { get; set; }

        public bool AllowSaveDeclarativeWorkflowAsTemplate { get; set; }

        public bool AllowSavePublishDeclarativeWorkflow { get; set; }

        public bool AllowSelfServiceUpgradeEvaluation { get; set; }

        public bool AllowSelfServiceUpgrade { get; set; }

        public Audit Audit
        {
            get
            {
                if (_audit == null)
                {
                    _audit = new SimAudit();
                }

                return _audit.Instance;
            }
        }

        public int AuditLogTrimmingRetention { get; set; }

        public bool CanUpgrade { get; set; }

        public int CompatibilityLevel { get; set; }

        public EventReceiverDefinitionCollection EventReceivers { get; set; }

        public FeatureCollection Features { get; set; }

        public Guid Id { get; set; }

        public string PrimaryUri { get; set; }

        public Uri Url { get; set; }

        public UserCustomActionCollection UserCustomActions { get; set; }

        public Web OpenWeb(Func<Web, bool> predicate)
        {
            return _webs.First(predicate);
        }

        private List GetCatalog(int typeCatalog)
        {
            return new ShimList().Instance;
        }


        public User Owner { get; set; }

    }
}
