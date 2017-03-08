using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace MessageLibary
{
    public class MessageResource
    {
        public String GoogleAPiKey { set; get; }
        public String AppleKeyPath { set; get; }
        public String AppleKeyPassWord { set; get; }

        public MessageResource()
        {
            this.GoogleAPiKey = ConfigurationManager.AppSettings["GoogleAPiKey"].ToString();
            this.AppleKeyPath = ConfigurationManager.AppSettings["AppleKeyPath"].ToString();
            this.AppleKeyPassWord = ConfigurationManager.AppSettings["AppleKeyPassWord"].ToString();
        }
    }
}
