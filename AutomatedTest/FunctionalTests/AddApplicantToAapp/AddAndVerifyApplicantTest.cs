//-----------------------------------------------------------------------
// <copyright file="AutoDecisionProcess.cs" company="Temenos">
// Copyright (c) Temenos All rights reserved.
// </copyright>
// <author>Cigniti</author>
//-----------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace AutomatedTest.FunctionalTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Threading;
    using AUT.Selenium.ApplicationSpecific.Pages;
    using Engine.Setup;
    using Engine.Factories;
    [TestClass]
    /// <summary>
    /// Temenos Application Tests
    /// </summary>
    /// <seealso cref="AutomatedTest.FunctionalTests.TestBaseTemplate" />
    public class AddAndVerifyApplicantTest : TestBaseTemplate
    {
         /// <summary>
        /// Test case for Add And Verify Appliciant
        /// </summary>
        [TestMethod]
        [TestCategory("Application")]
        [TestProperty("title", "Add And Verify Applicant")]
        public void AddAndVerifyApplicant()
        {          
            #region Add Applicant To App
            try
            {
                string approveApplicationNumber = null;
                //this.TESTREPORT.InitTestCase(TestContext.Properties["name"] as String, TestContext.Properties["name"] as String);
                //this.LoadBusinessTestData();
                //LoginPage.SignIn(EngineSetup.UserName, EngineSetup.Password);                
                HomePage.CreateNewApplication("Approve", this.validationTestData);
                approveApplicationNumber = HomePage.GetApplicationNumber();
                HomePage.SwitchAndVerifyHomePageFullyDisplayed();
                HomePage.NavigateToScreen(Constants.Applicants);
                HomePage.SwitchToCentralFrame();
                HomePage.VerifyScreenHeading(Constants.Applicants);
                Application.AddApplicants("Joint", "100114", "MARTINA BEACOMMON");
                HomePage.SwitchToParentFrame();
                HomePage.NavigateToScreen(Constants.CreditReporting);
                HomePage.SwitchToCentralFrame();
                HomePage.VerifyScreenHeading(Constants.CreditReporting);
                Application.AddUpdateVerifyCreditReportingSystemScreen("PRIMARY - MICHAEL CHACOMMON", "Experian");
                HomePage.SwitchToParentFrame();
                HomePage.NavigateToScreen(Constants.Applicants);
                HomePage.SwitchToCentralFrame();
                HomePage.VerifyScreenHeading(Constants.Applicants);
                Application.AddApplicants("Joint", "100120", "KENNETH ASACOMMON");
                HomePage.SwitchToParentFrame();
                HomePage.NavigateToScreen(Constants.Applicants);
                HomePage.SwitchToCentralFrame();
                HomePage.VerifyScreenHeading(Constants.Applicants);
                Application.VerifyAppliciantsData("KENNETH ASACOMMON", "100120");
                HomePage.SwitchToParentFrame();
                HomePage.NavigateToScreen(Constants.Applicants);
                HomePage.SwitchToCentralFrame();
                HomePage.VerifyScreenHeading(Constants.Applicants);
                Application.VerifyGridPanelAndFieldsOnPopupScreen("MICHAEL CHACOMMON", "Primary", "100119", "ATM Usage Restricted",
                    "317-555-1234", "317-555-5684", "mchacommon@email.com", "33.0", "B Paper - Med/Low Risk");
                HomePage.SwitchToParentFrame();
                HomePage.NavigateToScreen(Constants.Applicants);
                HomePage.SwitchToCentralFrame();
                HomePage.VerifyScreenHeading(Constants.Applicants);
                Application.VerifyGridPanelAndFieldsOnPopupScreen("MARTINA BEACOMMON", "Joint", "100114", "Warning Message 100114",
                    "317-555-5678", "317-555-5679", "mbeacommon@email.com", "65.4", "E Paper - High Risk sk");
                HomePage.SwitchToParentFrame();
                HomePage.NavigateToScreen(Constants.ApplicationPanelsAutomation);
                HomePage.SwitchToCentralFrame();
                HomePage.VerifyScreenHeading(Constants.ApplicationPanelsAutomation);
                Application.VerifyTextInAutomationPanel();
                //Application.ApplicationPanelScores("National Risk", "304", "572", "37", "37", "572");
                Application.EditAndVeifyApplicationPanelAdditionalApplicantDeclarations("Do you pay alimony?", "Yes");
                Application.EditAndVeifyApplicationPanelAdditionalApplicantDeclarations("Do you pay child support?", "Yes");
                Application.EditAndVeifyApplicationPanelPrimaryApplicantDeclarations("Do you expect your income to decline in the next 2 years?", "No");
                Application.EditAndVeifyApplicationPanelPrimaryApplicantDeclarations("Are you a US citizen?", "Yes");
                Application.SaveApplicationPanelAutomation();
                HomePage.SwitchToParentFrame();
                HomePage.NavigateToScreen(Constants.Liabilities);
                HomePage.SwitchToCentralFrame();
                HomePage.VerifyScreenHeading(Constants.Liabilities);
                Application.AddLiabilty("100", "Monthly", "15000.00", "10000.00", "550.00", "100100-2", "Medical", "Collection", "MICHAEL CHACOMMON", "100");
                //HomePage.SwitchToParentFrame();
                HomePage.SwitchToDefaultContent();
                HomePage.SwitchToTabFrame();
                HomePage.NavigateToScreen(Constants.LoanTermsAutomation);
                HomePage.SwitchToCentralFrame();
                HomePage.VerifyScreenHeading(Constants.LoanTermsAutomation);
                LoanTermsAutomation.EnterFieldValuesInLoanTermPanel(this.validationTestData);
                HomePage.SwitchToParentFrame();
                HomePage.NavigateToScreen(Constants.Applicants);
                HomePage.SwitchToCentralFrame();
                HomePage.VerifyScreenHeading(Constants.Applicants);
                Application.DeleteAppliciants("MARTINA BEACOMMON");
                HomePage.SwitchToParentFrame();
                Application.UpateAndRemoveWithdraw();
                HomePage.SwitchToDefaultContent();
                HomePage.CloseAndVerifyApplication(approveApplicationNumber);
                HomePage.ClickLogOff();              
            }
            catch (Exception ex)
            {
                this.testException = ex;
                Assert.Fail(ex.Message);

            }            
            #endregion
        }
    }
}