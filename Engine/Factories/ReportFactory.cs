using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestReporter;
using Engine.Setup;
using System.IO;
namespace Engine.Factories
{
    /// <summary>
    /// @Author - Debasish Pradhan
    /// </summary>
    public class ReportFactory
    {
        private static IReporter report = null;
        /// <summary>
        /// Gets the report.
        /// </summary>
        /// <param name="reportType">Type of the report.</param>
        /// <param name="replaceExisting">if set to <c>true</c> [replace existing].</param>
        /// <param name="isGallopLogoRequired">if set to <c>true</c> [is gallop logo required].</param>
        /// <returns></returns>
        public static IReporter GetReport(string reportType = "ExtentReport", bool replaceExisting = true, bool isGallopLogoRequired = true)
        {
            switch(reportType.ToLower())
            {
                
                case "gallopreport":
                    report = new CignitiReporter();
                    break;
                case "extentreport":
                
                default:
                    
                    report = new ExtentReporter(EngineSetup.TestReportFileName, replaceExisting, isGallopLogoRequired);                    
                    break;
                   
            }
            return report;
        }
        
    }
}
