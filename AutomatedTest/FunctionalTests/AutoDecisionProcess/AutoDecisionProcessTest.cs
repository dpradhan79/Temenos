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
        /// Test case Auto Decision Process.
        /// </summary>       
        [TestMethod, TestCategory("DecisionProcess"), TestCategory("AutoDecisionProcess"), TestProperty("title", "Auto Decision Process")]
        public void TestAutoDecisionProcess()
        {            
             #region Approve            
            try
            {
                string approveApplicationNumber, rejectApplicationNumber, reviewApplicationNumber = null;                
                HomePage.CreateNewApplication(Constants.Approve, this.validationTestData);
                approveApplicationNumber = HomePage.GetApplicationNumber();
                HomePage.SwitchAndVerifyHomePageFullyDisplayed();
                TemenosBasePage.NavigateToScreen(Constants.PrimaryApplicationAutomation);
                PrimaryApplicantAutomationScreen.EnterFieldValuesInPrimaryAppliciantPanel(Constants.MemeberShipStartDate, this.validationTestData["MemeberShipStartDate"]);
                PrimaryApplicantAutomationScreen.EnterFieldValuesInPrimaryAppliciantPanel(Constants.DoNotPullCredit);
                HomePage.SwitchToParentFrame();
                TemenosBasePage.NavigateToScreen(Constants.LoanTermsAutomation);
                LoanTermsAutomation.EnterFieldValuesInLoanTermPanel(this.validationTestData);
                HomePage.SwitchToParentFrame();
                HomePage.VerifyDecisioningApplication(Constants.LoRejected, this.validationTestData);
                HomePage.VerifyDecisioningApplication(Constants.AutoApproved, this.validationTestData);
                TemenosBasePage.NavigateToScreen(Constants.PrimaryApplicationAutomation);
                PrimaryApplicantAutomationScreen.VerifyMemberShipStartDate(this.validationTestData["MemeberShipStartDate"]);
                HomePage.SwitchToParentFrame();
                TemenosBasePage.NavigateToScreen(Constants.DecisionProcessAutomation);                
                DecisionProcessAutomation.VerifyReviewIndicators(this.validationTestData);
                HomePage.SwitchToDefaultContent();
                HomePage.CloseAndVerifyApplication(approveApplicationNumber);
                #endregion

                #region Reject
                HomePage.CreateNewApplication(Constants.Reject, this.validationTestData);
                rejectApplicationNumber = HomePage.GetApplicationNumber();
                HomePage.SwitchAndVerifyHomePageFullyDisplayed();
                TemenosBasePage.NavigateToScreen(Constants.LoanTermsAutomation);                
                LoanTermsAutomation.EnterFieldValuesInLoanTermPanel(this.validationTestData);
                HomePage.SwitchToParentFrame();
                HomePage.VerifyDecisioningApplication(Constants.AutoRejected, this.validationTestData);
                TemenosBasePage.NavigateToScreen(Constants.StipulationsAutomation);               
                StipulationsAutomation.AddStipulations(this.validationTestData);
                HomePage.SwitchToDefaultContent();
                HomePage.CloseAndVerifyApplication(rejectApplicationNumber);
                #endregion

                #region review
                HomePage.CreateNewApplication(Constants.Review, this.validationTestData);
                reviewApplicationNumber = HomePage.GetApplicationNumber();
                HomePage.SwitchAndVerifyHomePageFullyDisplayed();
                TemenosBasePage.NavigateToScreen(Constants.LoanTermsAutomation);               
                LoanTermsAutomation.EnterFieldValuesInLoanTermPanel(this.validationTestData);
                HomePage.SwitchToParentFrame();
                HomePage.VerifyDecisioningOfApplication();
                TemenosBasePage.NavigateToScreen(Constants.DecisionProcessAutomation);              
                DecisionProcessAutomation.VerifyReviewIndicators(this.validationTestData,1);
                HomePage.SwitchToParentFrame();
                TemenosBasePage.NavigateToScreen(Constants.PrimaryApplicationAutomation);              
                PrimaryApplicantAutomationScreen.SelectCurrentAddressState(this.validationTestData["CurrentAddressState"]);
                HomePage.SwitchToParentFrame();
                TemenosBasePage.NavigateToScreen(Constants.DecisionProcessAutomation); 
                DecisionProcessAutomation.VerifyReviewIndicators(this.validationTestData, 2);
                HomePage.SwitchToParentFrame();
                TemenosBasePage.NavigateToScreen(Constants.PrimaryApplicationAutomation);
                PrimaryApplicantAutomationScreen.SelectCurrentAddressState(this.validationTestData["CurrentAddressState1"]);
                HomePage.SwitchToDefaultContent();
                HomePage.CloseAndVerifyApplication(reviewApplicationNumber);
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