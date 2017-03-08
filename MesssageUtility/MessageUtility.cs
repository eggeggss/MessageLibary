using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesssageUtility
{
    public class MessageUtility
    {
        public static String JsonToString(object obj)
        {
           return JsonConvert.SerializeObject(obj);
        }

        public static T JsonToObject<T>(String str)
        {
           return  JsonConvert.DeserializeObject<T>(str);
        }


        public static String Pattern(String input)
        {
            input = input.Replace("0", "00");
            input = input.Replace("+", "01");
            input = input.Replace("/", "02");
            input = input.Replace("=", "03");
            return input;
        }

        public static String DePattern(String input)
        {
            input = input.Replace("00", "0");
            input = input.Replace("01", "+");
            input = input.Replace("02", "/");
            input = input.Replace("03", "=");
            return input;
        }
    }
}
