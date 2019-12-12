using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GallopReporter
{
    /// <summary>
    /// @Author - Debasish Pradhan
    /// </summary>
    public class TestCase
    {
        private string testCaseName = null;
        private string testCaseDescription = null;
        private string testCaseExcutionTimeUnit = null;
        private int testCaseExecutionTime = 0;
        private string testCaseStatus = null;
        private string testCaseReportLink = null;

        /// <summary>
        /// Gets or sets the testcasename.
        /// </summary>
        /// <value>
        /// The testcasename.
        /// </value>
        public string TESTCASENAME
        {
            get { return this.testCaseName; }
            set { this.testCaseName = value; }
        }

        /// <summary>
        /// Gets or sets the testcasedescription.
        /// </summary>
        /// <value>
        /// The testcasedescription.
        /// </value>
        public string TESTCASEDESCRIPTION
        {
            get { return this.testCaseDescription; }
            set { this.testCaseDescription = value; }
        }
        /// <summary>
        /// Gets or sets the testcaseexecutiontimeunit.
        /// </summary>
        /// <value>
        /// The testcaseexecutiontimeunit.
        /// </value>
        public string TESTCASEEXECUTIONTIMEUNIT
        {
            get { return this.testCaseExcutionTimeUnit; }
            set { this.testCaseExcutionTimeUnit = value; }
        }
        /// <summary>
        /// Gets or sets the testcaseexecutiontime.
        /// </summary>
        /// <value>
        /// The testcaseexecutiontime.
        /// </value>
        public int TESTCASEEXECUTIONTIME
        {
            get { return this.testCaseExecutionTime; }
            set { this.testCaseExecutionTime = value; }
        }
        /// <summary>
        /// Gets or sets the testcasestatus.
        /// </summary>
        /// <value>
        /// The testcasestatus.
        /// </value>
        public string TESTCASESTATUS
        {
            get { return this.testCaseStatus; }
            set { this.testCaseStatus = value; }
        }
        /// <summary>
        /// Gets or sets the testcasereportlink.
        /// </summary>
        /// <value>
        /// The testcasereportlink.
        /// </value>
        public string TESTCASEREPORTLINK
        {
            get { return this.testCaseReportLink; }
            set { this.testCaseReportLink = value; }
        }

    }
}
