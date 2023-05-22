using System;
using System.Net.Sockets;
using System.IO;

namespace SimpleHttpServer {

    public class HttpHandler {

        private readonly StreamReader sr;
        private readonly StreamWriter sw;
        private readonly TcpClient client;
        private readonly string filename;

        public HttpHandler(TcpClient client, String filename) {
            this.client = client;
            this.filename = filename;
            this.sr = new StreamReader(this.client.GetStream());
            this.sw = new StreamWriter(this.client.GetStream());
        }
        public void Do()
        {
            try
            {
                Console.WriteLine("Verbindung zu " + client.Client.RemoteEndPoint);
                string request = sr.ReadLine();
                Console.WriteLine("Request: " + request);

                if (request != null && request.Contains("GET"))
                {
                    string route = GetRouteFromRequest(request);

                    if (route == "/file")
                    {
                        ServeFile();
                    }
                    else if (route == "/")
                    {
                        ServeIndexHtml();
                    }
                    else if (route == "/download")
                    {
                        ServeFileBlob();
                    }
                    else
                    {
                        SendNotFoundResponse();
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                client.Close();
            }
        }

        private string GetRouteFromRequest(string request)
        {
            string[] requestParts = request.Split(' ');
            string route = requestParts[1];
            return route;
        }

        private void ServeFile()
        {
            // Read the file
            string theData;
            using (StreamReader file = new StreamReader(this.filename))
            {
                theData = file.ReadToEnd();
            }

            // Send the response
            sw.WriteLine("HTTP/1.0 200 OK");
            sw.WriteLine("Date: " + DateTime.Now.ToString());
            sw.WriteLine("Server: Cooler Super Toller Mega Enormer Fast schon fabelhaft insaner Fileserver");
            sw.WriteLine("Content-length: " + theData.Length);
            sw.WriteLine("Content-type: text/plain");
            sw.WriteLine(); // Empty line
            sw.WriteLine(theData);
            sw.Flush();

            Console.WriteLine("File gesendet");
        }

        private void ServeFileBlob()
        {
            byte[] fileBytes;
            using (FileStream file = new FileStream(this.filename, FileMode.Open, FileAccess.Read))
            {
                fileBytes = new byte[file.Length];
                file.Read(fileBytes, 0, fileBytes.Length);
            }

            // Send the response
            sw.WriteLine("HTTP/1.0 200 OK");
            sw.WriteLine("Date: " + DateTime.Now.ToString());
            sw.WriteLine("Server: Cooler Super Toller Mega Enormer Fast schon fabelhaft insaner Fileserver");
            sw.WriteLine("Content-length: " + fileBytes.Length);
            sw.WriteLine("Content-type: application/octet-stream");
            sw.WriteLine("Content-disposition: attachment; filename=\"" + Path.GetFileName(this.filename) + "\"");
            sw.WriteLine(); // Empty line
            sw.Flush();

            // Write the file bytes to the response stream
            client.GetStream().Write(fileBytes, 0, fileBytes.Length);

            Console.WriteLine("File blob gesendet");
        }

        private void SendNotFoundResponse()
        {
            sw.WriteLine("HTTP/1.0 404 Not Found");
            sw.WriteLine("Date: " + DateTime.Now.ToString());
            sw.WriteLine("Server: Cooler Super Toller Mega Enormer Fast schon fabelhaft insaner Fileserver");
            sw.WriteLine(); // Empty line
            sw.Flush();

            Console.WriteLine("404 response gesendet");
        }

        private void ServeIndexHtml()
        {
            string indexHtml = @"
            <html>
            <head>
                <title>Game Log</title>
            </head>
            <body>
                <h1>Game Log 'Tic Tac Toe'</h1>
                <iframe src=""/file"" width=""800"" height=""600""></iframe>
                <br>
                <a href=""/download"">Download File</a>
            </body>
            </html>";

            // Send the response
            sw.WriteLine("HTTP/1.0 200 OK");
            sw.WriteLine("Date: " + DateTime.Now.ToString());
            sw.WriteLine("Server: TestFileServer 1.0");
            sw.WriteLine("Content-length: " + indexHtml.Length);
            sw.WriteLine("Content-type: text/html");
            sw.WriteLine(); // Empty line
            sw.WriteLine(indexHtml);
            sw.Flush();

            Console.WriteLine("Index HTML gesendet");
        }
    }
}