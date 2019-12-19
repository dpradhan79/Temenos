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
    public class AddApplication : AbstractTemplatePage
    {

        #region UI Object Repository
        private By btnAdd = By.XPath("//button[text()='Add']");
        private By dialogAddApplicant = By.XPath("//div[@aria-describedby='aadialogAddApplicant']//span[@class='ui-dialog-title']");
        private By txtAccountNumber = By.Id("aaAccountNumber");
        private By dropdownApplicantRoles = By.XPath("//fieldset[@id='fldsetApplicantRoles']//select[@class='a-field-control a-dropdownlist form-control input-sm']");
        private By btnOK = By.XPath("//div[@aria-describedby='aadialogAddApplicant']//button[text()='OK']");
        private By btnSaveAndClose = By.Id("wsBtnSaveAndClose");
        private By btnGetCreditReport = By.Id("btnGetCreditReport");
        private By dialogGetCreditReport = By.XPath("//div[@aria-describedby='divGetCreditReport']//span[@class='ui-dialog-title']");
        private By dropdownPrimarySubject = By.Id("PrimarySubject");
        private By dropdownCreditBureau = By.Id("CreditBureau");
        private By btnSubmit = By.XPath("//div[@aria-describedby='divGetCreditReport']//button[text()='Submit']");
        private By txtFullName = By.Name("Field-FullName");
        private By loadingRetrievingCreditReport = By.XPath("//div[text()='Retrieving Credit Report...']");
        private By lblCreditBureauResponse = By.XPath("//div[text()='Credit Bureau Response']");
        private By btnCreditBureauResponseNo = By.XPath("//div[text()='Credit Bureau Response']/../../..//button[text()='No']");
        private By btnEdit = By.Id("btnEdit");
        private By loadingSpin = By.XPath("//div[text()='Loading...']");
        private By txtIdentificationAccountNumber = By.Name("Field-AccountNumber");
        private By dropdownApplicantType = By.Name("Field-ApplicantTypeId");
        private By lblCoreMessage = By.XPath("//th[text()='Core Message']/../../following-sibling::tbody//td[3]");
        private By lblHomePhone = By.XPath("//td[text()='Home Phone']/../td[2]");
        private By lblWorkPhone = By.XPath("//td[text()='Work Phone']/../td[2]");
        private By lblEmail = By.Name("Field-Email");
        private By lblAge = By.XPath("//input[@data-autobind='Field_Age']");
        private By dropdownRiskTier = By.Name("Field-RiskTierId");
        private By lblPanelText = By.XPath("//span[text()='Product Offering']/../following-sibling::div//font");
        private By tableScores = By.XPath("//span[text()='Scores']/../following-sibling::div//tbody//tr");
        #endregion

        #region Public Methods

        /// <summary>
        /// Method of add application
       /// <param name="accountNumber"></param>
        /// <param name="applicantNames"></param>
        /// <param name="appliciantroles"></param>
        /// </summary>

        public void AddApplicants(string roles,string accountNumber,string applicantNames)
        {
            try
            {
                driver.WaitElementExistsAndVisible(btnAdd);
                driver.ClickElement(btnAdd,"Add Appliciant");
                HomePage homePage = new HomePage();
                homePage.SwitchToParentFrame();
                driver.WaitElementTextEquals(dialogAddApplicant, "Add Applicant");
                driver.WaitElementPresent(txtAccountNumber);
                driver.SendKeysToElement(txtAccountNumber,accountNumber,"Account Number");
                driver.SelectDropdownItemByText(dropdownApplicantRoles, roles,"Appliciant Roles");
                driver.ClickElement(btnOK,"OK");
                homePage.SwitchToCentralFrame();
                driver.WaitElementExistsAndVisible(btnSaveAndClose);
                driver.VerifyTextValue(txtFullName, applicantNames);
                driver.ClickElement(btnSaveAndClose,"Save And Close");
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public void AddUpdateVerifyCreditReportingSystemScreen(string primarySubject, string creditBureau)
        {
            driver.WaitElementExistsAndVisible(btnGetCreditReport);
            driver.ClickElement(btnGetCreditReport, "Get crredit Report");
            driver.WaitElementExistsAndVisible(dialogGetCreditReport);
            driver.WaitElementTextEquals(dialogGetCreditReport, "Get Credit Report");
            driver.WaitElementPresent(dropdownPrimarySubject);
            driver.SelectDropdownItemByText(dropdownPrimarySubject, primarySubject,"Primary Subject");
            driver.SelectDropdownItemByText(dropdownCreditBureau, creditBureau, "Credit Bureau");
            driver.ClickElement(btnSubmit,"Submit");
            WaitTillElementDisappeared(loadingRetrievingCreditReport);
            if (driver.IsWebElementDisplayed(lblCreditBureauResponse))
            {
                driver.ClickElement(btnCreditBureauResponseNo, "Credit Bureau Response No");
            }

            By PrimarySubject = By.XPath("//form[@id='CreditReportingGrid']//tbody//tr[2]//td[text()='CHACOMMON, MICHAEL']");
            if (driver.IsWebElementDisplayed(PrimarySubject))
            {
                this.TESTREPORT.LogSuccess("Credit Reporting", "Added primary subject is displayed in grid");
            }
            else
            {
                this.TESTREPORT.LogSuccess("Credit Reporting", "Added primary subject is not displayed in grid",this.SCREENSHOTFILE);
            }

            By PrimarySubjectSelected = By.XPath("//td[text()='CHACOMMON, MICHAEL']/parent::tr/parent::tbody//td[@aria-describedby='CreditReportList_InUse']/input[@type='checkbox' and @checked='checked']");
            if (driver.IsWebElementDisplayed(PrimarySubjectSelected))
            {
                this.TESTREPORT.LogSuccess("Credit Reporting", "Added primary subject is displayed in grid and also use report selected by default");
            }
            else
            {
                this.TESTREPORT.LogSuccess("Credit Reporting", "Added primary subject is not displayed in grid", this.SCREENSHOTFILE);
            }
                
        }

        public void VerifyAppliciantsData(string name,string accountNum)
        {
            By selectAppliciants = By.XPath("//span[text()='Applicants']/../../following-sibling::div//div[@class='a-card a-card-message']/b[text()='" + name + "']");
            driver.ClickElementWithJavascript(selectAppliciants,"Select Appliciants "+name);
            driver.WaitElementExistsAndVisible(btnEdit);
            driver.ClickElement(btnEdit,"Edit");
            WaitTillElementDisappeared(loadingSpin);
            driver.WaitElementPresent(txtFullName);
            driver.VerifyTextValue(txtFullName, name);
            driver.VerifyTextValue(txtIdentificationAccountNumber, accountNum);
            driver.ClickElement(btnSaveAndClose, "Save And Close");
            WaitTillElementDisappeared(loadingSpin);
        }

        public void VerifyGridPanelAndFieldsOnPopupScreen(string name, string applicantType, string accountNum, string coreMessage, string homePhone, string workPhone, string email, string age, string riskTier)
        {
            try
            {
            By selectAppliciants = By.XPath("//span[text()='Applicants']/../../following-sibling::div//div[@class='a-card a-card-message']/b[text()='" + name + "']");
            driver.ClickElementWithJavascript(selectAppliciants, "Select Appliciants " + name);
            driver.WaitElementExistsAndVisible(btnEdit);
            driver.ClickElement(btnEdit, "Edit");
            WaitTillElementDisappeared(loadingSpin);
            driver.WaitElementPresent(txtFullName);
            driver.VerifyTextValue(txtFullName, name);
            driver.VerifyTextValue(txtIdentificationAccountNumber, accountNum);           
            string type = driver.GetSelectedOption(dropdownApplicantType, 10);
            driver.VerifyTextValue(type, applicantType);
            driver.VerifyTextValue(lblCoreMessage, coreMessage);
            driver.VerifyTextValue(lblHomePhone, homePhone);
            driver.VerifyTextValue(lblWorkPhone, workPhone);
            driver.VerifyTextValue(lblEmail, email);
            driver.VerifyTextValue(lblAge, age);
            string risk = driver.GetSelectedOption(dropdownRiskTier, 10);
            driver.VerifyTextValue(risk, riskTier);
            driver.ClickElement(btnSaveAndClose, "Save And Close");
            WaitTillElementDisappeared(loadingSpin);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }

        }

        public void VerifyTextInAutomationPanel()
        {
            try
            {
            driver.VerifyTextValue(lblPanelText, "Whether you're buying new or used or refinancing from another lender");
            driver.VerifyTextValue(lblPanelText, "Used Autos");
            driver.VerifyTextValue(lblPanelText, "Auto Refinance");
            driver.VerifyTextValue(lblPanelText, "RATES");
            driver.VerifyTextValue(lblPanelText, "AUTO ");
            driver.VerifyTextValue(lblPanelText, "LOANS ");
            driver.VerifyTextValue(lblPanelText, "NEW");
            driver.VerifyTextValue(lblPanelText, "USED ");
            driver.VerifyTextValue(lblPanelText, "REFINANCE");
            }
            catch (Exception ex)
            {
                 throw ex;
            }
        }

        public void ApplicationPanelScores(string model, string average, string high, string low, string name1, string name2)
        {
          int modelPosition = GetReviewIndicatorNamePosition(model);

          By scoreModel =By.XPath("//span[text()='Scores']/../following-sibling::div//tbody//tr["+modelPosition+"]/td[1]");
          By scoreAverage = By.XPath("//span[text()='Scores']/../following-sibling::div//tbody//tr["+modelPosition+"]/td[2]");
          By scoreHigh = By.XPath("//span[text()='Scores']/../following-sibling::div//tbody//tr["+modelPosition+"]/td[3]");
          By scoreLow = By.XPath("//span[text()='Scores']/../following-sibling::div//tbody//tr["+modelPosition+"]/td[4]");
          By scoreName1= By.XPath("//span[text()='Scores']/../following-sibling::div//tbody//tr["+modelPosition+"]/td[5]");
          By scoreName2 = By.XPath("//span[text()='Scores']/../following-sibling::div//tbody//tr["+modelPosition+"]/td[6]");
    
           try 
	{	        
		    driver.VerifyTextValue(scoreModel, model);
            driver.VerifyTextValue(scoreAverage, average);
            driver.VerifyTextValue(scoreHigh, high);
            driver.VerifyTextValue(scoreLow, low);
            driver.VerifyTextValue(scoreName1, name1);
            driver.VerifyTextValue(scoreName2, name2);
	}
	catch (Exception ex)
	{
		
		throw ex;
	}

        }

        public void EditAndVeifyApplicationPanelAdditionalApplicantDeclarations(string description)
        {
            try
            {
                By  descrip = By.XPath("//td[text()='"+description+"']");
                By  descripchk = By.XPath("//td[text()='"+description+"']/../td/input[@type='checkbox']");
                driver.ClickElement(descrip,description);
                driver.ClickElement(descripchk,"Check Box "+ description );
                bool selected = driver.FindElement(descripchk).Selected;
                if (selected)
                {
                    this.TESTREPORT.LogSuccess("Additional Appliciant Declarations", "User selected the appliciant declarations " + description);
                }
                else
                {
                    this.TESTREPORT.LogFailure("Additional Appliciant Declarations", "User selected the appliciant declarations " + description,this.SCREENSHOTFILE);
                }
            }
            catch (Exception)
            {
                
                throw;
            }

        }
        
        
        private int GetReviewIndicatorNamePosition(String model)
        {
            try
            {
                int position = 0;
                var rows = driver.FindElements(tableScores).Count;
                for (int i = 1; i <= rows; i++)
                {
                    By nameColumn = By.XPath("//span[text()='Scores']/../following-sibling::div//tbody//tr["+i+"]/td[1]");
                    if (driver.GetElementText(nameColumn).Equals(model))
                    {
                        position = i;
                        break;
                    }
                }
                return position;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        }
        
        #endregion

        #region Private Methods

        

        #endregion
    }