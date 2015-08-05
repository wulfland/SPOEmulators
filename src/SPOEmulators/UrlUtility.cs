namespace SPOEmulators
{
    using System;
    using System.Text.RegularExpressions;

    internal static class UrlUtility
    {
        public static bool IsUrlRootWeb(Uri webUrl)
        {
            var relativeUrl = webUrl.AbsolutePath.TrimEnd('/') + '/';

            relativeUrl = StripKnownManagedPath(relativeUrl, "sites");
            relativeUrl = StripKnownManagedPath(relativeUrl, "teams");
            relativeUrl = StripKnownManagedPath(relativeUrl, "portals");
            
            return relativeUrl == "/"; 
        }

        public static Uri GetRootWebUri(Uri webUrl)
        {
            var relativeUrl = webUrl.AbsolutePath.TrimEnd('/') + '/';

            var root = webUrl.AbsoluteUri.Replace(webUrl.AbsolutePath, string.Empty);
            root = AddKnownManagedPath(relativeUrl, "sites", root);
            root = AddKnownManagedPath(relativeUrl, "teams", root);
            root = AddKnownManagedPath(relativeUrl, "portals", root);

            return new Uri(root);
        }

        private static string StripKnownManagedPath(string relativeUrl, string knownManagedPath)
        {
            if (relativeUrl.StartsWith("/" + knownManagedPath + "/", StringComparison.OrdinalIgnoreCase))
            {
                var regex = new Regex("/" + knownManagedPath + "/.+?/", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                relativeUrl = '/' + regex.Replace(relativeUrl, string.Empty);
            }

            return relativeUrl;
        }

        private static string AddKnownManagedPath(string relativeUrl, string knownManagedPath, string root)
        {
            if (relativeUrl.StartsWith("/" + knownManagedPath + "/", StringComparison.OrdinalIgnoreCase))
            {
                var regex = new Regex("/" + knownManagedPath + "/.+?/", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                var match = regex.Match(relativeUrl);
                root += match.Value;
            }

            return root;
        }
    }
}
