using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CignitiReporter;
using StandardUtilities;
using TMTFactory;
namespace TestReporter
{
    /// <summary>
    /// @Author - Debasish Pradhan
    /// </summary>
    /// <seealso cref="TestReporter.IReporter" />
    public class GallopReporter : IReporter
    {
        private CReporter cReporter = null;
        /// <summary>
        /// Initializes a new instance of the <see cref="GallopReporter"/> class.
        /// </summary>
        public GallopReporter()
        {
            this.cReporter = CReporter.getCReporter("", "", "", false);
            this.cReporter.calculateSuiteStartTime();
        }
        /// <summary>
        /// Initializes the test case.
        /// </summary>
        /// <param name="testcaseName">Name of the testcase.</param>
        /// <param name="testCaseDescription">The test case description.</param>
        public void InitTestCase(string testcaseName, string testCaseDescription = "")
        {
            this.cReporter.initTestCase("", testcaseName, testCaseDescription, false);
        }

        /// <summary>
        /// Logs the success.
        /// </summary>
        /// <param name="stepName">Name of the step.</param>
        /// <param name="stepDescription">The step description.</param>
        /// <param name="screenShotPath">The screen shot path.</param>
        public void LogSuccess(string stepName, string stepDescription, string screenShotPath = null)
        {
            this.cReporter.SuccessReport(stepName, stepDescription);
        }

        /// <summary>
        /// Logs the failure.
        /// </summary>
        /// <param name="stepName">Name of the step.</param>
        /// <param name="stepDescription">The step description.</param>
        /// <param name="screenShotPath">The screen shot path.</param>
        public void LogFailure(string stepName, string stepDescription, string screenShotPath = null)
        {
            this.cReporter.failureReport(stepName, stepDescription, null);
        }

        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="screenShotPath">The screen shot path.</param>
        public void LogInfo(string message, string screenShotPath = null)
        {
            this.cReporter.SuccessReport("Info", message);
        }

        /// <summary>
        /// Logs the warning.
        /// </summary>
        /// <param name="stepName">Name of the step.</param>
        /// <param name="stepDescription">The step description.</param>
        /// <param name="screenShotPath">The screen shot path.</param>
        public void LogWarning(string stepName, string stepDescription, string screenShotPath = null)
        {
            this.cReporter.failureReport(stepName, stepDescription, null);
        }

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="screenShotPath">The screen shot path.</param>
        public void LogException(Exception ex, string screenShotPath = null)
        {
            this.cReporter.failureReport("Exception", ex.Message, null);
        }

        /// <summary>
        /// Logs the fatal.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="screenShotPath">The screen shot path.</param>
        public void LogFatal(Exception ex, string screenShotPath = null)
        {
            this.cReporter.failureReport("Exception", ex.Message, null);
        }

        /// <summary>
        /// Updates the test case status.
        /// </summary>
        public void UpdateTestCaseStatus()
        {
            this.cReporter.calculateTestCaseExecutionTime();
            this.cReporter.closeDetailedReport();
            this.cReporter.updateTestCaseStatus();
        }

        /// <summary>
        /// Updates the test case status in HTML/TestRail In Test Management Tool
        /// </summary>
        public void UpdateTestCaseStatus(TestRailClient trClient, int runId, String testCaseTitle, ResultStatus resultStatus)
        {
            this.UpdateTestCaseStatus();
            trClient.UpdateTestCaseStatus(runId, testCaseTitle, resultStatus);
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            this.cReporter.calculateSuiteExecutionTime();
            this.cReporter.createHtmlSummaryReport(StandardUtilities.FileUtilities.readPropertyFile("TestConfiguration.properties", "executablePath"), false);
            this.cReporter.closeSummaryReport();
            
           
        }

        /// <summary>
        /// Manipulates the test report.
        /// </summary>
        /// <param name="objTestReportManipulator">The object test report manipulator.</param>
        public void ManipulateTestReport(ITestReportManipulator objTestReportManipulator)
        {
            
        }
    }
}
