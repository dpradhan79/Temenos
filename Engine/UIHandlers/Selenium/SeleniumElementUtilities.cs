using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Edge;
using System.Windows.Forms;
using TestReporter;
using Engine.Setup;
using System.Collections.ObjectModel;



namespace Engine.UIHandlers.Selenium
{
    /// <summary>
    /// @Author - Debasish Pradhan
    /// </summary>
    public static class SeleniumElementUtilities
    {
        private static IReporter testReport = EngineSetup.TestReport;
        private static String mainWindow = null;
        #region Non-Public Methods
        
        /// <summary>
        /// Watches the spinner.
        /// </summary>
        /// <param name="driver">The driver.</param>
        internal static void WatchSpinner(IWebDriver driver)
        {
            By bySpinner = By.XPath("//div[@id='waitModalId']");
            By byLoading = By.XPath("//div[@class='wait']");
            try
            {
                if (driver.IsElementPresent(bySpinner))
                {
                    driver.WaitElementAttrEquals(bySpinner, "style", "display: none;", bySpinner.ToString());                                                                      
                }
                if(!(driver is InternetExplorerDriver)) // driver not IE
                {
                    WebDriverWait webDriverCondWait = new WebDriverWait(driver, TimeSpan.FromSeconds(EngineSetup.DefaultTimeoutForSelenium));
                    try
                    {
                        webDriverCondWait.Until(ExpectedConditions.InvisibilityOfElementLocated(byLoading));
                        driver.Highlight(byLoading);

                    }

                    catch (WebDriverTimeoutException)
                    {

                        testReport.LogWarning("WebDriverTimeoutException", String.Format("Control - {0} Was Still Visible After - {1} Secs", byLoading.ToString(), byLoading.ToString(), EngineSetup.DefaultTimeoutForSelenium), EngineSetup.GetScreenShotPath());
                    }
                }
                               
               
            }
            catch (Exception ex)
            {
               
                testReport.LogException(ex, EngineSetup.GetScreenShotPath());

            }
            Thread.Sleep(100);
        }

        /// <summary>
        /// Watches the spinner saving in progress.
        /// </summary>
        /// <param name="driver">The driver.</param>
        internal static void WatchSpinnerSavingInProgress(IWebDriver driver)
        {
            try
            {

                /*Wait Till Spiner is in progress*/

                WatchSpinner(driver);
                Thread.Sleep(500);

                ///*Wait Till Saving Mode is in progress*/
                //By byAutoSavingInProgess = By.XPath("//span[@class='dw-selector-illustrator dw-selector-form-saving valign msg_autosave saving']");
                //while (driver.FindElements(byAutoSavingInProgess).Count > 0)
                //{
                //    Thread.Sleep(100);
                //}
                //By byAutoSaved = By.XPath("//span[@class='dw-selector-illustrator dw-selector-form-saving valign msg_autosave saved']");

                //if (driver.FindElements(byAutoSaved).Count > 0)
                //{
                //    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(EngineSetup.DefaultTimeoutForSelenium));
                //    wait.Until(ExpectedConditions.TextToBePresentInElementLocated(byAutoSaved, "All changes saved."));
                //    Thread.Sleep(1000);
                //}

            }
            catch (Exception ex)
            {
                
                testReport.LogException(ex, EngineSetup.GetScreenShotPath());
            }
        }

        /// <summary>
        /// Watches the spinner for kendo drop down.
        /// </summary>
        /// <param name="driver">The driver.</param>
        internal static void WatchSpinnerForKendoDropDown(IWebDriver driver)
        {
            try
            {
                By byDropDownSpinnerForKendo = By.XPath("//span[@class='k-icon k-i-arrow-s k-loading']");
                int elapsedTime = 0;
                int maxTimeout = EngineSetup.DefaultTimeoutForSelenium * 1000;
                
                while (driver.FindElements(byDropDownSpinnerForKendo).Count > 0 && elapsedTime < maxTimeout)
                {
                   
                    testReport.LogInfo("Waiting for kendo dropdrown spinner to go off for all dropdowns");
                    Thread.Sleep(1000);
                    elapsedTime = elapsedTime + 1000;

                }
            }
            catch (Exception ex)
            {
                
                testReport.LogException(ex, EngineSetup.GetScreenShotPath());
            }
        }

