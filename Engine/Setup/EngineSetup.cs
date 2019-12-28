using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestReporter;
using System.IO;
using Engine.Factories;

namespace Engine.Setup
{
    /// <summary>
    /// @Author - Debasish Pradhan
    /// </summary>
    public class EngineSetup
    {
        private static string randomString = null;
        private const string FILETESTCONFIGURATION = "TestConfiguration.properties";
        public const string VALIDATIONTESTDATASHEETNAME = "ValidationTestData";
        private static string executablePath = StandardUtilities.FileUtilities.readPropertyFile(FILETESTCONFIGURATION, "testexecutablePath");

        #region Reporting Configuration

        private static string reportType = StandardUtilities.FileUtilities.readPropertyFile(FILETESTCONFIGURATION, "reportType"); 
        private static string testReportFile = StandardUtilities.FileUtilities.readPropertyFile(FILETESTCONFIGURATION, "testReportFile");
        private static IReporter testReportInternal = null;
        private static string screenShotFolder = new FileInfo(testReportFile).Directory.FullName + Path.DirectorySeparatorChar + "ScreenShots";
        private static int lastScreenShotCount = 1;
        
        #endregion

        #region framework configuration
        private static string browser = StandardUtilities.FileUtilities.readPropertyFile(FILETESTCONFIGURATION, "browser");
        private static int defaultTimeOutForSelenium = Int32.Parse(StandardUtilities.FileUtilities.readPropertyFile(FILETESTCONFIGURATION, "seleniumDefaultTimeOut"));
        public const int TimeOutConstant = 180;
        private static string webUrl = StandardUtilities.FileUtilities.readPropertyFile(FILETESTCONFIGURATION, "webUrl");
        private static string userName = StandardUtilities.FileUtilities.readPropertyFile(FILETESTCONFIGURATION, "userName");
        private static string password = StandardUtilities.FileUtilities.readPropertyFile(FILETESTCONFIGURATION, "password");
        private static string testDataFileName = StandardUtilities.FileUtilities.readPropertyFile(FILETESTCONFIGURATION, "testDataFileName");
        private static int testRailRunId = Convert.ToInt16(StandardUtilities.FileUtilities.readPropertyFile(FILETESTCONFIGURATION, "runId"));
        private static string projectName = StandardUtilities.FileUtilities.readPropertyFile(FILETESTCONFIGURATION, "projectName");
        private static string runName = StandardUtilities.FileUtilities.readPropertyFile(FILETESTCONFIGURATION, "runName");
        private static string testRailURL = StandardUtilities.FileUtilities.readPropertyFile(FILETESTCONFIGURATION, "testRailURL");
        private static string testRailUserName = StandardUtilities.FileUtilities.readPropertyFile(FILETESTCONFIGURATION, "testRailUserName");
        private static string testRailPassword = StandardUtilities.FileUtilities.readPropertyFile(FILETESTCONFIGURATION, "testRailPassword");
        private static string testRailPasswordFromJenkins = Environment.GetEnvironmentVariable("testrailPassword");
        #endregion

        #region Mail Configuration

        private static String sendMail = StandardUtilities.FileUtilities.readPropertyFile(FILETESTCONFIGURATION, "sendMail");
        private static String attachExecutionResult = StandardUtilities.FileUtilities.readPropertyFile(FILETESTCONFIGURATION, "attachExecutionResult");
        
        private static String smtpServer = StandardUtilities.FileUtilities.readPropertyFile(FILETESTCONFIGURATION, "smtpServer");
        private static int smtpPort = Convert.ToInt16(StandardUtilities.FileUtilities.readPropertyFile(FILETESTCONFIGURATION, "smtpPort"));
        private static String emailFrom = StandardUtilities.FileUtilities.readPropertyFile(FILETESTCONFIGURATION, "emailFrom");
        private static String mailToListSeparatedByComma = StandardUtilities.FileUtilities.readPropertyFile(FILETESTCONFIGURATION, "mailToListSeparatedByComma");
        private static String mailSubject = StandardUtilities.FileUtilities.readPropertyFile(FILETESTCONFIGURATION, "mailSubject");

