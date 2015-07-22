using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPOEmulators;

namespace CSOMSample
{
    [TestClass]
    public class WebTests
    {
        [TestMethod]
        public void SimWeb_can_change_web_title_fake()
        {
            using (var context = new SPOEmulationContext(IsolationLevel.Fake))
            {

                // set title for fake
                context.ClientContext.Web.Title = "Teamsite";


                // Get title
                context.ClientContext.Load(context.ClientContext.Web);
                context.ClientContext.Load(context.ClientContext.Web, w => w.Title);
                context.ClientContext.ExecuteQuery();
                var originalTitle = context.ClientContext.Web.Title;
                Assert.IsNotNull(originalTitle);

                // set title to something different
                context.ClientContext.Web.Title = "A new Title that is applied";
                context.ClientContext.Web.Update();
                context.ClientContext.ExecuteQuery();

                context.ClientContext.Load(context.ClientContext.Web, w => w.Title);
                context.ClientContext.ExecuteQuery();
                Assert.AreEqual("A new Title that is applied", context.ClientContext.Web.Title);
            }
        }


    }
}
