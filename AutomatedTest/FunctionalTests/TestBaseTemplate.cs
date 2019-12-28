using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestReporter;
using Engine.Setup;
using OpenQA.Selenium;
using StandardUtilities;
using Engine.Factories;
using System.Data;
using System.Linq;
using TMTFactory;
using TCMFactory;
using AUT.Selenium.ApplicationSpecific.Pages;
namespace AutomatedTest.FunctionalTests
{
    [TestClass]
    public class TestBaseTemplate
    {
        private String dataFileName = null;
        private int currentFileRowPointer = 1;
        protected static IWebDriver driver = null;
        protected Exception testException = null;
        protected static IList<TestCase> listTestCases = new List<TestCase>();

        #region PageObject
        public ApplicantPage Applicant = null;
        public TestRailClient testRail = null;
        public LoginPage LoginPage = null;
        public HomePage HomePage = null;
        public DecisionProcessAutomationPage DecisionProcessAutomation = null;
        public LoanTermsAutomationPage LoanTermsAutomation = null;
        public PrimaryApplicantAutomationPage PrimaryApplicantAutomationScreen = null;
        public StipulationsAutomationPage StipulationsAutomation = null;
        public DisbursePage Disburse = null;
        public TemenosBasePage TemenosBasePage = null;        
        #endregion

        /// <summary>
        /// Input Test Data
        /// </summary>
        protected Dictionary<string, string> validationTestData = new Dictionary<String, String>();       

        public TestContext TestContext { get; set; }
        
        public TestBaseTemplate()
        {        

            testRail = new TestRailClient(EngineSetup.TESTRAILURL, EngineSetup.TESTRAILUSERNAME, EngineSetup.TESTRAILPASSWORD);
            EngineSetup.TESTRAILRUNID = (int)testRail.GetRunID(EngineSetup.TESTRAILPROJECTNAME, EngineSetup.TESTRAILRUNNAME);
            Applicant = new ApplicantPage();
            LoginPage = new LoginPage();
            HomePage = new HomePage();
            PrimaryApplicantAutomationScreen = new PrimaryApplicantAutomationPage();
            DecisionProcessAutomation = new DecisionProcessAutomationPage();
            LoanTermsAutomation = new LoanTermsAutomationPage();
            StipulationsAutomation = new StipulationsAutomationPage();
            Disburse = new DisbursePage();
            TemenosBasePage = new TemenosBasePage();           
        }

        [AssemblyInitialize]
        public static void BeforeAllTestsExecution(TestContext testContext)
        {
            /****TODO - MultiThreading ****/
            #region WebApplication - Launch
            driver = WebDriverFactory.getWebDriver(EngineSetup.BROWSER);
            driver.Navigate().GoToUrl(EngineSetup.WEBURL);
            #endregion
        }
        [AssemblyCleanup]
        public static void AfterAllTestsExecution()
        {
            /****TODO - MultiThreading ****/
            //after execution, update extent report with gallop logo 
            /*driver can not be initialized in static method as driver is instance variable*/
            WebDriverFactory.getWebDriver().Close();
            WebDriverFactory.getWebDriver().Quit();
            WebDriverFactory.Free();
            EngineSetup.TestReport.Close();
            TestBaseTemplate.UpdateTestReport();
            if (EngineSetup.ISMAILREQUIRED.Equals("Yes", StringComparison.OrdinalIgnoreCase))
            {
                String emailBody = EmailSender.CreateHtmlBodyForMail(listTestCases);
                EmailSender.SendEmail(EngineSetup.SMTPSERVER, EngineSetup.SMTPPORT, EngineSetup.EMAILFROM, EngineSetup.EMAILTOLIST, null, EngineSetup.EMAILSUBJECT, emailBody, null, true);
            }
            
        }
        [ClassInitialize]
        public static void BeforeAllTestsInATestClassExecution(TestContext testContext)
        {

        }
        [ClassCleanup]
        public static void AfterAllTestsInATestClassExecution()
        {

        }

