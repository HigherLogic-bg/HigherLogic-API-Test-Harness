using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using APITest.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;

namespace APITest.Controllers
{
    public class DomainController : Controller
    {
        //
        // GET: /Domain/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Check(DomainModel d)
        {
            UriBuilder uriBuilder;
            string domain = d.Domain;
            if (domain.StartsWith("http") == false)
            {
                d.Domain = "http://" + domain;
            }
            try
            {
                uriBuilder = new UriBuilder(d.Domain);
            }
            catch (UriFormatException)
            {
                ModelState.AddModelError("", "");
                return View("Index");
            }
            string uri = uriBuilder.Uri.ToString();
            if (!Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                return RedirectToAction("Index");
            var tenantDetail = CheckWebService(d);
            if (tenantDetail == null)
            {
                ModelState.AddModelError("", "");
                return View("Index");
            }
            // There's got to be a better way
            tenantDetail.HomePage = uri;
            Session.Add("tenantKey", tenantDetail);
            return View("UserSignIn");
        }

        private TenantDetailModel CheckWebService(DomainModel domainModel)
        {
            TenantDetailModel tenantDetailModel = null;
            Uri uri;
            string domain = domainModel.Domain;
            if (domain.StartsWith("http") == false)
            {
                domain = "http://" + domain;
            }
            try
            {
                uri = new Uri(domainModel.Domain);
            }
            catch (UriFormatException)
            {
                return null;
            }

            string url = string.Format("api/v1.0/Authentication/GetTenantDetail?communityUrl={0}", Server.UrlEncode(uri.ToString()));
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.connectedcommunity.org/");



            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));


            // List all products.
            HttpResponseMessage response = client.GetAsync(url).Result;  // Blocking call!
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
                var tenantDetails = response.Content.ReadAsAsync<IEnumerable<TenantDetailModel>>().Result;
                foreach (var tenantDetail in tenantDetails)
                {
                    tenantDetailModel = tenantDetail;
                }
            }
            else
            {
            }
            return tenantDetailModel;
        }

        public ActionResult UserSignIn(LoginWithTenantDetailModel l)
        {
            Session.Add("validateLogin", true);
            return View();
        }

        public ActionResult Check2(LoginWithTenantDetailModel t, String preView)
        {
            var auth = CheckWebService(t);
            if (auth.AuthToken == null)
            {
                Session["validateLogin"] = false;
                ModelState.AddModelError("", "");
                return View("UserSignIn");
            }

            if (preView == "UserSignIn")
            {
                preView = "Methods";
            }
            return RedirectToAction("Methods", new {TenantKey = Server.UrlEncode(auth.TenantKey), AuthToken = Server.UrlEncode(auth.AuthToken)});
        }

        private AuthenticationModel CheckWebService(LoginWithTenantDetailModel l)
        {
            l.TenantDetail = (TenantDetailModel) Session["tenantKey"];
            AuthenticationModel authenticationModel = new AuthenticationModel();
            string url = "api/v1.0/Authentication/Login";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.connectedcommunity.org/");
            authenticationModel.TenantKey = l.TenantDetail.TenantKey;

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("HLTenantKey", authenticationModel.TenantKey);

            // List all products.
            HttpResponseMessage response = client.PostAsJsonAsync(url, l.Login).Result;  // Blocking call!
            if (response.IsSuccessStatusCode)
            {
                authenticationModel.AuthToken = ((string[])(response.Headers.GetValues("HLAuthToken")))[0];
            }
            return authenticationModel;
        }

        public ActionResult Methods(string tenantKey, string authToken)
        {
            ViewBag.Title = "Methods";
            string url = "api/v1.0/Contacts/GetWhoAmI";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.connectedcommunity.org/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("HLTenantKey", tenantKey);
            client.DefaultRequestHeaders.Add("HLAuthToken", Server.UrlDecode(authToken));

            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                return View();
            }
            else
            {
                return RedirectToAction("UserSignIn", "Domain", new { preView = "Methods" });
            }
        }

        public ActionResult TokenResetLogin( string oldUrl)
        {
            // Keep URL Here only updating Auth Key
            return View();
        }
    
    }

}
