using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SPOEmulators.Tests
{
    [TestClass]
    public class EmulatedTypesFakeTest
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

        [TestMethod]
        public void SimWeb_can_change_web_title_fake()
        {
            using (var context = new SPOEmulationContext(IsolationLevel.Fake))
            {
                // Get title
                context.ClientContext.Load(context.ClientContext.Web, w => w.Title);
                context.ClientContext.ExecuteQuery();
                var originalTitle = context.ClientContext.Web.Title;
                Assert.IsNotNull(originalTitle);

                // set title to something different
                context.ClientContext.Web.Title = "A new Title that is applied";
                context.ClientContext.ExecuteQuery();

                context.ClientContext.Load(context.ClientContext.Web, w => w.Title);
                context.ClientContext.ExecuteQuery();
                Assert.AreEqual("A new Title that is applied", context.ClientContext.Web.Title);

                // set title back
                context.ClientContext.Web.Title = originalTitle;
                context.ClientContext.ExecuteQuery();
            }   
        }
    }
}
