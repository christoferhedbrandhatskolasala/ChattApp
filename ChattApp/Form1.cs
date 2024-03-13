using ChattKlient;
using System.Text;
using System.Text.Json;

namespace ChattApp
{
    public partial class Form1 : Form
    {
        HttpClient client = new HttpClient();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HttpRequestMessage request = buildMessagePostRequest(
                txtSend.Text, txtServer.Text, txtUserName.Text);

            // gör anropet och vänta på svar
            HttpResponseMessage response = client.Send(request);
            string jsonResponse = response.Content.ReadAsStringAsync().Result;

            // TODO varningsmeddelande om fel
            Console.WriteLine(jsonResponse);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            string response = client.GetStringAsync(txtServer.Text + "/history").Result;
            List<ChatMessage> history = JsonSerializer.Deserialize<List<ChatMessage>>(response);
            updateHistory(history);
        }

        private void updateHistory(List<ChatMessage> history)
        {
            txtHistory.Clear();
            foreach (ChatMessage chatMessage in history)
            {
                txtHistory.AppendText(chatMessage.ToString() + Environment.NewLine + Environment.NewLine);
            }
        }

        private static HttpRequestMessage buildMessagePostRequest(
            string text, string url, string username)
        {
            // skapa request json data
            ChatMessage message = new ChatMessage()
            {
                Time = DateTime.Now,
                Sender = username,
                Text = text
            };
            string requestJson = JsonSerializer.Serialize(message);
            HttpContent content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            // skapa ett post anrop med ett visst url och json innehåll
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url + "/message"),
                Content = content,
                Method = HttpMethod.Post
            };

            return request;
        }
    }
}