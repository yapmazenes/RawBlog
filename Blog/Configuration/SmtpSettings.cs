﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Configuration
{
    public class SmtpSettings
    {
        public string From { get; set; }
        public string Server { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
    }
}
