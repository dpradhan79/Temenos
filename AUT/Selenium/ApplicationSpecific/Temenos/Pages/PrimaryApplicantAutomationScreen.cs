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
    public class PrimaryApplicantAutomationScreen : AbstractTemplatePage
    {
       
        #region UI Object Repository
        private By txtMembershipStartDate = By.Name("edit-PA-Field-MembershipStartDate");
        private By checkBoxDoNotPullCredit = By.Name("CheckboxNamePA-Field-DoNotPullCredit");
        private By lblSuccess = By.XPath("//div[text()='Success']");
        private By btnOk = By.XPath("//div[text()='Success']/../../following-sibling::div//button[text()='OK']");
        private By btnSave = By.Id("wsBtnSave");
        private By dropdownCurrentAddressState = By.Name("PA_CA-Field-StateId");

        #endregion

        #region Public Methods

        /// <summary>
        /// Enter field values in primary application panel 
        /// <param name="FieldsAndValuesToEnter">Enter the fields and values.</param>
        /// </summary>
        public void EnterFieldValuesInPrimaryAppliciantPanel(string FieldsAndValuesToEnter,string value="")
        {
            if (FieldsAndValuesToEnter.Equals("MemeberShipStartDate"))
            {
                EnterMemberShipStartDate(value);
            }
            else if (FieldsAndValuesToEnter.Equals("DoNotPullCredit"))
            {
                ClickOnDoNotPullCredit();
            }
            ClickOnSave();
            HandleSuccessPopup();
        }

        /// <summary>
       /// Enter membership start date
       /// <param name="startDate"></param>
       /// </summary>
        public void EnterMemberShipStartDate(string startDate)
        {
            driver.SendKeysToElementClearFirst(txtMembershipStartDate, startDate, "Membership Start Date");  
        }

        /// <summary>
        /// Verify membership start date
        /// <param name="startDate"></param>
        /// </summary>
        public void VerifyMemberShipStartDate(string startDate)
        {
            driver.VerifyTextValue(txtMembershipStartDate, startDate);
        }

        /// <summary>
        /// Method to click on do not pull credit
       /// </summary>
        public void ClickOnDoNotPullCredit()
        {
            driver.ClickElement(checkBoxDoNotPullCredit, "DoNotPullCredit");
        }

        public void SelectCurrentAddressState(String state)
        {
            driver.WaitElementPresent(dropdownCurrentAddressState);
            driver.SelectDropdownItemByText(dropdownCurrentAddressState, state, "Current Address State:");
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