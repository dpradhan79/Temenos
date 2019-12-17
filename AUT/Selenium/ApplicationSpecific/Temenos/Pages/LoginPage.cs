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

namespace AUT.Selenium.ApplicationSpecific.Pages
{
    public class LoginPage : AbstractTemplatePage
    {

        #region UI Object Repository
        private By txtUserName = By.Id("Username");
        private By txtPassword = By.Id("Password");
        private By btnLogin = By.Id("btnLogin");
        private By txtUserName1 = By.Id("identifierId");
        private By txtPassword2 = By.XPath("//input[@name='password']");
        private By btnNext = By.XPath("//span[text()='Next']");
        private By btnContinue = By.XPath("//button[text()='Continue']");
        private By spanHomePageLabel = By.XPath("//span[contains(@class,'x-tab-strip-text icon-home')]");
        private By lblCreate = By.XPath("//span[text()='Create']");

        #endregion

        #region Public Methodsc

        /// <summary>
        /// Method to SignIn
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public void SignIn(string userName, string password)
        {
            try
            {
                driver.WaitElementPresent(txtUserName);
                driver.SendKeysToElement(txtUserName, userName, "User Name");
                driver.SendKeysToElement(txtPassword, password, "Password");
                driver.ClickElement(btnLogin, "Login Button");
                if (driver.IsWebElementDisplayed(btnContinue))
                {
                    driver.ClickElement(btnContinue, "Continue");
                }
                WaitForHomePage();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Method to wait for home page
        /// </summary>
        private void WaitForHomePage()
        {
            try
            {
                driver.wait(ExpectedConditions.TitleContains("LMS.Automation/Core/Desktop/Desktop"), 20, "Waiting for home page");
                driver.WaitElementExistsAndVisible(spanHomePageLabel);
                driver.WaitElementPresent(lblCreate);
                this.TESTREPORT.LogSuccess("Home Page", "Sucessfully login to the application");
            }
            catch (Exception ex)
            {

                this.TESTREPORT.LogFailure("Home Page", "Failed to login to the application");
                throw ex;
            }
        }
        #endregion
    }
}