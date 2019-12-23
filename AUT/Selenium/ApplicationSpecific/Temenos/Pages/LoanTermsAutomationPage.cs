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

namespace AUT.Selenium.ApplicationSpecific.Pages
{
    public class LoanTermsAutomationPage : AbstractTemplatePage
    {

        #region UI Object Repository
        private By dropdownSolveFor = By.Name("Field-SolveForId");
        private By dropdownPaymentFrequency = By.Name("Field-PaymentFrequencyId");
        private By txtRequestedAmount = By.Name("Field-RequestedAmount");
        private By txtTerm = By.Name("Field-Term");
        private By btnSave = By.Id("wsBtnSave");
        private By lblSuccess = By.XPath("//div[text()='Success']");
        private By btnOk = By.XPath("//div[text()='Success']/../../following-sibling::div//button[text()='OK']");

        #endregion

        #region Public Methods

        /// <summary>
        ///Enter field values in loan term panel 
        /// <param name="solveFor"></param>
        /// <param name="paymentFrequency"></param>
        /// <param name="requestedAmount"></param>
        /// <param name="term"></param>
        /// </summary>
        public void EnterFieldValuesInLoanTermPanel(Dictionary<string, string> validationTestData)
        {
            try
            {
                driver.SelectByVisibleText(dropdownSolveFor, validationTestData["SolveFor"], "Solve For");
                driver.SelectByVisibleText(dropdownPaymentFrequency, validationTestData["PaymentFrequency"], "Payment Frequency");
                driver.SendKeysToElementClearFirst(txtRequestedAmount, validationTestData["RequestedAmount"], "Requested Amount");
                driver.SendKeysToElementClearFirst(txtTerm, validationTestData["Term"], "Term");
                ClickOnSave();
                HandleSuccessPopup();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Method to handle success popup
        /// </summary>
        private void HandleSuccessPopup()
        {
            try
            {
                driver.WaitElementPresent(lblSuccess);
                driver.ClickElement(btnOk, "OK");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Method to click on save button
        /// </summary>
        private void ClickOnSave()
        {
            try
            {
                driver.ClickElement(btnSave, "Save");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion
    }
}