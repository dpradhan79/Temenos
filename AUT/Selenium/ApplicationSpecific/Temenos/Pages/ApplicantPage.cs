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
    public class ApplicantPage : AbstractTemplatePage
    {
        //#region Page Object For BasePage
        //TemenosBasePage temenosBasePage = new TemenosBasePage();
        //#endregion
        
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
        private By lblAutoLabels = By.XPath("//span[text()='Application Panels - Automation']/../../following-sibling::div//h2");
        private By lblUsedAutos = By.XPath("//span[text()='Application Panels - Automation']/../../following-sibling::div//h3");
        private By lblAutoRefiance = By.XPath("(//span[text()='Application Panels - Automation']/../../following-sibling::div//h3)[2]");
        #endregion

        #region Public Methods

        /// <summary>
        /// Method of add application
        /// <param name="accountNumber"></param>
        /// <param name="applicantNames"></param>
        /// <param name="appliciantroles"></param>
        /// </summary>

        public void AddApplicants(Dictionary<string, string> validationTestData,int index)
        {
            try
            {
                driver.WaitElementExistsAndVisible(btnAdd);
                driver.ClickElement(btnAdd, "Add Appliciant");
                HomePage homePage = new HomePage();
                homePage.SwitchToParentFrame();
                driver.WaitElementTextEquals(dialogAddApplicant, "Add Applicant");
                driver.WaitElementPresent(txtAccountNumber);
                driver.SendKeysToElement(txtAccountNumber, validationTestData["ApplicantAccountNumber"+index], "Account Number");
                driver.SelectDropdownItemByText(dropdownApplicantRoles, validationTestData["Roles"+index], "Appliciant Roles");
                driver.ClickElement(btnOK, "OK");
                homePage.SwitchToCentralFrame();
                driver.WaitElementExistsAndVisible(btnSaveAndClose);
                driver.VerifyTextValue(txtFullName, validationTestData["ApplicantNames"+index]);
                driver.ClickElement(btnSaveAndClose, "Save And Close");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Method of add update verify credit reporting system screen
        /// <param name="PrimarySubject"></param>
        /// <param name="PrimarySubject"></param>       
        /// </summary>

        public void AddUpdateVerifyCreditReportingSystemScreen(Dictionary<string, string> validationTestData)
        {
            try
            {
                driver.WaitElementExistsAndVisible(btnGetCreditReport);
                driver.ClickElement(btnGetCreditReport, "Get crredit Report");
                driver.WaitElementExistsAndVisible(dialogGetCreditReport);
                driver.WaitElementTextEquals(dialogGetCreditReport, "Get Credit Report");
                driver.WaitElementPresent(dropdownPrimarySubject);
                driver.SelectDropdownItemByText(dropdownPrimarySubject, validationTestData["PrimarySubject"], "Primary Subject");
                driver.SelectDropdownItemByText(dropdownCreditBureau, validationTestData["CreditBureau"], "Credit Bureau");
                driver.ClickElement(btnSubmit, "Submit");
                WaitTillElementDisappeared(loadingRetrievingCreditReport);
                if (driver.IsWebElementDisplayed(lblCreditBureauResponse))
                {
                    driver.ClickElement(btnCreditBureauResponseNo, "Credit Bureau Response No");
                }

                By PrimarySubject = By.XPath("//form[@id='CreditReportingGrid']//tbody//tr[2]//td[text()='CHACOMMON, MICHAEL']");
                if (driver.IsWebElementDisplayed(PrimarySubject))
                {
                    this.TESTREPORT.LogSuccess("Credit Reporting", String.Format("Added primary subject <mark>{0}</mark> is displayed in grid", validationTestData["PrimarySubject"]));
                }
                else
                {
                    this.TESTREPORT.LogFailure("Credit Reporting", String.Format("Added primary subject <mark>{0}</mark> is not displayed in grid", validationTestData["PrimarySubject"]), this.SCREENSHOTFILE);
                    TemenosBasePage.dictError.Add("Credit Reporting", String.Format("Added primary subject {0} is not displayed in grid", validationTestData["PrimarySubject"]));
                }

                By PrimarySubjectSelected = By.XPath("//td[text()='CHACOMMON, MICHAEL']/parent::tr/parent::tbody//td[@aria-describedby='CreditReportList_InUse']/input[@type='checkbox' and @checked='checked']");
                if (driver.IsWebElementDisplayed(PrimarySubjectSelected))
                {
                    this.TESTREPORT.LogSuccess("Credit Reporting", String.Format("Added primary subject <mark>{0}</mark> is displayed in grid and use report selected by default", validationTestData["PrimarySubject"]));
                }
                else
                {
                    this.TESTREPORT.LogFailure("Credit Reporting", String.Format("Added primary subject <mark>{0}</mark> is displayed in grid and use report not selected by default", validationTestData["PrimarySubject"]),this.SCREENSHOTFILE);
                    TemenosBasePage.dictError.Add("Credit Reporting", String.Format("Added primary subject {0} is displayed in grid and use report not selected by default", validationTestData["PrimarySubject"]));
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            

        }

        /// <summary>
        /// Method of verify appliciants data
        /// <param name="ApplicantNames"></param>
        /// <param name="ApplicantAccountNumber"></param>       
        /// </summary>

        public void VerifyAppliciantsData(Dictionary<string, string> validationTestData,int index)
        {
            try
            {
            By selectAppliciants = By.XPath("//span[text()='Applicants']/../../following-sibling::div//div[@class='a-card a-card-message']/b[text()='" + validationTestData["ApplicantNames" + index] + "']");
            driver.ClickElementWithJavascript(selectAppliciants, "Select Appliciants " + validationTestData["ApplicantNames" + index]);
            driver.WaitElementExistsAndVisible(btnEdit);
            driver.ClickElement(btnEdit, "Edit");
            WaitTillElementDisappeared(loadingSpin);
            driver.WaitElementPresent(txtFullName);
            driver.VerifyTextValue(txtFullName, validationTestData["ApplicantNames" + index],"Applicant Name");
            driver.VerifyTextValue(txtIdentificationAccountNumber, validationTestData["ApplicantAccountNumber" + index],"Account Number");
            driver.ClickElement(btnSaveAndClose, "Save And Close");
            WaitTillElementDisappeared(loadingSpin);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        /// <summary>
        /// Method of verify grid panal and field on popup screen
        /// <param name="Name"></param>
        /// <param name="AccountNum"></param>       
        /// <param name="ApplicantType"></param>  
        /// <param name="CoreMessage"></param>  
        /// <param name="HomePhone"></param>  
        /// <param name="WorkPhone"></param>  
        /// <param name="Email"></param>  
        /// <param name="Age"></param>  
        /// <param name="RiskTier"></param> 
        /// </summary>

        public void VerifyGridPanelAndFieldsOnPopupScreen(Dictionary<string, string> validationTestData,int index)
        {
            try
            {
                By selectAppliciants = By.XPath("//span[text()='Applicants']/../../following-sibling::div//div[@class='a-card a-card-message']/b[text()='" + validationTestData["Name" + index] + "']");
                driver.ClickElementWithJavascript(selectAppliciants, "Select Appliciants " + validationTestData["Name"+index]);
                driver.WaitElementExistsAndVisible(btnEdit);
                driver.ClickElement(btnEdit, "Edit");
                WaitTillElementDisappeared(loadingSpin);
                driver.WaitElementPresent(txtFullName);
                driver.VerifyTextValue(txtFullName, validationTestData["Name"+index],"Full Name");
                driver.VerifyTextValue(txtIdentificationAccountNumber, validationTestData["AccountNum"+index],"Account Nuber");
                string type = driver.GetSelectedOption(dropdownApplicantType, 10);
                bool applicationTypeStatus = driver.VerifyTextValue(type, validationTestData["ApplicantType"+index],"Applicant Type");
                if (!applicationTypeStatus)
                {
                    TemenosBasePage.dictError.Add("Applicant Details - ApplicantType", String.Format("Actual Value does not matched with expected value: {0}", validationTestData["ApplicantType" + index]));
                }
                bool CoreMessageStatus = driver.VerifyTextValue(lblCoreMessage, validationTestData["CoreMessage"+index],"Core Message");
                if (!CoreMessageStatus)
                {
                    TemenosBasePage.dictError.Add("Applicant Details - CoreMessage", String.Format("Actual Value does not matched with expected value: {0}", validationTestData["CoreMessage" + index]));
                }
                bool HomePhoneStatus =driver.VerifyTextValue(lblHomePhone, validationTestData["HomePhone"+index],"Home Phone");
                if (!HomePhoneStatus)
                {
                    TemenosBasePage.dictError.Add("Applicant Details - HomePhone", String.Format("Actual Value does not matched with expected value: {0}", validationTestData["HomePhone" + index]));
                }
               bool WorkPhoneStatus = driver.VerifyTextValue(lblWorkPhone, validationTestData["WorkPhone"+index],"Work Phone");
               if (!WorkPhoneStatus)
                {
                    TemenosBasePage.dictError.Add("Applicant Details - WorkPhone", String.Format("Actual Value does not matched with expected value: {0}", validationTestData["WorkPhone" + index]));
                }
                bool EmailStatus = driver.VerifyTextValue(lblEmail, validationTestData["Email"+index],"Email");
                if (!EmailStatus)
                {
                    TemenosBasePage.dictError.Add("Applicant Details - Email", String.Format("Actual Value does not matched with expected value: {0}", validationTestData["Email" + index]));
                }
                bool AgeStatus = driver.VerifyTextValue(lblAge, validationTestData["Age"+index],"Age");
                if (!AgeStatus)
                {
                    TemenosBasePage.dictError.Add("Applicant Details - Age - "+validationTestData["Age"+index], String.Format("Actual Value does not matched with expected value: {0}", validationTestData["Age" + index]));
                }
                string risk = driver.GetSelectedOption(dropdownRiskTier, 10);
               bool RiskTierStatus = driver.VerifyTextValue(risk, validationTestData["RiskTier"+index],"RiskTier");
               if (!RiskTierStatus)
                {
                    TemenosBasePage.dictError.Add("Applicant Details - RiskTier - " + validationTestData["RiskTier" + index], String.Format("Actual Value does not matched with expected value: {0}", validationTestData["RiskTier" + index]));
                }
                driver.ClickElement(btnSaveAndClose, "Save And Close");
                WaitTillElementDisappeared(loadingSpin);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        /// <summary>
        /// Method of verify text in automation panel         
        /// </summary>

        public void VerifyTextInAutomationPanel()
        {
            try
            {                
                driver.VerifyTextValue(lblPanelText, "Whether you're buying new or used or refinancing from another lender");
                string textUsedAutos = driver.GetElementText(lblUsedAutos);
                driver.VerifyTextValue(textUsedAutos, "Used Autos");
                string textRefinance = driver.GetElementText(lblAutoRefiance);
                driver.VerifyTextValue(textRefinance, "Auto Refinance");
                driver.VerifyTextValue(lblPanelText, "RATES");
                string textAutoLabels = driver.GetElementText(lblAutoLabels);
                driver.VerifyTextValue(textAutoLabels, "AUTO LOANS - NEW, USED &REFINANCE");        
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method of application panel scores     
        /// </summary>

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

        /// <summary>
        /// Edit and verify application panel additional applicant declaration
        /// <param name="AditionalDeclaration"></param>
        /// <param name="AditionalDeclarationValue"></param>       
        /// </summary>

        public void EditAndVeifyApplicationPanelAdditionalApplicantDeclarations(Dictionary<string, string> validationTestData, int index)
        {
            try
            {
                By descrip = By.XPath("//table[@id='AADeclarationsList']//td[text()='" + validationTestData["AditionalDeclaration" + index] + "']");
                By descripchk = By.XPath("//table[@id='AADeclarationsList']//td[text()='" + validationTestData["AditionalDeclaration" + index] + "']/../td/input[@type='checkbox']");
                driver.ClickElement(descrip, validationTestData["AditionalDeclaration" + index]);
                driver.ClickElement(descripchk, "Check Box " + validationTestData["AditionalDeclaration" + index]);

                bool selected = driver.FindElement(descripchk).Selected;
                if (selected)
                {
                    this.TESTREPORT.LogSuccess("Additional Appliciant Declarations", String.Format("User selected the appliciant declarations <mark>{0}<mark>", validationTestData["AditionalDeclaration" + index]));
                }
                else
                {
                    this.TESTREPORT.LogFailure("Additional Appliciant Declarations", String.Format("Unable to select appliciant declarations <mark>{0}<mark>", validationTestData["AditionalDeclaration" + index]), this.SCREENSHOTFILE);
                    TemenosBasePage.dictError.Add("Additional Appliciant Declarations", String.Format("Unable to select appliciant declarations <mark>{0}<mark>", validationTestData["AditionalDeclaration" + index]));
                }
                By input = By.XPath("//table[@id='AADeclarationsList']//td[text()='" + validationTestData["AditionalDeclaration" + index] + "']/..//input[@name='AnswerExplanation']");
                driver.SendKeysToElement(input, validationTestData["AditionalDeclarationValue" + index], validationTestData["AditionalDeclaration" + index]);
                driver.ClickElement(input, validationTestData["AditionalDeclaration" + index]);
                //Actions act = new Actions(driver);
                //act.KeyDown(OpenQA.Selenium.Keys.Enter).KeyUp(OpenQA.Selenium.Keys.Enter).Build().Perform();
                driver.FindElement(input).SendKeys(OpenQA.Selenium.Keys.Enter);

                By explanationValue = By.XPath("//table[@id='AADeclarationsList']//td[text()='" + validationTestData["AditionalDeclaration" + index] + "']/../td[@aria-describedby='AADeclarationsList_AnswerExplanation']");
                driver.VerifyTextValue(explanationValue, validationTestData["AditionalDeclarationValue" + index]);

            }
            catch (Exception)
            {

                throw;
            }

        }
        /// <summary>
        /// Edit and verify application panel primary applicant declarations
        /// <param name="PrimaryDeclaration"></param>
        /// <param name="PrimaryDeclarationValue"></param>       
        /// </summary>

        public void EditAndVeifyApplicationPanelPrimaryApplicantDeclarations(Dictionary<string, string> validationTestData, int index)
        {
            try
            {
                By descrip = By.XPath("//table[@id='PADeclarationsList']//td[text()='" + validationTestData["PrimaryDeclaration" + index] + "']");
                By descripchk = By.XPath("//table[@id='PADeclarationsList']//td[text()='" + validationTestData["PrimaryDeclaration" + index] + "']/../td/input[@type='checkbox']");
                driver.ClickElement(descrip, validationTestData["PrimaryDeclaration" + index]);
                driver.ClickElement(descripchk, "Check Box " + validationTestData["PrimaryDeclaration" + index]);

                bool selected = driver.FindElement(descripchk).Selected;
                if (selected)
                {
                    this.TESTREPORT.LogSuccess("Primary Appliciant Declarations", String.Format("User selected the appliciant declarations <mark>{0}</mark>",validationTestData["PrimaryDeclaration" + index]));
                }
                else
                {
                    this.TESTREPORT.LogFailure("Primary Appliciant Declarations", String.Format("User is not selected the appliciant declarations <mark>{0}</mark>",validationTestData["PrimaryDeclaration" + index]),this.SCREENSHOTFILE);
                    TemenosBasePage.dictError.Add("Review Indicators Name", String.Format("Name : {0} is not present under Name column in Review Indicators grid", validationTestData["ReviewIndicatorName" + index]));
                }
                By input = By.XPath("//table[@id='PADeclarationsList']//td[text()='" + validationTestData["PrimaryDeclaration" + index] + "']/..//input[@name='AnswerExplanation']");
                driver.SendKeysToElement(input, validationTestData["PrimaryDeclarationValue" + index], validationTestData["PrimaryDeclaration" + index]);
                //Actions act = new Actions(driver);
                //act.KeyDown(OpenQA.Selenium.Keys.Enter).KeyUp(OpenQA.Selenium.Keys.Enter).Build().Perform();
                driver.FindElement(input).SendKeys(OpenQA.Selenium.Keys.Enter);

                By explanationValue = By.XPath("//table[@id='PADeclarationsList']//td[text()='" + validationTestData["PrimaryDeclaration" + index] + "']/../td[@aria-describedby='PADeclarationsList_AnswerExplanation']");
                driver.VerifyTextValue(explanationValue, validationTestData["PrimaryDeclarationValue" + index]);
            }
            catch (Exception)
            {

                throw;
            }

        }
        /// <summary>
        /// Method of save application panel automation           
        /// </summary>

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

        /// <summary>
        ///Method of add liabilty
        /// <param name="Secured"></param>
        /// <param name="Expense"></param> 
        /// <param name="Refi"></param>  
        /// <param name="Balance"></param>  
        /// <param name="Limit"></param>  
        /// <param name="PaymentAmount"></param>  
        /// <param name="PaymentFrequency""></param>  
        /// <param name="AccountNumber"></param>  
        /// <param name="Category"></param>  
        /// <param name="Type"></param> 
        /// </summary>

        public void AddLiabilty(Dictionary<string, string> validationTestData)
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
                driver.SendKeysToElement(txtBalance, validationTestData["Balance"], "Balance");
                driver.SendKeysToElement(txtLimit, validationTestData["Limit"], "Limit");
                driver.SendKeysToElement(txtPayment, validationTestData["Payment"], "Payment Amount");
                driver.SelectByVisibleText(dropdownPaymentFrequency, validationTestData["Frequency"], "Payment Frequency");
                driver.SendKeysToElement(txtAccount, validationTestData["LiabilityAccountNumber"], "Account Number");
                driver.SelectDropdownItemByText(dropdownCategory, validationTestData["Category"], "Category");
                driver.SelectDropdownItemByText(dropdownType, validationTestData["Type"], "Type");
                AddPercentageToApplicant(validationTestData["Name"], validationTestData["Value"]);
                driver.ClickElement(btnLiabilitySave, "Save");
                driver.SwitchBackToMainWindow();
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        /// <summary>
        /// Method of add percentage to applicant      
        /// </summary>

        private void AddPercentageToApplicant(string name, string value)
        {
            try
            {
                By applicantName = By.XPath("//td[text()='" + name + "']");
                By mapTOLiability = By.XPath("//td[text()='" + name + "']/..//input[@type='checkbox']");

                driver.ClickElement(applicantName, name);
                driver.ClickElement(mapTOLiability, "Check Box " + name);

                bool selected = driver.FindElement(mapTOLiability).Selected;
                if (selected)
                {
                    this.TESTREPORT.LogSuccess("Add Liabilty", String.Format("Map to Liabilty <mark>{0}<mark>", name) + name);
                }
                else
                {
                    this.TESTREPORT.LogFailure("Add Liabilty", String.Format("Map to Liabilty checkbox not selecte <mark>{0}<mark>",name), this.SCREENSHOTFILE);
                    TemenosBasePage.dictError.Add("Add Liabilty", String.Format("Map to Liabilty checkbox not selecte {0}", name));
                }
                By input = By.XPath("//td[text()='" + name + "']/..//input[@name='PercentageResponsible']");
                driver.FindElement(input).Clear();
                driver.SendKeysToElement(input, value, name);
                //Actions act = new Actions(driver);
                //act.KeyDown(OpenQA.Selenium.Keys.Enter).KeyUp(OpenQA.Selenium.Keys.Enter).Build().Perform();
                driver.FindElement(input).SendKeys(OpenQA.Selenium.Keys.Enter);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        ///Method of delete appliciant           
        /// </summary>
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
                    this.TESTREPORT.LogSuccess("Delete Applicant", String.Format("User deleted under Applicants <mark>{0}<mark>",name));
                }
                else
                {
                    this.TESTREPORT.LogFailure("Delete Applicant", String.Format("User not deleted under Applicants <mark>{0}<mark>", name),this.SCREENSHOTFILE);
                    TemenosBasePage.dictError.Add("Delete Applicant", String.Format("User not deleted under Applicants {0}", name));
                }
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        ///Method of update anad remove withdraw       
        /// </summary>

        public void UpateAndRemoveWithdraw()
        {
            try
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
                    this.TESTREPORT.LogSuccess("Send Adverse Action To", String.Format("All are selecetd by default"));
                }
                else
                {
                    this.TESTREPORT.LogFailure("Send Adverse Action To", String.Format("All are not selected by default "), this.SCREENSHOTFILE);
                    TemenosBasePage.dictError.Add("Send Adverse Action To", String.Format("All are not selected by default "));
                }
                driver.ClickElement(btnWithdrawApplication, "Withdraw");
                HandleSuccessPopup();

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
                driver.WaitElementPresent(lblSuccess);
                driver.ClickElement(btnOk, "OK");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        /// <summary>
        /// Method of get review indicator name position             
        /// </summary>
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