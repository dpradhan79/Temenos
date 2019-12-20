using System.Collections.Generic;

namespace CignitiReporter
{
    /// <summary>
    /// @Author - Debasish Pradhan
    /// </summary>
    public class TestResult
    {
        /* copied from TestEngine*/
        public static IDictionary<BrowserContext, int?> stepNum = new Dictionary<BrowserContext, int?>();
        public static IDictionary<BrowserContext, int?> PassNum = new Dictionary<BrowserContext, int?>();
        public static IDictionary<BrowserContext, int?> FailNum = new Dictionary<BrowserContext, int?>();
        public static IDictionary<BrowserContext, int?> passCounter = new Dictionary<BrowserContext, int?>();
        public static IDictionary<BrowserContext, int?> failCounter = new Dictionary<BrowserContext, int?>();
        public static IDictionary<BrowserContext, string> testName = new Dictionary<BrowserContext, string>();

        public static IDictionary<BrowserContext, string> testCaseExecutionTime =
            new Dictionary<BrowserContext, string>();

        /*testDescription contains BrowserContext as key , Map<testpackage:testname,testDescription> as value*/

        public static IDictionary<BrowserContext, IDictionary<string, string>> testDescription =
            new Dictionary<BrowserContext, IDictionary<string, string>>();

        public static IDictionary<BrowserContext, IDictionary<string, string>> testResults =
            new Dictionary<BrowserContext, IDictionary<string, string>>();

        /*copied from HtmlReportSupport*/

        public static IDictionary<BrowserContext, long?> iStartTime = new Dictionary<BrowserContext, long?>();
        public static IDictionary<BrowserContext, long?> iEndTime = new Dictionary<BrowserContext, long?>();
        public static IDictionary<BrowserContext, long?> iExecutionTime = new Dictionary<BrowserContext, long?>();
        public static IDictionary<BrowserContext, long?> iSuiteStartTime = new Dictionary<BrowserContext, long?>();
        public static IDictionary<BrowserContext, long?> iSuiteEndTime = new Dictionary<BrowserContext, long?>();

        public static IDictionary<BrowserContext, double?> iSuiteExecutionTime =
            new Dictionary<BrowserContext, double?>();

        public List<double?> list = new List<double?>();
        public static IDictionary<BrowserContext, long?> startStepTime = new Dictionary<BrowserContext, long?>();
        public static IDictionary<BrowserContext, long?> endStepTime = new Dictionary<BrowserContext, long?>();
        public static IDictionary<BrowserContext, double?> stepExecutionTime = new Dictionary<BrowserContext, double?>();
        public static IDictionary<BrowserContext, string> strTestName = new Dictionary<BrowserContext, string>();
        public static IDictionary<BrowserContext, string> startedAt = new Dictionary<BrowserContext, string>();
        public static IDictionary<BrowserContext, string> tc_name = new Dictionary<BrowserContext, string>();
        public static IDictionary<BrowserContext, string> packageName = new Dictionary<BrowserContext, string>();
        /*mapBrowserContextTestCaseRef contains BrowserContext as key , Map<testpackage:testname,status> as value*/

        public static IDictionary<BrowserContext, IDictionary<string, string>> mapBrowserContextTestCaseRef =
            new Dictionary<BrowserContext, IDictionary<string, string>>();

        public static IDictionary<BrowserContext, IDictionary<string, string>> executionTime =
            new Dictionary<BrowserContext, IDictionary<string, string>>();

        public string currentSuite = "";
        public int pCount = 0;
        public int fCount = 0;


        //public static string workingDir = System.getProperty("user.dir").replace(File.separator, "/");
        public static IDictionary<BrowserContext, int?> BFunctionNo = new Dictionary<BrowserContext, int?>();
    }
}