        #endregion
        /// <summary>
        /// Initializes the <see cref="EngineSetup"/> class.
        /// </summary>
        static EngineSetup()
        {
            if (Directory.Exists(screenShotFolder))
            {
                Directory.Delete(screenShotFolder, true);
            }
        }
        /// <summary>
        /// Gets the random value.
        /// </summary>
        /// <returns></returns>
        public static string GetRandomValue()
        {
            if(EngineSetup.randomString == null)
            {
                EngineSetup.randomString = String.Format("{0}{1}", Environment.MachineName, DateTime.Now.ToString("ddmmssff"));
            }
            return EngineSetup.randomString;
        }

        /// <summary>
        /// Gets or sets the application path.
        /// </summary>
        /// <value>
        /// The application path.
        /// </value>
        public static string ApplicationPath
        {
            get { return new FileInfo(EngineSetup.executablePath).FullName; }
            set { EngineSetup.executablePath = value; }
           
        }
        /// <summary>
        /// Gets or sets the type of the test report.
        /// </summary>
        /// <value>
        /// The type of the test report.
        /// </value>
        public static string TestReportType
        {
            get { return EngineSetup.reportType; }
            set { EngineSetup.reportType = value; }
        }
        /// <summary>
        /// Gets or sets the name of the test report file.
        /// </summary>
        /// <value>
        /// The name of the test report file.
        /// </value>
        public static string TestReportFileName
        {
            get { return new FileInfo(EngineSetup.testReportFile).FullName; }
            set { EngineSetup.testReportFile = value; }
        }

        /// <summary>
        /// Gets or sets the test screen shot folder.
        /// </summary>
        /// <value>
        /// The test screen shot folder.
        /// </value>
        public static string TestScreenShotFolder
        {
            get { return EngineSetup.screenShotFolder; }
            set { EngineSetup.screenShotFolder = value; }
        }
        /// <summary>
        /// Gets the test report.
        /// </summary>
        /// <value>
        /// The test report.
        /// </value>
        public static IReporter TestReport
        {
            get
            {
                if(EngineSetup.testReportInternal == null)
                {

                    EngineSetup.testReportInternal = ReportFactory.GetReport(EngineSetup.TestReportType,true, true);
                }
                return EngineSetup.testReportInternal;
            }
        }

        /// <summary>
        /// Test Data File Name
        /// </summary>
        public static string TestDataFileName
        {
            get
            {
                return EngineSetup.testDataFileName;
            }
            set
            {
                EngineSetup.testDataFileName = value;
            }
        }

        /// <summary>
        /// Gets the screen shot path.
        /// </summary>
        /// <returns></returns>
        public static string GetScreenShotPath()
        {
            string fileName = String.Format("{0}{1}ScreenShot.jpeg", EngineSetup.TestScreenShotFolder, Path.DirectorySeparatorChar);
            try
            {
                //Verifying if the file already exists, if so append the numbers 1,2  so on to the fine name.			

                FileInfo myPngImage = new FileInfo(fileName);
                if(!myPngImage.Directory.Exists)
                {
                    myPngImage.Directory.Create();
                }
                
                while (myPngImage.Exists)
                {
                    try
                    {
                        string tempFileName = null;
                        if (fileName.Contains("_"))
                        {
                           tempFileName = fileName.Substring(0, fileName.IndexOf('_'));
                           
                        }
                        else
                        {
                            tempFileName = fileName.Substring(0, fileName.IndexOf(".jpeg"));
                        }
                        fileName = tempFileName;
                        fileName = String.Format("{0}_{1}.jpeg", fileName, EngineSetup.lastScreenShotCount++);
                        myPngImage = new FileInfo(fileName);
                    }
                    catch(Exception ex)
                    {
                        EngineSetup.TestReport.LogException(ex, EngineSetup.GetScreenShotPath());
                    }
                    
                   
                }
                return fileName;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
                return fileName;
            }
        }

