namespace SPOEmulators
{
    using System;
    using System.Net;
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.SharePoint.Client;
    using SPOEmulators.EmulatedTypes;


    /// <summary>
    /// The emulation context for SharePoint emulation.
    /// </summary>
    public class SPOEmulationContext : IDisposable
    {
        readonly IDisposable _shimsContext;
        readonly IsolationLevel _isolationLevel;
        ClientContext _clientContext;
        bool _disposed;

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
            : this(isolationLevel, new ConnectionInformation())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SPOEmulationContext"/> class.
        /// </summary>
        /// <param name="isolationLevel">The level.</param>
        /// <param name="url">The url of the target web.</param>
        public SPOEmulationContext(IsolationLevel isolationLevel, string url)
            : this(isolationLevel, new ConnectionInformation { Url = new Uri(url) })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SPOEmulationContext"/> class.
        /// </summary>
        /// <param name="isolationLevel">The level.</param>
        /// <param name="connectionInformation">The connection informations for the target web.</param>
        public SPOEmulationContext(IsolationLevel isolationLevel, ConnectionInformation connectionInformation)
        {
            this._isolationLevel = isolationLevel;

            switch (isolationLevel)
            {
                case IsolationLevel.Fake:
                    // create shim context
                    _shimsContext = ShimsContext.Create();

                    // initialize all simulated types
                    _clientContext = InitializeSimulatedAPI(connectionInformation.Url);
                    break;
                case IsolationLevel.Integration:
                    // create shim context
                    _shimsContext = ShimsContext.Create();
                    Connect(connectionInformation);
                    break;
                case IsolationLevel.None:
                    Connect(connectionInformation);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        public void SetQueryResultsForFakeList(List instance, Func<CamlQuery, ListItemCollection> query)
        {
            if (_isolationLevel == SPOEmulators.IsolationLevel.Fake)
            {
                SimList.SetQueryResults(instance, query);
            }
        }

        public void SetQueryResultsForFakeList(List instance, params ListItem[] items)
        {
            if (_isolationLevel == SPOEmulators.IsolationLevel.Fake)
            {
                SimList.SetQueryResults(instance, items);
            }
        }

        private static ClientContext InitializeSimulatedAPI(Uri url)
        {
            var context = SimClientContext.Initialize(url);
            SimClientRuntimeContext.Initialize();
            SimUser.Initialize();
            SimWeb.Initialize();
            SimListItemCollection.Initialize();
            SimListCollection.Initialize();

            return context;
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
                if (_clientContext != null && _isolationLevel != IsolationLevel.Fake)
                    _clientContext.Dispose();

                if (_shimsContext != null)
                    _shimsContext.Dispose();

                _disposed = true;
            }
        }

        private void Connect(ConnectionInformation connectionInformation)
        {
            try
            {
                _clientContext = new ClientContext(connectionInformation.Url.AbsoluteUri);

                if (!string.IsNullOrWhiteSpace(connectionInformation.UserName))
                {
                    if (connectionInformation.ConnectionType == ConnectionType.O365)
                    {
                        _clientContext.Credentials = new SharePointOnlineCredentials(connectionInformation.UserName, connectionInformation.Password);
                    }
                    else
                    {
                        _clientContext.Credentials = new NetworkCredential(connectionInformation.UserName, connectionInformation.Password);
                    }
                }

                _clientContext.ExecuteQuery();
            }
            catch (Exception ex)
            {
                var msg = "Error connecting to the {0} site {1} with the given credentials. {2}";
                var format = string.Format(msg, connectionInformation.ConnectionType, connectionInformation.Url.AbsoluteUri, ex.Message);
                throw new InvalidOperationException(format);
            }
        }
    }
}
