using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using AUT.Selenium.CommonReUsablePages;
using Engine.UIHandlers.Selenium;
using System.Diagnostics;
using System.Threading;
using Engine.Factories;
using Engine.Setup;
using TestReporter;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Support.UI;

namespace AUT.Selenium.ApplicationSpecific.Pages
{
    public class Utility : AbstractTemplatePage
    {
        #region UI Object Repository
        private By txtSearchCombo = By.XPath("//input[@name='searchCombo']");
        private By txtBasicSearchData = By.XPath("//input[@name='basicSearchData']");
        private By tblSearchIcon = By.XPath("//table[@class='x-btn go x-btn-noicon x-box-item' or @class='x-btn go x-btn-noicon x-box-item x-btn-over']");
        private By btnIssueCase = By.XPath("//button[text()='Issue Case']");
        private By btnIssueSuccessfulOK = By.XPath("//button[text()='OK']");
        private By imgPolicyNumberFromSearchCombo = By.XPath("//div[@class='x-layer x-combo-list x-combo-list-searchCombo']//div//div[7]//img");

        private By spanLoading = By.XPath("//span[contains(text(),'Loading...')]/ancestor::div[@class=' x-window x-window-plain x-window-dlg' and not(contains(@style,'visibility: hidden;'))]");
        private By spanPleaseWait = By.XPath("//span[contains(text(),'Please Wait...')]/ancestor::div[@class=' x-window x-window-plain x-window-dlg' and not(contains(@style,'visibility: hidden;'))]");
        private By spanPendingCases = By.XPath("//span[contains(text(),'Loading Pending Cases for the User..')]/ancestor::div[@class=' x-window x-window-plain x-window-dlg' and not(contains(@style,'visibility: hidden;'))]");
        #endregion

        public string FILENUMBER = "";
        
        /// <summary>
        /// Method to close browser
        /// </summary>
        public void CloseBrowser()
        {
            driver.Manage().Cookies.DeleteAllCookies();
            driver.RefreshPage();
        }

        /// <summary>
        /// Method to wait until animation loading
        /// </summary>
        public void WaitUntilAnimationLoadingGetsDisappeard()
        {
            if (driver.IsWebElementDisplayed(By.Id("anim_loading")))
            {
                driver.RefreshPage();
                this.SimulateThinkTimeInMilliSecs(3000);

            }
        }


        /// <summary>
        /// Select value from drop down
        /// </summary>
        /// <param name="dropDownnToSelect">The elememt to be selected from drop down.</param>
        ///  <param name="controlName">Name of the control.</param>
        /// <param name="dropDownListClassName">Name of the list item class.</param>
        ///  <param name="elementToSelect">The element to be select from drop down.</param>
        public void SelectValueFromDropDownList(By dropDownnToSelect, string controlName, string dropDownListClassName, string elementToSelect)
        {
            if (elementToSelect.Length > 0)
            {
                this.SimulateThinkTimeInMilliSecs(1000);
                driver.ClickElement(dropDownnToSelect, controlName);
                this.SimulateThinkTimeInMilliSecs(3000);
                IList<IWebElement> List = driver.FindElements(By.XPath("//div[contains(@class,'x-layer x-combo-list') and contains(@style,'visibility: visible')]" +
                    "/div/div[contains(@class,'" + dropDownListClassName + "')]"));
                if (List.Count == 0)
                {
                    driver.ClickElement(dropDownnToSelect, controlName);
                    this.SimulateThinkTimeInMilliSecs(3000);
                }

                ClickAnElementFromList(dropDownListClassName, elementToSelect);
                this.SimulateThinkTimeInMilliSecs(2000);
            }
        }