        /// <summary>
        /// Gets the default timeout for selenium.
        /// </summary>
        /// <value>
        /// The default timeout for selenium.
        /// </value>
        public static int DefaultTimeoutForSelenium
        {
            get
            {
                return EngineSetup.defaultTimeOutForSelenium;
            }
        }

        /// <summary>
        /// Gets or sets the browser.
        /// </summary>
        /// <value>
        /// The browser.
        /// </value>
        public static string BROWSER
        {
            get
            {
               //environment variable will be read in case of Jenkins parameterized build execution
                return Environment.GetEnvironmentVariable("browser") !=null ? Environment.GetEnvironmentVariable("browser") : EngineSetup.browser;
            }
            set { EngineSetup.browser = value; }
        }

        /// <summary>
        /// Gets or sets the weburl.
        /// </summary>
        /// <value>
        /// The weburl.
        /// </value>
        public static string WEBURL
        {
            get { return Environment.GetEnvironmentVariable("webUrl") != null ? Environment.GetEnvironmentVariable("webUrl") : EngineSetup.webUrl; }
            set { EngineSetup.webUrl = value;}
        }

        /// <summary>
        /// User Name
        /// </summary>
        public static string UserName
        {
            get { return Environment.GetEnvironmentVariable("userName") != null ? Environment.GetEnvironmentVariable("userName") : EngineSetup.userName; }
            set { EngineSetup.userName = value; }
        }



        /// <summary>
        /// User Password
        /// </summary>
        public static string Password
        {
            get { return Environment.GetEnvironmentVariable("password") != null ? Environment.GetEnvironmentVariable("password") : EngineSetup.password; }
            set { EngineSetup.password = value; }
        }

        /// <summary>
        /// Gets or sets the Test Rail RunId.
        /// </summary>
        /// <value>
        /// RunId.
        /// </value>
        public static int TESTRAILRUNID
        {
            get
            {
                //environment variable will be read in case of Jenkins parameterized build execution
                return Environment.GetEnvironmentVariable("runId") != null ? Convert.ToInt16(Environment.GetEnvironmentVariable("runId")) : EngineSetup.testRailRunId;
            }
            set { EngineSetup.testRailRunId = value; }
        }

        /// <summary>
        /// Gets or sets the Mail Requirement.
        /// </summary>
        /// <value>
        /// sendMail.
        /// </value>
        public static String ISMAILREQUIRED
        {
            get { return Environment.GetEnvironmentVariable("sendMail") != null ? Environment.GetEnvironmentVariable("sendMail") : EngineSetup.sendMail; }
            set { EngineSetup.sendMail = value; }
        }

        /// <summary>
        /// Gets or sets the Attachment Requirement.
        /// </summary>
        /// <value>
        /// attachExecutionResult.
        /// </value>
        public static String ISATTACHMENTREQUIRED
        {
            get { return Environment.GetEnvironmentVariable("attachExecutionResult") != null ? Environment.GetEnvironmentVariable("attachExecutionResult") : EngineSetup.attachExecutionResult; }
            set { EngineSetup.attachExecutionResult = value; }
        }

        /// <summary>
        /// Gets or sets the smtpServer.
        /// </summary>
        /// <value>
        /// smtpServer.
        /// </value>
        public static String SMTPSERVER
        {
            get { return Environment.GetEnvironmentVariable("smtpServer") != null ? Environment.GetEnvironmentVariable("smtpServer") : EngineSetup.smtpServer; }
            set { EngineSetup.smtpServer = value; }
        }


