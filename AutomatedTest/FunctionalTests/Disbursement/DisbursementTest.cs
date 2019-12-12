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
        //[TestCategory("DecisionTests")]
         public void Disbursement()
         {
             #region Disbursement
            string approveApplicationNumber = null;
            this.TESTREPORT.InitTestCase("Disbursement Test", "Disbursement Test");
            LoginPage.SignIn(EngineSetup.UserName, EngineSetup.Password);
            //HomePage.CreateNewApplication("Loan", "Auto: Automation Auto Approve", "Branch - In Person", "100120");
            approveApplicationNumber = HomePage.GetApplicationNumber();
            HomePage.SwitchToTabAppFrame();          
            HomePage.NavigateToScreen(Constants.LoanTermsAutomation);
            HomePage.SwitchToCentralFrame();
            HomePage.VerifyScreenHeading(Constants.LoanTermsAutomation);
            LoanTermsAutomation.EnterFieldValuesInLoanTermPanel("Payment", "Monthly", "25000", "36");
            HomePage.SwitchToParentFrame();
            HomePage.NavigateToScreen(Constants.PrimaryApplicationAutomation);
            HomePage.SwitchToCentralFrame();
            HomePage.VerifyScreenHeading(Constants.PrimaryApplicationAutomation);
            PrimaryApplicantAutomationScreen.EnterFieldValuesInPrimaryAppliciantPanel("DoNotPullCredit");
            HomePage.SwitchToParentFrame();
            HomePage.VerifyDecisioningApplication("Auto Approved",false);
            HomePage.NavigateToScreen(Constants.StipulationsAutomation);
            HomePage.SwitchToCentralFrame();
            HomePage.VerifyScreenHeading(Constants.StipulationsAutomation);
            StipulationsAutomation.AddStipulations("Pest Inspection", "Disbursement", "Testing", "Akcelerant",true);
            HomePage.SwitchToParentFrame();
            Disburse.PerformDisbursementOfLoanApplication();
            HomePage.NavigateToScreen(Constants.StipulationsAutomation);
            HomePage.SwitchToCentralFrame();
            Disburse.DiscardTheChanges();
            StipulationsAutomation.AddStipulations("2 years tax returns", "Approval", "Testing", "Akcelerant", false);
            HomePage.SwitchToDefaultContent();
            HomePage.CloseAndVerifyApplication(approveApplicationNumber);
            HomePage.ClickLogOff();            
            TESTREPORT.LogInfo("Test Execution Completed");
            this.TESTREPORT.UpdateTestCaseStatus();
            #endregion
         }

    }
}