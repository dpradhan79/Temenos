using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RelevantCodes.ExtentReports;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using StandardUtilities;
using TestRail;

namespace TestReporter
{
    /// <summary>
    /// @Author - Debasish Pradhan
    /// </summary>
    /// <seealso cref="TestReporter.IReporter" />
    public class ExtentReporter : IReporter
    {
        private string filePath = null;
        private bool isGallopLogoRequired = false;
        private ExtentReports objExtentReport = null;
        private ExtentTest objExtentTest = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtentReporter"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="replaceExisting">if set to <c>true</c> [replace existing].</param>
        /// <param name="isGallopLogoRequired">if set to <c>true</c> [is gallop logo required].</param>
        public ExtentReporter(string filePath, bool replaceExisting, bool isGallopLogoRequired)
        {
            this.filePath = filePath;
            this.isGallopLogoRequired = isGallopLogoRequired;
            this.objExtentReport = new ExtentReports(this.filePath, replaceExisting);
            this.objExtentReport.LoadConfig("extentConfig.xml");
            FileInfo fileInfo = new FileInfo(this.filePath);
            try
            {
                if(!fileInfo.Directory.Exists)
                {
                    fileInfo.Directory.Create();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("Exception Encountered - {0}", ex.Message));
                Environment.Exit(0);
            }
            if(this.isGallopLogoRequired == true)
            {
                //copy gallop logo
                string sourceGallopLogo = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "cigniti_logo.png";
                string destGallopLogo = new FileInfo(this.filePath).Directory.ToString() + Path.DirectorySeparatorChar + "cigniti_logo.png";

                //copy client logo
                string sourceClientLogo = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "client_logo.png";
                string destClientLogo = new FileInfo(this.filePath).Directory.ToString() + Path.DirectorySeparatorChar + "client_logo.png";
                
                try
                {   
                    File.Copy(sourceGallopLogo, destGallopLogo, true);
                    File.Copy(sourceClientLogo, destClientLogo, true);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(String.Format("Exception Encountered - {0}", ex.Message));
                }
            }
        }
        /// <summary>
        /// Gets the file path.
        /// </summary>
        /// <returns></returns>
        public string GetFilePath()
        {
            return this.filePath;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if(obj is ExtentReporter)
            {
                ExtentReporter objExptentReporterTemp = (ExtentReporter)obj;
                if(objExptentReporterTemp.GetFilePath().Equals(this.GetFilePath()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.GetFilePath().Length;
        }
        /// <summary>
        /// Initializes the test case.
        /// </summary>
        /// <param name="testcaseName">Name of the testcase.</param>
        /// <param name="testCaseDescription">The test case description.</param>
        public void InitTestCase(string testcaseName, string testCaseDescription = "")
        {
            this.objExtentTest = this.objExtentReport.StartTest(testcaseName, testCaseDescription);
        }
        /// <summary>
        /// Logs the success.
        /// </summary>
        /// <param name="stepName">Name of the step.</param>
        /// <param name="stepDescription">The step description.</param>
        /// <param name="screenShotPath">The screen shot path.</param>
        public void LogSuccess(string stepName, string stepDescription, string screenShotPath = null)
        {
            this.objExtentTest.Log(LogStatus.Pass, stepName, stepDescription);
            this.TakeScreenShot(LogStatus.Pass, screenShotPath);
            this.objExtentReport.Flush();
        }

        /// <summary>
        /// Logs the failure.
        /// </summary>
        /// <param name="stepName">Name of the step.</param>
        /// <param name="stepDescription">The step description.</param>
        /// <param name="screenShotPath">The screen shot path.</param>
        public void LogFailure(string stepName, string stepDescription, string screenShotPath = null)
        {
            this.objExtentTest.Log(LogStatus.Fail, stepName, stepDescription);
            this.TakeScreenShot(LogStatus.Fail, screenShotPath);
            this.objExtentReport.Flush();
        }

        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="screenShotPath">The screen shot path.</param>
        public void LogInfo(string message, string screenShotPath = null)
        {
            this.objExtentTest.Log(LogStatus.Info,"Info", String.Format("{0}{1}{2}", "<mark>", message, "</mark>"));
            this.TakeScreenShot(LogStatus.Info, screenShotPath);
            this.objExtentReport.Flush();

        }

        /// <summary>
        /// Logs the warning.
        /// </summary>
        /// <param name="stepName">Name of the step.</param>
        /// <param name="stepDescription">The step description.</param>
        /// <param name="screenShotPath">The screen shot path.</param>
        public void LogWarning(string stepName, string stepDescription, string screenShotPath = null)
        {
            this.objExtentTest.Log(LogStatus.Warning, stepName, stepDescription);
            this.TakeScreenShot(LogStatus.Warning, screenShotPath);
            this.objExtentReport.Flush();
        }
        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="screenShotPath">The screen shot path.</param>
        public void LogException(Exception ex, string screenShotPath = null)
        {
            this.objExtentTest.Log(LogStatus.Error, ex);
            this.TakeScreenShot(LogStatus.Error, screenShotPath);
            this.objExtentReport.Flush();
        }

        /// <summary>
        /// Logs the fatal.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="screenShotPath">The screen shot path.</param>
        public void LogFatal(Exception ex, string screenShotPath = null)
        {
            this.objExtentTest.Log(LogStatus.Fatal, ex);
            this.TakeScreenShot(LogStatus.Error, screenShotPath);
            this.objExtentReport.Flush();
        }

        /// <summary>
        /// Updates the test case status.
        /// </summary>
        public void UpdateTestCaseStatus()
        {
            this.objExtentReport.EndTest(this.objExtentTest);
            this.objExtentReport.Flush();
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
            this.objExtentReport.Close();
        }
        /// <summary>
        /// Takes the screen shot.
        /// </summary>
        /// <param name="logStatus">The log status.</param>
        /// <param name="screenShotPath">The screen shot path.</param>
        private void TakeScreenShot(LogStatus logStatus, string screenShotPath)
        {

            if(screenShotPath != null)
            {
                FileUtilities.TakeScreenShot(screenShotPath);                
                string relativePath = FileUtilities.MakeRelativePath(this.filePath, screenShotPath);
                this.objExtentTest.Log(logStatus, "Snapshot Below : " + this.objExtentTest.AddScreenCapture(@relativePath));
            }
        }

        /// <summary>
        /// Manipulates the test report.
        /// </summary>
        /// <param name="objTestReportManipulator">The object test report manipulator.</param>
        public void ManipulateTestReport(ITestReportManipulator objTestReportManipulator)
        {
            objTestReportManipulator.Manipulate();
        }

    }
}
