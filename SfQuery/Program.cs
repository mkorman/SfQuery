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
        static void Main(string[] args)
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
            Console.WriteLine(client.GetAccounts());
            Console.ReadLine();
        }
    }
}
