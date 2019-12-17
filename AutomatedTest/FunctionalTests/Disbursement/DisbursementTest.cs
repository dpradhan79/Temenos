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
    using TestRail;

    [TestClass]
    /// <summary>
    /// Temenos Decision Process Tests
    /// </summary>
    /// <seealso cref="AutomatedTest.FunctionalTests.TestBaseTemplate" />
    public class DisbursementTest : TestBaseTemplate
    {
        /// <summary>
        /// Loads the business test data.
        /// </summary>
        public void LoadBusinessTestData()
        {
            this.validationTestData = this.loadValidationTestData(TestContext.TestName);
        }
        /// <summary>
        /// Test case for Disbursement.
        /// </summary>
        [TestMethod]
        [TestCategory("Disbursement")]
        [TestCategory("DecisionProcess")]
        [TestProperty("name", "Disbursement")]
        public void Disbursement()
        {
            ResultStatus testCaseResult = ResultStatus.Untested;
            #region Disbursement
            try
            {
                string approveApplicationNumber = null;
                this.LoadBusinessTestData();
                this.TESTREPORT.InitTestCase(TestContext.Properties["name"] as String, TestContext.Properties["name"] as String);
                LoginPage.SignIn(EngineSetup.UserName, EngineSetup.Password);
                HomePage.CreateNewApplication("Approve", this.validationTestData);
                approveApplicationNumber = HomePage.GetApplicationNumber();
                HomePage.SwitchAndVerifyHomePageFullyDisplayed();
                HomePage.NavigateToScreen(Constants.LoanTermsAutomation);
                HomePage.SwitchToCentralFrame();
                HomePage.VerifyScreenHeading(Constants.LoanTermsAutomation);
                LoanTermsAutomation.EnterFieldValuesInLoanTermPanel(this.validationTestData);
                HomePage.SwitchToParentFrame();
                HomePage.NavigateToScreen(Constants.PrimaryApplicationAutomation);
                HomePage.SwitchToCentralFrame();
                HomePage.VerifyScreenHeading(Constants.PrimaryApplicationAutomation);
                PrimaryApplicantAutomationScreen.EnterFieldValuesInPrimaryAppliciantPanel("DoNotPullCredit");
                HomePage.SwitchToParentFrame();
                HomePage.VerifyDecisioningApplication(Constants.AutoApproved, false);
                HomePage.NavigateToScreen(Constants.StipulationsAutomation);
                HomePage.SwitchToCentralFrame();
                HomePage.VerifyScreenHeading(Constants.StipulationsAutomation);
                StipulationsAutomation.AddStipulations(this.validationTestData, true);
                HomePage.SwitchToParentFrame();
                Disburse.PerformDisbursementOfLoanApplication();
                HomePage.NavigateToScreen(Constants.StipulationsAutomation);
                HomePage.SwitchToCentralFrame();
                Disburse.DiscardTheChanges();
                StipulationsAutomation.AddStipulations(this.validationTestData, false, 1);
                HomePage.SwitchToDefaultContent();
                HomePage.CloseAndVerifyApplication(approveApplicationNumber);
                HomePage.ClickLogOff();
                testCaseResult = ResultStatus.Passed;
                TESTREPORT.LogInfo("Test Execution Completed");

            }
            catch (Exception ex)
            {
                TESTREPORT.LogFailure("Failed", "Because of the exception " + ex);
                testCaseResult = ResultStatus.Failed;
                Assert.Fail(ex.Message);
            }
            finally
            {
                this.TESTREPORT.UpdateTestCaseStatus(testRail, EngineSetup.TESTRAILRUNID, TestContext.Properties["name"] as String, testCaseResult);

            }
        }
    }

      #endregion
}