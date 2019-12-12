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
        private static IWebDriver uniqueInstanceWebDriver = null;

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
        public static IWebDriver getWebDriver(string browser)
        {
            lock (typeof(IWebDriver))
            {
                if (WebDriverFactory.uniqueInstanceWebDriver == null)
                {
                    Console.WriteLine(String.Format("Current Directory - {0}", System.IO.Directory.GetCurrentDirectory()));
                    switch (browser.ToLower())
                    {
                        case ("chrome"):
                            ChromeOptions options = new ChromeOptions();
                            options.AddUserProfilePreference("download.prompt_for_download", true);
                            options.AddArguments("--disable-extensions");
                            //options.AddArgument("--no-sandbox");
                            uniqueInstanceWebDriver = new ChromeDriver(System.IO.Directory.GetCurrentDirectory(),options);
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
                            uniqueInstanceWebDriver = new InternetExplorerDriver(System.IO.Directory.GetCurrentDirectory(), ieOptions);
                            int implicitWaitTime = 20;
                            uniqueInstanceWebDriver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(implicitWaitTime));
                            uniqueInstanceWebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(implicitWaitTime));
                            break;
                        case ("phantomjs"):
                            uniqueInstanceWebDriver = new PhantomJSDriver();
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
                            uniqueInstanceWebDriver = new EdgeDriver(serverPath, edgeOptions);
                            uniqueInstanceWebDriver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(10));
                            break;
                        case ("firefox"):
                            FirefoxOptions firefoxOptions = new FirefoxOptions();
                            //add Firefox Options 
                            uniqueInstanceWebDriver = new FirefoxDriver();
                            break;
                    }

                    uniqueInstanceWebDriver.Manage().Window.Maximize();
                }
                return WebDriverFactory.uniqueInstanceWebDriver;
            }
        }

        /// <summary>
        /// Gets the web driver.
        /// </summary>
        /// <returns></returns>
        public static IWebDriver getWebDriver()
        {
            return WebDriverFactory.uniqueInstanceWebDriver;
        }

        /// <summary>
        /// Frees this instance.
        /// </summary>
        public static void Free()
        {
            WebDriverFactory.uniqueInstanceWebDriver = null;
        }
    }
}
