using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMTFactory;

namespace TestReporter
{
    /// <summary>
    /// @Author - Debasish Pradhan
    /// </summary>
    public interface IReporter
    {

        /// <summary>
        /// Initializes the test case.
        /// </summary>
        /// <param name="testcaseName">Name of the testcase.</param>
        /// <param name="testCaseDescription">The test case description.</param>
        void InitTestCase(string testcaseName, string testCaseDescription = "");
        /// <summary>
        /// Logs the success.
        /// </summary>
        /// <param name="stepName">Name of the step.</param>
        /// <param name="stepDescription">The step description.</param>
        /// <param name="screenShotPath">The screen shot path.</param>
        void LogSuccess(string stepName, string stepDescription, string screenShotPath = null);
        /// <summary>
        /// Logs the failure.
        /// </summary>
        /// <param name="stepName">Name of the step.</param>
        /// <param name="stepDescription">The step description.</param>
        /// <param name="screenShotPath">The screen shot path.</param>
        void LogFailure(string stepName, string stepDescription, string screenShotPath = null);
        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="screenShotPath">The screen shot path.</param>
        void LogInfo(string message, string screenShotPath = null);
        /// <summary>
        /// Logs the warning.
        /// </summary>
        /// <param name="stepName">Name of the step.</param>
        /// <param name="stepDescription">The step description.</param>
        /// <param name="screenShotPath">The screen shot path.</param>
        void LogWarning(string stepName, string stepDescription, string screenShotPath = null);
        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="screenShotPath">The screen shot path.</param>
        void LogException(Exception ex, string screenShotPath = null);
        /// <summary>
        /// Logs the fatal.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="screenShotPath">The screen shot path.</param>
        void LogFatal(Exception ex, string screenShotPath = null);
        /// <summary>
        /// Updates the test case status.
        /// </summary>
        void UpdateTestCaseStatus();

        /// <summary>
        /// Updates the test case status in HTML/TestRail In Test Management Tool
        /// </summary>
        void UpdateTestCaseStatus(TestRailClient trClient, int runId, String testCaseTitle, ResultStatus resultStatus);

        /// <summary>
        /// Closes this instance.
        /// </summary>
        /// 
        void Close();
        /// <summary>
        /// Manipulates the test report.
        /// </summary>
        /// <param name="objTestReportManipulator">The object test report manipulator.</param>
        void ManipulateTestReport(ITestReportManipulator objTestReportManipulator);
    }
}