        /// <summary>
        /// Simulates the send keys.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="chars">The chars.</param>
        internal static void SimulateSendKeys(IWebDriver driver, string chars)
        {
            try
            {
                SendKeys.SendWait(chars);
                testReport.LogSuccess("SimulateSendKeys", String.Format("Invoked SendKeys.SendWait for input - <mark>{0}</mark>", chars));
            }

            catch (Exception ex)
            {

                testReport.LogFailure("SimulateSendKeys", String.Format("Error While Invoking SendKeys.SendWait for input - <mark>{0}</mark>", chars));
                testReport.LogException(ex, EngineSetup.GetScreenShotPath());
            }
        }
       
        #endregion

        #region Public Methods
        /// <summary>
        /// Highlights the specified locator.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="locator">The locator.</param>
        /// <param name="count">The count.</param>
        public static void Highlight(this IWebDriver driver, By locator, int count = 5)
        {
            if (driver.IsElementPresent(locator))
            {
                try
                {
                    //blink the item
                    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                    for (int i = 0; i < count; i++)
                    {
                        js.ExecuteScript("arguments[0].setAttribute('style',arguments[1]);", driver.FindElement(locator), "color: green; border: 2px solid yellow;");
                        Thread.Sleep(50);
                        js.ExecuteScript("arguments[0].setAttribute('style',arguments[1]);", driver.FindElement(locator), "");
                    }
                }
                catch (Exception) { }
            }
        }
        /// <summary>
        /// Determines whether [is element present] [the specified by].
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="by">The by.</param>
        /// <returns></returns>
        public static bool IsElementPresent(this IWebDriver driver, By by)
        {
            IWebElement webElement = null;
            bool isPresent = false;
            try
            {
                webElement = driver.FindElement(by);
                isPresent = true;

            }
            catch (NoSuchElementException)
            {
                isPresent = false;
            }

            catch (WebDriverException)
            {
                isPresent = false;
            }
            catch (Exception)
            {
                isPresent = false;
            }

            return isPresent;
        }

