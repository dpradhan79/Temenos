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
    public class LoanTermsAutomation : AbstractTemplatePage
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
        public void EnterFieldValuesInLoanTermPanel(string solveFor, string paymentFrequency, string requestedAmount, string term)
        {
            driver.SelectByVisibleText(dropdownSolveFor, solveFor, "Solve For");
            driver.SelectByVisibleText(dropdownPaymentFrequency, paymentFrequency, "Payment Frequency");
            driver.SendKeysToElementClearFirst(txtRequestedAmount, requestedAmount, "Requested Amount");
            driver.SendKeysToElementClearFirst(txtTerm, term, "Term");
            ClickOnSave();
            HandleSuccessPopup();
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Method to handle success popup
        /// </summary>
        private void HandleSuccessPopup()
        {
            driver.WaitElementPresent(lblSuccess);
            driver.ClickElement(btnOk, "OK");
        }

        /// <summary>
        /// Method to click on save button
        /// </summary>
        private void ClickOnSave()
        {
            
            driver.ClickElement(btnSave, "Save");
        }

        /// <summary>
        /// Method to click on start application
        /// </summary>
        private void ClickOnStartApplication()
        {            
            //driver.ClickElement(btnStartApplication, "Start Application");
        }
        #endregion
    }
}