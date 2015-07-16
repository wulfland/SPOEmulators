using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SPOEmulators.Tests
{
    [TestClass]
    public class EmulatedTypesTest
    {
        [TestMethod]
        public void SimClientContext_creates_Web_for_fake()
        {
            using (var context = new SPOEmulationContext(IsolationLevel.Fake))
            {
                Assert.IsNotNull(context.ClientContext);
                Assert.IsNotNull(context.ClientContext.Web);

                context.ClientContext.ExecuteQuery();
            }
        }
    }
}
