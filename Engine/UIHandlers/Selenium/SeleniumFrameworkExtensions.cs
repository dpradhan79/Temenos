using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using TestReporter;
using Engine.Setup;
using OpenQA.Selenium.Support.UI;

namespace Engine.UIHandlers.Selenium
{
    /// <summary>
    /// @Author - Debasish Pradhan
    /// </summary>
    public static class SeleniumFrameworkExtensions
    {
        #region Private Members
        private static IReporter testReport = EngineSetup.TestReport;
        private static object activeContext = null;
        private static IWebDriver activeDriver = null;
        private static Type[] ignoredExceptions = new Type[] {typeof(NoSuchElementException),
                                                              typeof(ElementNotVisibleException),
                                                              typeof(StaleElementReferenceException),
                                                              typeof(ArgumentOutOfRangeException),
                                                              typeof(InvalidOperationException),
                                                              typeof(WebDriverTimeoutException),
                                                              typeof(WebDriverException) };
        internal static Actions activeDriverActions = null;

        #endregion

        #region Non-Public Methods

        /// <summary>
        /// if an error has occurred, try to complete the current function so the test can continue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="defaultTimeout"></param>
        /// <param name="logMsg"></param>
        /// <returns></returns>
        internal static T CompleteAction<T>(Func<T> func, int defaultTimeout, IWebDriver _driver, string logMsg = null)
        {
            bool errorHasOccurred = false;
            var begin = DateTime.UtcNow;
            while (true)
            {
                try
                {
                    if (errorHasOccurred && activeContext != null)
                    {
                        activeDriver.SwitchTo().DefaultContent();
                        if (activeContext is string)
                        {
                            //activeDriver.SwitchTo().Frame((dynamic)activeContext);
                            activeDriver.SwitchTo().Frame((string)activeContext);
                        }
                        else if (activeContext is int)
                        {
                            activeDriver.SwitchTo().Frame((int)activeContext);
                        }
                        else if (activeContext is By)
                        {
                            activeDriver.SwitchTo().Frame(activeDriver.FindElement((By)activeContext));
                        }
                    }
                    var returnObj = func.Invoke();
                    if (!String.IsNullOrEmpty(logMsg))
                    {

                        testReport.LogSuccess("UI Action", logMsg);
                    }
                    return returnObj;
                }

                catch (Exception ex)
                {
                    errorHasOccurred = true;
                    if ((DateTime.UtcNow - begin).TotalSeconds >= defaultTimeout)
                    {
                        //we should check kdashboard_EditProfileButtonind of exception it is ...currently, I am doing temp fix
                        if (ex.StackTrace.Contains("UnpackAndThrowOnError")
                            || ex.Message.Contains("Timed out waiting for page to load.")
                            || ex.Message.Contains("The HTTP request")
                            || ex.Message.Contains("timed out after 60 seconds")
                            || ex.Message.Contains("Element is obscured"))
                        {

                            testReport.LogWarning("UI Action", String.Format("From IgnoredException: Exception Type - {0}, Error: {1} StackTrace: {2}", ex.GetType(), ex.Message, ex.StackTrace),EngineSetup.GetScreenShotPath());
                            Thread.Sleep(5000);
                            errorHasOccurred = false;
                            throw;
                        }
                        else
                        {
                            testReport.LogException(ex, EngineSetup.GetScreenShotPath());
                            throw;
                        }

                    }
                    //maybe do an else if here for situations in which the time didnt take longer than the timeout, but a longer time than expected
                    System.Threading.Thread.Sleep(100);
                }
            }
        }

        public static IWebElement wait(this IWebDriver driver, Func<IWebDriver, IWebElement> condition, double timeout = 10, string message = "", double pollingIntervalSec = 0.5)
        {
            try
            {
                WebDriverWait drvWait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
                drvWait.Message = message;
                drvWait.IgnoreExceptionTypes(ignoredExceptions);
                drvWait.PollingInterval = TimeSpan.FromSeconds(pollingIntervalSec);
                return drvWait.Until(condition);
            }
            catch (WebDriverTimeoutException)
            {
                return null;
            }
        }

        public static bool wait(this IWebDriver driver, Func<IWebDriver, bool> condition, double timeout = 10, string message = "", double pollingIntervalSec = 0.5)
        {
            try
            {
                WebDriverWait drvWait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
                drvWait.Message = message;
                drvWait.IgnoreExceptionTypes(ignoredExceptions);
                drvWait.PollingInterval = TimeSpan.FromSeconds(pollingIntervalSec);
                return drvWait.Until(condition);
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }
        #endregion
    }
}
