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
    public class HomePage : AbstractTemplatePage
    {
       
        #region UI Object Repository
        private By btnCreate = By.XPath("//button[contains(@class,'icon-application')]");
        private By dropdownApplicationType = By.Id("ApplicationTypeId");
        private By dropdownApplyingFor = By.Id("SubProductId");
        private By dropdownApplicationSource = By.Id("ApplicationSourceId");
        private By txtAccountType = By.Id("AccountNumber");
        private By txtTIN = By.Id("TIN");
        private By dropdownChannel = By.Id("Channel");
        private By txtVendorName = By.Id("VendorName");
        private By btnStartApplication = By.Id("btnStartApplication");
        private By btnCreateNewApplication = By.XPath("//button[text()='Create New Application']");
        private By lblCreateNewApplication = By.XPath("//div[text()='Creating Application...']");
        private By divCreateApplicationLoadMsg = By.ClassName("loadmask-msg");
        private By frameTabApp = By.XPath("//iframe[contains(@name,'tabframeapp')]");
        private By btnDecision = By.Id("btnGetDecision"); 
        private By lblSuccess = By.XPath("//div[text()='Success']");
        private By btnOk = By.XPath("//div[text()='Success']/../../following-sibling::div//button[text()='OK']");
        private By lblDecisionSuccessMsg = By.XPath("//div[contains(@class,'a-icon-info')]");
        private By lblApplicationNumber = By.XPath("//span[contains(@class,'x-tab-strip-text icon-application')]");
        private By btnUserProfile = By.XPath("//div[@id='userbuttongrouplarge']//button[contains(@class,'icon-user')]");
        private By linkLogoff = By.XPath("//span[text()='Logoff']");
        private By btnDecline = By.Id("btnDecline");
        private By dropdownAdverseAction = By.Id("Decline_ddlAdverseAction");
        private By dropdownAvailableReason = By.Id("Decline_DeclineReason_twoboxavailable");
        private By btnDeclineDeclineReasonBtnAdd = By.Id("Decline_DeclineReason_BtnAdd");
        private By btnDeclineInDialog = By.XPath("//div[@class='ui-dialog-buttonset']//button[text()='Decline']");
        private By lblDeclineLone = By.XPath("//div[@aria-describedby='ws_Decline_Dialog']//span[@class='ui-dialog-title']");
        private By dropdownDeclineReasonAssigned = By.Id("Decline_DeclineReason_twoboxassigned");
        private By btnMore = By.Id("btnMenuOther");
        private By lnkRemoveDecision = By.Id("btnRemoveDecision");
        private By lblConfirmRemoveDecision = By.XPath("//div[text()='Confirm Remove Decision']");
        private By btnRemoveDecisionYes = By.XPath("//div[text()='Confirm Remove Decision']/../../following-sibling::div//button[text()='Yes']");
        private By btnConfirmLogoffYes = By.XPath("//button[text()='Yes']");
        private By formNewApplication = By.Id("frmNewApplication");
        private By lblNewApplication = By.XPath("//span[text()='New Application']");
        #endregion

        #region Public Methods

        /// <summary>
        /// Create nw application
        /// <param name="applicationType"></param>
        /// <param name="applyingfor"></param>
        /// <param name="applicationSource"></param>
        /// <param name="accountNumber"></param>
        /// </summary>
         public void CreateNewApplication(string applicationType,string applyingfor,string applicationSource,string accountNumber)
       // public void CreateNewApplication(string applicationType, Dictionary<string, string> validationTestData)
       {           
            ClickOnCreate();
            driver.SwitchToNewWindow();
            SimulateThinkTimeInMilliSecs(1000);
            driver.WaitElementPresent(formNewApplication);
            driver.WaitElementExistsAndVisible(lblNewApplication);
            driver.SelectByVisibleText(dropdownApplicationType, applicationType, "Application Type");
            if (applicationType.Equals("Approve"))
            {
                driver.SelectByVisibleText(dropdownApplyingFor, applyingfor, "Applying For");
            }
            else if (applicationType.Equals("Reject"))
            {
                driver.SelectByVisibleText(dropdownApplyingFor, applyingfor, "Applying For");
            }
            else if (applicationType.Equals("Review"))
            {
                driver.SelectByVisibleText(dropdownApplyingFor, applyingfor, "Applying For");
            }
            SimulateThinkTimeInMilliSecs(2000);
            driver.SelectByVisibleText(dropdownApplicationSource, applicationSource, "Application Source");
            driver.SendKeysToElement(txtAccountType, accountNumber, "Account Number");
            ClickOnStartApplication();
            // SimulateThinkTimeInMilliSecs(10000);           
            ClickOnCreateNewApplication();
            SimulateThinkTimeInMilliSecs(10000);
            driver.SwitchBackToMainWindow();


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
                By screen = By.XPath("//a[@class='dynatree-title' and text()='" + screenName + "']");
                driver.WaitElementPresent(screen,120);
                //driver.MoveToElement(screen, screenName, 30);
                driver.ClickElementWithJavascript(screen, screenName, 30);                             
               // driver.SwitchToDefaultFrame();
            }
            catch (Exception e)
            {

                throw;
            }
            
        }

        public void SwitchToCentralFrame()
        {
            driver.SwitchToFrameById("centerFrame");
        }

        public void SwitchToParentFrame()
        {
            driver.SwitchTo().ParentFrame();
        }

        public void SwitchToTabAppFrame()
        {
            SimulateThinkTimeInMilliSecs(3000);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(120));
            var element = wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(frameTabApp));
           // ExpectedConditions.FrameToBeAvailableAndSwitchToIt(frameTabApp);
            //WebDriverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            //wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.Id("FRAMEID")));
            //driver.SwitchToFrameByLocator(frameTabApp);
        }

        public void SwitchToDefaultContent()
        {
            driver.SwitchOutOfTheFrame();
        }

        public void VerifyScreenHeading(string screenName)
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

        public void VerifyDecisioningOfApplication()
        {
            driver.ClickElement(btnDecision, "Decision");
            driver.WaitElementPresent(lblSuccess);
            driver.VerifyTextValue(lblDecisionSuccessMsg, "The application was automatically approved");
            driver.ClickElement(btnOk, "OK");
        }

        public String GetApplicationNumber()
        {
            String applicationNumber = "";
            driver.WaitElementPresent(lblApplicationNumber,120);
            applicationNumber = driver.GetElementText(lblApplicationNumber);
            return applicationNumber;
            
        }

        public void CloseAndVerifyApplication(string applicationNumber)
        {
            String appNum = applicationNumber.Split('#')[1].Trim();
            By closeApplication = By.XPath("//li[@id='DesktopPages__app"+ appNum + "']/a[@class='x-tab-strip-close']");
            driver.ClickElementWithJavascript(closeApplication, "Close the application for" + applicationNumber);
            if (!driver.IsWebElementDisplayed(closeApplication)) 
            {
                this.TESTREPORT.LogSuccess("Close And verify Application", String.Format("Application Number : <mark>{0}</mark> is not present in desktop pages",applicationNumber));
            }
            else
            {
                this.TESTREPORT.LogFailure("Close And verify Application", String.Format("Application Number : <mark>{0}</mark> is present in desktop pages,not closed", applicationNumber));
            }
        }

        /// <summary>
        /// Click on logoff
        /// </summary>
        public void ClickLogOff()
        {
            driver.WaitElementPresent(btnUserProfile);
            driver.ClickElement(btnUserProfile,"User Profile");
            driver.ClickElementWithJavascript(linkLogoff, "Logoff");
            driver.WaitElementPresent(btnConfirmLogoffYes);
            driver.ClickElement(btnConfirmLogoffYes,"Yes");
        }

        /// <summary>
        /// Verify Decisioning Application
        /// </summary>
        public void VerifyDecisioningApplication(string postDecisionStatus,bool removeDecision)
        {
            if (postDecisionStatus.Equals("LO Rejected"))
            {
                driver.ClickElement(btnDecline, "Decline");
                driver.WaitElementPresent(lblDeclineLone);
                driver.VerifyTextValue(lblDeclineLone, "Decline Loan");
                driver.SelectDropdownItemByText(dropdownAdverseAction, "Other", "Adverse Action");
                driver.SelectDropdownItemByText(dropdownAvailableReason, "Other", "Available Decline Reason");
                driver.ClickElementWithJavascript(btnDeclineDeclineReasonBtnAdd, "Decline ReasonBtn Add");
                string declinedReasonAssigned = driver.GetSelectedOption(dropdownDeclineReasonAssigned);
                driver.VerifyTextValue(declinedReasonAssigned, "Other");
                driver.ClickElementWithJavascript(btnDeclineInDialog, "Decline");
                HandleSuccessPopup();                
            }
            else if (postDecisionStatus.Equals("Auto Approved"))
            {
                driver.ClickElement(btnDecision, "Decision");
                driver.WaitElementPresent(lblSuccess);
                driver.VerifyTextValue(lblDecisionSuccessMsg, "The application was automatically approved");
                driver.ClickElement(btnOk, "OK");
            }

            if (removeDecision)
            {
                driver.WaitElementPresent(btnMore);
                driver.ClickElement(btnMore, "More");
                driver.ClickElement(lnkRemoveDecision, "Remove Decision");
                driver.WaitElementPresent(lblConfirmRemoveDecision);
                driver.ClickElement(btnRemoveDecisionYes, "Yes");
                HandleSuccessPopup();
            }
           
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Click on create new application
        /// </summary>
        private void ClickOnCreate()
        {
            driver.WaitElementPresent(btnCreate);
            driver.ClickElement(btnCreate, "Create");
        }



        /// <summary>
        /// Click on create new application
        /// </summary>
        private void ClickOnCreateNewApplication()
        {
            driver.WaitElementPresent(btnCreateNewApplication);
            driver.ClickElement(btnCreateNewApplication, "Create New Application");
            WaitTillElementDisappeared(lblCreateNewApplication);
                // WaitTillElementDisappeared(lblCreateNewApplication, "Create New Application...", 180);
        }

        /// <summary>
        /// Method to click on start application
        /// </summary>
        private void ClickOnStartApplication()
        {            
            driver.ClickElement(btnStartApplication, "Start Application");
        }

        /// <summary>
        /// Method to handle success popup
        /// </summary>
        private void HandleSuccessPopup()
        {
            driver.WaitElementPresent(lblSuccess);
            driver.ClickElement(btnOk, "OK");
        }
        #endregion
    }
}