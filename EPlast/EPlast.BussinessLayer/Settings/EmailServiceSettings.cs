﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BussinessLayer.Settings
{
    public class EmailServiceSettings
    {
        public string SMTPServer { get; set; }
        public int Port { get; set; }
        public string SMTPServerLogin { get; set; }
        public string SMTPServerPassword { get; set; }
    }
}
