using System;
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
    public class StipulationsAutomation : AbstractTemplatePage
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
        public void AddStipulations(string stipulation, string requiredFor,string description,string comments,bool met=true)
        {           
            ClickOnAddStipulation();
            VerifyButtons();
            driver.WaitElementPresent(tableNewstipulations);
            driver.ClickElementWithJavascript(tableNewstipulations,"New Stipulations");            
            driver.SelectByVisibleText(dropdownStipulation,stipulation,"Stipulation");
            driver.SelectByVisibleText(dropdownRequiredFor, requiredFor, "Required For");
            driver.SendKeysToElement(txtDescription, description, "Description");
            driver.SendKeysToElement(txtComments, comments, "Comments");
            if (met)
            {
                driver.ClickElement(checkBoxMet, "Met");
            }
            ClickOnSave();
            HandleSuccessPopup();
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Method to click on save button
        /// </summary>
        private void ClickOnSave()
        {

            driver.ClickElement(btnSave, "Save");
        }

        /// <summary>
        /// Method to handle success popup
        /// </summary>
        private void HandleSuccessPopup()
        {
            //if()
            //driver.WaitElementPresent(lblSuccess, "Success");
            By ok = By.XPath("//div[text()='Validation Error']/../../following-sibling::div//button[text()='OK']");
            driver.ClickElement(ok, "OK");
        }

        /// <summary>
        /// Method to click on add stipulation
        /// </summary>
        private void ClickOnAddStipulation()
        {          
            driver.WaitElementPresent(btnAdd);
            driver.ClickElement(btnAdd, "Add");           
        }

       

        /// <summary>
        /// Method to verify buttons
        /// </summary>
        private void VerifyButtons()
        {            
            VerifyButtonIsPresent(btnDelete, "Delete");
            VerifyButtonIsPresent(btnEdit, "Edit");
        }

        /// <summary>
        /// Method of verify button is present
        /// <param name="buttonName">The buttonName.</param>
        ///  /// <param name="name">The name.</param>
        private void VerifyButtonIsPresent(By buttonName,string name)

        {
            try
            {
                if (driver.FindElement(buttonName).Displayed)
                {
                    this.TESTREPORT.LogSuccess(name +" button", String.Format(name +": <mark>{0}</mark> button is displayed", name));
                }
                else
                {
                    this.TESTREPORT.LogSuccess(name + " button", String.Format(name + ": <mark>{0}</mark> button is not displayed", name));
                }
            }
            catch (Exception)
            {

                this.TESTREPORT.LogSuccess(name + " button", String.Format(name + ": <mark>{0}</mark> button is not displayed", name));
            }
            
        }
        #endregion
    }
}