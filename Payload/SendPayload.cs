using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payload
{
    public class APNSJson
    {
        public ApnsMessage aps { set; get; }
    }

    public class ApnsMessage
    {
        public int content_available { set; get; }
        public String sound { set; get; }
        public String alert { set; get; }

        public int badge { set; get; }

    }


    public class GCMJson
    {
        public List<string> registration_ids = new List<string>();
        public String priority = String.Empty;
        public GCMMessage data = new GCMMessage();

    }


    public class GCMMessage
    {
        public string message = string.Empty;
        public string defaults = String.Empty;
        public int badge { set; get; }
    }

}
