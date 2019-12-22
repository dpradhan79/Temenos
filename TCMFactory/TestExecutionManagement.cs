using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMTFactory;
namespace TCMFactory
{
    public class TestExecutionManagement
    {

        public static int GetTotalPassTestCases(IList<TestCase> listTestCases)
        {
            int totalPass = 0;
            foreach (TestCase tc in listTestCases)
            {
                if (tc.TestExecutionStatus == ResultStatus.Passed)
                {
                    totalPass++;
                }
            }

            return totalPass;
        }
        

        public static int GetTotalFailedTestCases(IList<TestCase> listTestCases)
        {
            int totalFail = 0;
            foreach (TestCase tc in listTestCases)
            {
                if (tc.TestExecutionStatus == ResultStatus.Failed)
                {
                    totalFail++;
                }
            }

            return totalFail;
        }

        public static IList<TestCase> GetFailedTestCases(IList<TestCase> listTestCases)
        {
            IList<TestCase> listFailedTestCases = new List<TestCase>();
            foreach (TestCase tc in listTestCases)
            {
                if (tc.TestExecutionStatus == ResultStatus.Failed)
                {
                    listFailedTestCases.Add(tc);
                }
            }

            return listFailedTestCases;
        }

        public static IList<TestCase> GetPassedTestCases(IList<TestCase> listTestCases)
        {
            IList<TestCase> listPassedTestCases = new List<TestCase>();
            foreach (TestCase tc in listTestCases)
            {
                if (tc.TestExecutionStatus == ResultStatus.Passed)
                {
                    listPassedTestCases.Add(tc);
                }
            }
            return listPassedTestCases;
        }
                
    }
}
