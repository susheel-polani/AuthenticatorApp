using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace AuthenticatorApp
{
    internal class HTTPServer
    {
        public static void InitiateServer()
        {
            var config = new HttpSelfHostConfiguration("http://localhost:8080");
            Debug.WriteLine("config started");
            using (HttpSelfHostServer server = new HttpSelfHostServer(config))
            {
                server.OpenAsync().Wait();
                Console.WriteLine("Press Enter to quit.");
                Console.ReadLine();
            }
        }
    }
}
