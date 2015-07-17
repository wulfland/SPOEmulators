namespace SPOEmulators
{
    using System;
    using System.Diagnostics;
    using System.Security;

    public enum ConnectionType
    {
        O365, OnPrem
    }

    [Serializable]
    [DebuggerDisplay("{Url}")]
    public class ConnectionInformation
    {
        private Uri _url = new Uri("http://localhost");
        private ConnectionType _connectionType = ConnectionType.OnPrem;

        public Uri Url
        {
            [DebuggerStepThrough]
            get
            {
                return _url;
            }

            [DebuggerStepThrough]
            set
            {
                _url = value;
                SetConnectionType(_url);
            }
        }

        public ConnectionType ConnectionType
        {
            [DebuggerStepThrough]
            get { return _connectionType; }
        }

        public string UserName { get; set; }

        public SecureString Password { get; set; }

        public void SetPassword(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException("plainText");

            Password = new SecureString();

            foreach (var c in plainText.ToCharArray())
            {
                Password.AppendChar(c);
            }

            Password.MakeReadOnly();
        }

        private void SetConnectionType(Uri url)
        {
            if (url.Host.ToUpperInvariant().EndsWith("SHAREPOINT.COM"))
            {
                _connectionType = ConnectionType.O365;
            }
            else
            {
                _connectionType = ConnectionType.OnPrem;    
            }
        }
    }
}
