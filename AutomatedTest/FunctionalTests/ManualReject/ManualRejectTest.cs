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
    public class ManualRejectTest : TestBaseTemplate
    {
        /// <summary>
        /// Loads the business test data.
        /// </summary>
        public void LoadBusinessTestData()
        {
            this.validationTestData = this.loadValidationTestData(TestContext.TestName);
        }
        /// <summary>
        /// Test case for Manual Reject.
        /// </summary>
        [TestMethod]
        [TestCategory("DecisionTests")]
        [TestProperty("name", "Manual Reject")]
        public void ManualReject()
        {
            ResultStatus testCaseResult = ResultStatus.Untested;
            #region ManualReject
            try
            {
                string approveApplicationNumber = null;
                this.TESTREPORT.InitTestCase(TestContext.Properties["name"] as String, TestContext.Properties["name"] as String);
                this.LoadBusinessTestData();
                LoginPage.SignIn(EngineSetup.UserName, EngineSetup.Password);
                HomePage.CreateNewApplication("Approve", this.validationTestData);
                approveApplicationNumber = HomePage.GetApplicationNumber();
                HomePage.SwitchToTabAppFrame();
                HomePage.NavigateToScreen(Constants.LoanTermsAutomation);
                HomePage.SwitchToCentralFrame();
                HomePage.VerifyScreenHeading(Constants.LoanTermsAutomation);
                LoanTermsAutomation.EnterFieldValuesInLoanTermPanel(this.validationTestData);
                HomePage.SwitchToParentFrame();
                HomePage.VerifyDecisioningApplication(Constants.LoRejected, true);
                HomePage.SwitchToDefaultContent();
                HomePage.CloseAndVerifyApplication(approveApplicationNumber);
                HomePage.ClickLogOff();
                testCaseResult = ResultStatus.Passed;
                TESTREPORT.LogInfo("Test Execution Completed");

            }
            catch (Exception ex)
            {
                TESTREPORT.LogFailure("Failed", "because of the exception " + ex);
                testCaseResult = ResultStatus.Failed;
                Assert.Fail(ex.Message);
            }
            finally
            {
                this.TESTREPORT.UpdateTestCaseStatus(testRail, EngineSetup.TESTRAILRUNID, TestContext.Properties["name"] as String, testCaseResult);
               
            }
            #endregion
        }
    }
}