        /// <summary>
        /// Clicks an element from list.
        /// </summary>
        /// <param name="listItemClassName">Name of the list item class.</param>
        /// <param name="elementToBeClicked">The element to be clicked.</param>
        public void ClickAnElementFromList(string listItemClassName, string elementToBeClicked)
        {
            bool flag = false;
            //int ListCount = driver.FindElements(By.XPath("//div[@class='x-layer x-combo-list ']/div/div[contains(@class,'" + listItemClassName + "')]")).Count;
            IList<IWebElement> List = driver.FindElements(By.XPath("//div[contains(@class,'x-layer x-combo-list') and contains(@style,'visibility: visible')]" +
                "/div/div[contains(@class,'" + listItemClassName + "')]"));

            for (int i = 0; i < List.Count; i++)
            {
                if ((elementToBeClicked.Trim().ToLower()).Equals(List[i].Text.Trim().ToLower()))
                {
                    By txtElementToBeClicked = By.XPath("(//div[contains(@class,'x-layer x-combo-list') and contains(@style,'visibility: visible')]/div" +
                        "/div[contains(@class,'" + listItemClassName + "')])[" + (i + 1) + "]");
                    driver.ClickElement(txtElementToBeClicked, elementToBeClicked);
                    this.SimulateThinkTimeInMilliSecs(1000);
                    flag = true;
                    break;
                }
            }
            if (flag == true)
            {
                this.TESTREPORT.LogSuccess("Select Value", "Sucessfully selected value -- " + elementToBeClicked);
            }
            else
            {
                this.TESTREPORT.LogFailure("Select Value", "Unable to select value -- " + elementToBeClicked);
            }
        }

        /// <summary>
        /// Method to sendkeys to element
        /// <param name="locator">The locater.</param>
        /// <param name="controlName"> Name of the control.</param>
        /// <param name="text">Value of text.</param>
        /// </summary>
      public void SendKeysToElementIfDataNotNull(By locator, string text, string controlName)
        {
            if (text.Length > 0)
                driver.SendKeysToElementClearFirst(locator, text, controlName);
        }

        /// <summary>
        /// Method to Wait for seconds
        /// </summary>
        public void WaitForSeconds(int NumberOfSeconds)
        {
            this.SimulateThinkTimeInMilliSecs(NumberOfSeconds * 1000);
        }

        /// <summary>
        /// Method to sendkeys element
        /// </summary>
        /// <param name="requiredLocator">By Required locater.</param>
        /// <param name="governmentIdValue">Value of the government id.</param>
        /// <param name="firstName">Name of the string. </param>
        /// <param name="controlName"> Name of the control.</param>
        public void SendKeysToElementIfDataNotNull(By requiredLocator, string governmentIdValue, string firstName, string controlName)
        {
            if (firstName.Length > 0)
            {
                if (governmentIdValue.ToLower().Contains("random"))
                {
                    //driver.SendKeysToElementClearFirst(requiredLocator, EngineSetup.GenereateRandomNumber(RandomNumberType.GovernmentId), "SocialSecurityNumber");
                    TESTREPORT.LogSuccess(controlName, "Test data values is 'Random' so generating random number");
                }
                else if (governmentIdValue.Length > 9)
                {
                    AssignExistingSSN(requiredLocator, governmentIdValue);
                }
                else if (governmentIdValue.Length > 0)
                {
                    driver.SendKeysToElementClearFirst(requiredLocator, governmentIdValue, "SocialSecurityNumber");
                    TESTREPORT.LogSuccess(controlName, "Keying SS# as per test data");
                }
            }
        }

