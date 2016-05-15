using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace SfQuery
{
    public class SalesforceClient
    {
        private const string LOGIN_ENDPOINT = "https://login.salesforce.com/services/oauth2/token";
        private const string QUERY_ENDPOINT = "{0}/services/data/v36.0/";

        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AuthToken { get; set; }
        public string InstanceUrl { get; set; }

        // TODO: use RestSharps
        public void Login()
        {
            String response;
            using (var client = new HttpClient())
            {
                HttpContent content = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        {"grant_type", "password"},
                        {"client_id", ClientId},
                        {"client_secret", ClientSecret},
                        {"username", Username},
                        {"password", Password + Token}
                    }
                );
                HttpResponseMessage message = client.PostAsync(LOGIN_ENDPOINT, content).Result;
                response = message.Content.ReadAsStringAsync().Result;
            }
            Console.WriteLine($"Response: {response}");
            Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
            AuthToken = values["signature"];
            InstanceUrl = values["instance_url"];
            //Console.WriteLine($"Token: {AuthToken}");
        }

        public string GetAccounts()
        {
            var request = CreateQueryRequest("Account");
            return getResponseText(request);
        }

        private WebRequest CreateQueryRequest(string sObjectType)
        {
            var endpoint = string.Format(QUERY_ENDPOINT, InstanceUrl);
            Console.WriteLine($"Endpoint: {endpoint}");
            var request = (HttpWebRequest)WebRequest.Create(endpoint);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", $"Bearer {AuthToken}");

            return request;
        }

        private string getResponseText(WebRequest request)
        {
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new ApplicationException("Request failed. Received HTTP {response.StatusCode}");
                }

                using (var responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (var reader = new StreamReader(responseStream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            return null;
        }
    }
}
