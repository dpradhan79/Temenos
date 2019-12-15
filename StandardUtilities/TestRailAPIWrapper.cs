using Gurock.TestRail;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TestRail;
using TestRail.Types;

namespace StandardUtilities
{
   public class TestRailAPIWrapper
    {
        private static APIClient client;
        public const int result_PASS = 1;
        public const int result_FAIL = 5;
        public const int result_BLOCKED = 2;
        public const int result_RETEST = 4;

        public TestRailAPIWrapper(string serverUrl, string loginEmail, string loginPassword)
        {
            client = new APIClient(serverUrl);
            client.User = loginEmail;
            client.Password = loginPassword;
        }

        public void AddStatusToTest(long runid, long caseid, int statusId, String comments)
        {
            Hashtable data = new Hashtable();
           // data.Add
            data.Add("status_id", statusId);
            data.Add("comment", comments);
            JObject r = (JObject)client.SendPost("add_result_for_case/" + runid + "/" + caseid, data);
            
        }
        /*****Utilities Developed Using Zoosk Test Rail Client Similar to testrail-api-client In Java By PPadial********/
        public static void UpdateTestCaseStatus(TestRailClient trClient, int runId, String testCaseTitle, ResultStatus resultStatus)
        {
            try
            {
                TestRail.Types.Run run = trClient.GetRun((ulong)runId);
                List<TestRail.Types.Case> listTestCases = trClient.GetCases((ulong)run.ProjectID, (ulong)run.SuiteID);
                List<TestRail.Types.Test> listTests = trClient.GetTests((ulong)runId);
                //get all test cases from Project/Suite Level
                foreach (TestRail.Types.Case testCase in listTestCases)
                {
                    bool isTestFound = false;
                    Console.WriteLine(String.Format("Test Case Title = {0}, Id = {1}", testCase.Title, testCase.ID));
                    if (testCase.Title.Equals(testCaseTitle, StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (TestRail.Types.Test test in listTests)
                        {
                            if (test.CaseID == testCase.ID)
                            {
                                //Got The The Test Case To Be Update
                                isTestFound = true;
                                Console.WriteLine(String.Format("Test Case Title = {0}, Test Id = {1}, Case Id = {2}", test.Title, test.ID, test.CaseID));
                                CommandResult<ulong> cmdResult = trClient.AddResultForCase((ulong)runId, (ulong)test.CaseID, resultStatus);
                                Console.WriteLine("Test Case Updated Was With Status - ", cmdResult.WasSuccessful);
                                Console.WriteLine("Test Case Updated With Result - {0}", resultStatus);
                                Console.WriteLine("Result - ", cmdResult.Value);
                                break;
                            }

                        }
                    }
                    if (isTestFound == true)
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
    }


