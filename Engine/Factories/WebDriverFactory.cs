using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
namespace Engine.Factories
{
    /// <summary>
    /// @Author - Debasish Pradhan
    /// </summary>
    public class WebDriverFactory
    {
       
        /// <summary>
        /// Prevents a default instance of the <see cref="WebDriverFactory"/> class from being created.
        /// </summary>
        private WebDriverFactory()
        {
        }

        /// <summary>
        /// Gets the web driver.
        /// </summary>
        /// <param name="browser">The browser.</param>
        /// <returns></returns>
        public static IWebDriver getWebDriver(String browser)
        {

            IWebDriver wd = null;
            Console.WriteLine(String.Format("Current Directory - {0}", System.IO.Directory.GetCurrentDirectory()));
            switch (browser.ToLower())
            {
                case ("chrome"):
                    ChromeOptions options = new ChromeOptions();
                    options.AddUserProfilePreference("download.prompt_for_download", true);
                    options.AddArguments("--disable-extensions");
                    //options.AddArgument("--no-sandbox");
                    wd = new ChromeDriver(System.IO.Directory.GetCurrentDirectory(), options);
                    break;
                case ("ie"):
                    var ieOptions = new InternetExplorerOptions
                    {
                        EnableNativeEvents = false,
                        UnexpectedAlertBehavior = InternetExplorerUnexpectedAlertBehavior.Accept,
                        IntroduceInstabilityByIgnoringProtectedModeSettings = true,
                        EnablePersistentHover = true,
                        IgnoreZoomLevel = true,
                        EnsureCleanSession = true,
                        RequireWindowFocus = true
                    };
                    ieOptions.AddAdditionalCapability("disable-popup-blocking", true);
                    wd = new InternetExplorerDriver(System.IO.Directory.GetCurrentDirectory(), ieOptions);
                    int implicitWaitTime = 20;
                    wd.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(implicitWaitTime));
                    wd.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(implicitWaitTime));
                    break;
                case ("phantomjs"):
                    wd = new PhantomJSDriver();
                    break;
                case ("edge"):
                    string serverPath = "Microsoft Web Driver";
                    // location for MicrosoftWebDriver.exe
                    if (System.Environment.Is64BitOperatingSystem)
                    {
                        serverPath = System.IO.Path.Combine(System.Environment.ExpandEnvironmentVariables("%ProgramFiles(x86)%"), serverPath);
                    }
                    else
                    {
                        serverPath = System.IO.Path.Combine(System.Environment.ExpandEnvironmentVariables("%ProgramFiles%"), serverPath);
                    }

                    EdgeOptions edgeOptions = new EdgeOptions();
                    edgeOptions.PageLoadStrategy = EdgePageLoadStrategy.Eager;
                    wd = new EdgeDriver(serverPath, edgeOptions);
                    wd.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(10));
                    break;
                case ("firefox"):
                    FirefoxOptions firefoxOptions = new FirefoxOptions();
                    //add Firefox Options 
                    wd = new FirefoxDriver();
                    break;
            }

            wd.Manage().Window.Maximize();
            return wd;
               
        }

    }
}
