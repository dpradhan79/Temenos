using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMTFactory;
namespace TCMFactory
{
    public class TestCase
    {
        private String testCategory = null;
        private String testCaseId = null;
        private String testCaseName = null;
        private String testCaseDescription = null;       
        private ResultStatus iTestExecutionStatus;
        private ulong iTestExecutionTimeinMS = 0;


        public String TestCategory
        {
            get { return this.testCategory; }
            set { this.testCategory = value; }
        }

        public String TestCaseId
        {
            get { return this.testCaseId; }
            set { this.testCaseId = value; }
        }

        public String TestCaseName
        {
            get { return this.testCaseName; }
            set { this.testCaseName = value; }
        }

        public String TestCaseDescription
        {
            get { return this.testCaseDescription; }
            set { this.testCaseDescription = value; }
        }

        public ResultStatus TestExecutionStatus
        {
            get { return this.iTestExecutionStatus; }
            set { this.iTestExecutionStatus = value; }
        }
             
        public String convertTestExecutionStatusToString()
        {
            String testStatus = null;
            switch(this.TestExecutionStatus)
            {
                case ResultStatus.Passed :
                    testStatus = "Passed";
                    break;

                case ResultStatus.Failed :
                    testStatus = "Failed";
                    break;
            }

            return testStatus;
        }
    }
}
