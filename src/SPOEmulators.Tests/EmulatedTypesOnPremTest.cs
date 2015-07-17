using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPOEmulators.Tests.Properties;

namespace SPOEmulators.Tests
{
    [TestClass]
    public class EmulatedTypesOnPremTest
    {
        IsolationLevel _isolationLevel = IsolationLevel.Integration;
        string _url = Settings.Default.OnPremUrl; 

        [TestMethod]
        public void SimClientContext_creates_Web_for_fake()
        {
            using (var context = new SPOEmulationContext(_isolationLevel, _url))
            {
                Assert.IsNotNull(context.ClientContext);
                Assert.IsNotNull(context.ClientContext.Web);
            }
        }
    }
}
