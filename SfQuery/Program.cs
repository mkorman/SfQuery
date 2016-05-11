using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfQuery
{
    public class Program
    {
        static void Main(string[] args)
        {
            var client = new SalesforceClient
            {
                Username = args[0],
                Password = args[1],
                Token = args[2],
                ClientId = args[3],
                ClientSecret = args[4]
            };

            client.Login();
        }
    }
}
