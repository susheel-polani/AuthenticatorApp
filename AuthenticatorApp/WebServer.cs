using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace AuthenticatorApp
{
    internal class WebServer
    {
        sealed partial class App : Application
        {
            int port = 8000;

            /// <summary>
            /// Initializes the singleton application object.  This is the first line of authored code
            /// executed, and as such is the logical equivalent of main() or WinMain().
            /// </summary>
            public App()
            {
                StartServer();
            }

            private void StartServer()
            {
                StreamSocketListener listener = new StreamSocketListener();
                listener.BindServiceNameAsync(port.ToString());
                Debug.WriteLine("Bound to port: " + port.ToString());
                listener.ConnectionReceived += async (s, e) =>
                {
                    Debug.WriteLine("Got connection");
                    using (IInputStream input = e.Socket.InputStream)
                    {
                        var buffer = new Windows.Storage.Streams.Buffer(2);
                        await input.ReadAsync(buffer, buffer.Capacity, InputStreamOptions.Partial);
                    }

                    using (IOutputStream output = e.Socket.OutputStream)
                    {
                        using (Stream response = output.AsStreamForWrite())
                        {
                            response.Write(Encoding.ASCII.GetBytes("Hello, World!"), 0, 1);
                        }
                    }
                };
            }
        }
    }
}
