using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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

        static SalesforceClient()
        {
            // SF requires TLS 1.1 or 1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
        }

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
            AuthToken = values["access_token"];
            InstanceUrl = values["instance_url"];
            //Console.WriteLine($"Token: {AuthToken}");
        }

        public string GetAccounts()
        {
            using (var client = new HttpClient())
            {
                string restQuery = InstanceUrl + "/services/data/v33.0/sobjects/Account";
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, restQuery);
                request.Headers.Add("Authorization", "Bearer " + AuthToken);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.SendAsync(request).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }
    }
}
