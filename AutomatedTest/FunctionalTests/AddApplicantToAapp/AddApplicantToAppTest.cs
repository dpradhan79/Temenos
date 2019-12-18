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
    /// Temenos Application Tests
    /// </summary>
    /// <seealso cref="AutomatedTest.FunctionalTests.TestBaseTemplate" />
    public class AddApplicantToAppTest : TestBaseTemplate
    {
        /// <summary>
        /// Loads the business test data.
        /// </summary>
        public void LoadBusinessTestData()
        {
            this.validationTestData = this.loadValidationTestData(TestContext.TestName);
        }

        /// <summary>
        /// Test case for Add Applicant to App.
        /// </summary>
        [TestMethod]
        [TestCategory("Application")]
        [TestProperty("name", "Add Applicant To App")]
        public void AddApplicantToApp()
        {
            ResultStatus testCaseResult = ResultStatus.Untested;
            #region Add Applicant To App
            try
            {
                string approveApplicationNumber = null;
                this.TESTREPORT.InitTestCase(TestContext.Properties["name"] as String, TestContext.Properties["name"] as String);
                this.LoadBusinessTestData();
                LoginPage.SignIn(EngineSetup.UserName, EngineSetup.Password);
                //HomePage.CreateNewApplication("Approve", this.validationTestData);
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

                HomePage.SwitchToDefaultContent();
                HomePage.CloseAndVerifyApplication(approveApplicationNumber);
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
            #endregion
        }
    }
}