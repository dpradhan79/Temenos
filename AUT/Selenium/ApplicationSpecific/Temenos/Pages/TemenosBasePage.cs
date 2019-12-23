using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Setup;
using AUT.Selenium.CommonReUsablePages;
using OpenQA.Selenium;
using Engine.UIHandlers.Selenium;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using Engine.Factories;
using OpenQA.Selenium.Support.UI;
using System.Threading;

namespace AUT.Selenium.ApplicationSpecific.Pages
{
    public class TemenosBasePage : AbstractTemplatePage
    {

        #region UI Object Repository
        private By loadingSpin = By.XPath("//div[text()='Loading...']");
       

        #endregion

        #region Public Methods
        public void WaitUpToLoadingIconDisable()
        {
            SimulateThinkTimeInMilliSecs(5000); 
            WaitTillElementDisappeared(loadingSpin);
        }

        
          /// <summary>
        /// Navigate to screen
        /// <param name="screenflowHeaderName"></param>
        /// <param name="screenName">Name of the screen</param>
        /// </summary>
        public void NavigateToScreen(string screenName)
        {

            try
            {
               ClickOnScreen(screenName);
                SwitchToCentralFrame();
                WaitUpToLoadingIconDisable();
                VerifyScreenHeading(screenName);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        #endregion

        #region Private Methods
         /// <summary>
        /// Navigate to screen
        /// <param name="screenflowHeaderName"></param>
        /// <param name="screenName">Name of the screen</param>
        /// </summary>
        public void ClickOnScreen(string screenName)
        {

            try
            {
                Thread.Sleep(4000);
                By screen = By.XPath("//a[@class='dynatree-title' and text()='" + screenName + "']");
                driver.WaitElementPresent(screen, 120);
                // driver.MoveToElement(screen, screenName, 30);
                driver.ClickElementWithJavascript(screen, screenName, 30);
                // driver.SwitchToDefaultFrame();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

         /// <summary>
        /// Swith to central frame
        /// </summary>
        private void SwitchToCentralFrame()
        {
            try
            {
                Thread.Sleep(15000);
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(180));
                var element = wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt("centerFrame"));
                //driver.SwitchToFrameById("centerFrame");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

         /// <summary>
        /// Verifying the screen heading
        /// <param name="screenName"></param>
        /// </summary>
        private void VerifyScreenHeading(string screenName)
        {
            try
            {             

                By screenHeader = By.XPath("//div[contains(@class,'a-screen-titlebar')]//span[text()='" + screenName + "']");
                driver.WaitElementPresent(screenHeader);
            }
            catch (Exception)
            {

                throw;
            } 
            
        }

        #endregion
    }
}