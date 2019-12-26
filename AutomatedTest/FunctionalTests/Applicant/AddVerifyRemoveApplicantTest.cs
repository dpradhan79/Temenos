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
    public class AddVerifyRemoveApplicantTest : TestBaseTemplate
    {
         /// <summary>
        /// Test case for Add And Verify Appliciant
        /// </summary>
        [TestMethod, TestCategory("Application"), TestProperty("title", "Add And Verify Applicant")]       
       
        public void TestAddVerifyRemoveApplicant()
        {          
            #region Add Applicant To App
            try
            {
                string approveApplicationNumber = null;                              
                HomePage.CreateNewApplication(Constants.Approve, this.validationTestData);
                approveApplicationNumber = HomePage.GetApplicationNumber();
                HomePage.SwitchAndVerifyHomePageFullyDisplayed();
                TemenosBasePage.NavigateToScreen(Constants.Applicants);
                Applicant.AddApplicants(this.validationTestData,1);
                HomePage.SwitchToParentFrame();
                TemenosBasePage.NavigateToScreen(Constants.CreditReporting);
                Applicant.AddUpdateVerifyCreditReportingSystemScreen(this.validationTestData);
                HomePage.SwitchToParentFrame();
                TemenosBasePage.NavigateToScreen(Constants.Applicants);
                Applicant.AddApplicants(this.validationTestData,2);
                HomePage.SwitchToParentFrame();
                TemenosBasePage.NavigateToScreen(Constants.Applicants);
                Applicant.VerifyAppliciantsData(this.validationTestData, 2);
                HomePage.SwitchToParentFrame();
                TemenosBasePage.NavigateToScreen(Constants.Applicants);
                Applicant.VerifyGridPanelAndFieldsOnPopupScreen(this.validationTestData, 1);
                HomePage.SwitchToParentFrame();
                TemenosBasePage.NavigateToScreen(Constants.Applicants);
                Applicant.VerifyGridPanelAndFieldsOnPopupScreen(this.validationTestData, 2);
                HomePage.SwitchToParentFrame();
                TemenosBasePage.NavigateToScreen(Constants.ApplicationPanelsAutomation);
                Applicant.VerifyTextInAutomationPanel();
                Applicant.EditAndVeifyApplicationPanelAdditionalApplicantDeclarations(this.validationTestData, 1);
                Applicant.EditAndVeifyApplicationPanelAdditionalApplicantDeclarations(this.validationTestData, 2);
                Applicant.EditAndVeifyApplicationPanelPrimaryApplicantDeclarations(this.validationTestData, 1);
                Applicant.EditAndVeifyApplicationPanelPrimaryApplicantDeclarations(this.validationTestData, 2);
                Applicant.SaveApplicationPanelAutomation();
                HomePage.SwitchToParentFrame();
                TemenosBasePage.NavigateToScreen(Constants.Liabilities);
                Applicant.AddLiabilty(this.validationTestData);
                HomePage.SwitchToDefaultContent();
                HomePage.SwitchToTabFrame();
                TemenosBasePage.NavigateToScreen(Constants.LoanTermsAutomation);
                LoanTermsAutomation.EnterFieldValuesInLoanTermPanel(this.validationTestData);
                HomePage.SwitchToParentFrame();
                TemenosBasePage.NavigateToScreen(Constants.Applicants);
                Applicant.DeleteAppliciants(this.validationTestData["ApplicantNames1"]);
                HomePage.SwitchToParentFrame();
                Applicant.UpateAndRemoveWithdraw();
                HomePage.SwitchToDefaultContent();
                HomePage.CloseAndVerifyApplication(approveApplicationNumber);
                HomePage.ClickLogOff();
                TemenosBasePage.CheckFailures();
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