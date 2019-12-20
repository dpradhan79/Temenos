using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;
using TestReporter;
using Engine.Setup;
using OpenQA.Selenium;
using StandardUtilities;
using Engine.Factories;
using AUT.Selenium.ApplicationSpecific.Pages;
using System.Data;
using System.Linq;
using TMTFactory;
namespace AutomatedTest.FunctionalTests
{
    [TestClass]
    public class TestBaseTemplate
    {
        private string dataFileName = null;
        private int currentFileRowPointer = 1;
        public static IWebDriver driver = null;

        #region PageObject
        public AddApplication Application = null;
        public TestRailClient testRail = null;
        public LoginPage LoginPage = null;
        public HomePage HomePage = null;
        public DecisionProcessAutomation DecisionProcessAutomation = null;
        public LoanTermsAutomation LoanTermsAutomation = null;
        public PrimaryApplicantAutomationScreen PrimaryApplicantAutomationScreen = null;
        public StipulationsAutomation StipulationsAutomation = null;
        public Disburse Disburse = null;
        #endregion

        /// <summary>
        /// Input Test Data
        /// </summary>
        protected Dictionary<string, string> validationTestData = new Dictionary<String, String>();

        public TestContext TestContext { get; set; }

        public TestBaseTemplate()
        {
            const String testRailUrl = "http://atllmstestrail.akcelerant.com/";
            const String userName = "kote@cigniti.com";
            const String password = "Password1";
            
            testRail = new TestRailClient(testRailUrl, userName, password);
            Application = new AddApplication();
            LoginPage = new LoginPage();
            HomePage = new HomePage();
            PrimaryApplicantAutomationScreen = new PrimaryApplicantAutomationScreen();
            DecisionProcessAutomation = new DecisionProcessAutomation();
            LoanTermsAutomation = new LoanTermsAutomation();
            StipulationsAutomation = new StipulationsAutomation();
            Disburse = new Disburse();
        }

        [AssemblyInitialize]
        public static void BeforeAllTestsExecution(TestContext testContext)
        {
           
            #region WebApplication - Launch
            //EngineSetup.TestReport.InitTestCase("Launch Application", "Verify Application Is Launched Successfully");
            driver = WebDriverFactory.getWebDriver(EngineSetup.BROWSER);
            driver.Navigate().GoToUrl(EngineSetup.WEBURL);
            //EngineSetup.TestReport.LogSuccess(String.Format("Launch Application On Browser - {0}",EngineSetup.BROWSER), String.Format("Application - {0} Launch Successful", EngineSetup.WEBURL));
            //EngineSetup.TestReport.UpdateTestCaseStatus();

            #endregion
        }
        [AssemblyCleanup]
        public static void AfterAllTestsExecution()
        {

            //after execution, update extent report with gallop logo 
            /*driver can not be initialized in static method as driver is instance variable*/           
            WebDriverFactory.getWebDriver().Close();
            WebDriverFactory.getWebDriver().Quit();
            WebDriverFactory.Free();
            EngineSetup.TestReport.Close();
            TestBaseTemplate.UpdateTestReport();
            
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
            this.LoadBusinessTestData();
            this.TESTREPORT.InitTestCase(TestContext.TestName, TestContext.Properties["title"] as String);
            LoginPage.SignIn(EngineSetup.UserName, EngineSetup.Password);

        }

        ////Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void AfterEachTestCaseExecution()
        {
            
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
            testDataFileName = (testCategory == testDataPath) ? this.TESTDATAFILENAME : testScriptName + ".xlsx";
            DataTable inputTestDataTable = ExcelReader.ReadExcelFile(testDataPath, testDataFileName, sheetName, false, 2);
            DataRow[] testDataRows = inputTestDataTable.Select("TestScriptName = '" + testScriptName + "'");
            inputTestData = (testDataRows.Length > 0) ? testDataRows[0].Table.Columns.Cast<DataColumn>().ToDictionary(c => c.ColumnName, c => testDataRows[0][c].ToString()) : inputTestData;
            return inputTestData;
        }

        protected void LoadBusinessTestData()
        {
            this.validationTestData = this.loadValidationTestData(TestContext.TestName);
        }

    }
}
