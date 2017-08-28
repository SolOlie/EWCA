using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Newtonsoft.Json.Linq;

namespace FrontendSecure.Gateways
{
    public class LoginGateway
    {
        public HttpResponseMessage Login(string username, string password)
        {
            //setup login data
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            });

            //Request token
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://ewcustomeraccountingbackend20170828013231.azurewebsites.net/EWCustomerAccountingBackend");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.PostAsync("token", formContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = response.Content.ReadAsStringAsync().Result;
                    var jObject = JObject.Parse(responseJson);
                    string token = jObject.GetValue("access_token").ToString();
                    HttpContext.Current.Session["token"] = token;
                }

                return response;
            }

        }
    }
}
