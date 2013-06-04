using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace APITest.Controllers
{
    public class WidgetsController : Controller
    {
        //
        // GET: /Widgets/

        /*private JToken HLGetRequest(string tenantKey, string authToken, string url)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.connectedcommunity.org/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("HLTenantKey", tenantKey);
            client.DefaultRequestHeaders.Add("HLAuthToken", authToken);

            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                return null;
            }
        }*/

        private string getTenantKey(string domain)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.connectedcommunity.org/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync("api/v1.0/Authentication/GetTenantDetail?communityUrl=" + domain).Result;
            if (response.IsSuccessStatusCode)
            {
                JObject responseJO = (JObject) (JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result).First);
                return responseJO.First.First.ToString();
            }
            return null;
        }

        private string getAuthToken(string username, string password, string tenantKey)
        {
            string url = "api/v1.0/Authentication/Login";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.connectedcommunity.org/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("HLTenantKey", tenantKey);

            //JObject results = null;

            //HttpResponseMessage response = client.PostAsJsonAsync(url, new { Username = username, Password = password }).Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    results = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);
            //    return (string) results.Property("Token").Value;
            //}
            //return null;
            return "ofMS0rORUoGdJrY6qENdaOtpPSqxex9+q8ao+NpVIpBq3j/wRN8qdtgbzNii2wXga3Hotxvlc8Gx1ZlBvqdNCrkVEWWnxqLqBuWH4eqqvjJZLm/581M58Pt5PR/Zp7A1";
        }

        // Need to update when Steve adds new API for get all discussions
        private JArray GetLatestDiscussions(string tenantKey, string authToken)
        {
            string discussionKey = "076fc118-1caf-4bc5-a486-1d27beb12859";
            string url = "api/v1.0/Discussions/GetDiscussionPosts";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.connectedcommunity.org/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("HLTenantKey", tenantKey);
            client.DefaultRequestHeaders.Add("HLAuthToken", authToken);

            JArray results = null;

            HttpResponseMessage response = client.PostAsJsonAsync(url, new { MaxContentLength = 10, DiscussionKeyFilter = discussionKey, MaxNumberToRetrieve = 3, MaxSubjectLength = 4 }).Result;
            if (response.IsSuccessStatusCode)
            {
                var a = response.Content.ReadAsStringAsync().Result;
                results = (JArray) JsonConvert.DeserializeObject<dynamic>(a);
            }
            return results;
        }

        public ActionResult Index()
        {
            //ViewData["tenantKey"] = getTenantKey("https://hug.higherlogic.com/");
            //ViewData["authToken"] = getAuthToken("jrennert@higherlogic.com", "HigherLogic1234", (string) ViewData["tenantKey"]);
            ViewData["tenantKey"] = "b9d338a8-16fa-41a0-9aab-48640fea1458";
            ViewData["authToken"] = "ofMS0rORUoGdJrY6qENdaOtpPSqxex9+q8ao+NpVIpA9L0y4zle0FLZNpfgLkJrEKqkhp1TSAKVMIxSS4lwNNaes4v7/sShWfqOYZi94i1VYk3h7Rd2h7aapL1+vo/hH";
            return View();
        }

        public ActionResult LatestDiscussions(string tenantKey, string authToken)
        {
            ViewData["tenantKey"] = tenantKey;
            ViewData["authToken"] = authToken;
            JArray discussionPosts = GetLatestDiscussions(tenantKey, authToken);
            ViewData["discussionPosts"] = discussionPosts;
            return View();
        }

        public ActionResult SubscribedDiscussions()
        {
            return View();
        }
    }
}