        /// <summary>
        /// Gets or sets the smtpPort.
        /// </summary>
        /// <value>
        /// smtpPort.
        /// </value>
        public static int SMTPPORT
        {
            get { return Environment.GetEnvironmentVariable("smtpPort") != null ? Convert.ToInt16(Environment.GetEnvironmentVariable("smtpPort")) : EngineSetup.smtpPort; }
            set { EngineSetup.smtpPort = value; }
        }

        /// <summary>
        /// Gets or sets the emailFrom.
        /// </summary>
        /// <value>
        /// emailFrom.
        /// </value>
        public static String EMAILFROM
        {
            get { return Environment.GetEnvironmentVariable("emailFrom") != null ? Environment.GetEnvironmentVariable("emailFrom") : EngineSetup.emailFrom; }
            set { EngineSetup.emailFrom = value; }
        }

        /// <summary>
        /// Gets or sets the emailFrom.
        /// </summary>
        /// <value>
        /// emailFrom.
        /// </value>
        public static String EMAILTOLIST
        {
            get { return Environment.GetEnvironmentVariable("mailToListSeparatedByComma") != null ? Environment.GetEnvironmentVariable("mailToListSeparatedByComma") : EngineSetup.mailToListSeparatedByComma; }
            set { EngineSetup.mailToListSeparatedByComma = value; }
        }

        /// <summary>
        /// Gets or sets the emailFrom.
        /// </summary>
        /// <value>
        /// emailFrom.
        /// </value>
        public static String EMAILSUBJECT
        {
            get { return Environment.GetEnvironmentVariable("mailSubject") != null ? Environment.GetEnvironmentVariable("mailSubject") : EngineSetup.mailSubject; }
            set { EngineSetup.mailSubject = value; }
        }

        /// <summary>
        /// Gets or sets the projectname.
        /// </summary>
        /// <value>
        /// The projectname.
        /// </value>
        public static string PROJECTNAME
        {
            get
            {
                //environment variable will be read in case of Jenkins parameterized build execution
                return Environment.GetEnvironmentVariable("projectname") != null ? Environment.GetEnvironmentVariable("projectname") : EngineSetup.projectName;
            }
            set { EngineSetup.projectName = value; }
        }

        /// <summary>
        /// Gets or sets the runname.
        /// </summary>
        /// <value>
        /// The runname.
        /// </value>
        public static string RUNNAME
        {
            get
            {
                //environment variable will be read in case of Jenkins parameterized build execution
                return Environment.GetEnvironmentVariable("runname") != null ? Environment.GetEnvironmentVariable("runname") : EngineSetup.runName;
            }
            set { EngineSetup.runName = value; }
        }

        /// <summary>
        /// Gets or sets the testrailurl.
        /// </summary>
        /// <value>
        /// The runname.
        /// </value>
        public static String TESTRAILURL
        {
            get
            {
                //environment variable will be read in case of Jenkins parameterized build execution
                return Environment.GetEnvironmentVariable("testrailurl") != null ? Environment.GetEnvironmentVariable("testrailurl") : EngineSetup.testRailURL;
            }
            set { EngineSetup.testRailURL = value; }
        }

        /// <summary>
        /// Gets or sets the testrailusername.
        /// </summary>
        /// <value>
        /// The testrailusername.
        /// </value>
        public static String TESTRAILUSERNAME
        {
            get
            {
                //environment variable will be read in case of Jenkins parameterized build execution
                return Environment.GetEnvironmentVariable("testrailusername") != null ? Environment.GetEnvironmentVariable("testrailusername") : EngineSetup.testRailUserName;
            }
            set { EngineSetup.testRailUserName = value; }
        }

        public static String TESTRAILPASSWORD
        {
            get
            {
                //environment variable will be read in case of Jenkins parameterized build execution
                return Environment.GetEnvironmentVariable("testRailPassword") != null ? Environment.GetEnvironmentVariable("testRailPassword") : EngineSetup.testRailPassword;
            }
            set { EngineSetup.testRailPassword = value; }
            
        }

    }
}
