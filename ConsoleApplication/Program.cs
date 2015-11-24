using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to facilites console sample type \"1\" for Identity Number Sample or \"2\" for Bank Account Sample");
            var consoleResponse = Console.ReadLine();
            int type;
            HttpWebResponse webResponse;

            if (!int.TryParse(consoleResponse, out type))
            {
                Console.WriteLine("Please enter No.");
                consoleResponse = Console.ReadLine();
            }

            Console.WriteLine("Please enter User Name:");
            string userName = Console.ReadLine();
            Console.WriteLine("Please enter Password");
            string password = Console.ReadLine();

            if (type == 1)
            {
                Console.WriteLine("Enter Identity Number:");
                string idNo = Console.ReadLine();
                webResponse = IdentityNumber(userName, password, idNo);
            }
            else
            {
                webResponse = BankAccount(userName, password);
            }

            // Check Response OK
            if (webResponse.StatusCode != HttpStatusCode.OK) Console.WriteLine("{0}", webResponse.Headers);

            // Place Resopnse in stream to be used by the program
            using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
            {
                string s = reader.ReadToEnd();
                Console.WriteLine(s);
                Console.WriteLine(webResponse.Headers.ToString());
                Console.ReadLine();
            }
        }

        static private HttpWebResponse BankAccount(string username, string password)
        {
            // Define URL (HTTPS is required)
            string url;
            url = "https://services.facilities.co.za/valid8api/api/BankAccount";
            var webRequest = (HttpWebRequest)WebRequest.Create(url);

            // Specify Method
            webRequest.Method = "POST";

            // Specify Content Type
            webRequest.ContentType = "application/json";

            // Specify User Agent
            webRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";

            // Specify User and password Basic Authentication
            string autorization = username + ":" + password;

            // Adds Encoded auth details to header
            byte[] binaryAuthorization = System.Text.Encoding.UTF8.GetBytes(autorization);

            string postBody = "{\"BrachCode\" : \"009953\", \"AccountType\" : \"cur\", \"AccountNumber\": \"023375337\" }";

            byte[] buf = Encoding.UTF8.GetBytes(postBody);

            autorization = Convert.ToBase64String(binaryAuthorization);
            autorization = "Basic " + autorization;
            webRequest.Headers.Add("AUTHORIZATION", autorization);
            webRequest.GetRequestStream().Write(buf, 0, buf.Length);

            // Makes request / gets response
            return (HttpWebResponse)webRequest.GetResponse();


        }

        static private HttpWebResponse IdentityNumber(string username, string password, string idNo)
        {
            // Define URL (HTTPS is required)
            string url;
            url = "https://services.facilities.co.za/valid8api/api/identitynumbers";
            var webRequest = (HttpWebRequest)WebRequest.Create(url);

            // Specify Method
            webRequest.Method = "POST";

            // Specify Content Type
            webRequest.ContentType = "application/json";

            // Specify User Agent
            webRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";

            // Specify User and password Basic Authentication
            string autorization = username + ":" + password;

            // Adds Encoded auth details to header
            byte[] binaryAuthorization = System.Text.Encoding.UTF8.GetBytes(autorization);

            string postBody = "{\"Value\" : \"" + idNo + "\", }";

            byte[] buf = Encoding.UTF8.GetBytes(postBody);

            autorization = Convert.ToBase64String(binaryAuthorization);
            autorization = "Basic " + autorization;
            webRequest.Headers.Add("AUTHORIZATION", autorization);
            webRequest.GetRequestStream().Write(buf, 0, buf.Length);

            // Makes request / gets response
            return (HttpWebResponse)webRequest.GetResponse();


        }
    }
}
