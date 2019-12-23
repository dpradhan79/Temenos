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
    public class DisbursementTest : TestBaseTemplate
    {
         /// <summary>
        /// Test case for Disbursement.
        /// </summary>
        [TestMethod]
        [TestCategory("Disbursement")]
        [TestCategory("DecisionProcess")]
        [TestProperty("title", "Disbursement")]
        public void TestDisbursement()
        {           
            #region Disbursement
            try
            {
                string approveApplicationNumber = null;                
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

            }
            catch (Exception ex)
            {               
                this.testException = ex;
                Assert.Fail(ex.Message);
            }            
        }
    }

      #endregion
}