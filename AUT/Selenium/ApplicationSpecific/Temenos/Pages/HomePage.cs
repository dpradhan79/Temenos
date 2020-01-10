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
        public HomePage(IWebDriver wd) : base(wd) { }
       
       
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
        private By ulMenuOther = By.Id("mnuOther");
        private By lnkRemoveDecision = By.Id("btnRemoveDecision");
        private By lblConfirmRemoveDecision = By.XPath("//div[text()='Confirm Remove Decision']");
        private By btnRemoveDecisionYes = By.XPath("//div[text()='Confirm Remove Decision']/../../following-sibling::div//button[text()='Yes']");
        private By btnConfirmLogoffYes = By.XPath("//button[text()='Yes']");
        private By formNewApplication = By.Id("frmNewApplication");
        private By lblNewApplication = By.XPath("//span[text()='New Application']");
        private By dialogSelectAplication = By.Id("dialogSelectApplication");
        private By lblReviewActiveAppAndPromotionalOffers = By.XPath("//span[text()='Review Active Applications and/or Promotional Offers']");
        private By loadingIconRemoveDecision = By.XPath("//div[text()='Removing Decision']");
        #endregion

        #region Public Methods

        /// <summary>
        /// Create nw application
        /// <param name="applicationType"></param>
        /// <param name="applyingfor"></param>
        /// <param name="applicationSource"></param>
        /// <param name="accountNumber"></param>
        /// </summary>
         //public void CreateNewApplication(string applicationType,string applyingfor,string applicationSource,string accountNumber)
        public void CreateNewApplication(string applyingfor, Dictionary<string, string> validationTestData)
        {
            try
            {
                ClickOnCreate();
                this.driver.SwitchToNewWindow();
                SimulateThinkTimeInMilliSecs(1000);
                this.driver.WaitElementPresent(formNewApplication);
                this.driver.WaitElementExistsAndVisible(lblNewApplication);
                this.driver.SelectDropdownItemByText(dropdownApplicationType, validationTestData["ApplicationType"], "Application Type");
                    
                if (applyingfor.Equals("Approve"))
                {
                    this.driver.SelectDropdownItemByText(dropdownApplyingFor, validationTestData["ApproveApplyingFor"], "Applying For");
                }
                else if (applyingfor.Equals("Reject"))
                {
                    this.driver.SelectDropdownItemByText(dropdownApplyingFor, validationTestData["RejectApplyingFor"], "Applying For");
                }
                else if (applyingfor.Equals("Review"))
                {
                    this.driver.SelectDropdownItemByText(dropdownApplyingFor, validationTestData["ReviewApplyingFor"], "Applying For");
                }
                //SimulateThinkTimeInMilliSecs(2000);
                this.driver.WaitElementPresent(dropdownApplicationSource);
                this.driver.SelectByVisibleText(dropdownApplicationSource, validationTestData["ApplicationSource"], "Application Source");
                this.driver.SendKeysToElement(txtAccountType, validationTestData["AccountNumber"], "Account Number");
                ClickOnStartApplication();
                // SimulateThinkTimeInMilliSecs(10000);           
                ClickOnCreateNewApplication();
               // SimulateThinkTimeInMilliSecs(10000);
                this.driver.SwitchBackToMainWindow();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Swith to tab frame 
        /// </summary>
        public void SwitchToTabFrame()
        {
            try
            {
                Thread.Sleep(15000);
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(240));
                var element = wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(frameTabApp));

            }
            catch (Exception ex)
            {

                throw ex;
            }

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
                Thread.Sleep(4000);
                By screen = By.XPath("//a[@class='dynatree-title' and text()='" + screenName + "']");
                this.driver.WaitElementPresent(screen, 120);
                // this.driver.MoveToElement(screen, screenName, 30);
                this.driver.ClickElementWithJavascript(screen, screenName, 30);
                // this.driver.SwitchToDefaultFrame();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// Swith to central frame
        /// </summary>
        public void SwitchToCentralFrame()
        {
            try
            {
                Thread.Sleep(15000);
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(180));
                var element = wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt("centerFrame"));
                //this.driver.SwitchToFrameById("centerFrame");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// Swith to Parent frame
        /// </summary>
        public void SwitchToParentFrame()
        {
            try
            {
                this.driver.SwitchTo().ParentFrame();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// Swith to tab frame and verifying home page is fully displayed
        /// </summary>
        public void SwitchAndVerifyHomePageFullyDisplayed()
        {
            try
            {
                Thread.Sleep(15000);
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(240));
                var element = wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(frameTabApp));
                // ExpectedConditions.FrameToBeAvailableAndSwitchToIt(frameTabApp);
                //WebDriverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                //wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.Id("FRAMEID")));
                //this.driver.SwitchToFrameByLocator(frameTabApp);
                this.driver.WaitElementPresent(By.Id("divToolbar"));
                this.driver.WaitElementExistsAndVisible(By.Id("summary"));
                this.driver.WaitElementExistsAndVisible(By.Id("centerFrameContainer"));

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// Swith to default content
        /// </summary>
        public void SwitchToDefaultContent()
        {
            try
            {
                this.driver.SwitchOutOfTheFrame();
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
        public void VerifyScreenHeading(string screenName)
        {
            try
            {             

                By screenHeader = By.XPath("//div[contains(@class,'a-screen-titlebar')]//span[text()='" + screenName + "']");
                this.driver.WaitElementPresent(screenHeader);
            }
            catch (Exception)
            {

                throw;
            } 
            
        }

        /// <summary>
        /// Verify decisioning of application
        /// </summary>
        public void VerifyDecisioningOfApplication()
        {
            try
            {
                this.driver.ClickElement(btnDecision, "Decision");
                this.driver.WaitElementPresent(lblSuccess);
                this.driver.VerifyTextValue(lblDecisionSuccessMsg, "The application was automatically approved");
                this.driver.ClickElement(btnOk, "OK");
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        /// <summary>
        /// Reading application number from application
        /// </summary>
        public String GetApplicationNumber()
        {
            try
            {
                 String applicationNumber = "";
            this.driver.WaitElementPresent(lblApplicationNumber,120);
            applicationNumber = this.driver.GetElementText(lblApplicationNumber);
            return applicationNumber;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            
        }

        /// <summary>
        /// Close and verify Application
        /// </summary>
        public void CloseAndVerifyApplication(string applicationNumber)
        {
            try
            {
                String appNum = applicationNumber.Split('#')[1].Trim();
                By closeApplication = By.XPath("//li[@id='DesktopPages__app"+ appNum + "']/a[@class='x-tab-strip-close']");
                this.driver.ClickElementWithJavascript(closeApplication, "Close the application for" + applicationNumber);
                if (!this.driver.IsWebElementDisplayed(closeApplication)) 
                {
                this.TESTREPORT.LogSuccess("Close And verify Application", String.Format("Application Number : <mark>{0}</mark> is not present in Home Page",applicationNumber));
                }
                else
                {
                this.TESTREPORT.LogFailure("Close And verify Application", String.Format("Application Number : <mark>{0}</mark> is present in Home Page ,not closed", applicationNumber));
                TemenosBasePage.dictError.Add("Close And verify Application", String.Format("Application Number : {0} is present in Home Page ,not closed", applicationNumber));
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        /// <summary>
        /// Click on logoff
        /// </summary>
        public void ClickLogOff()
        {
            try
            {
                this.driver.WaitElementPresent(btnUserProfile);
                this.driver.ClickElement(btnUserProfile,"User Profile");
                this.driver.ClickElementWithJavascript(linkLogoff, "Logoff");
                this.driver.WaitElementPresent(btnConfirmLogoffYes);
                this.driver.ClickElement(btnConfirmLogoffYes,"Yes");
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        /// <summary>
        /// Verify Decisioning Application
        /// </summary>
        public void VerifyDecisioningApplication(string postDecisionStatus, Dictionary<string, string> validationTestData)
        {
            try
            {
                if (postDecisionStatus.Equals("LO Rejected"))
            {
                this.driver.ClickElement(btnDecline, "Decline");
                this.driver.WaitElementPresent(lblDeclineLone);
                this.driver.VerifyTextValue(lblDeclineLone, "Decline Loan");
                this.driver.SelectDropdownItemByText(dropdownAdverseAction, "Other", "Adverse Action");
                this.driver.SelectDropdownItemByText(dropdownAvailableReason, "Other", "Available Decline Reason");
                this.driver.ClickElementWithJavascript(btnDeclineDeclineReasonBtnAdd, "Decline ReasonBtn Add");
                string declinedReasonAssigned = this.driver.GetSelectedOption(dropdownDeclineReasonAssigned);
                this.driver.VerifyTextValue(declinedReasonAssigned, "Other");
                this.driver.ClickElementWithJavascript(btnDeclineInDialog, "Decline");
                HandleSuccessPopup();                
            }
            else if (postDecisionStatus.Equals("Auto Approved"))
            {
                this.driver.WaitElementExistsAndVisible(btnDecision);
                this.driver.ClickElement(btnDecision, "Decision");
                this.driver.WaitElementPresent(lblSuccess);
                this.driver.VerifyTextValue(lblDecisionSuccessMsg, "The application was automatically approved");
                this.driver.ClickElement(btnOk, "OK");
            }
            else if (postDecisionStatus.Equals("Auto Rejected"))
            {
                this.driver.WaitElementExistsAndVisible(btnDecision);
                this.driver.ClickElement(btnDecision, "Decision");
                this.driver.WaitElementPresent(lblSuccess);
                this.driver.VerifyTextValue(lblDecisionSuccessMsg, "The application was automatically approved");
                this.driver.ClickElement(btnOk, "OK");
            }


                if (validationTestData["RemoveDecision"].Equals("True"))
            {
                this.driver.WaitElementPresent(btnMore);
                this.driver.ClickElementWithJavascript(btnMore, "More");
                Thread.Sleep(7000);
                this.driver.WaitElementPresent(ulMenuOther);
                this.driver.WaitElementExistsAndVisible(lnkRemoveDecision);
                this.driver.WaitElementPresent(lnkRemoveDecision);
                this.driver.ClickElement(lnkRemoveDecision, "Remove Decision");
                WaitTillElementDisappeared(loadingIconRemoveDecision);
                this.driver.WaitElementPresent(lblConfirmRemoveDecision);
                this.driver.ClickElement(btnRemoveDecisionYes, "Yes");
                HandleSuccessPopup();
            }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
           
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Click on create new application
        /// </summary>
        private void ClickOnCreate()
        {            
            try
            {
                this.driver.WaitElementPresent(btnCreate);
                this.driver.ClickElement(btnCreate, "Create");
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            
        }



        /// <summary>
        /// Click on create new application
        /// </summary>
        private void ClickOnCreateNewApplication()
        {
            try
            {
                this.driver.WaitElementPresent(btnCreateNewApplication);
            this.driver.ClickElement(btnCreateNewApplication, "Create New Application");
           // WaitTillElementDisappeared(lblCreateNewApplication);
             WebDriverWait webDriverCondWait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(180));
             webDriverCondWait.Until(ExpectedConditions.InvisibilityOfElementLocated(lblCreateNewApplication));
                // WaitTillElementDisappeared(lblCreateNewApplication, "Create New Application...", 180);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        /// <summary>
        /// Method to click on start application
        /// </summary>
        private void ClickOnStartApplication()
        {
            try
            {
                this.driver.ClickElement(btnStartApplication, "Start Application");
                this.driver.WaitElementExistsAndVisible(dialogSelectAplication);
                this.driver.WaitElementPresent(lblReviewActiveAppAndPromotionalOffers);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        /// <summary>
        /// Method to handle success popup
        /// </summary>
        private void HandleSuccessPopup()
        {
            try
            {
                this.driver.WaitElementPresent(lblSuccess);
                this.driver.ClickElement(btnOk, "OK");
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        #endregion
    }
}