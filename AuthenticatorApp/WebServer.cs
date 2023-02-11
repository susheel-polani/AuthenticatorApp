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
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage;

namespace AuthenticatorApp
{
    internal class WebServer
    {
        public class HTTPServer : IDisposable
        {

            private static HTTPServer instance;

            public static HTTPServer Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = new HTTPServer(8000);
                    }
                    return instance;
                }
            }


            private const uint BufferSize = 8192;

            private readonly StreamSocketListener listener;
            private int port;

            public HTTPServer(int port)
            {
                this.port = port;
                this.listener = new StreamSocketListener();
                this.listener.ConnectionReceived += (s, e) => ProcessRequestAsync(e.Socket);
            }

            public void StartServer()
            {
#pragma warning disable CS4014
                this.listener.BindServiceNameAsync(port.ToString());
#pragma warning restore CS4014
            }

            public void Dispose()
            {
                this.listener.Dispose();
            }

            private async void ProcessRequestAsync(StreamSocket socket)
            {
                // this works for text only
                StringBuilder request = new StringBuilder();
                using (IInputStream input = socket.InputStream)
                {
                    byte[] data = new byte[BufferSize];
                    IBuffer buffer = data.AsBuffer();
                    uint dataRead = BufferSize;
                    while (dataRead == BufferSize)
                    {
                        await input.ReadAsync(buffer, BufferSize, InputStreamOptions.Partial);
                        request.Append(Encoding.UTF8.GetString(data, 0, data.Length));
                        dataRead = buffer.Length;
                    }
                }

                using (IOutputStream output = socket.OutputStream)
                {
                    HTTPRequest requestInfo = new HTTPRequest(request.ToString());

                    if (requestInfo.Method == "GET")
                    {
                        wait WriteResponseAsync(requestInfo, output);
                    }
                    else
                    {
                        throw new InvalidDataException("HTTP method not supported: " + requestInfo.Method);
                    }
                }
            }

            private async Task WriteResponseAsync(HTTPRequest request, IOutputStream os)
            {
                using (Stream resp = os.AsStreamForWrite())
                {
                    bool exists = false;
                    if (request.Path.StartsWith("/PUT_URL_HERE"))
                    {
                        exists = true;

                        try
                        {
                            StorageFile fs = await StorageFile.GetFileFromPathAsync("FILEPATH");
                            Stream readStream = await fs.OpenStreamForReadAsync();

                            long length = readStream.Length;

                            string header = String.Format("HTTP/1.1 200 OK\r\n" +
                                "Connection: keep-alive\r\n" +
                                "Content-Type: text/html\r\n" +
                                "Content-Length: {1}\r\n" +
                                "\r\n",,
                                length);

                            byte[] headerArray = Encoding.UTF8.GetBytes(header);
                            await resp.WriteAsync(headerArray, 0, headerArray.Length);

                            readStream.Position = start;

                            var bytesToRead = length;

                            byte[] buffer = new byte[BufferSize];
                            int bytesRead = 0;
                            while (bytesToRead > 0 && (bytesRead = await readStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                            {
                                bytesToRead -= bytesRead;
                                await resp.WriteAsync(buffer, 0, bytesRead);
                            }

                        }
                        catch (FileNotFoundException)
                        {
                            exists = false;
                        }
                    }

                    if (!exists)
                    {
                        byte[] headerArray = Encoding.UTF8.GetBytes(
                                              "HTTP/1.1 404 Not Found\r\n" +
                                              "Content-Length:0\r\n" +
                                              "Connection: close\r\n\r\n");
                        await resp.WriteAsync(headerArray, 0, headerArray.Length);
                    }

                    await resp.FlushAsync();
                }
            }
        }
    }
}
