using Gurock.TestRail;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
    }
    }


