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
    /// Temenos Decision Process Tests
    /// </summary>
    /// <seealso cref="AutomatedTest.FunctionalTests.TestBaseTemplate" />
    public class AutoDecisionProcessTests : TestBaseTemplate
    {
        /// <summary>
        /// Loads the business test data.
        /// </summary>
        public void LoadBusinessTestData()
        {
            this.validationTestData = this.loadValidationTestData(TestContext.TestName, "");
        }

        /// <summary>
        /// Test case Auto Decision Process.
        /// </summary>
        [TestMethod]
        //[TestCategory("DecisionTests")]
        public void AutoDecisionProcess()
        {

            //try
            //{
            //    TestRailAPI.AddStatusToTest(1L, 1L, 5, "Automation Result for Test from visualstudio passed testststtstttsts");

            //}
            //catch (Exception)
            //{
               
            //    throw;
            //}    
            
            #region Approve
            string approveApplicationNumber,rejectApplicationNumber, reviewApplicationNumber = null;
            this.LoadBusinessTestData();
            this.TESTREPORT.InitTestCase("AutoDecisionProcess", "AutoDecisionProcess");

            LoginPage.SignIn(EngineSetup.UserName, EngineSetup.Password);
            HomePage.CreateNewApplication("Approve",this.validationTestData);
           // HomePage.CreateNewApplication("Loan", "Auto: Automation Auto Approve", "Branch - In Person", "100120");
            approveApplicationNumber = HomePage.GetApplicationNumber();
            HomePage.SwitchToTabAppFrame();
            HomePage.NavigateToScreen(Constants.PrimaryApplicationAutomation);
            HomePage.SwitchToCentralFrame();
            HomePage.VerifyScreenHeading(Constants.PrimaryApplicationAutomation);
            PrimaryApplicantAutomationScreen.EnterFieldValuesInPrimaryAppliciantPanel("MemeberShipStartDate", "2/27/2014");
            PrimaryApplicantAutomationScreen.EnterFieldValuesInPrimaryAppliciantPanel("DoNotPullCredit");
            HomePage.SwitchToParentFrame();
            HomePage.NavigateToScreen(Constants.LoanTermsAutomation);
            HomePage.SwitchToCentralFrame();
            HomePage.VerifyScreenHeading(Constants.LoanTermsAutomation);
           // LoanTermsAutomation.EnterFieldValuesInLoanTermPanel("Payment", "Monthly", "25000", "36");
            LoanTermsAutomation.EnterFieldValuesInLoanTermPanel(this.validationTestData);
            HomePage.SwitchToParentFrame();
            HomePage.VerifyDecisioningApplication(Constants.LoRejected,true);
            HomePage.VerifyDecisioningApplication(Constants.AutoApproved, true);
            HomePage.NavigateToScreen(Constants.PrimaryApplicationAutomation);
            HomePage.SwitchToCentralFrame();
            HomePage.VerifyScreenHeading(Constants.PrimaryApplicationAutomation);
            PrimaryApplicantAutomationScreen.VerifyMemberShipStartDate("2/27/2014");
            HomePage.SwitchToParentFrame();
            HomePage.NavigateToScreen(Constants.DecisionProcessAutomation);
            HomePage.SwitchToCentralFrame();
            HomePage.VerifyScreenHeading(Constants.DecisionProcessAutomation);
            DecisionProcessAutomation.VerifyReviewIndicators("Bankruptcy", "No bankruptcy within the last 4 years.");
            HomePage.SwitchToDefaultContent();
            HomePage.CloseAndVerifyApplication(approveApplicationNumber);
            #endregion

            #region Reject
           // HomePage.CreateNewApplication("Loan", "Auto: Automation Auto Reject", "Branch - In Person", "100120");
            HomePage.CreateNewApplication("Reject", this.validationTestData);
            rejectApplicationNumber = HomePage.GetApplicationNumber();
            HomePage.SwitchToTabAppFrame();
            HomePage.NavigateToScreen(Constants.LoanTermsAutomation);
            HomePage.SwitchToCentralFrame();
            HomePage.VerifyScreenHeading(Constants.LoanTermsAutomation);
            //LoanTermsAutomation.EnterFieldValuesInLoanTermPanel("Payment", "Monthly", "25000", "36");
            LoanTermsAutomation.EnterFieldValuesInLoanTermPanel(this.validationTestData);
            HomePage.SwitchToParentFrame();
            HomePage.VerifyDecisioningApplication(Constants.AutoRejected,true);
            HomePage.NavigateToScreen(Constants.StipulationsAutomation);
            HomePage.SwitchToCentralFrame();
            HomePage.VerifyScreenHeading(Constants.StipulationsAutomation);
           // StipulationsAutomation.AddStipulations("2 years tax returns", "Approval", "Testing", "Akcelerant");
            StipulationsAutomation.AddStipulations(this.validationTestData);
            HomePage.SwitchToDefaultContent();
            HomePage.CloseAndVerifyApplication(rejectApplicationNumber);
            #endregion

            #region review
           // HomePage.CreateNewApplication("Loan", "Auto: Automation Pending Review", "Branch - In Person", "100120");
            HomePage.CreateNewApplication("Review", this.validationTestData);
            reviewApplicationNumber = HomePage.GetApplicationNumber();
            HomePage.SwitchToTabAppFrame();
            HomePage.NavigateToScreen(Constants.LoanTermsAutomation);
            HomePage.SwitchToCentralFrame();
            HomePage.VerifyScreenHeading(Constants.LoanTermsAutomation);
           // LoanTermsAutomation.EnterFieldValuesInLoanTermPanel("Payment", "Monthly", "25000", "36");
            LoanTermsAutomation.EnterFieldValuesInLoanTermPanel(this.validationTestData);
            HomePage.SwitchToParentFrame();
            HomePage.VerifyDecisioningOfApplication();
            HomePage.NavigateToScreen(Constants.DecisionProcessAutomation);
            HomePage.SwitchToCentralFrame();
            HomePage.VerifyScreenHeading(Constants.DecisionProcessAutomation);
            DecisionProcessAutomation.VerifyReviewIndicators("OutOfState", "Physical and current address are not in PA");
            HomePage.SwitchToParentFrame();
            HomePage.NavigateToScreen(Constants.PrimaryApplicationAutomation);
            HomePage.SwitchToCentralFrame();
            HomePage.VerifyScreenHeading(Constants.PrimaryApplicationAutomation);
            PrimaryApplicantAutomationScreen.SelectCurrentAddressState("Pennsylvania");
            HomePage.SwitchToParentFrame();
            HomePage.NavigateToScreen(Constants.DecisionProcessAutomation);
            HomePage.SwitchToCentralFrame();
            HomePage.VerifyScreenHeading(Constants.DecisionProcessAutomation);
            DecisionProcessAutomation.VerifyReviewIndicators("InState", "Physical and current address are in PA");
            HomePage.SwitchToParentFrame();
            HomePage.NavigateToScreen(Constants.PrimaryApplicationAutomation);
            HomePage.SwitchToCentralFrame();
            HomePage.VerifyScreenHeading(Constants.PrimaryApplicationAutomation);
            PrimaryApplicantAutomationScreen.SelectCurrentAddressState("Illinois");
            HomePage.SwitchToDefaultContent();
            HomePage.CloseAndVerifyApplication(reviewApplicationNumber);

            HomePage.ClickLogOff();
            #endregion
            TESTREPORT.LogInfo("Test Execution Completed");
            this.TESTREPORT.UpdateTestCaseStatus();
        }
    }
}