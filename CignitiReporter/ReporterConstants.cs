using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Reflection;
using StandardUtilities;
namespace CignitiReporter
{
    /// <summary>
    /// @Author - Debasish Pradhan
    /// </summary>
    public static class ReporterConstants
    {
        public static string GetReporterFile()
        {
            //string startupPath = System.IO.Directory.GetCurrentDirectory();
            //string configReporterFile = Path.Combine(startupPath, "Configurations") + "\\Reporter.properties";
            string configReporterFile = "Reporter.properties";
            return configReporterFile;
        }

        public const string TEST_CASE_STATUS_PASS = "PASS";
        public const string TEST_CASE_STATUS_FAIL = "FAIL";

        public static readonly string CLIENT_NAME = FileUtilities.readPropertyFile(GetReporterFile(),
            "clientName");

        public static readonly string DEVICE_NAME = FileUtilities.readPropertyFile(GetReporterFile(),
            "deviceName");

        public static readonly string ITERAION_MODE = FileUtilities.readPropertyFile(GetReporterFile(),
            "iterationMode");

        public static readonly string SUITE_NAME = FileUtilities.readPropertyFile(GetReporterFile(),
            "suiteName");

        public static readonly string ON_ERROR_ACTION = FileUtilities.readPropertyFile(GetReporterFile(),
            "onErrorAction");

        public static readonly bool? BOOLEAN_SINGLE_THREAD =
            bool.Parse(FileUtilities.readPropertyFile(GetReporterFile(), "singleThreadExecution"));

        public static readonly string APP_UNDER_TEST = FileUtilities.readPropertyFile(GetReporterFile(),
            "AUT");

        public static readonly string APP_BASE_URL = FileUtilities.readPropertyFile(GetReporterFile(),
            "baseUrl");

        public static readonly string BROWSER_NAME = FileUtilities.readPropertyFile(GetReporterFile(),
            "browserName");

        public static readonly string BROWSER_VERSION = FileUtilities.readPropertyFile(GetReporterFile(),
            "version");

        public static readonly string BROWSER_PLATFORM = FileUtilities.readPropertyFile(GetReporterFile(),
            "platform");

        public static readonly string REPORT_FORMAT = FileUtilities.readPropertyFile(GetReporterFile(),
            "reportFormat");
        public static readonly string EXECUTION_MODE = FileUtilities.readPropertyFile(GetReporterFile(),
            "executionMode");
        public static readonly string LOCATION_RESULT = FileUtilities.readPropertyFile(GetReporterFile(),
            "executionMode").Equals("Remote",StringComparison.OrdinalIgnoreCase) ? FileUtilities.readPropertyFile(GetReporterFile(),
            "locationResultRemote") : FileUtilities.readPropertyFile(GetReporterFile(),
            "locationResultLocal");

        public static readonly string FOLDER_SCREENRSHOTS =
            FileUtilities.readPropertyFile(GetReporterFile(), "folderScreenShot");

        public static readonly string LOCATION_CLIENT_LOGO =
            FileUtilities.readPropertyFile(GetReporterFile(), "locationClientLogo");

        public static readonly string LOCATION_COMPANY_LOGO =
            FileUtilities.readPropertyFile(GetReporterFile(), "locationCompanyLogo");

        public static readonly string LOCATION_FAILED_LOGO =
            FileUtilities.readPropertyFile(GetReporterFile(), "locationFailedLogo");

        public static readonly string LOCATION_MINUS_LOGO =
            FileUtilities.readPropertyFile(GetReporterFile(), "locationMinusLogo");

        public static readonly string LOCATION_PASSED_LOGO =
            FileUtilities.readPropertyFile(GetReporterFile(), "locationPassedLogo");

        public static readonly string LOCATION_PLUS_LOGO = FileUtilities.readPropertyFile(
            GetReporterFile(), "locationPlusLogo");

        public static readonly string LOCATION_WARNING_LOGO =
            FileUtilities.readPropertyFile(GetReporterFile(), "locationWarningLogo");

        public static readonly bool? BOOLEAN_ONSUCCESS_SCREENSHOT =
            bool.Parse(FileUtilities.readPropertyFile(GetReporterFile(), "onSuccessScreenshot"));
       
    }
}