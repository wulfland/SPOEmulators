# SPOEmulators (beta)
Autor: Mike Kaufmann (http://writeabout.net)  
Version: 0.1-pre (__beta__)  
License: [MIT](https://github.com/wulfland/SPOEmulators/blob/master/LICENSE, "MIT license") 

## Description
SPOEmulators is a framework that helps you to write unit- and integration tests against Office 365 or SharePoint on premise using the client side object model (CSOM). It uses the Microsoft Fakes Framework to emulate the SharePoint or O365 CSOM.  

The benefit is, that you an write your tests against the real backend as integraton tests. If your code works you can add a little more effort to convert the test to an isolated unit test. Like this it executes much faster and yu do not have to wrestle with passwords in your app.config file. To ensure that the code still works you can add a seperate config file on your test machine an execute the same test as an integration test.   

>Note the framework is still a __beta__. Don't expect everything to be emulated! Please report bugs, issues and feature requests as git hub issues.

## Installation
SPOEmulators are available via nuget. Make sure to include prereleases and install the package SPOEmulators. You can install the package with the following command at the Package Manager Console:

```powershell
Install-Package SPOEmulators -IncludePrerelease
```

## Prerequisits
SPOEmulators depend on the Microsoft Fakes Framework. This was only available in Visual Studio 2012 in the Ultimate Edition until Update 3. In later Versions it is available in the Premium and Enterprise edition.

## Usage
You begin by creating a `SPOEmulationContext`. There are three options for the isolation level parameter.

```csharp
using (var context = new SPOEmulationContext(IsolationLevel.Fake)
{
    // you test code
}
``` 

Isolation Level | Description
----------------|-------------
Fake | All calls to the SharePoint CSOM are isolated. You do not need to provide any further parameters in this mode. A `ShimsContext` is also created for you. 
Integration | All calls are executed against the real SharePoint server. You need to specify minimum a URL if you are on premise. For O365 you also need to pass in the user name and passwort using the `ConnectionInformation` object. The `SPOEmulationContext` creates a seperate `ClientContext` that can be used to clean up or prepare the target site in your tests. It also creates a `ShimsContext`.
None | The same like integration but with absolute nothing enabled. No `ShimsContext` and no `ClientContext` are created.


## Sample
```csharp
    [TestClass]
    public class WebTests
    {
        IsolationLevel _isolationLevel = Settings.Default.IsolationLevel;
        ConnectionInformation _connectionInformation = new ConnectionInformation
        {
            Url = new Uri(Settings.Default.Url)
        };

        public WebTests()
        {
            if (_isolationLevel != IsolationLevel.Fake)
            {
                _connectionInformation.UserName = Settings.Default.User;
                _connectionInformation.SetPassword(Settings.Default.Password);
            }
        }

        [TestMethod]
        public void ProvisioningEngine_sets_web_title_to_department_name()
        {
            using (var context = new SPOEmulationContext(_isolationLevel, _connectionInformation))
            {
                // Get title
                context.ClientContext.Load(context.ClientContext.Web, w => w.Title);
                context.ClientContext.ExecuteQuery();
                var originalTitle = context.ClientContext.Web.Title;

                var sut = new ProvisioningEngine();
                
                // set title of web to department name
                sut.SetDepartmentTitle(context.ClientContext, context.ClientContext.Web);
                
                context.ClientContext.Load(context.ClientContext.Web, w => w.Title);
                context.ClientContext.ExecuteQuery();
                Assert.AreEqual("Department A", context.ClientContext.Web.Title);

                // Clean up title for integration testing
                if (_isolationLevel != IsolationLevel.Fake)
                {
                    context.ClientContext.Web.Title = originalTitle;
                    context.ClientContext.Web.Update();
                    context.ClientContext.ExecuteQuery();
                }
            }
        }
    }
```

