namespace SPOEmulators
{
    using System;
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.SharePoint.Client;
    using SPOEmulators.EmulatedTypes;


    /// <summary>
    /// The emulation context for SharePoint emulation.
    /// </summary>
    public class SPOEmulationContext : IDisposable
    {
        readonly IDisposable    _shimsContext;
        readonly IsolationLevel _isolationLevel;
        ClientContext           _clientContext;
        bool                    _disposed;

        /// <summary>
        /// Gets the isolation level.
        /// </summary>
        /// <value>
        /// The isolation level.
        /// </value>
        public IsolationLevel IsolationLevel
        {
            [System.Diagnostics.DebuggerStepThrough]
            get
            {
                return _isolationLevel;
            }
        }

        /// <summary>
        /// Gets the current client context.
        /// </summary>
        /// <value>
        /// The current client context.
        /// </value>
        public ClientContext ClientContext
        {
            [System.Diagnostics.DebuggerStepThrough]
            get
            {
                return _clientContext;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SPOEmulationContext"/> class.
        /// </summary>
        /// <param name="isolationLevel">The level.</param>
        public SPOEmulationContext(IsolationLevel isolationLevel)
            : this(isolationLevel, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SPOEmulationContext"/> class.
        /// </summary>
        /// <param name="isolationLevel">The level.</param>
        public SPOEmulationContext(IsolationLevel isolationLevel, string url)
        {
            this._isolationLevel = isolationLevel;

            switch (isolationLevel)
            {
                case IsolationLevel.Fake:
                    // create shim context
                    _shimsContext = ShimsContext.Create();

                    // initialize all simulated types
                    InitializeSimulatedAPI();

                    // Set reference to the simulated site and web in the context
                    //site = SPContext.Current.Site;
                    // web = SPContext.Current.Web;
                    _clientContext = new SimClientContext().Instance;
                    break;
                case IsolationLevel.Integration:
                    // create shim context
                    _shimsContext = ShimsContext.Create();

                    _clientContext = new ClientContext(url);

                    // Load the real spite and spweb objects from sharpoint
                    //site = new SPSite(url);
                    //web = site.OpenWeb();

                    // Inject the real webs to the context using shims.
                    //ShimSPContext.CurrentGet = () => new ShimSPContext
                    //{
                    //    SiteGet = () => this.site,
                    //    WebGet = () => this.web
                    //};
                    break;
                case IsolationLevel.None:
                    _clientContext = new ClientContext(url);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        private static void InitializeSimulatedAPI()
        {
            SimClientContext.Initialize();
            SimWeb.Initialize();

            //SimHttpContext.Initialize();
            //SimHttpRequest.Initialize();
            //SimHttpResponse.Initialize();
            //SimSPContext.Initialize();
            //SimSPEventPropertiesBase.Initialize();
            //SimSPField.Initialize();
            //SimSPFieldCollection.Initialize();
            //SimSPFieldIndex.Initialize();
            //SimSPFieldIndexCollection.Initialize();
            //SimSPFieldLink.Initialize();
            //SimSPFieldLinkCollection.Initialize();
            //SimSPFieldUrlValue.Initialize();
            //SimSPFile.Initialize();
            //SimSPFileCollection.Initialize();
            //SimSPFolder.Initialize();
            //SimSPFolderCollection.Initialize();
            //SimSPItem.Initialize();
            //SimSPItemEventDataCollection.Initialize();
            //SimSPItemEventProperties.Initialize();
            //SimSPList.Initialize();
            //SimSPListCollection.Initialize();
            //SimSPListEventProperties.Initialize();
            //SimSPListItem.Initialize();
            //SimSPListItemCollection.Initialize();
            //SimSPSecurableObject.Initialize();
            //SimSPSecurity.Initialize();
            //SimSPSite.Initialize();
            //SimSPWeb.Initialize();
            //SimSPWebCollection.Initialize();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (_clientContext != null)
                    _clientContext.Dispose();

                if (_shimsContext != null)
                    _shimsContext.Dispose();

                _disposed = true;
            }
        }
    }
}