        /// <summary>
        /// Method to assign existing SSN
        ///<param name="requiredLocator"> Required locater.</param>
        /// <param name="governmentIdValue">Value of the government id.</param>
        /// <returns>Returns value.</returns>
       /// </summary>
        private string AssignExistingSSN(By requiredLocator, string governmentId)
        {
            string retrunValue = string.Empty;

            switch (governmentId)
            {
                case "Insured SSN":
                    retrunValue = GetAttributeValue(By.Name("PrimInsGovtID"));
                    driver.SendKeysToElementClearFirst(requiredLocator, retrunValue, "Primary Insured SSN");
                    break;
                case "Additional Insured SSN":
                    retrunValue = GetAttributeValue(By.XPath("//div[@class='x-grid3-cell-inner x-grid3-col-SSN']"));
                    driver.SendKeysToElement(requiredLocator, retrunValue, "Addional Insured SSN");
                    break;
                case "Child1 SSN":
                    retrunValue = GetAttributeValue(By.XPath("(//div[@class='x-grid3-cell-inner x-grid3-col-GovtID'])[1]"));
                    driver.SendKeysToElementClearFirst(requiredLocator, retrunValue, "Child1 SSN");
                    break;
                case "Child2 SSN":
                    retrunValue = GetAttributeValue(By.XPath("(//div[@class='x-grid3-cell-inner x-grid3-col-GovtID'])[2]"));
                    driver.SendKeysToElementClearFirst(requiredLocator, retrunValue, "Child2 SSN");
                    break;
                case "Child3 SSN":
                    retrunValue = GetAttributeValue(By.XPath("(//div[@class='x-grid3-cell-inner x-grid3-col-GovtID'])[3]"));
                    driver.SendKeysToElementClearFirst(requiredLocator, retrunValue, "Child3 SSN");
                    break;
                default:
                    break;
            }

            return retrunValue;
        }

        /// <summary>
        /// Gets the attribute value
        /// </summary>
        /// <param name="locator">The locater.</param>
        /// <returns>Returns attribute value.</returns>
        private string GetAttributeValue(By locator)
        {
            string returnValue = driver.GetElementAttribute(locator, "value");

            if (returnValue == "" || returnValue == null)
                returnValue = driver.GetElementText(locator);
            else if (string.IsNullOrEmpty(returnValue))
                returnValue = "NaN";

            return returnValue;
        }

        /// <summary>
        /// gets the name of the column
        /// </summary>
        /// <param name="sheetName"> Name of the sheet</param>
        /// <param name="columnName">Name of the column </param>
        /// <param name="suffix"> </param>
        /// <returns>Returns column name in string format.</returns>
        public string GetColumnName(string sheetName, string columnName, object suffix)
        {
            string expectedColumnName = sheetName + "{0}" + suffix;
            return string.Format(expectedColumnName, columnName);
        }

        /// <summary>
        /// Gets the name of the column
        /// </summary>
        /// <param name="sheetName">Name of the sheet.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="index">value of index.</param>
        /// <param name="alphabet">The alphabet.</param>
        /// <returns>Returns column name in string format.</returns>
        public string GetColumnName(string sheetName, string columnName, object index, char alphabet = '\0')
        {
            string expectedColumnName = sheetName + "{0}" + index + alphabet;
            return string.Format(expectedColumnName, columnName);
        }

        //public By GetRequiredRowToSelect(string[] requiredColumnNames)
        //{
        //    string requiredRowToClick = "//div[text()='{0}']/parent::td/preceding::td/div[text()='{1}']/parent::td/parent::tr";

        //    return By.XPath(string.Format(requiredRowToClick, requiredColumnNames[0], requiredColumnNames[1]));
        //}

        /// <summary>Gets the required row to select.</summary>
        /// <param name="requiredColumnNames">Name of the required column.</param>
        /// <returns>Returns the element XPath with Index.</returns>
        public By GetRequiredRowToSelect(string[] requiredColumnNames)
        {
            string requiredRowToClick = string.Format("//div[text()='{0}']", requiredColumnNames[0]);

            if (requiredColumnNames.Length > 1)
            {
                for (int i = 1; i < requiredColumnNames.Length; i++)
                {
                    requiredRowToClick = requiredRowToClick + string.Format("/parent::td//following-sibling::td/div[text()='{0}']", requiredColumnNames[i]);
                }
            }

            requiredRowToClick = requiredRowToClick + "/parent::td/parent::tr";
            return By.XPath(string.Format("({0})[{1}]", requiredRowToClick, GetVisibleElementIndex(requiredRowToClick)));
        }

