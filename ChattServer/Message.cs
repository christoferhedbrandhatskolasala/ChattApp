﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattServer
{
    internal class ChatMessage
    {
        public DateTime Time { get; set; }
        public string Sender { get; set; }
        public string Text { get; set; }
    }
}
