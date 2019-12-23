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
        //public void LoadBusinessTestData()
        //{
        //    this.validationTestData = this.loadValidationTestData(TestContext.TestName);
        //}

        /// <summary>
        /// Test case Auto Decision Process.
        /// </summary>
        [TestMethod]
        [TestCategory("AutoDecisionProcess")]
        [TestCategory("DecisionProcess")]
        [TestProperty("title", "Auto Decision Process")]
        public void TestAutoDecisionProcess()
        {
            #region Approve            
            try
            {
                string approveApplicationNumber, rejectApplicationNumber, reviewApplicationNumber = null;                
                HomePage.CreateNewApplication("Approve", this.validationTestData);
                approveApplicationNumber = HomePage.GetApplicationNumber();
                HomePage.SwitchAndVerifyHomePageFullyDisplayed();
                HomePage.NavigateToScreen(Constants.PrimaryApplicationAutomation);
                HomePage.SwitchToCentralFrame();
                HomePage.VerifyScreenHeading(Constants.PrimaryApplicationAutomation);
                PrimaryApplicantAutomationScreen.EnterFieldValuesInPrimaryAppliciantPanel(Constants.MemeberShipStartDate, this.validationTestData["MemeberShipStartDate"]);
                PrimaryApplicantAutomationScreen.EnterFieldValuesInPrimaryAppliciantPanel(Constants.DoNotPullCredit);
                HomePage.SwitchToParentFrame();
                HomePage.NavigateToScreen(Constants.LoanTermsAutomation);
                HomePage.SwitchToCentralFrame();
                HomePage.VerifyScreenHeading(Constants.LoanTermsAutomation);
                LoanTermsAutomation.EnterFieldValuesInLoanTermPanel(this.validationTestData);
                HomePage.SwitchToParentFrame();
                HomePage.VerifyDecisioningApplication(Constants.LoRejected, true);
                HomePage.VerifyDecisioningApplication(Constants.AutoApproved, true);
                HomePage.NavigateToScreen(Constants.PrimaryApplicationAutomation);
                HomePage.SwitchToCentralFrame();
                HomePage.VerifyScreenHeading(Constants.PrimaryApplicationAutomation);
                PrimaryApplicantAutomationScreen.VerifyMemberShipStartDate(this.validationTestData["MemeberShipStartDate"]);
                HomePage.SwitchToParentFrame();
                HomePage.NavigateToScreen(Constants.DecisionProcessAutomation);
                HomePage.SwitchToCentralFrame();
                HomePage.VerifyScreenHeading(Constants.DecisionProcessAutomation);
                DecisionProcessAutomation.VerifyReviewIndicators(this.validationTestData);
                HomePage.SwitchToDefaultContent();
                HomePage.CloseAndVerifyApplication(approveApplicationNumber);
            #endregion

                #region Reject
                HomePage.CreateNewApplication("Reject", this.validationTestData);
                rejectApplicationNumber = HomePage.GetApplicationNumber();
                HomePage.SwitchAndVerifyHomePageFullyDisplayed();
                HomePage.NavigateToScreen(Constants.LoanTermsAutomation);
                HomePage.SwitchToCentralFrame();
                HomePage.VerifyScreenHeading(Constants.LoanTermsAutomation);
                LoanTermsAutomation.EnterFieldValuesInLoanTermPanel(this.validationTestData);
                HomePage.SwitchToParentFrame();
                HomePage.VerifyDecisioningApplication(Constants.AutoRejected, true);
                HomePage.NavigateToScreen(Constants.StipulationsAutomation);
                HomePage.SwitchToCentralFrame();
                HomePage.VerifyScreenHeading(Constants.StipulationsAutomation);
                StipulationsAutomation.AddStipulations(this.validationTestData);
                HomePage.SwitchToDefaultContent();
                HomePage.CloseAndVerifyApplication(rejectApplicationNumber);
                #endregion

                #region review
                HomePage.CreateNewApplication("Review", this.validationTestData);
                reviewApplicationNumber = HomePage.GetApplicationNumber();
                HomePage.SwitchAndVerifyHomePageFullyDisplayed();
                HomePage.NavigateToScreen(Constants.LoanTermsAutomation);
                HomePage.SwitchToCentralFrame();
                HomePage.VerifyScreenHeading(Constants.LoanTermsAutomation);
                LoanTermsAutomation.EnterFieldValuesInLoanTermPanel(this.validationTestData);
                HomePage.SwitchToParentFrame();
                HomePage.VerifyDecisioningOfApplication();
                HomePage.NavigateToScreen(Constants.DecisionProcessAutomation);
                HomePage.SwitchToCentralFrame();
                HomePage.VerifyScreenHeading(Constants.DecisionProcessAutomation);
                DecisionProcessAutomation.VerifyReviewIndicators(this.validationTestData,1);
                HomePage.SwitchToParentFrame();
                HomePage.NavigateToScreen(Constants.PrimaryApplicationAutomation);
                HomePage.SwitchToCentralFrame();
                HomePage.VerifyScreenHeading(Constants.PrimaryApplicationAutomation);
                PrimaryApplicantAutomationScreen.SelectCurrentAddressState("Pennsylvania");
                HomePage.SwitchToParentFrame();
                HomePage.NavigateToScreen(Constants.DecisionProcessAutomation);
                HomePage.SwitchToCentralFrame();
                HomePage.VerifyScreenHeading(Constants.DecisionProcessAutomation);
                DecisionProcessAutomation.VerifyReviewIndicators(this.validationTestData, 2);
                HomePage.SwitchToParentFrame();
                HomePage.NavigateToScreen(Constants.PrimaryApplicationAutomation);
                HomePage.SwitchToCentralFrame();
                HomePage.VerifyScreenHeading(Constants.PrimaryApplicationAutomation);
                PrimaryApplicantAutomationScreen.SelectCurrentAddressState("Illinois");
                HomePage.SwitchToDefaultContent();
                HomePage.CloseAndVerifyApplication(reviewApplicationNumber);
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