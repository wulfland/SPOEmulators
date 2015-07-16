namespace SPOEmulators
{
    using System;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.SharePoint.Client;
using SPOEmulators.EmulatedTypes;
    //using SPOEmulators.EmulatedTypes;

    /// <summary>
    /// The emulation context for SharePoint emulation.
    /// </summary>
    public class SPOEmulationContext : IDisposable
    {
        readonly IDisposable shimsContext;
        readonly IsolationLevel isolationLevel;
        ClientContext clientContext;
        bool disposed;

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
                return isolationLevel;
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
                return clientContext;
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
            this.isolationLevel = isolationLevel;

            switch (isolationLevel)
            {
                case IsolationLevel.Fake:
                    // create shim context
                    shimsContext = ShimsContext.Create();

                    // initialize all simulated types
                    InitializeSimulatedAPI();

                    // Set reference to the simulated site and web in the context
                    //site = SPContext.Current.Site;
                    // web = SPContext.Current.Web;
                    clientContext = new SimClientContext().Instance;
                    break;
                case IsolationLevel.Integration:
                    // create shim context
                    shimsContext = ShimsContext.Create();

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
                    // Do not use shimscontext or any kind of fake. Load the real spite and spweb objects from sharpoint.
                    //site = new SPSite(url);
                    //web = site.OpenWeb();
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
            if (!disposed)
            {
                if (shimsContext != null)
                    shimsContext.Dispose();

                disposed = true;
            }
        }
    }
}
