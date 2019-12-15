using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestRail;
using TestRail.Types;
using StandardUtilities;
namespace AutomatedTest.UnitTests
{
    [TestClass]
    public class UnitTest1
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

        //[TestMethod]
        public void TestTestRail()
        {
            const String testRailUrl = "https://cignitipoc.testrail.io/";
            const String userName = "debasish.pradhan@cigniti.com";
            const String password = "Temp1234";
            const int runId = 3;
            TestRailClient testRail = null;
            testRail = new TestRailClient(testRailUrl, userName, password);
            String [] testCaseTitle = {"Validate Billing Has Cashier Signature", "Inventory Should Should Generate Alert When Item Is Under Threshold"};
            
            TestRailAPIWrapper.UpdateTestCaseStatus(testRail, runId, testCaseTitle[1], ResultStatus.Passed);
        }
    }
}
