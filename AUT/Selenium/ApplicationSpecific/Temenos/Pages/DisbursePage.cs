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
using System.Threading;
using OpenQA.Selenium.Interactions;

namespace AUT.Selenium.ApplicationSpecific.Pages
{
    public class DisbursePage : AbstractTemplatePage
    {

        #region UI Object Repository
        private By btnDisburse = By.Id("btnDisburse");
        private By lblDisburseApplication = By.XPath("//div[@aria-describedby='ws_Disburse_Dialog']//span[@class='ui-dialog-title']");
        private By formDisburse = By.Id("LoanDisclaimer");
        private By checkboxDisburseAcceptLoanTerms = By.Id("checkbox-Disburse_AcceptLoanTerms");
        private By btnDialogDisburse = By.XPath("//div[@aria-describedby='ws_Disburse_Dialog']//button[text()='Disburse']");
        private By loadingDisbursing = By.XPath("//div[text()='Disbursing Application']");
        private By btnErrorOk = By.XPath("//div[text()='Error']/../../following-sibling::div//button[text()='OK']");
        private By btnDiscardChanges = By.XPath("//button[text()='Discard Changes and Continue']");
        #endregion

        #region Public Methods

        /// <summary>
        ///Method of perform disbursement of loan application
        /// <param name="Disburse"></param>
        /// <param name="Accept"></param>
        /// <param name="Disbursemessage"></param>
        /// <param name="Disburseapplication"></param>
        /// </summary>
        public void PerformDisbursementOfLoanApplication()
        {
            try
            {
                string currentDate = DateTime.Today.ToString("MM/dd/yyyy");
                string Month = currentDate.Split('/')[0];
                string dayNumeric = currentDate.Split('/')[1];
                string Year = currentDate.Split('/')[2];
                String expectedText = "Transactions will be sent to the Core Processor indicating that the funds were made available as of the Lifecycle Management Suite Disbursement Date, " + Month + "/" + dayNumeric + "/" + Year + ". This date affects interest accrual on your core processor and may require documents to be regenerated. Select Disburse to continue or Cancel to change the Disbursement Date.";
                driver.ClickElement(btnDisburse, "Disburse");
                Thread.Sleep(3000);
                if (driver.IsWebElementDisplayed(btnDiscardChanges))
                {
                    driver.ClickElement(btnDiscardChanges, "Discard Changes and Continue");
                }
                driver.WaitElementPresent(lblDisburseApplication);
                driver.VerifyTextValue(lblDisburseApplication, "Disburse Application");
                driver.WaitElementPresent(formDisburse);
                driver.VerifyTextValue(formDisburse, expectedText, "Disburse Message");
                driver.ClickElement(checkboxDisburseAcceptLoanTerms, "Accept");
                driver.ClickElement(btnDialogDisburse, "Disburse");
                WaitTillElementDisappeared(loadingDisbursing);
                driver.ClickElement(btnErrorOk, "OK");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        /// <summary>
        /// Method of discard the changes
       /// </summary>
        public void DiscardTheChanges()
        {
            try
            {
                Actions act = new Actions(driver);
                act.SendKeys(OpenQA.Selenium.Keys.Enter).Build().Perform();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion
    }
}