using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}
