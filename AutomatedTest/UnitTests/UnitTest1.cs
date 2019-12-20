using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StandardUtilities;
using AutomatedTest.FunctionalTests;
using Engine.Setup;
using TMTFactory;
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
        [TestProperty("title", "Validate Billing Has Cashier Signature")]
        public void TestTestRail1()
        {
            const String testRailUrl = "https://cignitipoc.testrail.io/";
            const String userName = "debasish.pradhan@cigniti.com";
            const String password = "Temp1234";

            int runId = 3;
            this.TESTREPORT.InitTestCase(TestContext.TestName, TestContext.Properties["title"] as String);
            TestRailClient testRail = null;
            testRail = new TestRailClient(testRailUrl, userName, password);
            ResultStatus testCaseResult = ResultStatus.Failed;
            this.TESTREPORT.UpdateTestCaseStatus(testRail, EngineSetup.TESTRAILRUNID, TestContext.Properties["title"] as String, testCaseResult);
        }

        [TestMethod]
        [TestProperty("title", "Inventory Should Should Generate Alert When Item Is Under Threshold")]
        public void TestTestRail2()
        {
            const String testRailUrl = "http://atllmstestrail.akcelerant.com/";
            const String userName = "kote@cigniti.com";
            const String password = "Password1";
            const int runId = 12;
            this.TESTREPORT.InitTestCase(TestContext.TestName, TestContext.Properties["title"] as String);

            TestRailClient testRail = null;
            testRail = new TestRailClient(testRailUrl, userName, password);
            ResultStatus testCaseResult = ResultStatus.Failed;
            this.TESTREPORT.UpdateTestCaseStatus(testRail, runId, TestContext.Properties["title"] as String, testCaseResult);
        }

        [TestMethod]
        public void TestSendMail()
        {
            StandardUtilities.EmailSender.SendEmail("smtp.gmail.com", 587, "debasish.gallop@gmail.com", "debasish.pradhan@cigniti.com", null, "Test", "Hi Welcome...", null, true);
        }
        
    }
}
