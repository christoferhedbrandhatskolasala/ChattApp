using System.Net;
using System.Text;
using System.Text.Json;

namespace ChattServer
{
    internal class Program
    {
        private const string url = @"http://localhost:8000/";

        private static readonly List<ChatMessage> history = new List<ChatMessage>();

        // todo most of this code should me moved to a server class
        static void Main(string[] args)
        {
            // skapa och starta en webbserver
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();
            Console.WriteLine("Listening for connections on {0}", url);

            while (true)
            {
                // vänta på ett anslutning
                HttpListenerContext ctx = listener.GetContextAsync().Result;

                // titta på anropet
                HttpListenerRequest req = ctx.Request;
                Console.WriteLine("request url: " + req.Url);
                Console.WriteLine("request method: " + req.HttpMethod);
                //Console.WriteLine("request query string keys: " + string.Join(",", req.QueryString.AllKeys));
                //Console.WriteLine("request query string firstname: " + req.QueryString["firstname"]);

                if (req.RawUrl == "/message" && req.HttpMethod == HttpMethod.Post.ToString())
                {
                    // read message json
                    StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
                    string jsonRequest = reader.ReadToEnd();
                    Console.WriteLine("request json body: " + jsonRequest);
                    ChatMessage chatMessage = JsonSerializer.Deserialize<ChatMessage>(jsonRequest);
                    // add message to history
                    history.Add(chatMessage);
                    // respond ok
                    chatMessageRespond(ctx.Response);
                }

                if (req.RawUrl == "/history" && req.HttpMethod == HttpMethod.Get.ToString())
                {
                    historyRespond(ctx.Response);
                }
                    //StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
                    //string jsonRequest = reader.ReadToEnd();

                    ////using (var reader = new StreamReader(req.InputStream, req.ContentEncoding))
                    ////{
                    ////    text = reader.ReadToEnd();
                    ////}
                    //// TODO read json content
                    //Console.WriteLine("request json body: " + jsonRequest);
                    //ChatMessage chatMessage = JsonSerializer.Deserialize<ChatMessage>(jsonRequest);


                    // skapa svaret
                    //string webpage = "<marquee>The cake is a lie!</marquee>";
                    //HttpListenerResponse resp = ctx.Response;
                    //byte[] data = Encoding.UTF8.GetBytes(webpage);
                    //resp.ContentType = "text/html";
                    //resp.ContentEncoding = Encoding.UTF8;
                    //resp.ContentLength64 = data.LongLength;

                    //// skriv svaret och stäng anslutningen
                    //resp.OutputStream.WriteAsync(data, 0, data.Length).Wait();
                    //resp.Close();
                }

                // stäng webbservern
                listener.Close();
        }

        private static void chatMessageRespond(HttpListenerResponse resp)
        {
            byte[] data = Encoding.UTF8.GetBytes("ok");
            resp.ContentType = "text/html"; // TODO html?
            resp.ContentEncoding = Encoding.UTF8;
            resp.ContentLength64 = data.LongLength;

            // skriv svaret och stäng anslutningen
            resp.OutputStream.WriteAsync(data, 0, data.Length).Wait();
            resp.Close();
        }

        private static void historyRespond(HttpListenerResponse resp)
        {
            string responseJson = JsonSerializer.Serialize(history);

            byte[] data = Encoding.UTF8.GetBytes(responseJson);


            resp.ContentType = "application/json";
            resp.ContentEncoding = Encoding.UTF8;
            resp.ContentLength64 = data.LongLength;

            // skriv svaret och stäng anslutningen
            resp.OutputStream.WriteAsync(data, 0, data.Length).Wait();
            resp.Close();
        }
    }
}