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
using OpenQA.Selenium.Interactions;
using System.Threading;

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
        private By btnSave = By.Id("wsBtnSave");
        private By btnAddLiabilities = By.XPath("//button[text()='Add']");
        private By divAddLiability = By.Id("EditLiabilityContainer");
        private By spanAddLiability = By.XPath("//div[@id='EditLiabilityContainer']//span[text()='Add Liability']");
        private By checkboxSecured = By.Id("checkbox-Field-IsSecured");
        private By checkboxExpense = By.Id("checkbox-Field-IsExpense");
        private By checkboxRefi = By.Id("checkbox-Field-IsRefinanceAddon");
        private By txtPecInclude = By.Id("Field-AdjustmentPercentage");
        private By dropdownType = By.Id("Field-AccountTypeId");
        private By txtBalance = By.Id("Field-Balance");
        private By txtPayment = By.Id("Field-PaymentAmount");
        private By dropdownPaymentFrequency = By.Id("Field-PaymentFrequencyId");
        private By txtLimit = By.Id("Field-Limit");
        private By dropdownCategory = By.Id("Field-CategoryId");
        private By txtAccount = By.Id("Field-AccountNumber");
        private By btnLiabilitySave = By.XPath("//button[text()='Save and Close']");
        private By btnDeleteApplicants = By.Id("btnDelete");
        private By lblDeleteAppliciant = By.XPath("//div[text()='Delete Applicant']");
        private By btnYes = By.XPath("//div[text()='Delete Applicant']/../../..//button[text()='Yes']");
        private By lblSuccess = By.XPath("//div[text()='Success']");
        private By btnOk = By.XPath("//div[text()='Success']/../../following-sibling::div//button[text()='OK']");
        private By btnMore = By.Id("btnMenuOther");
        private By btnWithdraw = By.Id("btnWithdraw");
        private By lblWithdrawApplication = By.XPath("//span[text()='Withdraw Application']");
        private By dropdownWithdrawnReason = By.Id("Withdraw_ddlDeclineReason");
        private By sendAdverseActionTo = By.XPath("//input[contains(@id,'checkbox-Withdraw_AdverseApplicant')]");
        private By checkboxSendAdverseActionToCheckState = By.XPath("//input[contains(@id,'checkbox-Withdraw_AdverseApplicant') and @checked='checked']");
        private By btnWithdrawApplication = By.XPath("//button[text()='Withdraw']");
        #endregion

        #region Public Methods

        /// <summary>
        /// Method of add application
        /// <param name="accountNumber"></param>
        /// <param name="applicantNames"></param>
        /// <param name="appliciantroles"></param>
        /// </summary>

        public void AddApplicants(string roles, string accountNumber, string applicantNames)
        {
            try
            {
                driver.WaitElementExistsAndVisible(btnAdd);
                driver.ClickElement(btnAdd, "Add Appliciant");
                HomePage homePage = new HomePage();
                homePage.SwitchToParentFrame();
                driver.WaitElementTextEquals(dialogAddApplicant, "Add Applicant");
                driver.WaitElementPresent(txtAccountNumber);
                driver.SendKeysToElement(txtAccountNumber, accountNumber, "Account Number");
                driver.SelectDropdownItemByText(dropdownApplicantRoles, roles, "Appliciant Roles");
                driver.ClickElement(btnOK, "OK");
                homePage.SwitchToCentralFrame();
                driver.WaitElementExistsAndVisible(btnSaveAndClose);
                driver.VerifyTextValue(txtFullName, applicantNames);
                driver.ClickElement(btnSaveAndClose, "Save And Close");
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
            driver.SelectDropdownItemByText(dropdownPrimarySubject, primarySubject, "Primary Subject");
            driver.SelectDropdownItemByText(dropdownCreditBureau, creditBureau, "Credit Bureau");
            driver.ClickElement(btnSubmit, "Submit");
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
                this.TESTREPORT.LogSuccess("Credit Reporting", "Added primary subject is not displayed in grid", this.SCREENSHOTFILE);
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

        public void VerifyAppliciantsData(string name, string accountNum)
        {
            By selectAppliciants = By.XPath("//span[text()='Applicants']/../../following-sibling::div//div[@class='a-card a-card-message']/b[text()='" + name + "']");
            driver.ClickElementWithJavascript(selectAppliciants, "Select Appliciants " + name);
            driver.WaitElementExistsAndVisible(btnEdit);
            driver.ClickElement(btnEdit, "Edit");
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
                By autoLabels = By.XPath("//span[text()='Application Panels - Automation']/../../following-sibling::div//h2");                
                driver.VerifyTextValue(lblPanelText, "Whether you're buying new or used or refinancing from another lender");
                driver.VerifyTextValue(lblPanelText, "Used Autos");
                driver.VerifyTextValue(lblPanelText, "Auto Refinance");
                driver.VerifyTextValue(lblPanelText, "RATES");
                string text = driver.GetElementText(autoLabels);
                driver.VerifyTextValue(autoLabels, "AUTO LOANS - NEW, USED &REFINANCE");        
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ApplicationPanelScores(string model, string average, string high, string low, string name1, string name2)
        {
            int modelPosition = GetReviewIndicatorNamePosition(model);

            By scoreModel = By.XPath("//span[text()='Scores']/../following-sibling::div//tbody//tr[" + modelPosition + "]/td[1]");
            By scoreAverage = By.XPath("//span[text()='Scores']/../following-sibling::div//tbody//tr[" + modelPosition + "]/td[2]");
            By scoreHigh = By.XPath("//span[text()='Scores']/../following-sibling::div//tbody//tr[" + modelPosition + "]/td[3]");
            By scoreLow = By.XPath("//span[text()='Scores']/../following-sibling::div//tbody//tr[" + modelPosition + "]/td[4]");
            By scoreName1 = By.XPath("//span[text()='Scores']/../following-sibling::div//tbody//tr[" + modelPosition + "]/td[5]");
            By scoreName2 = By.XPath("//span[text()='Scores']/../following-sibling::div//tbody//tr[" + modelPosition + "]/td[6]");

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

        public void EditAndVeifyApplicationPanelAdditionalApplicantDeclarations(string description, string value)
        {
            try
            {
                By descrip = By.XPath("//table[@id='AADeclarationsList']//td[text()='" + description + "']");
                By descripchk = By.XPath("//table[@id='AADeclarationsList']//td[text()='" + description + "']/../td/input[@type='checkbox']");
                driver.ClickElement(descrip, description);
                driver.ClickElement(descripchk, "Check Box " + description);

                bool selected = driver.FindElement(descripchk).Selected;
                if (selected)
                {
                    this.TESTREPORT.LogSuccess("Additional Appliciant Declarations", "User selected the appliciant declarations " + description);
                }
                else
                {
                    this.TESTREPORT.LogFailure("Additional Appliciant Declarations", "User selected the appliciant declarations " + description, this.SCREENSHOTFILE);
                }
                By input = By.XPath("//table[@id='AADeclarationsList']//td[text()='" + description + "']/..//input[@name='AnswerExplanation']");
                driver.SendKeysToElement(input, value, description);
                driver.ClickElement(input, description);
                //Actions act = new Actions(driver);
                //act.KeyDown(OpenQA.Selenium.Keys.Enter).KeyUp(OpenQA.Selenium.Keys.Enter).Build().Perform();
                driver.FindElement(input).SendKeys(OpenQA.Selenium.Keys.Enter);

                By explanationValue = By.XPath("//table[@id='AADeclarationsList']//td[text()='" + description + "']/../td[@aria-describedby='AADeclarationsList_AnswerExplanation']");
                driver.VerifyTextValue(explanationValue, value);

            }
            catch (Exception)
            {

                throw;
            }

        }

        public void EditAndVeifyApplicationPanelPrimaryApplicantDeclarations(string description, string value)
        {
            try
            {
                By descrip = By.XPath("//table[@id='PADeclarationsList']//td[text()='" + description + "']");
                By descripchk = By.XPath("//table[@id='PADeclarationsList']//td[text()='" + description + "']/../td/input[@type='checkbox']");
                driver.ClickElement(descrip, description);
                driver.ClickElement(descripchk, "Check Box " + description);

                bool selected = driver.FindElement(descripchk).Selected;
                if (selected)
                {
                    this.TESTREPORT.LogSuccess("Primary Appliciant Declarations", "User selected the appliciant declarations " + description);
                }
                else
                {
                    this.TESTREPORT.LogFailure("Primary Appliciant Declarations", "User selected the appliciant declarations " + description, this.SCREENSHOTFILE);
                }
                By input = By.XPath("//table[@id='PADeclarationsList']//td[text()='" + description + "']/..//input[@name='AnswerExplanation']");
                driver.SendKeysToElement(input, value, description);
                //Actions act = new Actions(driver);
                //act.KeyDown(OpenQA.Selenium.Keys.Enter).KeyUp(OpenQA.Selenium.Keys.Enter).Build().Perform();
                driver.FindElement(input).SendKeys(OpenQA.Selenium.Keys.Enter);

                By explanationValue = By.XPath("//table[@id='PADeclarationsList']//td[text()='" + description + "']/../td[@aria-describedby='PADeclarationsList_AnswerExplanation']");
                driver.VerifyTextValue(explanationValue, value);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void SaveApplicationPanelAutomation()
        {
            try
            {
                driver.WaitElementPresent(btnSave);
                driver.ClickElement(btnSave, "Save");
                Thread.Sleep(3000);
                HandleSuccessPopup();
                WaitTillElementDisappeared(loadingSpin);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public void AddLiabilty(string percentageInclude, string frequency, string balance, string limit, string payment, string accountNumber,
            string category, string type, string name, string value)
        {

            try
            {

                driver.ClickElement(btnAddLiabilities, "Add button");
                Thread.Sleep(3000);
                driver.SwitchToNewWindow();
                driver.ClickElement(checkboxSecured, "Secured");
                driver.ClickElement(checkboxExpense, "Expense");
                driver.ClickElement(checkboxRefi, "Refi");
                //driver.SendKeysToElement(txtPecInclude, percentageInclude, "% Included");
                driver.SendKeysToElement(txtBalance, balance, "Balance");
                driver.SendKeysToElement(txtLimit, limit, "Limit");
                driver.SendKeysToElement(txtPayment, payment, "Payment Amount");
                driver.SelectByVisibleText(dropdownPaymentFrequency, frequency, "Payment Frequency");
                driver.SendKeysToElement(txtAccount, accountNumber, "Account Number");
                driver.SelectDropdownItemByText(dropdownCategory, category, "Category");
                driver.SelectDropdownItemByText(dropdownType, type, "Type");
                AddPercentageToApplicant(name, value);
                driver.ClickElement(btnLiabilitySave, "Save");
                driver.SwitchBackToMainWindow();
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        private void AddPercentageToApplicant(string name, string value)
        {

            By applicantName = By.XPath("//td[text()='" + name + "']");
            By mapTOLiability = By.XPath("//td[text()='" + name + "']/..//input[@type='checkbox']");

            driver.ClickElement(applicantName, name);
            driver.ClickElement(mapTOLiability, "Check Box " + name);

            bool selected = driver.FindElement(mapTOLiability).Selected;
            if (selected)
            {
                this.TESTREPORT.LogSuccess("Add Liabilty", "Map to Liabilty" + name);
            }
            else
            {
                this.TESTREPORT.LogFailure("Add Liabilty", "Map to Liabilty " + name, this.SCREENSHOTFILE);
            }
            By input = By.XPath("//td[text()='" + name + "']/..//input[@name='PercentageResponsible']");
            driver.FindElement(input).Clear();
            driver.SendKeysToElement(input, value, name);
            //Actions act = new Actions(driver);
            //act.KeyDown(OpenQA.Selenium.Keys.Enter).KeyUp(OpenQA.Selenium.Keys.Enter).Build().Perform();
            driver.FindElement(input).SendKeys(OpenQA.Selenium.Keys.Enter);


        }

        public void DeleteAppliciants(string name)
        {
            try
            {
                By selectApplicants = By.XPath("//span[text()='Applicants']/../../following-sibling::div//div[@class='a-card a-card-message']/b[text()='" + name + "']");
                driver.ClickElementWithJavascript(selectApplicants, "Select Appliciants " + name);
                driver.WaitElementExistsAndVisible(btnDeleteApplicants);
                driver.ClickElement(btnDeleteApplicants, "Delete");
                driver.WaitElementExistsAndVisible(lblDeleteAppliciant);
                driver.WaitElementPresent(btnYes);
                driver.ClickElement(btnYes, "Yes");
                Thread.Sleep(3000);
                WaitTillElementDisappeared(loadingSpin);
                HandleSuccessPopup();
                if (!driver.IsWebElementDisplayed(selectApplicants))
                {
                    this.TESTREPORT.LogSuccess("Delete Applicant", "User deleted under Applicants " + name);
                }
                else
                {
                    this.TESTREPORT.LogFailure("Delete Applicant", "User not deleted under Applicants " + name, this.SCREENSHOTFILE);
                }
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void UpateAndRemoveWithdraw()
        {
            driver.ClickElement(btnMore, "More");
            driver.ClickElement(btnWithdraw, "Withdraw");
            driver.WaitElementExistsAndVisible(lblWithdrawApplication);
            driver.WaitElementExistsAndVisible(dropdownWithdrawnReason);
            driver.SelectDropdownItemByText(dropdownWithdrawnReason, "Applicant Changed Mind", "Withdrawn Reason");
            int count = driver.FindElements(sendAdverseActionTo).Count;
            int checkedCount = driver.FindElements(checkboxSendAdverseActionToCheckState).Count;
            if (count == checkedCount)
            {
                this.TESTREPORT.LogSuccess("Send Adverse Action To", "All are selecetd by default ");
            }
            else
            {
                this.TESTREPORT.LogFailure("Send Adverse Action To", "All are not selected by default ", this.SCREENSHOTFILE);
            }
            driver.ClickElement(btnWithdrawApplication, "Withdraw");
            HandleSuccessPopup();
        }

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

        private int GetReviewIndicatorNamePosition(String model)
        {
            try
            {
                int position = 0;
                var rows = driver.FindElements(tableScores).Count;
                for (int i = 1; i <= rows; i++)
                {
                    By nameColumn = By.XPath("//span[text()='Scores']/../following-sibling::div//tbody//tr[" + i + "]/td[1]");
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