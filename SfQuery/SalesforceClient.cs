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
        private const String LOGIN_ENDPOINT = "https://login.salesforce.com/services/oauth2/token";

        public String Username { get; set; }
        public String Password { get; set; }
        public String Token { get; set; }
        public String ClientId { get; set; }
        public String ClientSecret { get; set; }
        public String AuthToken { get; set; }

        public void Login()
        {
            var request = CreateLoginRequest();
            var response = getResponseText(request);
            Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
            AuthToken = values["signature"];

            Console.WriteLine($"Token: {AuthToken}");
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

        private String getResponseText(WebRequest request)
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
