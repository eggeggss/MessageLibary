using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MessageLibary
{

    public class CallBackContent
    {
        public string message_id { get; set; }
        public string error { get; set; }
    }

    public abstract class PushCallBack
    {
        public List<String> Devices { set; get; }
        public int success { get; set; }
        public int failure { get; set; }
        public String Type { set; get; }

        public abstract String ResponseHandle();

    }

    public class GCMCallBack : PushCallBack
    {
        public GCMCallBack(String type)
        {
            this.Type = type;
        }
        public long multicast_id { get; set; }

        public int canonical_ids { get; set; }
        public List<CallBackContent> results { get; set; }

        public override string ResponseHandle()
        {
            List<String> invalids = new List<string>();
            List<String> valids = new List<string>();

            if (this.failure > 0)
            {
                for (int row = 0; row < this.results.Count; row++)
                {
                    if (!String.IsNullOrEmpty(this.results[row].error))
                    {
                        invalids.Add(Devices[row]);
                    }
                }
            }

            valids = Devices.Where(a => !invalids.Any(m => m == a)).ToList();

            ResponseBack response = new ResponseBack() { Type = Type, Success = valids, Fail = invalids };

            return JsonConvert.SerializeObject(response);
            //return base.ResponseHandle();
        }
    }

    public class APNSCallBack : PushCallBack
    {
        public List<String> valids = new List<string>();
        public List<String> invalids = new List<string>();
        public APNSCallBack(String type)
        {
            this.Type = type;
        }

        public override string ResponseHandle()
        {

            ResponseBack response = new ResponseBack() { Type = Type, Success = valids, Fail = invalids };
            return JsonConvert.SerializeObject(response);

        }
    }

    public class ResponseBack
    {
        public String Type { set; get; }
        public List<String> Success { set; get; }
        public List<String> Fail { set; get; }
    }

}
