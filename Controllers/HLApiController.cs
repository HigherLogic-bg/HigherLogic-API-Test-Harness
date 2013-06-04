using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace APITest.Controllers
{
    public class HLApiController : Controller
    {
        //
        // GET: /HLApi/

        private JToken HLGetRequest(string tenantKey, string authToken, string url)
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
        }

        public ActionResult GetWhoAmI(string tenantKey, string authToken)
        {
            JObject results = (JObject) HLGetRequest(tenantKey, authToken, "api/v1.0/Contacts/GetWhoAmI");
            if (results != null)
            {
                ViewData["results"] = results;
                ViewData["whoAmI"] = JsonConvert.SerializeObject(results, Formatting.Indented);
                return View();
            }
            return RedirectToAction("UserSignIn", "Domain", new { preView = "GetWhoAmI" });
        }

        public ActionResult PostWhoAmI(string tenantKey, string authToken)
        {
            string url = "api/v1.0/Contacts/WhoAmI";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.connectedcommunity.org/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("HLTenantKey", tenantKey);
            client.DefaultRequestHeaders.Add("HLAuthToken", authToken);

            var content = new System.Net.Http.FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>()
            });

            JObject results = null;
            string whoAmI = null;

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                results = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);
                whoAmI = JsonConvert.SerializeObject(results, Formatting.Indented);
            }
            else {
                return RedirectToAction("UserSignIn", "Domain", new { preView = "PostWhoAmI" });
            }
            ViewData["results"] = results;
            ViewData["whoAmI"] = whoAmI;
            return View();
        }

        public ActionResult GetContactWithContactKey(string tenantKey, string authToken, string contactName)
        {
            JArray contactArray = (JArray)HLGetRequest(tenantKey, authToken, "api/v1.0/Contacts/GetMyContacts");
            List<string> contactList = new List<string>();
            
            foreach (JObject j in contactArray)
            {
                contactList.Add(j.Property("FirstName").Value + " " + j.Property("LastName").Value);
            }
            
            ViewData["contacts"] = contactList;
            JObject first = (JObject)contactArray.First;
            if (contactName == null)
            {
                ViewData["contactKey"] = first.Properties().ElementAt<JProperty>(2).Value;
            }
            else
            {
                int index = contactList.IndexOf(contactName);
                JObject contact;
                if (index == -1)
                {
                    contact = first;
                }
                else
                {
                    contact = (JObject)contactArray.ElementAt<JToken>(index);
                }
                ViewData["contactKey"] = contact.Properties().ElementAt<JProperty>(2).Value;
            }
            JObject results = (JObject)HLGetRequest(tenantKey, authToken, "api/v1.0/Contacts/GetContact?contactKey=" + ViewData["contactKey"]);
            if (results != null)
            {
                ViewData["results"] = results;
                ViewData["getContact"] = JsonConvert.SerializeObject(results, Formatting.Indented);
                return View();
            }
            return RedirectToAction("UserSignIn", "Domain", new { preView = "GetContactWithContactName" });
        }

        public ActionResult GetContactWithLegacyContactKey(string tenantKey, string authToken, string contactName)
        {

            JArray contactArray = (JArray)HLGetRequest(tenantKey, authToken, "api/v1.0/Contacts/GetMyContacts");
            List<string> contactList = new List<string>();

            foreach (JObject j in contactArray)
            {
                contactList.Add(j.Property("FirstName").Value + " " + j.Property("LastName").Value);
            }

            ViewData["contacts"] = contactList;
            JObject first = (JObject)contactArray.First;
            if (contactName == null)
            {
                ViewData["legacyContactKey"] = first.Properties().ElementAt<JProperty>(14).Value;
            }
            else
            {
                int index = contactList.IndexOf(contactName);
                JObject contact;
                if (index == -1)
                {
                    contact = first;
                }
                else
                {
                    contact = (JObject)contactArray.ElementAt<JToken>(index);
                }
                ViewData["legacyContactKey"] = contact.Properties().ElementAt<JProperty>(14).Value;
            }
            JObject results = (JObject)HLGetRequest(tenantKey, authToken, "api/v1.0/Contacts/GetContact?legacyContactKey=" + ViewData["legacyContactKey"]);
            if (results != null)
            {
                ViewData["results"] = results;
                ViewData["getContact"] = JsonConvert.SerializeObject(results, Formatting.Indented);
                return View();
            }
            return RedirectToAction("UserSignIn", "Domain", new { preView = "GetContactWithContactName" });

        }

        public ActionResult GetMyContacts(string tenantKey, string authToken)
        {
            ViewData["results"] = (JArray)HLGetRequest(tenantKey, authToken, "api/v1.0/Contacts/GetMyContacts");
            ViewData["getMyContacts"] = JsonConvert.SerializeObject(ViewData["results"], Formatting.Indented);
            if (ViewData["results"] == null)
            {
                return RedirectToAction("UserSignIn", "Domain", new { preView = "GetMyContacts" });
            }
            return View();
        }

        public ActionResult GetDiscussion(string tenantKey, string authToken, string discussionKey)
        {
            JObject results = null;
            if (discussionKey != null)
            {
                ViewData["discussionKey"] = discussionKey;
            }
            else
            {
                return View();
            }
            results = (JObject)HLGetRequest(tenantKey, authToken, "api/v1.0/Discussions/GetDiscussion?discussionKey=" + discussionKey);
            if (results != null)
            {
                ViewData["results"] = results;
                ViewData["getDiscussion"] = JsonConvert.SerializeObject(results, Formatting.Indented);
                return View();
            }
            return RedirectToAction("UserSignIn", "Domain", new { preView = "GetDiscussion" });
        }

        public ActionResult GetDiscussionPost(string tenantKey, string authToken, string discussionPostKey)
        {
            JObject results = null;
            if (discussionPostKey != null)
            {
                ViewData["discussionPostKey"] = discussionPostKey;
            }
            else
            {
                return View();
            }
            results = (JObject)HLGetRequest(tenantKey, authToken, "api/v1.0/Discussions/GetDiscussionPost?discussionPostKey=" + discussionPostKey);
            if (results != null)
            {
                ViewData["results"] = results;
                ViewData["getDiscussionPost"] = JsonConvert.SerializeObject(results, Formatting.Indented);
                return View();
            }
            return RedirectToAction("UserSignIn", "Domain", new { preView = "GetDiscussionPost" });
        }

        public ActionResult GetSubscribedDiscussions(string tenantKey, string authToken)
        {
            ViewData["results"] = (JArray)HLGetRequest(tenantKey, authToken, "api/v1.0/Discussions/GetSubscribedDiscussions");
            ViewData["getSubscribedDiscussions"] = JsonConvert.SerializeObject(ViewData["results"], Formatting.Indented);
            if (ViewData["results"] == null)
            {
                return RedirectToAction("UserSignIn", "Domain", new { preView = "GetSubscribedDiscussions" });
            }
            return View();
        }

        public ActionResult GetDiscussionPosts(string tenantKey, string authToken, string discussionKey)
        {
            JArray results = null;
            if (discussionKey != null)
            {
                ViewData["discussionKey"] = discussionKey;
            }
            else
            {
                return View();
            }
            results = (JArray)HLGetRequest(tenantKey, authToken, "api/v1.0/Discussions/GetDiscussionPosts?discussionKey=" + discussionKey);
            if (results != null)
            {
                ViewData["results"] = results;
                ViewData["getDiscussionPosts"] = JsonConvert.SerializeObject(results, Formatting.Indented);
                return View();
            }
            return RedirectToAction("UserSignIn", "Domain", new { preView = "GetDiscussionPosts" });
        }

    }
}
