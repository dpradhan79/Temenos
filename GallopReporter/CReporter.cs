using System;
using System.Collections.Generic;
using log4net;
using System.IO;
using Microsoft.Win32;
using System.Text;
using OpenQA.Selenium;
using StandardUtilities;
namespace GallopReporter
{
    /// <summary>
    /// @Author - Debasish Pradhan
    /// </summary>
    public class CReporter
    {
        private static readonly ILog LOG = LogManager.GetLogger(typeof(CReporter).Name);
        private BrowserContext browserContext = null;
        private string reportPath = null;

        private static IDictionary<BrowserContext, CReporter> mapBrowserContextReporter =
            new Dictionary<BrowserContext, CReporter>();

        private string[] package_testname;
        private static DateTime _testDate = DateTime.Now;
        private static List<TestCase> failedTestCases = null;
        private static List<TestCase> allTestCases = null;
        public static string REPORTDIRECTORY = null;
        private static string EXECUTIONENVIRONMENT = null;
        /// <summary>
        /// Initializes a new instance of the <see cref="CReporter"/> class.
        /// </summary>
        /// <param name="browserName">Name of the browser.</param>
        /// <param name="version">The version.</param>
        /// <param name="platform">The platform.</param>
        /// <param name="append">if set to <c>true</c> [append].</param>
        private CReporter(string browserName, string version, string platform, bool append)
        {
            this.browserContext = BrowserContext.getBrowserContext(browserName, version, platform);
            this.reportPath = this.filePath();
            LOG.Info("instance member browserContext was set to : ");
            LOG.Info(this.browserContext);
        }

        /// <summary>
        /// Gets the c reporter.
        /// </summary>
        /// <param name="browserName">Name of the browser.</param>
        /// <param name="version">The version.</param>
        /// <param name="platform">The platform.</param>
        /// <param name="append">if set to <c>true</c> [append].</param>
        /// <returns></returns>
        public static CReporter getCReporter(string browserName, string version, string platform, bool append)
        {
            lock (typeof(CReporter))
            {
                CReporter reporter = null;
                BrowserContext browserContext = BrowserContext.getBrowserContext(browserName, version, platform);
                //reporter = CReporter.mapBrowserContextReporter[browserContext];
                if (CReporter.mapBrowserContextReporter.ContainsKey(browserContext) == false)
                {
                    reporter = new CReporter(browserName, version, platform, append);
                    LOG.Info("Instance Of CReporter Created");
                    CReporter.mapBrowserContextReporter[browserContext] = reporter;
                    LOG.Info("reporter was placed into CReporter.mapBrowserContextReporter");
                }

                reporter = CReporter.mapBrowserContextReporter[browserContext];

                return reporter;
            }
        }
        /// <summary>
        /// Copies the logos.
        /// </summary>
        /// <param name="logos">The logos.</param>
        private void copyLogos(string destFolderPath, params string[] logos)
        {
            DirectoryInfo destFolder =
               new DirectoryInfo(destFolderPath);
            foreach (string logo in logos)
            {
                LOG.Info("Current Logo Name : " + logo);
                Directory.GetCurrentDirectory();
                FileInfo logoFile = new FileInfo(logo);
                /* copy File if exist */
                if (logoFile.Exists)
                {
                    try
                    {
                        File.Copy(logoFile.FullName,
                            destFolder.FullName + Path.DirectorySeparatorChar + this.getFileName(logo), true);
                        //FileUtils.copyFileToDirectory(logoFile, destFolder);
                        LOG.Info(logoFile + "copied to " + destFolder);
                    }
                    catch (IOException e)
                    {
                        // TODO Auto-generated catch block
                        LOG.Info(logoFile + "could not be copied to " + destFolder);
                        LOG.Info("IOException Encountered : " + e.Message);
                        Console.WriteLine(e.ToString());
                        Console.Write(e.StackTrace);
                    }
                }
            }
        }

        /// <summary>
        /// Files the path.
        /// </summary>
        /// <returns></returns>
        public string filePath()
        {
            if (CReporter.REPORTDIRECTORY == null)
            {
                string browserName = this.browserContext.BrowserName;
                string browserVersion = this.browserContext.BrowserVersion;
                string browserPlatform = this.browserContext.BrowserPlatform;

                LOG.Debug("browser name = " + browserName);
                CReporter.REPORTDIRECTORY = browserName;
                /* While implementing for cloud saucelab commented to create folder as set for browser execution*/
                /*
                switch (browserName.ToLower())
                {
                    case "firefox":
                        strDirectory = "firefox";
                        break;

                    case "chrome":
                        strDirectory = "chrome";
                        break;
                    case "ie":
                        strDirectory = "IE";
                        break;
                    default:
                        strDirectory = browserName;
                    
                        break;
                }
                 * */
                //CReporter.strDirectory = CReporter.strDirectory + "-" + browserVersion + "-" + browserPlatform;

                //string strResultMainDir = @ReporterConstants.LOCATION_RESULT + Path.DirectorySeparatorChar + DateTime.Today.ToString("dd-MMM-yyyy") + Path.DirectorySeparatorChar + ReporterConstants.EXECUTION_MODE + Path.DirectorySeparatorChar + browserName + Path.DirectorySeparatorChar + CReporter.EXECUTIONENVIRONMENT;
                string strResultMainDir = @ReporterConstants.LOCATION_RESULT + Path.DirectorySeparatorChar + DateTime.Today.ToString("dd-MMM-yyyy") + Path.DirectorySeparatorChar + browserName + Path.DirectorySeparatorChar + CReporter.EXECUTIONENVIRONMENT;
                DirectoryInfo resultMainDir =
                    new DirectoryInfo(strResultMainDir);
                LOG.Info("resultMainDir = " + resultMainDir);
                if (resultMainDir.Exists == false)
                {
                    try
                    {
                        resultMainDir.Create();
                    }
                    catch (Exception e)
                    {
                        LOG.Info("Exception Encountered : " + e.Message);
                    }
                }
                string srtResultDir = @strResultMainDir + Path.DirectorySeparatorChar + System.Environment.MachineName + "_" + browserVersion + "_" + browserPlatform + "_" + _testDate.ToString("HH.mm.ss_tt");
                DirectoryInfo resultDir =
                   new DirectoryInfo(srtResultDir);
                LOG.Info("resultDir = " + resultDir);
                if (resultDir.Exists == false)
                {
                    try
                    {
                        resultDir.Create();
                    }
                    catch (Exception e)
                    {
                        LOG.Info("Exception Encountered : " + e.Message);
                    }
                }


                string strScreenShotDir = @srtResultDir + Path.DirectorySeparatorChar + ReporterConstants.FOLDER_SCREENRSHOTS;
                DirectoryInfo screenShotDir =
                    new DirectoryInfo(strScreenShotDir);

                if (screenShotDir.Exists == false)
                {
                    try
                    {
                        screenShotDir.Create();
                        this.copyLogos(strScreenShotDir, ReporterConstants.LOCATION_CLIENT_LOGO, ReporterConstants.LOCATION_COMPANY_LOGO,
                            ReporterConstants.LOCATION_FAILED_LOGO, ReporterConstants.LOCATION_MINUS_LOGO,
                            ReporterConstants.LOCATION_PASSED_LOGO, ReporterConstants.LOCATION_PLUS_LOGO,
                            ReporterConstants.LOCATION_WARNING_LOGO);
                    }
                    catch (Exception e)
                    {
                        LOG.Info("Exception Encountered : " + e.Message);
                    }
                }

                try
                {
                    CReporter.REPORTDIRECTORY = resultDir.FullName;
                }
                catch (IOException e)
                {
                    // TODO Auto-generated catch block
                    LOG.Error("IOException Encountered : " + e.Message);
                    Console.WriteLine(e.ToString());
                    Console.Write(e.StackTrace);
                }
            }
            return CReporter.REPORTDIRECTORY;
        }

        /// <summary>
        /// Dates the stamp.
        /// </summary>
        /// <returns></returns>
        private static string dateStamp()
        {
            DateTime date = DateTime.Now;
            return date.ToShortDateString();
        }

        /// <summary>
        /// Dates the time.
        /// </summary>
        /// <returns></returns>
        private static string dateTime()
        {
            DateTime todaysDate = DateTime.Now;
            string formattedDate = String.Format("{0:dd - MM - yyyy HH: mm:ss a}", todaysDate);
            return formattedDate;
        }

        /// <summary>
        /// Gets the time.
        /// </summary>
        /// <returns></returns>
        private static string getTime()
        {
            DateTime todaysDate = DateTime.Now;
            string formattedDate = String.Format("{0:HH:mm:ss a}", todaysDate);
            return formattedDate;
        }

        /// <summary>
        /// Times the stamp.
        /// </summary>
        /// <returns></returns>
        private static string timeStamp()
        {
            return DateTime.Now.ToString();
        }


        /// <summary>
        /// Oses the environment.
        /// </summary>
        /// <returns></returns>
        private static string osEnvironment()
        {
            var os = Environment.OSVersion;
            String subKey = @"SOFTWARE\Wow6432Node\Microsoft\Windows NT\CurrentVersion";
            RegistryKey key = Registry.LocalMachine;
            RegistryKey skey = key.OpenSubKey(subKey);
            string osName = skey.GetValue("ProductName").ToString();
            // TODO : Should work on os.arch
            return "Current suit executed on : " + osName + "/version : " + os.VersionString + "/Architecture ";
            // + System.getProperty("os.arch");
        }

        /// <summary>
        /// Gets the name of the host.
        /// </summary>
        /// <returns></returns>
        private static string getHostName()
        {
            return System.Net.Dns.GetHostName();
        }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        private string getFileName(string filePath)
        {
            string fileNameOnly = null;
            FileInfo file = new FileInfo(filePath);
            try
            {
                if (file.Exists)
                {
                    fileNameOnly = file.Name.ToString();
                }
            }
            catch (Exception e)
            {
                LOG.Error("Exception Encountered : " + e.Message);
                throw e;
            }
            return fileNameOnly;
        }

