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
        [TestCategory("ManualReject")]
        [TestCategory("DecisionProcess")]
        [TestProperty("title", "Manual Reject")]
        public void TestManualReject()        {
           
            #region ManualReject
            try
            {
                string approveApplicationNumber = null;
                HomePage.CreateNewApplication("Approve", this.validationTestData);
                approveApplicationNumber = HomePage.GetApplicationNumber();
                HomePage.SwitchAndVerifyHomePageFullyDisplayed();
                TemenosBasePage.NavigateToScreen(Constants.LoanTermsAutomation);
                LoanTermsAutomation.EnterFieldValuesInLoanTermPanel(this.validationTestData);
                HomePage.SwitchToParentFrame();
                HomePage.VerifyDecisioningApplication(Constants.LoRejected, this.validationTestData);
                HomePage.SwitchToDefaultContent();
                HomePage.CloseAndVerifyApplication(approveApplicationNumber);
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