using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPOEmulators.Tests.Properties;

namespace SPOEmulators.Tests
{
    [TestClass]
    public class SPOEmulationContextTest
    {
        [TestMethod]
        public void SPOEmulationContext_creates_shimContext_for_fake()
        {
            using (var sut = new SPOEmulationContext(IsolationLevel.Fake))
            {
                var shimsContext = new PrivateObject(sut).GetField("_shimsContext");

                Assert.IsNotNull(shimsContext);
            }
        }

        [TestMethod]
        public void SPOEmulationContext_creates_shimContext_for_integration()
        {
            using (var sut = new SPOEmulationContext(IsolationLevel.Integration, Settings.Default.OnPremUrl))
            {
                var shimsContext = new PrivateObject(sut).GetField("_shimsContext");

                Assert.IsNotNull(shimsContext);
            }
        }

        [TestMethod]
        public void SPOEmulationContext_does_NOT_create_shimContext_for_none()
        {
            using (var sut = new SPOEmulationContext(IsolationLevel.None, Settings.Default.OnPremUrl))
            {
                var shimsContext = new PrivateObject(sut).GetField("_shimsContext");

                Assert.IsNull(shimsContext);
            }
        }
    }
}
