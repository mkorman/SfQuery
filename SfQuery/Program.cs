using System;
using System.Configuration;

namespace SfQuery
{
    public class Program
    {
        private static SalesforceClient CreateClient()
        {
            return new SalesforceClient
            {
                Username = ConfigurationManager.AppSettings["username"],
                Password = ConfigurationManager.AppSettings["password"],
                Token = ConfigurationManager.AppSettings["token"],
                ClientId = ConfigurationManager.AppSettings["clientId"],
                ClientSecret = ConfigurationManager.AppSettings["clientSecret"]
            };
        }

        static void Main(string[] args)
        {
            var client = CreateClient();

            if (args.Length > 0)
            {
                client.Login();
                Console.WriteLine(client.Query(args[0]));
            }
            else
            {
                client.Login();
                Console.WriteLine(client.Describe("Account"));
                Console.WriteLine(client.Describe("Contact"));
                Console.WriteLine(client.QueryEndpoints());
                Console.WriteLine(client.Query("SELECT Name from Contact"));
            }
            Console.ReadLine();
        }
    }
}