        /// <summary>
        /// Reports the creater.
        /// </summary>
        private void reportCreater()
        {
            switch (ReporterConstants.REPORT_FORMAT.ToLower())
            {
                case "html":

                    this.htmlCreateReport();

                    break;
                default:

                    this.htmlCreateReport();
                    break;
            }
        }

        /// <summary>
        /// Makes the unique image path.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        private static string makeUniqueImagePath(string fileName)
        {
            string newFileName = fileName;
            try
            {
                //Verifying if the file already exists, if so append the numbers 1,2  so on to the fine name.			

                FileInfo myPngImage = new FileInfo(fileName);
                int counter = 1;
                while (myPngImage.Exists)
                {
                    string tempFileName = fileName.Substring(0, fileName.Length - ".jpeg".Length);
                    fileName = tempFileName;
                    newFileName = fileName + "_" + counter + ".jpeg";
                    myPngImage = new FileInfo(newFileName);
                    counter++;
                }
                return newFileName;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
                return newFileName;
            }
        }

        /// <summary>
        /// Screens the shot.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="fileName">Name of the file.</param>
        private void screenShot(IWebDriver driver, string fileName)
        {
            //added by debasish as a part of stabilization in case of IE. SaveAsFile placed under try/catch
            try
            {
                if (driver != null)
                {
                    Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
                    ss.SaveAsFile(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                else
                {

                    FileUtilities.TakeScreenShot(fileName);
                }
                
            }
            catch (Exception)
            {
            }

        }

        /// <summary>
        /// Ons the success.
        /// </summary>
        /// <param name="strStepName">Name of the string step.</param>
        /// <param name="strStepDes">The string step DES.</param>
        private void onSuccess(string strStepName, string strStepDes)
        {
            FileInfo file =
                new FileInfo(this.reportPath + System.IO.Path.DirectorySeparatorChar +
                             TestResult.strTestName[this.browserContext] + "_Results" + ".html");
            // "SummaryReport.html"
            /* + TestResult.timeStamp */
            StreamWriter writer = null;

            string imgSrc = "'." + System.IO.Path.DirectorySeparatorChar + ReporterConstants.FOLDER_SCREENRSHOTS +
                            System.IO.Path.DirectorySeparatorChar +
                            this.getFileName(ReporterConstants.LOCATION_PASSED_LOGO) + "'";
            int? stepNumValue = null;
            if (TestResult.stepNum.ContainsKey(this.browserContext))
            {
                stepNumValue = TestResult.stepNum[this.browserContext];
            }

            if (stepNumValue != null)
            {
                TestResult.stepNum[this.browserContext] = stepNumValue + 1;
            }
            else
            {
                TestResult.stepNum.Add(this.browserContext, stepNumValue + 1);
            }


            try
            {
                // testdescrption.put(TestTitleDetails.x.toString(),
                // TestResult.testDescription.get(TestTitleDetails.x));
                string strPackageName = TestResult.packageName[this.browserContext];
                string strTcName = TestResult.tc_name[this.browserContext];
                if (
                    !TestResult.mapBrowserContextTestCaseRef[this.browserContext][strPackageName + ":" + strTcName]
                        .Equals(ReporterConstants.TEST_CASE_STATUS_FAIL))
                {
                    if (
                        TestResult.mapBrowserContextTestCaseRef[this.browserContext].ContainsKey(strPackageName + ":" +
                                                                                                 strTcName))
                    {
                        TestResult.mapBrowserContextTestCaseRef[this.browserContext][strPackageName + ":" + strTcName] =
                            ReporterConstants.TEST_CASE_STATUS_PASS;
                    }
                    else
                    {
                        TestResult.mapBrowserContextTestCaseRef[this.browserContext].Add(
                            strPackageName + ":" + strTcName, ReporterConstants.TEST_CASE_STATUS_PASS);
                    }

                    // map.put(TestTitleDetails.x.toString(),
                    // TestResult.testDescription.get(TestTitleDetails.x.toString()));
                }
                writer = new System.IO.StreamWriter(file.FullName, true);
                writer.Write("<tr class='content2' >");
                writer.Write("<td>" + TestResult.stepNum[this.browserContext] + "</td> ");
                writer.Write("<td class='justified'>" + strStepName + "</td>");
                writer.Write("<td class='justified'>" + strStepDes + "</td> ");

                writer.Write("<td class='Pass' align='center'><img  src=" + imgSrc + " width='18' height='18'/></td> ");
                int? passNumValue = null;
                if (TestResult.PassNum.ContainsKey(this.browserContext))
                {
                    passNumValue = TestResult.PassNum[this.browserContext];
                }

                if (passNumValue != null)
                {
                    TestResult.PassNum[this.browserContext] = passNumValue + 1;
                }
                else
                {
                    TestResult.PassNum.Add(this.browserContext, passNumValue + 1);
                }

                string strPassTime = CReporter.getTime();
                writer.Write("<td><small>" + strPassTime + "</small></td> ");
                writer.Write("</tr> ");
                writer.Close();
            }
            catch (Exception e)
            {
                LOG.Info("Exception Encountered : " + e.Message);
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
        }

        /// <summary>
        /// Ons the failure.
        /// </summary>
        /// <param name="strStepName">Name of the string step.</param>
        /// <param name="strStepDes">The string step DES.</param>
        /// <param name="fileName">Name of the file.</param>
        public void onFailure(string strStepName, string strStepDes, string fileName)
        {
            string href = "./" +
                          StringHelperClass.SubstringSpecial(fileName,
                              fileName.IndexOf(ReporterConstants.FOLDER_SCREENRSHOTS), fileName.Length);
            string imgSrc = "'." + System.IO.Path.DirectorySeparatorChar + ReporterConstants.FOLDER_SCREENRSHOTS +
                            System.IO.Path.DirectorySeparatorChar +
                            this.getFileName(ReporterConstants.LOCATION_FAILED_LOGO) + "'";
            StreamWriter writer = null;
            try
            {
                FileInfo file =
                    new FileInfo(this.reportPath + System.IO.Path.DirectorySeparatorChar +
                                 TestResult.strTestName[this.browserContext] + "_Results" + ".html");
                // "SummaryReport.html"
                /* + TestResult.timeStamp + */

                writer = new System.IO.StreamWriter(file.FullName, true);
                int? stepNumValue = null;
                if (TestResult.stepNum.ContainsKey(this.browserContext))
                {
                    stepNumValue = TestResult.stepNum[this.browserContext];
                }

                if (stepNumValue != null)
                {
                    TestResult.stepNum[this.browserContext] = stepNumValue + 1;
                }
                else
                {
                    TestResult.stepNum.Add(this.browserContext, stepNumValue + 1);
                }

                writer.Write("<tr class='content2' >");
                writer.Write("<td>" + TestResult.stepNum[this.browserContext] + "</td> ");
                writer.Write("<td class='justified'>" + strStepName + "</td>");
                writer.Write("<td class='justified'>" + strStepDes + "</td> ");

                int? failNumValue = null;
                if (TestResult.FailNum.ContainsKey(this.browserContext))
                {
                    failNumValue = TestResult.FailNum[this.browserContext];
                }
                if (stepNumValue != null)
                {
                    if (TestResult.FailNum.ContainsKey(this.browserContext))
                    {
                        TestResult.FailNum[this.browserContext] = failNumValue + 1;
                    }
                    else
                    {
                        TestResult.FailNum.Add(this.browserContext, failNumValue + 1);
                    }
                }

                /*
                 * writer.write("<td class='Fail' align='center'><a  href='"+
                 * "./Screenshots"+"/" + strStepDes.replace(" ", "_") + "_" +
                 * TestResult.timeStamp + ".jpeg'"+
                 * " alt= Screenshot  width= 15 height=15 style='text-decoration:none;'><img  src='./Screenshots/failed.ico' width='18' height='18'/></a></td>"
                 * );
                 */
                // New Screen shot code to avoid overriding \\\\
                writer.Write("<td class='Fail' align='center'><a  href='" + href + "'" +
                             " alt= Screenshot  width= 15 height=15 style='text-decoration:none;'><img  src=" + imgSrc +
                             "height='18'/>View Screenshot</a></td>");

                string strFailTime = CReporter.getTime();
                writer.Write("<td><small>" + strFailTime + "</small></td> ");
                writer.Write("</tr> ");
                writer.Close();
                string strPackageName = TestResult.packageName[this.browserContext];
                string strTcName = TestResult.tc_name[this.browserContext];
                if (
                    !TestResult.mapBrowserContextTestCaseRef[this.browserContext][strPackageName + ":" + strTcName]
                        .Equals("PASS"))
                {
                    TestResult.mapBrowserContextTestCaseRef[this.browserContext].Add(strPackageName + ":" + strTcName,
                        "FAIL");
                }
            }
            catch (Exception e)
            {
                LOG.Info("Exception Encountered : " + e.Message);
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
        }

        /// <summary>
        /// Ons the warning.
        /// </summary>
        /// <param name="strStepName">Name of the string step.</param>
        /// <param name="strStepDes">The string step DES.</param>
        private void onWarning(string strStepName, string strStepDes)
        {
            StreamWriter writer = null;
            try
            {
                FileInfo file =
                    new FileInfo(this.reportPath + System.IO.Path.DirectorySeparatorChar +
                                 TestResult.strTestName[this.browserContext] + "_Results" + ".html");
                // "SummaryReport.html"
                /* + TestResult.timeStamp */
                string imgSrc = "'." + System.IO.Path.DirectorySeparatorChar + ReporterConstants.FOLDER_SCREENRSHOTS +
                                System.IO.Path.DirectorySeparatorChar +
                                this.getFileName(ReporterConstants.LOCATION_WARNING_LOGO) + "'";
                writer = new System.IO.StreamWriter(file.FullName, true);
                int? stepNumValue = TestResult.stepNum[this.browserContext];
                if (stepNumValue != null)
                {
                    if (TestResult.stepNum.ContainsKey(this.browserContext))
                    {
                        TestResult.stepNum.Remove(this.browserContext);
                    }
                    TestResult.stepNum.Add(this.browserContext, stepNumValue + 1);
                }

                writer.Write("<tr class='content2' >");
                writer.Write("<td>" + TestResult.stepNum[this.browserContext] + "</td> ");
                writer.Write("<td class='justified'>" + strStepName + "</td>");
                writer.Write("<td class='justified'>" + strStepDes + "</td> ");
                // TestResult.FailNum = TestResult.FailNum + 1;

                writer.Write("<td class='Fail'  align='center'><a  href='" + "." + System.IO.Path.DirectorySeparatorChar +
                             ReporterConstants.FOLDER_SCREENRSHOTS + System.IO.Path.DirectorySeparatorChar +
                             strStepDes.Replace(" ", "_") + ".jpeg'" +
                             " alt= Screenshot  width= 15 height=15 style='text-decoration:none;'><img  src=" + imgSrc +
                             " width='18' height='18'/></a></td>"); /*
													 * + "_" +
													 * TestResult.timeStamp
													 */

                string strFailTime = CReporter.getTime();
                writer.Write("<td><small>" + strFailTime + "</small></td> ");
                writer.Write("</tr> ");
                writer.Close();
            }
            catch (Exception e)
            {
                LOG.Info("Exception Encountered : " + e.Message);
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
        }

        /// <summary>
        /// Reports the step.
        /// </summary>
        /// <param name="StepDesc">The step desc.</param>
        private void reportStep(string StepDesc)
        {
            StepDesc = StepDesc.Replace(" ", "_");

            FileInfo file =
                new FileInfo(this.reportPath + System.IO.Path.DirectorySeparatorChar +
                             TestResult.strTestName[this.browserContext] + "_Results" + ".html");
            // "SummaryReport.html"
            /*+ TestResult.timeStamp*/
            StreamWriter writer = null;

            try
            {
                writer = new System.IO.StreamWriter(file.FullName, true);
                int? bFunctionNo = TestResult.BFunctionNo[this.browserContext];
                if (bFunctionNo != null && bFunctionNo > 0)
                {
                    writer.Write("</tbody>");
                }
                writer.Write("<tbody>");
                writer.Write("<tr class='section'> ");
                writer.Write("<td colspan='5' onclick=toggleMenu('" + StepDesc + TestResult.stepNum[this.browserContext] +
                             "')>+ " + StepDesc + "</td>");
                writer.Write("</tr> ");
                writer.Write("</tbody>");
                writer.Write("<tbody id='" + StepDesc + TestResult.stepNum[this.browserContext] +
                             "' style='display:table-row-group'>");
                writer.Close();

                TestResult.BFunctionNo.Add(this.browserContext, bFunctionNo + 1);
            }
            catch (Exception)
            {
            }
        }



        /// <summary>
        /// Gets the browser context.
        /// </summary>
        /// <value>
        /// The browser context.
        /// </value>
        public virtual BrowserContext BrowserContext
        {
            get { return this.browserContext; }
        }

        private void htmlCreateReport()
        {
            FileInfo file = new FileInfo(this.reportPath + "/" + TestResult.strTestName + "_Results" + ".html");
            // "Results.html"
            /*+ TestResult.timeStamp*/
            if (file.Exists)
            {
                file.Delete();
            }
        }



        /// <summary>
        /// Creates the HTML summary report.
        /// </summary>
        /// <param name="Url">The URL.</param>
        /// <param name="append">if set to <c>true</c> [append].</param>
        public void createHtmlSummaryReport(string Url, bool append) //throws Exception
        {
            //FileInfo file = new FileInfo(this.filePath() + "/" + "SummaryResults" + ".html"); 
            FileInfo file = new FileInfo(this.reportPath + "/" + "SummaryResults" + ".html");
            StreamWriter writer = null;
            string imgSrcClientLogo = "." + System.IO.Path.DirectorySeparatorChar +
                                      ReporterConstants.FOLDER_SCREENRSHOTS + System.IO.Path.DirectorySeparatorChar +
                                      this.getFileName(ReporterConstants.LOCATION_CLIENT_LOGO);
            string imgSrcCompanyLogo = "." + System.IO.Path.DirectorySeparatorChar +
                                       ReporterConstants.FOLDER_SCREENRSHOTS + System.IO.Path.DirectorySeparatorChar +
                                       this.getFileName(ReporterConstants.LOCATION_COMPANY_LOGO);
            //string imgSrcClientLogo = this.getFileName(ReporterConstants.LOCATION_CLIENT_LOGO);
            //string imgSrcCompanyLogo = this.getFileName(ReporterConstants.LOCATION_COMPANY_LOGO);


            if (file.Exists)
            {
                file.Delete();
            }
            writer = new System.IO.StreamWriter(file.FullName, append);
            try
            {
                writer.Write("<!DOCTYPE html>");
                writer.Write("<html> ");
                writer.Write("<head> ");
                writer.Write("<meta charset='UTF-8'> ");
                writer.Write("<title>Automation Execution Results Summary</title>");

                writer.Write("<style type='text/css'>");
                writer.Write("body {");
                writer.Write("background-color: #FFFFFF; ");
                writer.Write("font-family: Verdana, Geneva, sans-serif; ");
                writer.Write("text-align: center; ");
                writer.Write("} ");

                writer.Write("small { ");
                writer.Write("font-size: 0.7em; ");
                writer.Write("} ");

                writer.Write("table { ");
                writer.Write("box-shadow: 9px 9px 10px 4px #BDBDBD;");
                writer.Write("border: 0px solid #4D7C7B;");
                writer.Write("border-collapse: collapse; ");
                writer.Write("border-spacing: 0px; ");
                writer.Write("width: 1000px; ");
                writer.Write("margin-left: auto; ");
                writer.Write("margin-right: auto; ");
                writer.Write("} ");

                writer.Write("tr.heading { ");
                writer.Write("background-color: #041944;");
                writer.Write("color: #FFFFFF; ");
                writer.Write("font-size: 0.7em; ");
                writer.Write("font-weight: bold; ");
                writer.Write(
                    "background:-o-linear-gradient(bottom, #999999 5%, #000000 100%);	background:-webkit-gradient( linear, left top, left bottom, color-stop(0.05, #999999), color-stop(1, #000000) );");
                writer.Write("background:-moz-linear-gradient( center top, #999999 5%, #000000 100% );");
                writer.Write(
                    "filter:progid:DXImageTransform.Microsoft.gradient(startColorstr=#999999, endColorstr=#000000);	background: -o-linear-gradient(top,#999999,000000);");
                writer.Write("} ");

                writer.Write("tr.subheading { ");
                writer.Write("background-color: #6A90B6;");
                writer.Write("color: #000000; ");
                writer.Write("font-weight: bold; ");
                writer.Write("font-size: 0.7em; ");
                writer.Write("text-align: justify; ");
                writer.Write("} ");

                writer.Write("tr.section { ");
                writer.Write("background-color: #A4A4A4; ");
                writer.Write("color: #333300; ");
                writer.Write("cursor: pointer; ");
                writer.Write("font-weight: bold;");
                writer.Write("font-size: 0.8em; ");
                writer.Write("text-align: justify;");
                writer.Write(
                    "background:-o-linear-gradient(bottom, #56aaff 5%, #e5e5e5 100%);	background:-webkit-gradient( linear, left top, left bottom, color-stop(0.05, #56aaff), color-stop(1, #e5e5e5) );");
                writer.Write("background:-moz-linear-gradient( center top, #56aaff 5%, #e5e5e5 100% );");
                writer.Write(
                    "filter:progid:DXImageTransform.Microsoft.gradient(startColorstr=#56aaff, endColorstr=#e5e5e5);	background: -o-linear-gradient(top,#56aaff,e5e5e5);");

                writer.Write("} ");

                writer.Write("tr.subsection { ");
                writer.Write("cursor: pointer; ");
                writer.Write("} ");

                writer.Write("tr.content { ");
                writer.Write("background-color: #FFFFFF; ");
                writer.Write("color: #000000; ");
                writer.Write("font-size: 0.7em; ");
                writer.Write("display: table-row; ");
                writer.Write("} ");

                writer.Write("tr.content2 { ");
                writer.Write("background-color:#;E1E1E1");
                writer.Write("border: 1px solid #4D7C7B;");
                writer.Write("color: #000000; ");
                writer.Write("font-size: 0.7em; ");
                writer.Write("display: table-row; ");
                writer.Write("} ");

                writer.Write("td, th { ");
                writer.Write("padding: 5px; ");
                writer.Write("border: 1px solid #4D7C7B; ");
                writer.Write("text-align: inherit\0/; ");
                writer.Write("} ");

                writer.Write("th.Logos { ");
                writer.Write("padding: 5px; ");
                writer.Write("border: 0px solid #4D7C7B; ");
                writer.Write("text-align: inherit /;");
                writer.Write("} ");

                writer.Write("td.justified { ");
                writer.Write("text-align: justify; ");
                writer.Write("} ");

                writer.Write("td.pass {");
                writer.Write("font-weight: bold; ");
                writer.Write("color: green; ");
                writer.Write("} ");

                writer.Write("td.fail { ");
                writer.Write("font-weight: bold; ");
                writer.Write("color: red; ");
                writer.Write("} ");

                writer.Write("td.done, td.screenshot { ");
                writer.Write("font-weight: bold; ");
                writer.Write("color: black; ");
                writer.Write("} ");
                writer.Write("td.debug { ");
                writer.Write("font-weight: bold; ");
                writer.Write("color: blue; ");
                writer.Write("} ");

                writer.Write("td.warning { ");
                writer.Write("font-weight: bold; ");
                writer.Write("color: orange; ");
                writer.Write("} ");
                writer.Write("</style> ");

                writer.Write("<script> ");
                writer.Write("function toggleMenu(objID) { ");
                writer.Write(" if (!document.getElementById) return;");
                writer.Write(" var ob = document.getElementById(objID).style; ");
                writer.Write("if(ob.display === 'none') { ");
                writer.Write(" try { ");
                writer.Write(" ob.display='table-row-group';");
                writer.Write("} catch(ex) { ");
                writer.Write("	 ob.display='block'; ");
                writer.Write("} ");
                writer.Write("} ");
                writer.Write("else { ");
                writer.Write(" ob.display='none'; ");
                writer.Write("} ");
                writer.Write("} ");
                writer.Write("function toggleSubMenu(objId) { ");
                writer.Write("for(i=1; i<10000; i++) { ");
                writer.Write("var ob = document.getElementById(objId.concat(i)); ");
                writer.Write("if(ob === null) { ");
                writer.Write("break; ");
                writer.Write("} ");
                writer.Write("if(ob.style.display === 'none') { ");
                writer.Write("try { ");
                writer.Write(" ob.style.display='table-row'; ");
                writer.Write("} catch(ex) { ");
                writer.Write("ob.style.display='block'; ");
                writer.Write("} ");
                writer.Write(" } ");
                writer.Write("else { ");
                writer.Write("ob.style.display='none'; ");
                writer.Write("} ");
                writer.Write(" } ");
                writer.Write("} ");
                writer.Write("</script> ");
                writer.Write("</head> ");

                writer.Write("<body> ");
                writer.Write("</br>");

                writer.Write("<table id='Logos'>");
                writer.Write("<colgroup>");
                writer.Write("<col style='width: 25%' />");
                writer.Write("<col style='width: 25%' />");
                writer.Write("<col style='width: 25%' />");
                writer.Write("<col style='width: 25%' />");
                writer.Write("</colgroup> ");
                writer.Write("<thead> ");

                writer.Write("<tr class='content'>");
                writer.Write("<th class ='Logos' colspan='2' >");
                writer.Write("<img align ='left' src= " + imgSrcClientLogo + "></img>");
                writer.Write("</th>");
                writer.Write("<th class = 'Logos' colspan='2' > ");
                writer.Write("<img align ='right' src=  " + imgSrcCompanyLogo + "></img>");
                writer.Write("</th> ");
                writer.Write("</tr> ");

                writer.Write("</thead> ");
                writer.Write("</table> ");

                writer.Write("<table id='header'> ");
                writer.Write("<colgroup> ");
                writer.Write("<col style='width: 25%' /> ");
                writer.Write("<col style='width: 25%' /> ");
                writer.Write("<col style='width: 25%' /> ");
                writer.Write(" <col style='width: 25%' /> ");
                writer.Write("</colgroup> ");

                writer.Write("<thead> ");

                writer.Write("<tr class='heading'> ");
                writer.Write("<th colspan='4' style='font-family:Copperplate Gothic Bold; font-size:1.4em;'> ");
                writer.Write("Automation Execution Result Summary ");
                writer.Write("</th> ");
                writer.Write("</tr> ");
                writer.Write("<tr class='subheading'> ");
                writer.Write("<th>&nbsp;Date&nbsp;&&nbsp;Time&nbsp;:&nbsp;" + "" + "</th> ");
                // writer.Write("<th>&nbsp;:&nbsp;08-Apr-2013&nbsp;06:24:21&nbsp;PM</th> ");
                writer.Write("<th> &nbsp;" + CReporter.dateTime() + "&nbsp;</th> ");
                writer.Write("<th>&nbsp;OnError&nbsp;:&nbsp;</th> ");
                writer.Write("<th>" + ReporterConstants.ON_ERROR_ACTION + "</th> ");
                writer.Write("</tr> ");
                writer.Write("<tr class='subheading'> ");
                writer.Write("<th>&nbsp;Suite Executed&nbsp;:&nbsp;</th> ");
                writer.Write("<th>" + ReporterConstants.SUITE_NAME + "</th> ");
                writer.Write("<th>&nbsp;Browser&nbsp;:</th> ");
                writer.Write("<th>" + this.browserContext.BrowserName + "</th> ");
                writer.Write("</tr> ");


                writer.Write("<tr class='subheading'> ");
                writer.Write("<th>&nbsp;Host Name&nbsp;:</th> ");
                writer.Write("<th>" + System.Net.Dns.GetHostName() + "</th> ");
                writer.Write("<th>&nbsp;No.&nbsp;Of&nbsp;Threads&nbsp;:&nbsp;</th> ");
                writer.Write("<th>" + "NA" + "</th> ");
                writer.Write("</tr> ");

                writer.Write("<tr class='subheading'> ");
                writer.Write("<th>&nbsp;Device&nbsp;Name&nbsp;:</th> ");
                writer.Write("<th>" + ReporterConstants.DEVICE_NAME + "</th> ");
                writer.Write("<th>Version&nbsp;:</th> ");
                writer.Write("<th>" + this.browserContext.BrowserVersion + "</th> ");
                writer.Write("</tr> ");

                writer.Write("<tr class='subheading'> ");
                writer.Write("<th colspan='4'> ");
                writer.Write("&nbsp;Environment -  " + Url + "");
                writer.Write("</th> ");
                writer.Write("</tr> ");
                writer.Write("</thead> ");
                writer.Write("</table> ");
                writer.Write("<table id='main'> ");
                writer.Write("<colgroup> ");
                writer.Write("<col style='width: 5%' /> ");
                writer.Write("<col style='width: 35%' /> ");
                writer.Write("<col style='width: 42%' /> ");
                writer.Write("<col style='width: 10%' /> ");
                writer.Write("<col style='width: 8%' /> ");
                writer.Write("</colgroup> ");
                writer.Write("<thead> ");
                writer.Write("<tr class='heading'> ");
                writer.Write("<th>S.NO</th> ");
                writer.Write("<th>Test Case</th> ");
                writer.Write("<th>Description</th> ");
                writer.Write("<th>Time</th> ");
                writer.Write("<th>Status</th> ");
                writer.Write("</tr> ");
                writer.Write("</thead> ");

                /*get corresponding map to browserContext*/
                IDictionary<string, string> testCaseRef = TestResult.mapBrowserContextTestCaseRef[this.browserContext];

                IEnumerator<KeyValuePair<string, string>> iterator1 = testCaseRef.GetEnumerator();
                int serialNo = 1;
                while (iterator1.MoveNext())
                {
                    KeyValuePair<string, string> mapEntry1 = (KeyValuePair<string, string>)iterator1.Current;
                    /*key contains packagename:testmethod*/
                    LOG.Info("Key of mapEntry1 : " + mapEntry1.Key);
                    this.package_testname = mapEntry1.Key.Split(':');
                    LOG.Info("package is present in package_testname[0] : " + this.package_testname[0]);
                    LOG.Info("test method is present in package_testname[1] : " + this.package_testname[1]);
                    string testCaseExecutionStatus = (string)mapEntry1.Value;
                    LOG.Info("value against package_testname is : " + testCaseExecutionStatus);
                    writer.Write("<tbody> ");
                    writer.Write("<tr class='content2' > ");
                    writer.Write("<td class='justified'>" + serialNo + "</td>");
                    if (testCaseExecutionStatus.Equals(ReporterConstants.TEST_CASE_STATUS_PASS))
                    {
                        writer.Write("<td class='justified'><a href='" + package_testname[1] + "_Results" + ".html#'" +
                                     "' target='about_blank'>" +
                                     this.package_testname[1].Substring(0,
                                         this.package_testname[1].LastIndexOf("-", StringComparison.Ordinal)) + "</a></td>");
                        /*+ TestResult.timeStamp*/
                    }
                    else
                    {
                        writer.Write("<td class='justified'><a href='" + this.package_testname[1] + "_Results" +
                                     ".html'" + " target='about_blank'>" +
                                     this.package_testname[1].Substring(0,
                                         this.package_testname[1].LastIndexOf("-", StringComparison.Ordinal)) + "</a></td>");
                        /*+ TestResult.timeStamp */
                    }
                    string localTestDescription = "";
                    if (TestResult.testDescription != null)
                    {
                        IDictionary<string, string> mapTestDescription = TestResult.testDescription[this.browserContext];
                        if (mapTestDescription != null)
                        {
                            localTestDescription = mapTestDescription[this.package_testname[1]];
                        }
                    }
                    writer.Write("<td class='justified'>" + localTestDescription + "</td>");

                    writer.Write("<td>" + TestResult.executionTime[this.browserContext][this.package_testname[1]] +
                                 " Seconds</td>");
                    if (
                        TestResult.testResults[this.browserContext][this.package_testname[1]].Equals(
                            ReporterConstants.TEST_CASE_STATUS_PASS))
                    {
                        writer.Write("<td class='pass'>" + ReporterConstants.TEST_CASE_STATUS_PASS + "</td> ");
                    }
                    else
                    {
                        writer.Write("<td class='fail'>" + ReporterConstants.TEST_CASE_STATUS_FAIL + "</td> ");
                    }
                    writer.Write("</tr>");
                    writer.Write("</tbody> ");
                    serialNo = serialNo + 1;
                }
                writer.Flush();
                writer.Close();
            }
            catch (Exception)
            {
                writer.Flush();
                writer.Close();
            }
        }

        /// <summary>
        /// Tests the header.
        /// </summary>
        /// <param name="testName">Name of the test.</param>
        /// <param name="append">if set to <c>true</c> [append].</param>
        public virtual void testHeader(string testName, bool append)
        {
            StreamWriter writer = null;

            try
            {
                string imgSrcClientLogo = "." + System.IO.Path.DirectorySeparatorChar +
                                          ReporterConstants.FOLDER_SCREENRSHOTS + System.IO.Path.DirectorySeparatorChar +
                                          this.getFileName(ReporterConstants.LOCATION_CLIENT_LOGO);
                string imgSrcCompanyLogo = "." + System.IO.Path.DirectorySeparatorChar +
                                           ReporterConstants.FOLDER_SCREENRSHOTS + System.IO.Path.DirectorySeparatorChar +
                                           this.getFileName(ReporterConstants.LOCATION_COMPANY_LOGO);
                //string imgSrcClientLogo = this.getFileName(ReporterConstants.LOCATION_CLIENT_LOGO);
                //string imgSrcCompanyLogo = this.getFileName(ReporterConstants.LOCATION_COMPANY_LOGO);
                if (TestResult.strTestName.ContainsKey(this.browserContext))
                {
                    TestResult.strTestName[this.browserContext] = testName;
                }
                else
                {
                    TestResult.strTestName.Add(this.browserContext, testName);
                }

                FileInfo file =
                    new FileInfo(this.reportPath + System.IO.Path.DirectorySeparatorChar +
                                 TestResult.strTestName[this.browserContext] + "_Results" + ".html"); // "Results.html"
                /*+ TestResult.timeStamp */
                writer = new System.IO.StreamWriter(file.FullName, append);

                writer.Write("<!DOCTYPE html> ");
                writer.Write("<html>");
                writer.Write("<head> ");
                writer.Write("<meta charset='UTF-8'> ");
                writer.Write("<title>" + TestResult.strTestName[this.browserContext] + " Execution Results</title> ");

                writer.Write("<style type='text/css'> ");
                writer.Write("body { ");
                writer.Write("background-color: #FFFFFF; ");
                writer.Write("font-family: Verdana, Geneva, sans-serif; ");
                writer.Write("text-align: center; ");
                writer.Write("} ");

                writer.Write("small { ");
                writer.Write("font-size: 0.7em; ");
                writer.Write("} ");

                writer.Write("table { ");
                writer.Write("box-shadow: 9px 9px 10px 4px #BDBDBD;");
                writer.Write("border: 0px solid #4D7C7B; ");
                writer.Write("border-collapse: collapse; ");
                writer.Write("border-spacing: 0px; ");
                writer.Write("width: 1000px; ");
                writer.Write("margin-left: auto; ");
                writer.Write("margin-right: auto; ");
                writer.Write("} ");

                writer.Write("tr.heading { ");
                writer.Write("background-color: #041944; ");
                writer.Write("color: #FFFFFF; ");
                writer.Write("font-size: 0.7em; ");
                writer.Write("font-weight: bold; ");
                writer.Write(
                    "background:-o-linear-gradient(bottom, #999999 5%, #000000 100%);	background:-webkit-gradient( linear, left top, left bottom, color-stop(0.05, #999999), color-stop(1, #000000) );");
                writer.Write("background:-moz-linear-gradient( center top, #999999 5%, #000000 100% );");
                writer.Write(
                    "filter:progid:DXImageTransform.Microsoft.gradient(startColorstr=#999999, endColorstr=#000000);	background: -o-linear-gradient(top,#999999,000000);");
                writer.Write("} ");

                writer.Write("tr.subheading { ");
                writer.Write("background-color: #FFFFFF; ");
                writer.Write("color: #000000; ");
                writer.Write("font-weight: bold; ");
                writer.Write("font-size: 0.7em; ");
                writer.Write("text-align: justify; ");
                writer.Write("} ");

                writer.Write("tr.section { ");
                writer.Write("background-color: #A4A4A4; ");
                writer.Write("color: #333300; ");
                writer.Write("cursor: pointer; ");
                writer.Write("font-weight: bold; ");
                writer.Write("font-size: 0.7em; ");
                writer.Write("text-align: justify; ");
                writer.Write(
                    "background:-o-linear-gradient(bottom, #56aaff 5%, #e5e5e5 100%);	background:-webkit-gradient( linear, left top, left bottom, color-stop(0.05, #56aaff), color-stop(1, #e5e5e5) );");
                writer.Write("background:-moz-linear-gradient( center top, #56aaff 5%, #e5e5e5 100% );");
                writer.Write(
                    "filter:progid:DXImageTransform.Microsoft.gradient(startColorstr=#56aaff, endColorstr=#e5e5e5);	background: -o-linear-gradient(top,#56aaff,e5e5e5);");
                writer.Write("} ");

                writer.Write("tr.subsection { ");
                writer.Write("cursor: pointer; ");
                writer.Write("} ");

                writer.Write("tr.content { ");
                writer.Write("background-color: #FFFFFF; ");
                writer.Write("color: #000000; ");
                writer.Write("font-size: 0.7em; ");
                writer.Write("display: table-row; ");
                writer.Write("} ");

                writer.Write("tr.content2 { ");
                writer.Write("background-color: #E1E1E1; ");
                writer.Write("border: 1px solid #4D7C7B;");
                writer.Write("color: #000000; ");
                writer.Write("font-size: 0.75em; ");
                writer.Write("display: table-row; ");
                writer.Write("} ");

                writer.Write("td, th { ");
                writer.Write("padding: 5px; ");
                writer.Write("border: 1px solid #4D7C7B; ");
                writer.Write("text-align: inherit\0/; ");
                writer.Write("} ");

                writer.Write("th.Logos { ");
                writer.Write("padding: 5px; ");
                writer.Write("border: 0px solid #4D7C7B; ");
                writer.Write("text-align: inherit /;");
                writer.Write("} ");

                writer.Write("td.justified { ");
                writer.Write("text-align: justify; ");
                writer.Write("} ");

                writer.Write("td.pass { ");
                writer.Write("font-weight: bold; ");
                writer.Write("color: green; ");
                writer.Write("} ");

                writer.Write("td.fail { ");
                writer.Write("font-weight: bold; ");
                writer.Write("color: red; ");
                writer.Write("} ");

                writer.Write("td.done, td.screenshot { ");
                writer.Write("font-weight: bold; ");
                writer.Write("color: black; ");
                writer.Write("} ");

                writer.Write("td.debug { ");
                writer.Write("font-weight: bold;");
                writer.Write("color: blue; ");
                writer.Write("} ");

                writer.Write("td.warning { ");
                writer.Write("font-weight: bold; ");
                writer.Write("color: orange; ");
                writer.Write("} ");
                writer.Write("</style> ");

                writer.Write("<script> ");
                writer.Write("function toggleMenu(objID) { ");
                writer.Write("if (!document.getElementById) return; ");
                writer.Write("var ob = document.getElementById(objID).style; ");
                writer.Write("if(ob.display === 'none') { ");
                writer.Write("try { ");
                writer.Write("ob.display='table-row-group'; ");
                writer.Write("} catch(ex) { ");
                writer.Write("ob.display='block'; ");
                writer.Write("} ");
                writer.Write("} ");
                writer.Write("else { ");
                writer.Write("ob.display='none'; ");
                writer.Write("} ");
                writer.Write("} ");
                writer.Write("function toggleSubMenu(objId) { ");
                writer.Write("for(i=1; i<10000; i++) { ");
                writer.Write("var ob = document.getElementById(objId.concat(i)); ");
                writer.Write("if(ob === null) { ");
                writer.Write("break; ");
                writer.Write("} ");
                writer.Write("if(ob.style.display === 'none') { ");
                writer.Write("try { ");
                writer.Write("ob.style.display='table-row'; ");
                writer.Write("} catch(ex) { ");
                writer.Write("ob.style.display='block'; ");
                writer.Write("} ");
                writer.Write("} ");
                writer.Write("else { ");
                writer.Write("ob.style.display='none'; ");
                writer.Write("} ");
                writer.Write("} ");
                writer.Write("} ");
                writer.Write("</script> ");
                writer.Write("</head> ");

                writer.Write(" <body> ");
                writer.Write("</br>");

                writer.Write("<table id='Logos'>");
                writer.Write("<colgroup>");
                writer.Write("<col style='width: 25%' />");
                writer.Write("<col style='width: 25%' />");
                writer.Write("<col style='width: 25%' />");
                writer.Write("<col style='width: 25%' />");
                writer.Write("</colgroup> ");
                writer.Write("<thead> ");


                writer.Write("<tr class='content'>");
                writer.Write("<th class ='Logos' colspan='2' >");
                writer.Write("<img align ='left' src= " + imgSrcClientLogo + "></img>");
                writer.Write("</th>");
                writer.Write("<th class = 'Logos' colspan='2' > ");
                writer.Write("<img align ='right' src= " + imgSrcCompanyLogo + "></img>");
                writer.Write("</th> ");
                writer.Write("</tr> ");
                writer.Write("</thead> ");
                writer.Write("</table> ");

                writer.Write("<table id='header'> ");
                writer.Write("<colgroup> ");
                writer.Write("<col style='width: 25%' /> ");
                writer.Write("<col style='width: 25%' /> ");
                writer.Write("<col style='width: 25%' /> ");
                writer.Write("<col style='width: 25%' /> ");
                writer.Write("</colgroup> ");
                writer.Write(" <thead> ");

                writer.Write("<tr class='heading'> ");
                writer.Write("<th colspan='4' style='font-family:Copperplate Gothic Bold; font-size:1.4em;'> ");
                writer.Write("**" + TestResult.strTestName[this.browserContext] + " Execution Results **");
                writer.Write("</th> ");
                writer.Write("</tr> ");
                writer.Write("<tr class='subheading'> ");
                writer.Write("<th>&nbsp;Date&nbsp;&&nbsp;Time&nbsp;:&nbsp;</th> ");

                writer.Write("<th>" + CReporter.dateTime() + "</th> ");
                writer.Write("<th>&nbsp;Iteration&nbsp;Mode&nbsp;:&nbsp;</th> ");
                writer.Write("<th>" + ReporterConstants.ITERAION_MODE + "</th> ");
                writer.Write("</tr> ");

                writer.Write("<tr class='subheading'> ");
                writer.Write("<th>Device&nbsp;Name&nbsp;:</th> ");
                writer.Write("<th>" + ReporterConstants.DEVICE_NAME + "</th> ");
                writer.Write("<th>Version&nbsp;:</th> ");
                writer.Write("<th>" + ReporterConstants.BROWSER_VERSION + "</th> ");
                writer.Write("</tr> ");

                writer.Write("<tr class='subheading'> ");
                writer.Write("<th>&nbsp;Browser&nbsp;:&nbsp;</th> ");
                writer.Write("<th>" + this.browserContext.BrowserName + "</th> ");
                writer.Write(" <th>&nbsp;Executed&nbsp;on&nbsp;:&nbsp;</th> ");
                writer.Write("<th>" + System.Net.Dns.GetHostName() + "</th> ");
                writer.Write("</tr> ");
                writer.Write("</thead> ");
                writer.Write("</table> ");

                writer.Write("<table id='main'> ");
                writer.Write("<colgroup> ");
                writer.Write("<col style='width: 5%' /> ");
                writer.Write("<col style='width: 26%' /> ");
                writer.Write("<col style='width: 51%' /> ");
                writer.Write("<col style='width: 8%' /> ");
                writer.Write("<col style='width: 10%' /> ");
                writer.Write("</colgroup> ");
                writer.Write("<thead> ");
                writer.Write("<tr class='heading'> ");
                writer.Write("<th>S.NO</th> ");
                writer.Write("<th>Steps</th> ");
                writer.Write("<th>Details</th> ");
                writer.Write("<th>Status</th> ");
                writer.Write("<th>Time</th> ");
                writer.Write("</tr> ");
                writer.Write("</thead> ");
                writer.Close();

                string strPackageName = TestResult.packageName[this.browserContext];
                string strTcName = TestResult.tc_name[this.browserContext];

                /*get test case status map*/
                //IDictionary<string, string> mapTestCaseStatus = TestResult.mapBrowserContextTestCaseRef[this.browserContext];
                IDictionary<string, string> mapTestCaseStatus = null;
                if (TestResult.mapBrowserContextTestCaseRef.ContainsKey(this.browserContext) == false)
                {
                    mapTestCaseStatus = new Dictionary<string, string>();
                    //mapTestCaseStatus[strPackageName + ":" + strTcName] = "status";
                    mapTestCaseStatus.Add(strPackageName + ":" + strTcName, "status");
                    TestResult.mapBrowserContextTestCaseRef.Add(this.browserContext, mapTestCaseStatus);
                }
                else
                {
                    mapTestCaseStatus = TestResult.mapBrowserContextTestCaseRef[this.browserContext];
                    mapTestCaseStatus.Add(strPackageName + ":" + strTcName, "status");
                    TestResult.mapBrowserContextTestCaseRef[this.browserContext] = mapTestCaseStatus;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                try
                {
                    writer.Dispose();
                    //writer.Flush();//ToDo: Causes Object Disposed Exception
                    writer.Close();
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// Closes the detailed report.
        /// </summary>
        public virtual void closeDetailedReport()
        {
            FileInfo file =
                new FileInfo(this.reportPath + System.IO.Path.DirectorySeparatorChar +
                             TestResult.strTestName[this.browserContext] + "_Results" + ".html");
            // "SummaryReport.html"
            /*+ TestResult.timeStamp*/
            StreamWriter writer = null;

            try
            {
                writer = new System.IO.StreamWriter(file.FullName, true);
                writer.Write("</table>");
                writer.Write("<table id='footer'>");
                writer.Write("<colgroup>");
                writer.Write("<col style='width: 25%' />");
                writer.Write("<col style='width: 25%' />");
                writer.Write("<col style='width: 25%' />");
                writer.Write("<col style='width: 25%' />");
                writer.Write("</colgroup>");
                writer.Write("<tfoot>");
                writer.Write("<tr class='heading'> ");
                writer.Write("<th colspan='4'>Execution Time In Seconds (Includes Report Creation Time) : " +
                             TestResult.executionTime[this.browserContext][TestResult.tc_name[this.browserContext]] +
                             "&nbsp;</th> ");
                writer.Write("</tr> ");
                writer.Write("<tr class='content'>");
                writer.Write("<td class='pass'>&nbsp;Steps Passed&nbsp;:</td>");
                writer.Write("<td class='pass'> " + TestResult.PassNum[this.browserContext] + "</td>");
                writer.Write("<td class='fail'>&nbsp;Steps Failed&nbsp;: </td>");
                writer.Write("<td class='fail'>" + TestResult.FailNum[this.browserContext] + "</td>");
                writer.Write("</tr>");
                writer.Close();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Closes the summary report.
        /// </summary>
        public virtual void closeSummaryReport()
        {
            FileInfo file =
                new FileInfo(this.reportPath + System.IO.Path.DirectorySeparatorChar + "SummaryResults" + ".html");
            // "SummaryReport.html"
            /*+ TestResult.timeStamp*/
            StreamWriter writer = null;
            try
            {
                //get pass/fail test cases count
                int passTestCasesCount = this.GetTotalPassTestCases();
                int failTestCasesCount = this.GetTotalFailTestCases();


                writer = new System.IO.StreamWriter(file.FullName, true);

                writer.Write("<table id='footer'>");
                writer.Write("<colgroup>");
                writer.Write("<col style='width: 25%' />");
                writer.Write("<col style='width: 25%' />");
                writer.Write("<col style='width: 25%' />");
                writer.Write("<col style='width: 25%' /> ");
                writer.Write("</colgroup> ");
                writer.Write("<tfoot>");
                writer.Write("<tr class='heading'>");
                writer.Write("<th colspan='4'>Total Duration  In Mins (Including Report Creation) : " +
                             this.GetTestExecutionTimeInMins() + "</th>");
                writer.Write("</tr>");
                writer.Write("<tr class='content'>");
                writer.Write("<td class='pass'>&nbsp;Tests Passed&nbsp;:</td>");


                //entry for pass test cases count
                writer.Write("<td class='pass'> " + passTestCasesCount + "</td> ");
                writer.Write("<td class='fail'>&nbsp;Tests Failed&nbsp;:</td>");

                //entry for fail test cases count
                writer.Write("<td class='fail'> " + failTestCasesCount + "</td> ");
                writer.Write("</tr>");
                writer.Write("</tfoot>");
                writer.Write("</table> ");


                writer.Flush();
                writer.Close();
            }
            catch (Exception)
            {
                writer.Flush();
                writer.Close();
            }
        }

        /// <summary>
        /// Calculates the test case start time.
        /// </summary>
        public void calculateTestCaseStartTime()
        {
            if (TestResult.iStartTime.ContainsKey(this.browserContext))
            {
                TestResult.iStartTime[this.browserContext] = DateTimeHelperClass.CurrentUnixTimeMillis();
            }
            else
            {
                TestResult.iStartTime.Add(this.browserContext, DateTimeHelperClass.CurrentUnixTimeMillis());
            }
        }

        public virtual void calculateTestCaseExecutionTime()
        {
            if (TestResult.iEndTime.ContainsKey(this.browserContext))
            {
                TestResult.iEndTime[this.browserContext] = DateTimeHelperClass.CurrentUnixTimeMillis();
            }
            else
            {
                TestResult.iEndTime.Add(this.browserContext, DateTimeHelperClass.CurrentUnixTimeMillis());
            }

            long? iExecutionTimeValue = TestResult.iEndTime[this.browserContext] -
                                        TestResult.iStartTime[this.browserContext];
            if (TestResult.iExecutionTime.ContainsKey(this.browserContext))
            {
                TestResult.iExecutionTime[this.browserContext] = iExecutionTimeValue;
            }
            else
            {
                TestResult.iExecutionTime.Add(this.browserContext, iExecutionTimeValue);
            }

            long execTimeInSecs = (long)(TestResult.iExecutionTime[this.browserContext] * 0.001);
            string testCaseName = TestResult.tc_name[this.browserContext];
            //IDictionary<string, string> mapTCExecTime = TestResult.executionTime[this.browserContext];
            IDictionary<string, string> mapTCExecTime = null;
            if (TestResult.executionTime.ContainsKey(this.browserContext) == false)
            {
                mapTCExecTime = new Dictionary<string, string>();
                mapTCExecTime.Add(testCaseName, Convert.ToString(execTimeInSecs));
                if (TestResult.endStepTime.ContainsKey(this.browserContext))
                {
                    TestResult.executionTime[this.browserContext] = mapTCExecTime;
                }
                else
                {
                    TestResult.executionTime.Add(this.browserContext, mapTCExecTime);
                }
            }
            else
            {
                mapTCExecTime = TestResult.executionTime[this.browserContext];
                mapTCExecTime[testCaseName] = Convert.ToString(execTimeInSecs);
                //mapTCExecTime.Add(testCaseName, Convert.ToString(execTimeInSecs));
                TestResult.executionTime[this.browserContext] = mapTCExecTime;
            }
        }

        /// <summary>
        /// Calculates the suite start time.
        /// </summary>
        public virtual void calculateSuiteStartTime()
        {
            TestResult.iSuiteStartTime.Add(this.browserContext, DateTimeHelperClass.CurrentUnixTimeMillis());
            //Newly added
        }

        /// <summary>
        /// Calculates the suite execution time.
        /// </summary>
        public virtual void calculateSuiteExecutionTime()
        {
            TestResult.iSuiteEndTime.Add(this.browserContext, DateTimeHelperClass.CurrentUnixTimeMillis());
            //Newly added
            double dblSuiteexecTime =
                (double)
                    ((TestResult.iSuiteEndTime[this.browserContext] - TestResult.iSuiteStartTime[this.browserContext]) /
                     1000.000);
            double? DoubleSuiteExecTime = new double?(dblSuiteexecTime);
            TestResult.iSuiteExecutionTime.Add(this.browserContext, DoubleSuiteExecTime);
        }

        /// <summary>
        /// Successes the report.
        /// </summary>
        /// <param name="strStepName">Name of the string step.</param>
        /// <param name="strStepDes">The string step DES.</param>
        public virtual void SuccessReport(string strStepName, string strStepDes)
        {
            switch (ReporterConstants.REPORT_FORMAT.ToLower())
            {
                case "html":
                    /*take screen shot if Screenshot is required for passed test cases*/
                    if (ReporterConstants.BOOLEAN_ONSUCCESS_SCREENSHOT == true)
                    {
                    }
                    this.onSuccess(strStepName, strStepDes);
                    break;

                default:
                    /*take screen shot if Screenshot is required for passed test cases*/
                    if (ReporterConstants.BOOLEAN_ONSUCCESS_SCREENSHOT == true)
                    {
                        /*WebDriver webDriver = WebDriverFactory.getWebDriver(null, this.testContext.getCurrentXmlTest().getParameter("browser"));
                        ActionEngine.screenShot(webDriver , testUtil.filePath()+"/"+"Screenshots"+"/"
                                + strStepDes.replace(" ", "_") + "_"
                                + TestEngine.timeStamp + ".jpeg");*/
                    }
                    this.onSuccess(strStepName, strStepDes);
                    break;
            }
        }

        /// <summary>
        /// Failures the report.
        /// </summary>
        /// <param name="strStepName">Name of the string step.</param>
        /// <param name="strStepDes">The string step DES.</param>
        /// <param name="webDrivers">The web drivers.</param>
        public virtual void failureReport(string strStepName, string strStepDes, params IWebDriver[] webDrivers)
        {
            switch (ReporterConstants.REPORT_FORMAT.ToLower())
            {
                case "html":

                    string reportDescription = strStepDes;
                    //// New Screen shot code to avoid overriding \\\\			
                    strStepDes = strStepDes.Replace(":", "_");
                    strStepDes = strStepDes.Replace(",", "_");
                    strStepDes = strStepDes.Replace("&", "_");
                    strStepDes = strStepDes.Replace(" ", "_");
                    strStepDes = strStepDes.Replace("'", "_");
                    /*strStepDes customized for DirectWorks*/
                    strStepDes = "Image_" + Guid.NewGuid().ToString();
                    /**/
                    string fileName = this.reportPath + System.IO.Path.DirectorySeparatorChar +
                                      ReporterConstants.FOLDER_SCREENRSHOTS + System.IO.Path.DirectorySeparatorChar +
                                      strStepDes + ".jpeg";

                    fileName = makeUniqueImagePath(fileName);

                    /*Logic to take screenshot*/
                    /*WebDriver webDriver = WebDriverFactory.getWebDriver(null, this.testContext.getCurrentXmlTest().getParameter("browser"));
                    ActionEngine.screenShot(webDriver, fileName+".jpeg");*/
                    if (webDrivers != null)
                    {
                        foreach (IWebDriver webDriver in webDrivers)
                        {
                            this.screenShot(webDriver, fileName);
                            break;
                        }
                    }
                    else //taking screenshot using visual studio capability
                    { 
                        FileUtilities.TakeScreenShot(fileName); 
                    }

                    this.onFailure(strStepName, reportDescription, fileName);
                    //New Screenshot code Starts//
                    //if(!screenShotSource.equalsIgnoreCase("Selenium"))
                    //	screenShotSource = "Selenium";
                    //New Screenshot code ends//
                    break;

                default:

                    //// New Screen shot code to avoid overriding \\\\

                    fileName = this.reportPath + System.IO.Path.DirectorySeparatorChar +
                               ReporterConstants.FOLDER_SCREENRSHOTS + System.IO.Path.DirectorySeparatorChar +
                               strStepDes.Replace(" ", "_");
                    fileName = makeUniqueImagePath(fileName);

                    /*Logic to take screenshot*/
                    /*WebDriver webDriver = WebDriverFactory.getWebDriver(null, this.testContext.getCurrentXmlTest().getParameter("browser"));
                    ActionEngine.screenShot(webDriver, fileName+".jpeg");*/


                    this.onFailure(strStepName, strStepDes, fileName + ".jpeg");
                    //New Screenshot code Starts//
                    //if(!screenShotSource.equalsIgnoreCase("Selenium"))
                    //	screenShotSource = "Selenium";
                    //New Screenshot code ends//
                    break;
            }
        }


        /// <summary>
        /// Warnings the report.
        /// </summary>
        /// <param name="strStepName">Name of the string step.</param>
        /// <param name="strStepDes">The string step DES.</param>
        public virtual void warningReport(string strStepName, string strStepDes)
        {
            switch (ReporterConstants.REPORT_FORMAT.ToLower())
            {
                case "html":
                    /*logic to take screen shot*/
                    /*WebDriver webDriver = WebDriverFactory.getWebDriver(null, this.testContext.getCurrentXmlTest().getParameter("browser"));
                    ActionEngine.screenShot(webDriver,testUtil.filePath()+"/"+"Screenshots"+"/"
                            + strStepDes.replace(" ", "_") + "_" + TestEngine.timeStamp
                            + ".jpeg");	*/

                    this.onWarning(strStepName, strStepDes);
                    break;

                default:
                    /*logic to take screen shot*/
                    /*WebDriver webDriver = WebDriverFactory.getWebDriver(null, this.testContext.getCurrentXmlTest().getParameter("browser"));
                    ActionEngine.screenShot(webDriver,testUtil.filePath()+"/"+"Screenshots"+"/"
                            + strStepDes.replace(" ", "_") + "_" + TestEngine.timeStamp
                            + ".jpeg");	*/

                    this.onWarning(strStepName, strStepDes);
                    break;
            }
        }


        /// <summary>
        /// Initializes the test case description.
        /// </summary>
        /// <param name="testCaseDescription">The test case description.</param>
        public virtual void initTestCaseDescription(string testCaseDescription)
        {
            //if (testCaseDescription != null)
            //{
            //    IDictionary<string, string> mapTestDescription = TestResult.testDescription[this.BrowserContext];
            //    if (mapTestDescription == null)
            //    {
            //        mapTestDescription = new Dictionary<string, string>();

            //    }
            //    mapTestDescription.Add(TestResult.tc_name[this.browserContext],testCaseDescription);
            //    TestResult.testDescription.Add(this.browserContext, mapTestDescription);
            //}

            if (testCaseDescription != null)
            {
                IDictionary<string, string> mapTestDescription = null;
                if (TestResult.testDescription.ContainsKey(this.browserContext) == false)
                {
                    mapTestDescription = new Dictionary<string, string>();
                    mapTestDescription.Add(TestResult.tc_name[this.browserContext], testCaseDescription);
                    TestResult.testDescription.Add(this.browserContext, mapTestDescription);
                }
                else
                {
                    mapTestDescription = TestResult.testDescription[this.BrowserContext];
                    mapTestDescription.Add(TestResult.tc_name[this.browserContext], testCaseDescription);
                    TestResult.testDescription[this.browserContext] = mapTestDescription;
                }
            }
        }

        /// <summary>
        /// Initializes the test case.
        /// </summary>
        /// <param name="packageName">Name of the package.</param>
        /// <param name="testCaseName">Name of the test case.</param>
        /// <param name="testCaseDescription">The test case description.</param>
        /// <param name="appendTestCaseResult">if set to <c>true</c> [append test case result].</param>
        public virtual void initTestCase(string packageName, string testCaseName, string testCaseDescription,
            bool appendTestCaseResult)
        {
            if (TestResult.tc_name.ContainsKey(this.browserContext))
            {
                TestResult.tc_name[this.browserContext] = testCaseName + "-";
            }
            else
            {
                TestResult.tc_name.Add(this.browserContext, testCaseName + "-");
            }

            if (TestResult.packageName.ContainsKey(this.browserContext))
            {
                TestResult.packageName[this.browserContext] = packageName;
            }
            else
            {
                TestResult.packageName.Add(this.browserContext, packageName);
            }

            this.testHeader(TestResult.tc_name[this.browserContext], appendTestCaseResult);

            /*stepNum*/
            if (TestResult.stepNum.ContainsKey(this.browserContext))
            {
                TestResult.stepNum[this.browserContext] = 0;
            }
            else
            {
                TestResult.stepNum.Add(browserContext, 0);
            }

            /*PassNum*/
            if (TestResult.PassNum.ContainsKey(this.browserContext))
            {
                TestResult.PassNum[this.browserContext] = 0;
            }
            else
            {
                TestResult.PassNum.Add(browserContext, 0);
            }

            /*PassNum*/
            if (TestResult.FailNum.ContainsKey(this.browserContext))
            {
                TestResult.FailNum[this.browserContext] = 0;
            }
            else
            {
                TestResult.FailNum.Add(browserContext, 0);
            }

            if (TestResult.testName.ContainsKey(this.browserContext))
            {
                TestResult.testName[this.browserContext] = testCaseName;
            }
            else
            {
                TestResult.testName.Add(browserContext, testCaseName);
            }

            this.calculateTestCaseStartTime();
            if (testCaseDescription != null)
            {
                this.initTestCaseDescription(testCaseDescription);
            }
        }

        /// <summary>
        /// Updates the test case status.
        /// </summary>
        public virtual void updateTestCaseStatus()
        {
            if (TestResult.FailNum[this.browserContext] != 0)
            {
                int? failCount = 0;
                if (TestResult.failCounter.ContainsKey(this.browserContext) == false)
                {
                    failCount = 1;
                }
                else
                {
                    failCount = TestResult.failCounter[this.browserContext] + 1;
                }
                //int? failCount = TestResult.failCounter[this.browserContext] == null ? 1 : TestResult.failCounter[this.browserContext] + 1;
                
                 TestResult.failCounter[this.browserContext] = failCount;
                

                IDictionary<string, string> mapResult = null;
                if (TestResult.testResults.ContainsKey(this.browserContext) == false)
                {
                    mapResult = new Dictionary<string, string>();
                }
                //IDictionary<string, string> mapResult = TestResult.testResults[this.browserContext];
                else
                {
                    mapResult = TestResult.testResults[this.browserContext];
                }
                mapResult[TestResult.tc_name[this.browserContext]] = ReporterConstants.TEST_CASE_STATUS_FAIL;
                
                    TestResult.testResults[this.browserContext] = mapResult;
                
            }
            else
            {
                int? passCount = 0;
                if (TestResult.passCounter.ContainsKey(this.browserContext) == false)
                {
                    passCount = 1;
                }
                else
                {
                    passCount = TestResult.passCounter[this.browserContext] + 1;
                }
                //int? passCount = TestResult.passCounter[this.browserContext] == null ? 1 : TestResult.passCounter[this.browserContext] + 1;
                
                TestResult.passCounter[this.browserContext] = passCount;
               

                IDictionary<string, string> mapResult = null;
                if (TestResult.testResults.ContainsKey(this.browserContext) == false)
                {
                    mapResult = new Dictionary<string, string>();
                }
                else
                {
                    mapResult = TestResult.testResults[this.browserContext];
                }
               
                  mapResult[TestResult.tc_name[this.browserContext]] = ReporterConstants.TEST_CASE_STATUS_PASS;
               

                
                 TestResult.testResults[this.browserContext] = mapResult;
               
            }
            
        }

        //////////////

        internal static class DateTimeHelperClass
        {
            private static readonly System.DateTime Jan1st1970 = new System.DateTime(1970, 1, 1, 0, 0, 0,
                System.DateTimeKind.Utc);

            internal static long CurrentUnixTimeMillis()
            {
                return (long)(System.DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
            }
        }

        public int GetTotalPassTestCases()
        {
            int? passTestCasesCount = 0;

            /*passCounter*/
            if (TestResult.passCounter.ContainsKey(this.browserContext))
            {
                passTestCasesCount = TestResult.passCounter[this.browserContext];
            }

            return passTestCasesCount.Value;
        }
        public int GetTotalFailTestCases()
        {
            int? failTestCasesCount = 0;

            /*passCounter*/
            if (TestResult.failCounter.ContainsKey(this.browserContext))
            {
                failTestCasesCount = TestResult.failCounter[this.browserContext];
            }

            return failTestCasesCount.Value;
        }

        public int GetTestExecutionTimeInSecs()
        {
            return (int)((double)TestResult.iSuiteExecutionTime[this.browserContext]);
        }

        public int GetTestExecutionTimeInMins()
        {
            int executionTimeInSecs = this.GetTestExecutionTimeInSecs();

            int executionTimeInMins = (int)(Math.Ceiling((double)(executionTimeInSecs / 60)));
            return executionTimeInMins;
        }

        public List<TestCase> GetAllTestCases()
        {
            try
            {
                IDictionary<string, string> testCaseRef = TestResult.mapBrowserContextTestCaseRef[this.browserContext];
                
                allTestCases = new List<TestCase>();
                IEnumerator<KeyValuePair<string, string>> iterator1 = testCaseRef.GetEnumerator();

                while (iterator1.MoveNext())
                {
                    TestCase testcase = new TestCase();
                    if (!(iterator1.Current.Key.Contains("Send Mail")))
                    {
                        KeyValuePair<string, string> mapEntry1 = (KeyValuePair<string, string>)iterator1.Current;

                        LOG.Info("Key of mapEntry1 : " + mapEntry1.Key);
                        this.package_testname = mapEntry1.Key.Split(':');
                        LOG.Info("package is present in package_testname[0] : " + this.package_testname[0]);
                        LOG.Info("test method is present in package_testname[1] : " + this.package_testname[1]);
                        string testCaseExecutionStatus = (string)mapEntry1.Value;
                        LOG.Info("value against package_testname is : " + testCaseExecutionStatus);

                        testcase.TESTCASENAME = this.package_testname[1].ToString();
                        testcase.TESTCASEDESCRIPTION = this.package_testname[1].ToString();
                        testcase.TESTCASESTATUS = TestResult.testResults[this.browserContext][this.package_testname[1]].ToString();
                        testcase.TESTCASEEXECUTIONTIME = Convert.ToInt32(TestResult.executionTime[this.browserContext][this.package_testname[1]]);
                        testcase.TESTCASEEXECUTIONTIMEUNIT = "Seconds";
                        allTestCases.Add(testcase);
                    }
                    testcase = null;

                }
                return CReporter.allTestCases;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public List<TestCase> GetFailedTestCases()
        {
            try
            {
                IDictionary<string, string> testCaseRef = TestResult.mapBrowserContextTestCaseRef[this.browserContext];
                
                failedTestCases = new List<TestCase>();

                IEnumerator<KeyValuePair<string, string>> iterator1 = testCaseRef.GetEnumerator();

                while (iterator1.MoveNext())
                {
                    TestCase testcase = new TestCase();

                    if (!(iterator1.Current.Key.Contains("Send Mail")))
                    {
                        KeyValuePair<string, string> mapEntry1 = (KeyValuePair<string, string>)iterator1.Current;
                        LOG.Info("Key of mapEntry1 : " + mapEntry1.Key);
                        this.package_testname = mapEntry1.Key.Split(':');
                        LOG.Info("package is present in package_testname[0] : " + this.package_testname[0]);
                        LOG.Info("test method is present in package_testname[1] : " + this.package_testname[1]);
                        string testCaseExecutionStatus = (string)mapEntry1.Value;
                        LOG.Info("value against package_testname is : " + testCaseExecutionStatus);

                        if (TestResult.testResults[this.browserContext][this.package_testname[1]].Equals(ReporterConstants.TEST_CASE_STATUS_FAIL))
                        {
                            testcase.TESTCASENAME = this.package_testname[1].ToString();
                            testcase.TESTCASEDESCRIPTION = this.package_testname[1].ToString();
                            testcase.TESTCASESTATUS = TestResult.testResults[this.browserContext][this.package_testname[1]].ToString();
                            testcase.TESTCASEEXECUTIONTIME = Convert.ToInt32(TestResult.executionTime[this.browserContext][this.package_testname[1]]);
                            testcase.TESTCASEEXECUTIONTIMEUNIT = "Seconds";
                            testcase.TESTCASEREPORTLINK = String.Format(@"{0}\{1}",this.filePath(), this.package_testname[1] + "_Results.html");
                            failedTestCases.Add(testcase);
                        }
                        testcase = null;
                    }
                }


                return CReporter.failedTestCases;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }

    internal static class StringHelperClass
    {
        //----------------------------------------------------------------------------------
        //	This method replaces the Java String.substring method when 'start' is a
        //	method call or calculated value to ensure that 'start' is obtained just once.
        //----------------------------------------------------------------------------------
        internal static string SubstringSpecial(this string self, int start, int end)
        {
            return self.Substring(start, end - start);
        }

        //------------------------------------------------------------------------------------
        //	This method is used to replace calls to the 2-arg Java String.startsWith method.
        //------------------------------------------------------------------------------------
        internal static bool StartsWith(this string self, string prefix, int toffset)
        {
            return self.IndexOf(prefix, toffset, System.StringComparison.Ordinal) == toffset;
        }

        //------------------------------------------------------------------------------
        //	This method is used to replace most calls to the Java String.split method.
        //------------------------------------------------------------------------------
        internal static string[] Split(this string self, string regexDelimiter, bool trimTrailingEmptyStrings)
        {
            string[] splitArray = System.Text.RegularExpressions.Regex.Split(self, regexDelimiter);

            if (trimTrailingEmptyStrings)
            {
                if (splitArray.Length > 1)
                {
                    for (int i = splitArray.Length; i > 0; i--)
                    {
                        if (splitArray[i - 1].Length > 0)
                        {
                            if (i < splitArray.Length)
                                System.Array.Resize(ref splitArray, i);

                            break;
                        }
                    }
                }
            }

            return splitArray;
        }

        //-----------------------------------------------------------------------------
        //	These methods are used to replace calls to some Java String constructors.
        //-----------------------------------------------------------------------------
        internal static string NewString(sbyte[] bytes)
        {
            return NewString(bytes, 0, bytes.Length);
        }

        internal static string NewString(sbyte[] bytes, int index, int count)
        {
            return System.Text.Encoding.UTF8.GetString((byte[])(object)bytes, index, count);
        }

        internal static string NewString(sbyte[] bytes, string encoding)
        {
            return NewString(bytes, 0, bytes.Length, encoding);
        }

        internal static string NewString(sbyte[] bytes, int index, int count, string encoding)
        {
            return System.Text.Encoding.GetEncoding(encoding).GetString((byte[])(object)bytes, index, count);
        }

        //--------------------------------------------------------------------------------
        //	These methods are used to replace calls to the Java String.getBytes methods.
        //--------------------------------------------------------------------------------
        internal static sbyte[] GetBytes(this string self)
        {
            return GetSBytesForEncoding(System.Text.Encoding.UTF8, self);
        }

        internal static sbyte[] GetBytes(this string self, string encoding)
        {
            return GetSBytesForEncoding(System.Text.Encoding.GetEncoding(encoding), self);
        }

        private static sbyte[] GetSBytesForEncoding(System.Text.Encoding encoding, string s)
        {
            sbyte[] sbytes = new sbyte[encoding.GetByteCount(s)];
            encoding.GetBytes(s, 0, s.Length, (byte[])(object)sbytes, 0);
            return sbytes;
        }
    }
}