        ////Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void BeforeEachTestCaseExecution()
        {
            /****TODO - MultiThreading ****/
            //#region WebApplication - Launch
            //driver = WebDriverFactory.getWebDriver(EngineSetup.BROWSER);
            //driver.Navigate().GoToUrl(EngineSetup.WEBURL);
            //#endregion

            this.testException = null;
            
            this.TESTREPORT.InitTestCase(TestContext.TestName, TestContext.Properties["title"] as String);
            this.LoadBusinessTestData();
            LoginPage.SignIn(EngineSetup.APPUSERNAME, EngineSetup.APPPASSWORD);
        }

        ////Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void AfterEachTestCaseExecution()
        {
            /****TODO - MultiThreading ****/
            //#region Close WebDriver
            //WebDriverFactory.getWebDriver().Close();
            //WebDriverFactory.getWebDriver().Quit();
            //WebDriverFactory.Free();
            //#endregion
            TestCase testCase = new TestCase();
            testCase.TestCaseName = TestContext.TestName;
            testCase.TestCaseDescription = TestContext.Properties["title"] as String;
            
            switch(TestContext.CurrentTestOutcome)
            {
                case UnitTestOutcome.Passed:
                    testCase.TestExecutionStatus = ResultStatus.Passed;
                    testCase.TestExecutionResultMsg = "Test Execution Completed Successfully";
                    this.TESTREPORT.LogInfo("Test Execution Completed Successfully");
                    this.TESTREPORT.UpdateTestCaseStatus();
                    this.testRail.UpdateTestCaseStatus(EngineSetup.TESTRAILRUNID, TestContext.Properties["title"] as String, TestContext.CurrentTestOutcome.ToString(), testCase.TestExecutionResultMsg);                    
                    break;

                case UnitTestOutcome.Failed:
                    testCase.TestExecutionStatus = ResultStatus.Failed;
                    testCase.TestExecutionResultMsg = this.testException.Message;
                    this.TESTREPORT.LogFailure("Failed", "Exception Encountered - " + this.testException);
                    this.TESTREPORT.UpdateTestCaseStatus();
                    this.testRail.UpdateTestCaseStatus(EngineSetup.TESTRAILRUNID, TestContext.Properties["title"] as String, TestContext.CurrentTestOutcome.ToString(), this.testException.Message);                    
                    break;

                default:
                    testCase.TestExecutionStatus = ResultStatus.Failed;
                    testCase.TestExecutionResultMsg = this.testException.Message;
                    this.TESTREPORT.LogFailure("Failed", "Exception Encountered - " + this.testException);
                    this.TESTREPORT.UpdateTestCaseStatus();
                    this.testRail.UpdateTestCaseStatus(EngineSetup.TESTRAILRUNID, TestContext.Properties["title"] as String, TestContext.CurrentTestOutcome.ToString(), this.testException.Message);
                    break;

            }
            listTestCases.Add(testCase);
            
        }


        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>

        protected string RANDVALUE
        {
            get { return EngineSetup.GetRandomValue(); }

        }

        protected string DATAFILENAME
        {
            get { this.dataFileName = String.Format("{0}{1}", this.GetType().Name, ".csv"); return this.dataFileName; }

        }

        protected int DATAFILEROWPOINTER
        {
            get { return this.currentFileRowPointer; }
            set { this.currentFileRowPointer = value; }
        }

        protected IReporter TESTREPORT
        {
            get { return EngineSetup.TestReport; }
        }

