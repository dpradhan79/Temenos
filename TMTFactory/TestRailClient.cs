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

        public void UpdateTestCaseStatus(int runId, String testCaseTitle, ResultStatus executionStatus, String comment = null)
        {
            JArray tests = (JArray)apiClient.SendGet("get_tests/" + runId);
            foreach (JObject test in tests)
            {
                if ((test["title"].ToString().Equals(testCaseTitle, StringComparison.OrdinalIgnoreCase)))
                {
                    Dictionary<object, object> resultStatus = new Dictionary<object, object>();
                    resultStatus.Add("status_id", executionStatus);
                    resultStatus.Add("comment", comment);

                    JObject resp = (JObject)apiClient.SendPost(String.Format("add_result_for_case/{0}/{1}", runId, test["case_id"]), resultStatus);
                    Console.WriteLine(resp);
                }
            }
        }
    }
}