        /// <summary>
        /// Enter the value in plain textbox 
        /// </summary>
        /// <param name="locator">The locater.</param>
        /// <param name="textToEnter">Enter to the text.</param>
        /// <param name="controlName">Name of the control.</param>
       public void EnterValuesInPlainTextBox(By locator, string textToEnter, string controlName)
        {
            ReadOnlyCollection<IWebElement> elements = driver.FindElements(locator);
            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i].Displayed == true)
                {
                    if (textToEnter.Length > 0)
                    {
                        elements[i].Clear();
                        elements[i].SendKeys(textToEnter);
                        break;
                    }
                }
            }

            this.SimulateThinkTimeInMilliSecs(3000);
        }

        /// <summary>
        ///  Method to click on dropdown and select the value from list
        /// </summary>
        /// <param name="locator">The locater.</param>
        /// <param name="valueToSelect">Select the value.</param>
        /// <param name="className">Name of the class.</param>
        public void SelectPlainDropDownValues(By locator, string className, string valueToSelect)
        {
            ReadOnlyCollection<IWebElement> elements = driver.FindElements(locator);
            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i].Displayed == true && valueToSelect.Length > 0)
                {
                    elements[i].Click();
                    this.SimulateThinkTimeInMilliSecs(2000);
                    ClickAnElementFromList(className, valueToSelect);
                    break;
                }
            }

            this.SimulateThinkTimeInMilliSecs(2000);
        }

        /// <summary>
        ///  Method to validate textbox values
        /// </summary>
        /// <param name="locator">The locater.</param>
        /// <param name="textToValidate">Validte the text.</param>
        /// <param name="controlName">Name of the control.</param>
        public void ValidateTextBoxValues(By locator, string textToValidate, string controlName)
        {
            ReadOnlyCollection<IWebElement> elements = driver.FindElements(locator);

            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i].Displayed == true)
                {
                    driver.VerifyTextValue(locator, textToValidate, controlName);
                    break;
                }
            }

            this.SimulateThinkTimeInMilliSecs(2000);
        }

        /// <summary>
        /// Get the Xpath
        /// </summary>
        /// <param name="locator">The locater. </param>
        /// <param name="attributeName">Name of the attribute. </param>
        /// <returns></returns>
        public By GetXpath(string locator, string attributeName)
        {
            return By.XPath(string.Format(locator, attributeName));
        }

        /// <summary>
        /// Gets the index of the visible element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>Visible element's index</returns>
        public int GetVisibleElementIndex(string element)
        {
            ReadOnlyCollection<IWebElement> elements = driver.FindElements(By.XPath(element));
            int elementIndex = 0;

            foreach (var item in elements)
            {
                if (elements[elementIndex].Displayed)
                {
                    elementIndex++;
                    break;
                }

                elementIndex++;
            }

            //for (int i = 1; i < elements.Count; i++)
            //{
            //    if (elements[i].Displayed == true)
            //    {
            //        elementIndex = i;
            //        break;
            //    }

            //    elementIndex = i;
            //}

            this.SimulateThinkTimeInMilliSecs(2000);
            return elementIndex;
        }
        /// <summary>
        /// Gets visile element with index 
        /// </summary>
        /// <param name="element">THe element.</param>
        /// <returns>Visible element's index</returns>

        public By GetVisileElementWithIndex(string element)
        {
            By visibleElement = null;

            ReadOnlyCollection<IWebElement> elements = driver.FindElements(By.XPath(element));
            int elementIndex = 0;

            foreach (var item in elements)
            {
                if (elements[elementIndex].Displayed)
                {
                    elementIndex++;
                    break;
                }

                elementIndex++;
            }

            element = string.Format("{0}[{1}]", element, elementIndex);

            return By.XPath(element);
        }

        /// <summary>
        /// Gets key in date
        /// </summary>
        /// <param name="date"> </param>
        /// <returns>date</returns>
        public string KeyInDate(string date)
        {
            if (date.ToLower() == "current date")
                date = DateTime.Now.ToString("MM/dd/yyyy");

            return date;
        }
    }
}
