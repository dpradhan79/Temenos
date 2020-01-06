using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Gurock.TestRail;
namespace TMTFactory
{
    public class TestRailClient
    {
         APIClient apiClient = null;

        public TestRailClient(String testRailBaseURL, String userName, String password)
        {
            apiClient = new APIClient(testRailBaseURL);
            apiClient.User = userName;
            apiClient.Password = password;
        }

        public void UpdateTestCaseStatus<T>(int runId, String testCaseTitle, T executionStatus, String comment = null)
        {                       
            JArray tests = (JArray)apiClient.SendGet("get_tests/" + runId);
            foreach (JObject test in tests)
            {
                if ((test["title"].ToString().Equals(testCaseTitle, StringComparison.OrdinalIgnoreCase)))
                {
                    Dictionary<object, object> resultStatus = new Dictionary<object, object>();
                    //If executionStatus is provided in String convert, then convert to integer format
                    if (executionStatus is String)
                    {
                        if (executionStatus.ToString().Equals("Passed", StringComparison.OrdinalIgnoreCase))
                        {
                            resultStatus.Add("status_id", ResultStatus.Passed);
                        }
                        if (executionStatus.ToString().Equals("Failed", StringComparison.OrdinalIgnoreCase))
                        {
                            resultStatus.Add("status_id", ResultStatus.Failed);
                        }
                    }
                    else
                    {
                        resultStatus.Add("status_id", executionStatus);
                    }
                    resultStatus.Add("comment", comment);
                    JObject resp = (JObject)apiClient.SendPost(String.Format("add_result_for_case/{0}/{1}", runId, test["case_id"]), resultStatus);
                    Console.WriteLine(resp);
                    break;
                }
            }
        }

        public ulong GetProjectID(string projectName)
        {
            ulong projectID = 0;
            JArray protects = (JArray)apiClient.SendGet("get_projects");
            foreach (JObject test in protects)
            {
                if ((test["name"].ToString().Equals(projectName, StringComparison.OrdinalIgnoreCase)))
                {
                  string id = test["id"].ToString();
                  projectID = ulong.Parse(id);                   
                }                
            }
            return projectID;
        }

        public ulong GetSuiteID(string projectName,string suiteName)
        {
            ulong suiteID = 0;
            JArray suites = (JArray)apiClient.SendGet("get_suites/" + GetProjectID(projectName));
            foreach (JObject test in suites)
            {
                if ((test["name"].ToString().Equals(suiteName, StringComparison.OrdinalIgnoreCase)))
                {
                    string id = test["id"].ToString();
                    suiteID = ulong.Parse(id);
                }
            }
            return suiteID;
        }


        public ulong GetRunID(string projectName,string runName)
        {
            ulong runID = 0;
            JArray runNames = (JArray)apiClient.SendGet("get_runs/" + GetProjectID(projectName));
            foreach (JObject test in runNames)
            {
                if ((test["name"].ToString().Equals(runName, StringComparison.OrdinalIgnoreCase)))
                {
                    string id = test["id"].ToString();
                    runID = ulong.Parse(id);
                }
            }
            return runID;
        }
         
    }
}