        protected string SCREENSHOTFILE
        {
            get { return EngineSetup.GetScreenShotPath(); }
        }
        /*To update extentReport to have Gallop Logo*/
        protected static void UpdateTestReport()
        {
            /*Dictionary should contain
            * SourceFile
            * Text1ToBeReplaced
            * Text1ToReplace
            * Text2ToBeReplaced
            * Text2ToReplace 
            * Text3ToBeReplaced
            * Text3ToReplace 
         
            */
            Dictionary<string, string> keyValuePair = new Dictionary<string, string>();
            if (EngineSetup.TestReport is ExtentReporter)
            {

                keyValuePair["SourceFile"] = EngineSetup.TestReportFileName;

                //Text1ToBeReplaced
                string str = "<div class='logo-container'>\r\n                                    <a class='logo-content' href='http://extentreports.relevantcodes.com'>\r\n                                        <span>ExtentReports</span>\r\n                                    </a>\r\n                                    <a href='#' data-activates='slide-out' class='button-collapse hide-on-large-only'><i class='mdi-navigation-apps'></i></a>\r\n                                </div>";


                keyValuePair["Text1ToBeReplaced"] = str;


                //Text1ToReplace
                str = "<div class='logo-container' style='height:50px;width:200px;'>\r\n                                    <a href='http://www.cigniti.com/'>\r\n                                        <img border='2' alt='Cigniti' src='cigniti_logo.png' width='200' height='35'>\r\n                                    </a>\r\n                                    <a href='#' data-activates='slide-out' class='button-collapse hide-on-large-only'><i class='mdi-navigation-apps'></i></a>\r\n                                </div>";

                keyValuePair["Text1ToReplace"] = str;


                //Text2ToBeReplaced
                str = "<span class='report-name'>";
                keyValuePair["Text2ToBeReplaced"] = str;

                //Text2ToReplace
                str = "<span class='report-name'><div style='width:220px;' align='right'>";
                keyValuePair["Text2ToReplace"] = str;

                //Text3ToBeReplaced
                str = "</span> <span class='report-headline'>";
                keyValuePair["Text3ToBeReplaced"] = str;

                //Text3ToReplace
                str = "</div></span> <span class='report-headline'>";
                keyValuePair["Text3ToReplace"] = str;
            }
            ITestReportManipulator updateExtentReport = new ExtentReportManipulator(keyValuePair);
            EngineSetup.TestReport.ManipulateTestReport(updateExtentReport);

        }

        protected string readCSV(string columnName)
        {
            string data = "";
            try
            {
                data = FileUtilities.GetCSVData(this.DATAFILENAME,
                                         columnName.Trim(), this.DATAFILEROWPOINTER);
                data = data.Replace("{random}", this.RANDVALUE);
            }
            catch (Exception ex)
            {
                this.TESTREPORT.LogFailure("readCSV", ex.Message);
            }
            return data;
        }

        protected string TESTDATAFILENAME
        {
            get
            {
                string testDataFileName = EngineSetup.TestDataFileName + ".xlsx";
                Console.WriteLine("testDataFileName : " + testDataFileName);
                return testDataFileName;
            }
        }

        protected string TESTDATAFOLDERPATH
        {
            get
            {
                string testDataFolderPath = EngineSetup.ApplicationPath + "\\";
                Console.WriteLine("testDataFolderPath " + testDataFolderPath);
                return testDataFolderPath;
            }
        }

        protected Dictionary<string, string> loadValidationTestData(string testScriptName, string testCategory="")
        {
            return LoadTestData(testScriptName, testCategory, EngineSetup.VALIDATIONTESTDATASHEETNAME);
        }

        protected Dictionary<string, string> LoadTestData(string testScriptName, string testCategory, string sheetName)
        {
            String testDataPath = System.IO.Directory.GetCurrentDirectory();
            Dictionary<string, string> inputTestData = new Dictionary<String, String>();
            var testDataFileName = (string)null;
            testDataFileName = (testCategory == testDataPath) ? this.TESTDATAFILENAME : testScriptName.Replace("Test","").Trim() + ".xlsx";
            DataTable inputTestDataTable = ExcelReader.ReadExcelFile(testDataPath, testDataFileName, sheetName, false, 2);
            DataRow[] testDataRows = inputTestDataTable.Select("TestScriptName = '" + testScriptName.Replace("Test", "").Trim() + "'");
            inputTestData = (testDataRows.Length > 0) ? testDataRows[0].Table.Columns.Cast<DataColumn>().ToDictionary(c => c.ColumnName, c => testDataRows[0][c].ToString()) : inputTestData;
            return inputTestData;
        }

        protected void LoadBusinessTestData()
        {
            this.validationTestData = this.loadValidationTestData(TestContext.TestName);
        }

    }
}
