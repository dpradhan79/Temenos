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
        public void EnterFieldValuesInPrimaryAppliciantPanel(string FieldsAndValuesToEnter, string value = "")
        {
            try
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
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Enter membership start date
        /// <param name="startDate"></param>
        /// </summary>
        public void EnterMemberShipStartDate(string startDate)
        {
            try
            {
                driver.SendKeysToElementClearFirst(txtMembershipStartDate, startDate, "Membership Start Date");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Verify membership start date
        /// <param name="startDate"></param>
        /// </summary>
        public void VerifyMemberShipStartDate(string startDate)
        {
            try
            {
                driver.VerifyTextValue(txtMembershipStartDate, startDate);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Method to click on do not pull credit
        /// </summary>
        public void ClickOnDoNotPullCredit()
        {
            try
            {
                driver.ClickElement(checkBoxDoNotPullCredit, "DoNotPullCredit");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void SelectCurrentAddressState(String state)
        {
            try
            {
                driver.WaitElementPresent(dropdownCurrentAddressState);
                driver.SelectDropdownItemByText(dropdownCurrentAddressState, state, "Current Address State:");
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