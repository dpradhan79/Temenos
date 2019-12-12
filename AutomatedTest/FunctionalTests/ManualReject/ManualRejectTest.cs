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
    public class ManualRejectTest : TestBaseTemplate
    {    
        /// <summary>
        /// Test case for Manual Reject.
        /// </summary>
        [TestMethod]
        //[TestCategory("DecisionTests")]
        public void ManualReject()
        {       
            #region ManualReject
            string approveApplicationNumber = null;
            this.TESTREPORT.InitTestCase("ManualReject", "ManualReject");
            LoginPage.SignIn(EngineSetup.UserName, EngineSetup.Password);
            //HomePage.CreateNewApplication("Approve",this.validationTestData);
            HomePage.CreateNewApplication("Loan", "Auto: Automation Auto Approve", "Branch - In Person", "100120");
            approveApplicationNumber = HomePage.GetApplicationNumber();
            HomePage.SwitchToTabAppFrame();          
            HomePage.NavigateToScreen(Constants.LoanTermsAutomation);
            HomePage.SwitchToCentralFrame();
            HomePage.VerifyScreenHeading(Constants.LoanTermsAutomation);
            LoanTermsAutomation.EnterFieldValuesInLoanTermPanel("Payment", "Monthly", "25000", "36");
            HomePage.SwitchToParentFrame();
            HomePage.VerifyDecisioningApplication("LO Rejected",true);
            HomePage.SwitchToDefaultContent();
            HomePage.CloseAndVerifyApplication(approveApplicationNumber);
            HomePage.ClickLogOff();           
            TESTREPORT.LogInfo("Test Execution Completed");
            this.TESTREPORT.UpdateTestCaseStatus();
            #endregion
        }
    }
}