using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestRail;
using TestRail.Types;
using StandardUtilities;
using AutomatedTest.FunctionalTests;
namespace AutomatedTest.UnitTests
{
    [TestClass]
    public class UnitTest1 : TestBaseTemplate
    {
       
        //[TestMethod]
        public void TestMethod1()
        {
            string aut = StandardUtilities.FileUtilities.readPropertyFile("TestConfiguration.properties", "executablePath");
        }

        //[TestMethod]
        public void TestDate()
        {
            string currentDate = DateTime.Today.ToString("MM/dd/yyyy");
        }

        [TestMethod]
        [TestProperty("name", "Validate Billing Has Cashier Signature")]
        public void TestTestRail1()
        {
            const String testRailUrl = "https://cignitipoc.testrail.io/";
            const String userName = "debasish.pradhan@cigniti.com";
            const String password = "Temp1234";
            const int runId = 3;
            this.TESTREPORT.InitTestCase(TestContext.Properties["name"] as String, TestContext.Properties["name"] as String);
            TestRailClient testRail = null;
            testRail = new TestRailClient(testRailUrl, userName, password);
            ResultStatus testCaseResult = ResultStatus.Passed;
            this.TESTREPORT.UpdateTestCaseStatus(testRail, runId, TestContext.Properties["name"] as String, testCaseResult);
        }

        [TestMethod]
        [TestProperty("name", "Inventory Should Should Generate Alert When Item Is Under Threshold")]
        public void TestTestRail2()
        {
            const String testRailUrl = "https://cignitipoc.testrail.io/";
            const String userName = "debasish.pradhan@cigniti.com";
            const String password = "Temp1234";
            const int runId = 3;
            this.TESTREPORT.InitTestCase(TestContext.Properties["name"] as String, TestContext.Properties["name"] as String);
            TestRailClient testRail = null;
            testRail = new TestRailClient(testRailUrl, userName, password);
            ResultStatus testCaseResult = ResultStatus.Passed;
            this.TESTREPORT.UpdateTestCaseStatus(testRail, runId, TestContext.Properties["name"] as String, testCaseResult);
        }
        
    }
}
