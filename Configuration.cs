using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspagemV3
{
    public static class Configuration
    {
        public static SmtpConfiguration Smtp = new();


        public class SmtpConfiguration
        {

          public string Host { get; set; } = "smtp.office365.com";

            public int Port { get; set; } = 587;
            public string UserName { get; set; } = "DevLeandroTestes@outlook.com";
            public string Password { get; set; } = "LeandroTestes";
        }

    }

}