        /// <summary>
        /// wait for the element to load on the page. waits for it to exists then for it to be visible.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="locator"></param>
        public static void WaitElementExistsAndVisible(this IWebDriver driver, By locator)
        {
            WatchSpinner(driver);
            //In case of IE, ExpectedConditions defined for chrome/implementation through WebDriverWait.Until causes Timeout. Issue with IEDriver
            if (driver is InternetExplorerDriver || driver is EdgeDriver)
            {
                try
                {
                    int MAXTIMEOUT = EngineSetup.DefaultTimeoutForSelenium * 1000;
                    int threadSleepTime = 1000;
                    while (threadSleepTime <= MAXTIMEOUT && driver.IsElementPresent(locator) == false)
                    {
                        if (threadSleepTime == MAXTIMEOUT)
                        {
                            
                            testReport.LogFailure("WaitElementExistsAndVisible", String.Format("Exception thrown by SeleniumElementUtilities.WaitElementExistsAndVisible : Object With By <mark>{0}</mark> Not Found", locator.ToString()), EngineSetup.GetScreenShotPath());
                        }
                        Thread.Sleep(1000);
                        threadSleepTime = threadSleepTime + 1000;
                    }

                }
                catch (Exception)
                {

                }

            }
            else //if not IE and edge
            {
                try
                {
                    ExpectedConditions.ElementExists(locator).Invoke(driver);
                    ExpectedConditions.ElementIsVisible(locator).Invoke(driver);
                    ExpectedConditions.ElementToBeClickable(locator).Invoke(driver);

                }
                catch (Exception)
                {
                    //AutomationLogging.LogErrorMessage(String.Format("Exception thrown by SeleniumElementUtilities.WaitElementExistsAndVisible : Object With By {0} Not Found", locator.ToString()), driver, true);
                }

            }
            //scroll bar
            if (driver.IsElementPresent(locator))
            {
                //scroll bar
                object objElementTop = ((IJavaScriptExecutor)driver).ExecuteScript(
                        "return arguments[0].getBoundingClientRect().top;", driver.FindElement(locator));
                double dblElementTop = Convert.ToDouble(objElementTop);
                try
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0,dblElementTop)", "");
                }
                catch (Exception)
                {

                }

            }
            driver.Highlight(locator);

        }

       
        /// <summary>
        /// waits for an element (with the given locator) to be present on the page
        /// a timeout can be defined or the default will be assigned
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="locator"></param>
        /// <param name="defaultTimeout"></param>
        /// <returns></returns>
        public static IWebDriver WaitElementPresent(this IWebDriver driver, By locator,
            int defaultTimeout = EngineSetup.TimeOutConstant)
        {
            return SeleniumFrameworkExtensions.CompleteAction(() =>
            {
                driver.WaitElementExistsAndVisible(locator);
                /*return*/
                driver.FindElement(locator);
                return driver;
            }
                , defaultTimeout, driver, String.Format("Element found (locator -> <mark>{0}</mark>)", locator));
        }

        /// <summary>
        /// Elements the present.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="locator">The locator.</param>
        /// <param name="controlName">Name of the control.</param>
        /// <param name="defaultTimeout">The default timeout.</param>
        /// <returns></returns>
        public static bool ElementPresent(this IWebDriver driver, By locator, string controlName,
            int defaultTimeout = EngineSetup.TimeOutConstant)
        {
            return SeleniumFrameworkExtensions.CompleteAction(() => { WatchSpinner(driver); return driver.FindElements(locator).Count > 0; }
                , defaultTimeout, driver,
                String.Format("Checked For Presence Of Element - ControlName: <mark>{0}</mark>", controlName));
        }

        /// <summary>
        /// Webs the element present.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="locator">The locator.</param>
        /// <param name="defaultTimeout">The default timeout.</param>
        /// <returns></returns>
        public static bool WebElementPresent(this IWebDriver driver, IWebElement locator,
            int defaultTimeout = EngineSetup.TimeOutConstant)
        {
            return SeleniumFrameworkExtensions.CompleteAction(() => { WatchSpinner(driver); return locator != null; }
                , defaultTimeout, driver);
        }


        /// <summary>
        /// waits for a given element to be stale.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="locator"></param>
        /// <param name="defaultTimeout"></param>
        /// <returns></returns>
        /// 
        //this doesnt work....i need the element so when i click i can then wait if stale
        public static IWebDriver WaitElementStale(this IWebDriver driver, By locator,
            int defaultTimeout = EngineSetup.TimeOutConstant)
        {
            var element = driver.FindElement(locator);
            return SeleniumFrameworkExtensions.CompleteAction(() =>
            {
                try
                {
                    element.GetAttribute("id");
                }
                catch (StaleElementReferenceException)
                {
                    return driver;
                }
                //make custom exception?
                throw new WebDriverTimeoutException(String.Format("Element did not become stale within <mark>{0}</mark> seconds",
                    defaultTimeout));
            }, defaultTimeout, driver);
        }

        //public static IWebDriver WaitElementDisplayed(this IWebDriver driver, By locator, int defaultTimeout = EngineSetup.TimeOutConstant)
        //{
        //    return CompleteAction(() =>
        //    {
        //        if (driver.FindElement(locator).Displayed)
        //        {
        //            return driver;
        //        }
        //        throw new WebDriverTimeoutException(String.Format("Web driver timeout - Element not displayed within" + defaultTimeout + " seconds (locator -> {0})", locator));
        //    }, defaultTimeout);
        //}

        /// <summary>
        /// waits for a given element's attribute to equal the expected value.
        /// a timeout can be specified or the default timeout is assigned
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="locator"></param>
        /// <param name="attrName"></param>
        /// <param name="attrValue"></param>
        /// <param name="defaultTimeout"></param>
        /// <returns></returns>
        public static IWebDriver WaitElementAttrEquals(this IWebDriver driver, By locator, string attrName,
            string attrValue, string controlName, int defaultTimeout = EngineSetup.TimeOutConstant)
        {
            return SeleniumFrameworkExtensions.CompleteAction(() =>
            {
                if (driver.FindElement(locator).GetAttribute(attrName).Contains(attrValue))
                {
                    return driver;
                }
                throw new Exception(
                    String.Format(
                        "Attribute (<mark>{0}</mark>) not equal to specified value (<mark>{1}</mark>) after <mark>{2}</mark> seconds: locator -> <mark>{3}</mark>", attrName,
                        attrValue, defaultTimeout, locator));
            }, defaultTimeout, driver, ""/*"Waiting for spinner...."*/);
            //}, defaultTimeout, String.Format("Element -> {0} - attribute ({1}) equals {2}", controlName, attrName, attrValue));
        }

        /// <summary>
        /// Waits the element text equals.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="locator">The locator.</param>
        /// <param name="textValue">The text value.</param>
        /// <param name="defaultTimeout">The default timeout.</param>
        /// <returns></returns>
        public static IWebDriver WaitElementTextEquals(this IWebDriver driver, By locator, string textValue,
            int defaultTimeout = EngineSetup.TimeOutConstant)
        {
            return SeleniumFrameworkExtensions.CompleteAction(() =>
            {
                WatchSpinner(driver);
                if (driver.FindElement(locator).Text == textValue)
                {
                    return driver;
                }
                throw new Exception(
                    String.Format("Text not equal to specified value (<mark>{0}</mark>) after <mark>{1}</mark> seconds: locator -> <mark>{2}</mark>", textValue,
                        defaultTimeout, locator));
            }, defaultTimeout, driver, String.Format("Element -> <mark>{0}</mark> - text equal to <mark>'{1}'</mark>", locator, textValue));
        }

        /// <summary>
        /// Returns the specified attribute of the given control
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="locator"></param>
        /// <param name="attributeName"></param>
        /// <param name="defaultTimeout"></param>
        /// <returns></returns>
        public static string GetElementAttribute(this IWebDriver driver, By locator, string attributeName,
            int defaultTimeout = EngineSetup.TimeOutConstant)
        {
            return
                SeleniumFrameworkExtensions.CompleteAction(
                    () => { WatchSpinner(driver); return driver.FindElement(locator).GetAttribute(attributeName); }, defaultTimeout, driver);
        }

        /// <summary>
        /// returns the specified CSS value of the given attribute
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="locator"></param>
        /// <param name="propertyName"></param>
        /// <param name="defaultTimeout"></param>
        /// <returns></returns>
        public static string GetElementCssValue(this IWebDriver driver, By locator, string propertyName,
            int defaultTimeout = EngineSetup.TimeOutConstant)
        {
            return
                SeleniumFrameworkExtensions.CompleteAction(
                    () => { WatchSpinner(driver); return driver.FindElement(locator).GetCssValue(propertyName); }, defaultTimeout, driver);
        }

        /// <summary>
        /// get the text of a given element
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="locator"></param>
        /// <param name="defaultTimeout"></param>
        /// <returns></returns>
        public static string GetElementText(this IWebDriver driver, By locator,
            int defaultTimeout = EngineSetup.TimeOutConstant)
        {
            return SeleniumFrameworkExtensions.CompleteAction(() => { WatchSpinner(driver); return driver.FindElement(locator).Text; },
                defaultTimeout, driver);
        }

        /// <summary>
        /// Validate text of a given element
        /// </summary>
        /// <param name="driver"> selenium driver instance</param>
        /// <param name="locator"> location information</param>
        /// <param name="expectedValue"> value to be validated</param>
        /// <param name="defaultTimeout"></param>
        /// <returns></returns>

        public static bool VerifyTextValue(this IWebDriver driver, By locator, string expectedValue, string controlName = "",
            int defaultTimeout = 0)
        {
            bool flag = false;
            return SeleniumFrameworkExtensions.CompleteAction(() =>
            {
                string txt = "" + driver.FindElement(locator).GetAttribute("textContent");
                if (txt == "")
                    txt = "" + driver.FindElement(locator).GetAttribute("value");
                if (txt == "")
                    txt = "" + driver.FindElement(locator).GetAttribute("text");
                if (expectedValue.Length > 0)
                {
                    flag = txt.Trim().ToUpper().Contains(expectedValue.ToUpper().Trim());
                    if (flag)
                    {
                        testReport.LogSuccess("Verify Value - " + controlName, String.Format("The element's text value: <mark>{0}</mark> matched successfully with expected value: <mark>{1}</mark>", txt, expectedValue));
                    }
                    else
                    {
                        testReport.LogFailure("Verify Value - " + controlName, String.Format("The element's text value <mark>{0}</mark> does not matched with expected value: <mark>{1}</mark>", txt, expectedValue), EngineSetup.GetScreenShotPath());
                    }

                }
                return flag;

            },
                defaultTimeout, driver);
        }

       public static bool VerifyTextValue(this IWebDriver driver, string expectedValue, string actual, string controlName = "",
    int defaultTimeout = 0)
        {
            bool flag = false;
            return SeleniumFrameworkExtensions.CompleteAction(() =>
            {
                flag = actual.Trim().ToUpper().Contains(expectedValue.ToUpper().Trim());
                if (flag)
                {
                    testReport.LogSuccess("VerifyTextValue - " + controlName, String.Format("The element's text value: <mark>{0}</mark> matched successfully with expected value: <mark>{1}</mark>", actual, expectedValue));
                }
                else
                {
                    testReport.LogFailure("VerifyTextValue - " + controlName, String.Format("The element's text value <mark>{0}</mark> does not matched with expected value: <mark>{1}</mark>", actual, expectedValue), EngineSetup.GetScreenShotPath());
                }

                return flag;
            },
                defaultTimeout, driver);
        }
    

        /// <summary>
        /// Gets the element property.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="locator">The locator.</param>
        /// <param name="propName">Name of the property.</param>
        /// <param name="defaultTimeout">The default timeout.</param>
        /// <returns></returns>
        public static object GetElementProperty(this IWebDriver driver, By locator, string propName,
            int defaultTimeout = EngineSetup.TimeOutConstant)
        {
            return
                SeleniumFrameworkExtensions.CompleteAction(
                    () =>
                    {
                        WatchSpinner(driver);
                        return ((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0][arguments[1]];",
                            driver.FindElement(locator), propName);
                    }, defaultTimeout, driver);
        }

        /// <summary>
        /// Gets the name of the element tag.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="locator">The locator.</param>
        /// <param name="defaultTimeout">The default timeout.</param>
        /// <returns></returns>
        public static string GetElementTagName(this IWebDriver driver, By locator,
            int defaultTimeout = EngineSetup.TimeOutConstant)
        {
            return SeleniumFrameworkExtensions.CompleteAction(() => { WatchSpinner(driver); return driver.FindElement(locator).TagName; },
                defaultTimeout, driver);
        }

        /// <summary>
        /// Gets the element bounds.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="locator">The locator.</param>
        /// <returns></returns>
        public static object GetElementBounds(this IWebDriver driver, By locator)
        {
            return
                SeleniumFrameworkExtensions.CompleteAction(
                    () =>
                    {
                        return
                            ((IJavaScriptExecutor)driver).ExecuteScript(
                                "return arguments[0].getBoundingClientRect();", driver.FindElement(locator));
                    },
                    EngineSetup.DefaultTimeoutForSelenium, driver);
        }

        /// <summary>
        /// Gets the element top.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="locator">The locator.</param>
        /// <returns></returns>
        public static object GetElementTop(this IWebDriver driver, By locator)
        {
            return
                SeleniumFrameworkExtensions.CompleteAction(
                    () =>
                    {
                        return
                            ((IJavaScriptExecutor)driver).ExecuteScript(
                                "return arguments[0].getBoundingClientRect().top;", driver.FindElement(locator));
                    },
                    EngineSetup.DefaultTimeoutForSelenium, driver);
        }

        /// <summary>
        /// Gets the element left.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="locator">The locator.</param>
        /// <returns></returns>
        public static object GetElementLeft(this IWebDriver driver, By locator)
        {
            return
                SeleniumFrameworkExtensions.CompleteAction(
                    () =>
                    {
                        return
                            ((IJavaScriptExecutor)driver).ExecuteScript(
                                "return arguments[0].getBoundingClientRect().left;", driver.FindElement(locator));
                    },
                    EngineSetup.DefaultTimeoutForSelenium, driver);
        }

        /// <summary>
        /// Gets the width of the element.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="locator">The locator.</param>
        /// <returns></returns>
        public static object GetElementWidth(this IWebDriver driver, By locator)
        {
            return
                SeleniumFrameworkExtensions.CompleteAction(
                    () =>
                    {
                        return
                            ((IJavaScriptExecutor)driver).ExecuteScript(
                                "return arguments[0].getBoundingClientRect().width;", driver.FindElement(locator));
                    },
                    EngineSetup.DefaultTimeoutForSelenium, driver);
        }

        /// <summary>
        /// Gets the height of the element.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="locator">The locator.</param>
        /// <returns></returns>
        public static object GetElementHeight(this IWebDriver driver, By locator)
        {
            return
                SeleniumFrameworkExtensions.CompleteAction(
                    () =>
                    {
                        return
                            ((IJavaScriptExecutor)driver).ExecuteScript(
                                "return arguments[0].getBoundingClientRect().height;", driver.FindElement(locator));
                    },
                    EngineSetup.DefaultTimeoutForSelenium, driver);
        }

        /// <summary>
        /// Gets the dropdown selected option text.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="locator">The locator.</param>
        /// <param name="defaultTimeout">The default timeout.</param>
        /// <returns></returns>
        public static string GetDropdownSelectedOptionText(this IWebDriver driver, By locator,
            int defaultTimeout = EngineSetup.TimeOutConstant)
        {
            return SeleniumFrameworkExtensions.CompleteAction(() =>
            {
                driver.WaitElementExistsAndVisible(locator);
                return new SelectElement(driver.FindElement(locator)).SelectedOption.Text;
            }, defaultTimeout, driver);
        }

        /// <summary>
        /// Gets the dropdown selected option text.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="locator">The locator.</param>
        /// <param name="defaultTimeout">The default timeout.</param>
        /// <returns></returns>
        public static string GetSelectedOption(this IWebDriver driver, By locator,
            int defaultTimeout = EngineSetup.TimeOutConstant)
        {
            string value = "";
            
                try
                {
                    SelectElement selectElement = new SelectElement(driver.FindElement(locator));
                    //String value = selectElement.SelectedOption.Text;
                    IList<IWebElement> elements = selectElement.Options;

                    //Get the count of options
                    int elementsSize = elements.Count;

                    //Print all elements
                    for (int i = 0; i < elementsSize; i++)
                    {
                        value=elements.ElementAt(i).Text;
                    }
                }
                catch (Exception e)
                {

                    Console.WriteLine("Exception");
                }


                return value;
        }


        /// <summary>
        /// Scrolls to page bottom.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <returns></returns>
        public static object ScrollToPageBottom(this IWebDriver driver)
        {
            
            testReport.LogInfo("ScrollToPageBottom Will Be Called");
            object obj = null;
            try
            {
                obj =
                    ((IJavaScriptExecutor)driver).ExecuteScript(
                        "window.scrollTo(0,document.body.scrollHeight)", "");

                testReport.LogSuccess("ScrollToPageBottom", "ScrollToPageBottom Was Called Successfully");
                Thread.Sleep(1000);

            }
            catch (Exception)
            {

            }
            return obj;

        }
        /// <summary>
        /// Scrolls to page top.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <returns></returns>
        public static object ScrollToPageTop(this IWebDriver driver)
        {
            
            testReport.LogInfo("ScrollToPageTop Will Be Called");
            object obj = null;
            try
            {
                obj =
                    ((IJavaScriptExecutor)driver).ExecuteScript(
                        "window.scrollTo(0, - document.body.scrollHeight)", "");

                testReport.LogSuccess("ScrollToPageTop", "ScrollToPageTop Was Called Successfully");
                Thread.Sleep(1000);
            }
            catch (Exception)
            {

            }
            return obj;

        }

        /*
         Parameter	Type	Description
            horizantalAlongXAxis	Number	Required. How many pixels to scroll by, along the x-axis (horizontal). Positive values will scroll to the left, while negative values will scroll to the right
            verticalAlongYAxis	Number	Required. How many pixels to scroll by, along the y-axis (vertical). Positive values will scroll down, while negative values scroll up
         */
        /// <summary>
        /// Scrolls the page.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="horizantalAlongXAxis">The horizantal along x axis.</param>
        /// <param name="verticalAlongYAxis">The vertical along y axis.</param>
        /// <returns></returns>
        public static object ScrollPage(this IWebDriver driver, int horizantalAlongXAxis = 0, int verticalAlongYAxis = 200)
        {
           
            testReport.LogInfo("ScrollPage Will Be Called");
            object obj = null;
            try
            {
                obj =
                    ((IJavaScriptExecutor)driver).ExecuteScript(
                        "window.scrollBy(horizantalAlongXAxis, verticalAlongYAxis)");

                testReport.LogSuccess("ScrollPage", String.Format("ScrollPage Was Called Successfully With horizantalAlongXAxis - {0}, verticalAlongYAxis - {1}", horizantalAlongXAxis, verticalAlongYAxis));
                Thread.Sleep(1000);
            }
            catch (Exception)
            {

            }
            return obj;

        }

        /// <summary>
        /// Waits the element enabled.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="locator">The locator.</param>
        /// <param name="defaultTimeout">The default timeout.</param>
        /// <returns></returns>
        public static IWebDriver WaitElementEnabled(this IWebDriver driver, By locator,
            int defaultTimeout = EngineSetup.TimeOutConstant)
        {
            return SeleniumFrameworkExtensions.CompleteAction(() =>
            {
                driver.WaitElementEnabled(locator);
                /*return*/
                driver.FindElement(locator);
                return driver;
            }
                , defaultTimeout, driver, String.Format("Element found (locator -> <mark>{0}</mark>)", locator));
        }

        public static void SwitchToNewWindow(this IWebDriver driver)
        {
            mainWindow = driver.CurrentWindowHandle;
            string popupWindow = null;
            ReadOnlyCollection<String> handles = driver.WindowHandles;

            foreach (string windowHandle in handles)
            {
                if (windowHandle != mainWindow)
                {
                    popupWindow = windowHandle;
                    break;
                }

            }
            driver.SwitchTo().Window(popupWindow);
        }

        public static void SwitchBackToMainWindow(this IWebDriver driver)
        {
            //driver.Close();
            driver.SwitchTo().Window(mainWindow);
        }

        /// <summary>
        /// Determines whether [is web element displayed] [the specified locator].
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="locator">The locator.</param>
        /// <returns>
        ///   <c>true</c> if [is web element displayed] [the specified locator]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsWebElementDisplayed(this IWebDriver driver, By locator)
        {
            bool elementStatus;
            bool isPresent = false;
            System.Threading.Thread.Sleep(3000);
            try
            {
                elementStatus = driver.FindElement(locator).Displayed;
                isPresent = elementStatus;

            }
            catch (NoSuchElementException)
            {
                isPresent = false;
            }

            catch (WebDriverException)
            {
                isPresent = false;
            }
            catch (Exception)
            {
                isPresent = false;
            }

            return isPresent;
        }

        #endregion
    }
}
