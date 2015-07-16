using System;
using System.Reflection;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SPOEmulators.Tests
{
    [TestClass]
    public class SPOEmulationContextTest
    {
        [TestMethod]
        public void SPOEmulationContext_creates_shimContext_for_fake()
        {
            var sut = new SPOEmulationContext(IsolationLevel.Fake);
            var shimsContext = new PrivateObject(sut).GetField("shimsContext");

            Assert.IsNotNull(shimsContext);
        }

        [TestMethod]
        public void SPOEmulationContext_creates_shimContext_for_integration()
        {
            var sut = new SPOEmulationContext(IsolationLevel.Integration);
            var shimsContext = new PrivateObject(sut).GetField("shimsContext");

            Assert.IsNotNull(shimsContext);
        }

        [TestMethod]
        public void SPOEmulationContext_does_NOT_create_shimContext_for_none()
        {
            var sut = new SPOEmulationContext(IsolationLevel.None);
            var shimsContext = new PrivateObject(sut).GetField("shimsContext");

            Assert.IsNull(shimsContext);
        }
    }
}
