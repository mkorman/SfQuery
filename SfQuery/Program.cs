using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace SfQuery
{
    public class Program
    {
        private static void start()
        {
            var client = new SalesforceClient
            {
                Username = ConfigurationManager.AppSettings["username"],
                Password = ConfigurationManager.AppSettings["password"],
                Token = ConfigurationManager.AppSettings["token"],
                ClientId = ConfigurationManager.AppSettings["clientId"],
                ClientSecret = ConfigurationManager.AppSettings["clientSecret"]
            };

            client.Login();
            client.GetAccounts();
            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            start();
        }
    }
}
