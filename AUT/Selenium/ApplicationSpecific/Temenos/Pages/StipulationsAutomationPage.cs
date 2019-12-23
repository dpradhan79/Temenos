﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.UIHandlers.Selenium;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Engine.Setup;
using AUT.Selenium.CommonReUsablePages;
using System.Threading;
using OpenQA.Selenium.Interactions;

namespace AUT.Selenium.ApplicationSpecific.Pages
{
    public class StipulationsAutomationPage : AbstractTemplatePage
    {

        #region UI Object Repository
        private By btnAdd = By.Id("btnAddStip");
        private By btnAddMultiple = By.Id("btnAddMultiple");
        private By btnEdit = By.Id("btnEditStip");
        private By btnDelete = By.Id("btnDeleteStip");
        private By dropdownStipulation = By.Name("StipName");
        private By dropdownRequiredFor = By.Name("RequiredFor");
        private By txtDescription = By.Name("Description");
        private By txtComments = By.Name("Comments");
        private By tableNewstipulations = By.XPath("//table[@id='stipulationList']//tr[@class='ui-widget-content jqgrow ui-row-ltr ui-state-highlight']");
        private By lblSuccess = By.XPath("//div[text()='Success']");
        private By btnOk = By.XPath("//div[text()='Success']/../../following-sibling::div//button[text()='OK']");
        private By btnSave = By.Id("wsBtnSave");
        private By checkBoxMet = By.Name("IsMet");
        #endregion

        #region Public Methods

        /// <summary>
        /// Method of add stipulation
        /// <param name="stipulation">The stipulation.</param>
        /// <param name="requiredFor">The requiredfor.</param>
        /// <param name="description">The descrition.</param>
        /// <param name="comments">The comments.</param>
        /// </summary>
        public void AddStipulations(Dictionary<string, string> validationTestData, bool met = true, int index = 0)
        {
            try
            {
                ClickOnAddStipulation();
                VerifyButtons();
                driver.WaitElementPresent(tableNewstipulations);
                driver.ClickElementWithJavascript(tableNewstipulations, "New Stipulations");
                driver.SelectByVisibleText(dropdownStipulation, validationTestData["Stipulation" + index], "Stipulation");
                driver.SelectByVisibleText(dropdownRequiredFor, validationTestData["RequiredFor" + index], "Required For");
                driver.SendKeysToElement(txtDescription, validationTestData["Description" + index], "Description");
                driver.SendKeysToElement(txtComments, validationTestData["Comments" + index], "Comments");
                if (met)
                {
                    driver.ClickElement(checkBoxMet, "Met");
                }
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

        /// <summary>
        /// Method to handle success popup
        /// </summary>
        private void HandleSuccessPopup()
        {
            try
            {
                By ok = By.XPath("//div[text()='Validation Error']/../../following-sibling::div//button[text()='OK']");
                driver.ClickElement(ok, "OK");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Method to click on add stipulation
        /// </summary>
        private void ClickOnAddStipulation()
        {
            try
            {
                driver.WaitElementPresent(btnAdd);
                driver.ClickElement(btnAdd, "Add");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



        /// <summary>
        /// Method to verify buttons
        /// </summary>
        private void VerifyButtons()
        {
            try
            {
                VerifyButtonIsPresent(btnDelete, "Delete");
                VerifyButtonIsPresent(btnEdit, "Edit");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Method of verify button is present
        /// <param name="buttonName">The buttonName.</param>
        ///  /// <param name="name">The name.</param>
        private void VerifyButtonIsPresent(By buttonName, string name)
        {
            try
            {
                if (driver.FindElement(buttonName).Displayed)
                {
                    this.TESTREPORT.LogSuccess(name + " button", String.Format(name + ": <mark>{0}</mark> button is displayed", name));
                }
                else
                {
                    this.TESTREPORT.LogSuccess(name + " button", String.Format(name + ": <mark>{0}</mark> button is not displayed", name));
                }
            }
            catch (Exception ex)
            {

                this.TESTREPORT.LogSuccess(name + " button", String.Format(name + ": <mark>{0}</mark> button is not displayed", name));
                throw ex;
            }

        }
        #endregion
    }
}