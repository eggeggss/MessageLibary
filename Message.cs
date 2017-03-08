using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PushSharp.Apple;
using PushSharp.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace MessageLibary
{


    public class Message
    {
        //public static String API_KEY = "AIzaSyD-_wh3ZtccNr1fNC5P-7KLV3syVZ7oprE";
        //public static String KEYPATH = "c:\\certs\\auoapns.p12";
        //public static String PASS = "Auo+apple";
        //public static String KEYPATH = "c:\\certs\\final_push_production.p12";
        //public static String PASS = "!Abcd12345";

        public static String API_KEY;
        public static String KEYPATH;
        public static String PASS;

        static Message()
        {
            MessageResource resource = new MessageResource();
            API_KEY = resource.GoogleAPiKey;
            KEYPATH = resource.AppleKeyPath;
            PASS = resource.AppleKeyPassWord;
        }


        public static String PrepareGCMJSON(List<String> deviceList, int baget, bool mute, String message)
        {
            GCMJson json = new GCMJson();
            json.registration_ids.AddRange(deviceList);
            if (mute)
            {
                json.data = new GCMMessage() { defaults = "4" };
            }
            else
            {
                json.data = new GCMMessage() { defaults = "1" };
            }
            json.data.message = message;
            json.data.badge = baget;
            json.priority = "high";
            String result = JsonConvert.SerializeObject(json);
            return result;
        }
        //群播
        public static String SendGcmBroadCast(String message, int baget, bool mute, List<String> devices)
        {
            String JsonString = PrepareGCMJSON(devices, baget, mute, message);

            byte[] JsonBytes = Encoding.UTF8.GetBytes(JsonString);
            String callbackresult = "";
            WebRequest request = WebRequest.Create("https://android.googleapis.com/gcm/send");
            request.Method = "POST";
            request.Headers[HttpRequestHeader.Authorization] = "key=" + API_KEY;
            request.ContentType = @"application/json";
            request.Credentials = CredentialCache.DefaultCredentials;
            request.ContentLength = JsonString.Length;

            Stream stream = request.GetRequestStream();
            stream.Write(JsonBytes, 0, JsonBytes.Length);
            stream.Close();

            try
            {
                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                callbackresult = reader.ReadToEnd();
                reader.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            PushCallBack callback = JsonConvert.DeserializeObject<GCMCallBack>(callbackresult);
            callback.Devices = devices;
            return callback.ResponseHandle();
        }

        public static String PrepareAPNSJSON(String message, int badge, bool mute)
        {
            APNSJson json = new APNSJson();
            ApnsMessage aps = new ApnsMessage();

            if (mute)
            {
                aps.content_available = 1;
                aps.badge = badge;
            }
            else
            {
                aps.alert = message;
                aps.content_available = 1;
                aps.sound = "default";
                aps.badge = badge;
            }

            json.aps = aps;
            return JsonConvert.SerializeObject(json);
        }

        public static String SendAPNSBroadCast(String message, int baget, bool mute, List<String> devices)
        {
            ApplePushChannelSettings setting = new ApplePushChannelSettings(true, KEYPATH, PASS, true);
            ApplePushService service = new ApplePushService(setting);
            AppleNotificationPayload payload = new AppleNotificationPayload();

            List<String> valids = new List<string>();
            List<String> invalids = new List<string>();

            if (mute)
            {
                payload.Sound = "";
                payload.Alert = new AppleNotificationAlert() { Body = "" };

            }
            else
            {
                payload.Sound = "default";
                payload.Alert = new AppleNotificationAlert() { Body = message };

            }
            payload.Badge = baget;

            
            object obj = new object();
            int total = 0;

            service.OnNotificationFailed += (object sender, INotification notification, Exception error) =>
            {
                lock (obj)
                {
                    total++;
                    invalids.Add((String)notification.Tag);
                }
                Console.WriteLine("Fail / " + notification.Tag + "/ " + total);
            };

            service.OnNotificationSent += (object sender, INotification notification) =>
            {
                lock (obj)
                {
                    total++;
                    valids.Add((String)notification.Tag);
                }
                Console.WriteLine("Ok / " + total);
            };


            foreach (var item in devices)
            {
                AppleNotification sendNotify = new AppleNotification(item, payload);
                sendNotify.Tag = item;
                service.QueueNotification(sendNotify);
            }

            while (total != devices.Count) ;

            System.Threading.Thread.Sleep(3000);

            service.Stop();

            PushCallBack pushcallback = new APNSCallBack("iOS")
            {
                valids = valids,
                invalids = invalids,
                success = valids.Count,
                failure = invalids.Count,
                Devices = devices
            };

            return JsonConvert.SerializeObject(pushcallback);
        }
       
        private static byte[] HexStringToByteArray(String DeviceID)
        {
            byte[] deviceToken = new byte[DeviceID.Length / 2];
            for (int i = 0; i < deviceToken.Length; i++)
                deviceToken[i] = byte.Parse(DeviceID.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
            return deviceToken;
        }

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;
            else // Do not allow this client to communicate with unauthenticated servers.
                return false;
        }


    }

    


}