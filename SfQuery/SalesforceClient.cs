using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;


namespace SfQuery
{
    public class SalesforceClient
    {
        private const string LOGIN_ENDPOINT = "https://login.salesforce.com/services/oauth2/token";
        private const string QUERY_ENDPOINT = "https://{0}.salesforce.com/services/data/v{1}/sobjects/{2}";

        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AuthToken { get; set; }

        public void Login()
        {
            var request = CreateLoginRequest();
            var response = getResponseText(request);
            Console.WriteLine(response);
            Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
            AuthToken = values["signature"];

            Console.WriteLine($"Token: {AuthToken}");
        }

        public string GetAccounts()
        {
            var request = CreateQueryRequest("Account");
            return getResponseText(request);
        }

        // TODO: use RestSharps
        private HttpWebRequest CreateLoginRequest()
        {
            var request = (HttpWebRequest)WebRequest.Create(LOGIN_ENDPOINT);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            var postData = $"grant_type=password&client_id={ClientId}&client_secret={ClientSecret}&username={Username}&password={Password}{Token}";
            var bytes = Encoding.GetEncoding("iso-8859-1").GetBytes(postData);
            request.ContentLength = bytes.Length;

            using (var writeStream = request.GetRequestStream())
            {
                writeStream.Write(bytes, 0, bytes.Length);
            }

            return request;
        }

        private WebRequest CreateQueryRequest(string sObjectType)
        {
            var endpoint = string.Format(QUERY_ENDPOINT, "emea", "36.0", sObjectType);
            var request = (HttpWebRequest)WebRequest.Create(endpoint);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers.Add("Authorisation:",$"Bearer:{AuthToken}");

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
