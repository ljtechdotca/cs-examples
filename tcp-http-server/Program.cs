using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Text.Json;

// create a new instance of tcp listener
var ipAddress = IPAddress.Loopback;
var listener = new TcpListener(ipAddress, 3000);

// start listening
listener.Start();

while (true)
{
    // define client and stream
    var client = listener.AcceptTcpClient();
    var stream = client.GetStream();

    // read request
    Encoding utf8 = Encoding.UTF8;
    byte[] bytes = new byte[1024];
    var read = stream.Read(bytes);
    if (read > 0)
    {
        string requestString = utf8.GetString(bytes);
        string[] line = requestString.Split("\n")[0].Split(" ");
        string method = line[0];
        string url = line[1];
        string version = line[2];

        if (url.StartsWith("/api") && url.Contains("?"))
        {
            // parse request
            var parameters = new Dictionary<string, string>();
            string[] queryString = url.Split("?");
            string[] queries = queryString[1].Split("&");
            foreach (string query in queries)
            {
                string[] pair = query.Split("=");
                (string key, string value) = (pair[0], pair[1]);
                parameters[key] = value;
            }
            object request = new
            {
                method = method,
                parameters = parameters,
            };
            // create response
            string status = "HTTP/1.1 200 OK";
            stream.Write(utf8.GetBytes(status));
            stream.Write(utf8.GetBytes("\r\n"));
            string contentType = "Content-Type: application/json";
            stream.Write(utf8.GetBytes(contentType));
            stream.Write(utf8.GetBytes("\n\n"));
            string json = JsonSerializer.Serialize(request);
            stream.Write(utf8.GetBytes(json));
        }
        else
        {
            // create response
            string status = "HTTP/1.1 400 BAD";
            stream.Write(utf8.GetBytes(status));
            stream.Write(utf8.GetBytes("\r\n"));
            string contentType = "Content-Type: text/html";
            stream.Write(utf8.GetBytes(contentType));
            stream.Write(utf8.GetBytes("\n\n"));
            string html = File.ReadAllText("404.html");
            stream.Write(utf8.GetBytes(html));
        }
    }

    // close client and stream
    client.Close();
    stream.Close();
}
