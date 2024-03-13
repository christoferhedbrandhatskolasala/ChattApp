using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattKlient
{
    internal class ChatMessage
    {
        public DateTime Time { get; set; }
        public string Sender { get; set; }
        public string Text { get; set; }

        public override string ToString()
        {
            return Sender + " " + Time.ToString("yyyy-MM-dd HH:mm") + 
                Environment.NewLine + Text;
        }
    }
}
