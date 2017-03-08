using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            // ParseURL("https://myauom.auo.com/Service/getallnotice.aspx?device=Android");
            Parse("https://myauom.auo.com/Service/getallnotice.aspx?device=Android");

            Console.ReadKey();
        }

        private static void Parse(String url)
        {
            WebClient client = new WebClient();
            //client.Headers.Add(HttpRequestHeader.Cookie, ".AUOASPXFORMSAUTH40=B33650AED5012DA3B2FFAB5B8AFAA44A3E3889594C947D47368D2F3ACF492CB99F41EB5EC809EEA665011D8AA38546E92F36CA4AE46C6601B5222F769F58334638000128D3C711A7E82733C3DEC912930490B05A7C8D0B2E580B97E73BBB67E5B97A8DAFE1A507517EBD02AD1CB95FF85DCE365C97761E06734D541BF0A01204390499AB989CEEA337E65CFE4DC8A769E5C0846533ECB1392AACB91A366B002AE70429A50EEF898B8D7682FDBBFA8DB632A750344D434A6D23C3EF852477EA3393F5A6F9A923B30E9CBD87F7538B3B160F07379600D639D9022268AE02793440DB3CCA6BF6E777CA536F75D55431CE4DBB4D51FF530B7516DA83F9774E233EA6AEED420E2EFF7CBAFCADA45A38FF4AFCE592196978B1E2E16C9DA98EC9521BD5860846A36C2521931B6D71C1F278311204582973B784454ADDAF308B0993590F0287F1A78E65816CD672135A13C89BFB443AC8BF6B8A19F1CCF7B231");
            client.Headers.Add(HttpRequestHeader.Cookie, ".AUOASPXFORMSAUTH40=B33650AED5012DA3B2FFAB5B8AFAA44A3E3889594C947D47368D2F3ACF492CB99F41EB5EC809EEA665011D8AA38546E92F36CA4AE46C6601B5222F769F58334638000128D3C711A7E82733C3DEC912930490B05A7C8D0B2E580B97E73BBB67E5B97A8DAFE1A507517EBD02AD1CB95FF85DCE365C97761E06734D541BF0A01204390499AB989CEEA337E65CFE4DC8A769E5C0846533ECB1392AACB91A366B002AE70429A50EEF898B8D7682FDBBFA8DB632A750344D434A6D23C3EF852477EA3393F5A6F9A923B30E9CBD87F7538B3B160F07379600D639D9022268AE02793440DB3CCA6BF6E777CA536F75D55431CE4DBB4D51FF530B7516DA83F9774E233EA6AEED420E2EFF7CBAFCADA45A38FF4AFCE592196978B1E2E16C9DA98EC9521BD5860846A36C2521931B6D71C1F278311204582973B784454ADDAF308B0993590F0287F1A78E65816CD672135A13C89BFB443AC8BF6B8A19F1CCF7B231");
            string data = client.DownloadString(url);
            Console.WriteLine(data);
        }
        private static void ParseURL(string _URL)
        {
            WebRequest myWebRequest = WebRequest.Create(_URL);
            myWebRequest.Timeout = 10000;
            myWebRequest.Credentials = new NetworkCredential("AUInfo", "Ittd0#$6236", "");
            //myWebRequest.Proxy = new WebProxy(new Uri("http://proxy.hinet.net:8080"), true, strByPassIP);
            //myWebRequest.Proxy.Credentials = new NetworkCredential("Name", "PWD", "Domain Name");
            CookieContainer cookie = new CookieContainer();
            string strHtml = string.Empty;
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)(myWebRequest);
            myHttpWebRequest.Timeout = 10000;
            myHttpWebRequest.Method = "GET";
            myHttpWebRequest.Accept = "text/html";
            myHttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; Windows NT 5.2; Windows NT 6.0; Windows NT 6.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727; MS-RTC LM 8; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729; .NET CLR 4.0C; .NET CLR 4.0E)";
            WebResponse myWebResponse = myHttpWebRequest.GetResponse();
            Stream myStream = myWebResponse.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myStream);
            strHtml = myStreamReader.ReadToEnd();
            //用 html agility pack 解析
            HtmlDocument doc = new HtmlDocument();

            //doc.LoadHtml(strHtml);
            //測試用, 找到指定 String 在 Html 中的位置
            //SearchNodes(doc.DocumentNode.ChildNodes, "Root");
            //HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("/html[1]/body[1]/table[1]/tr[4]/td[1]/table[1]/tr[2]/td[3]/placeholder[1]/table[1]/tr[1]/td[1]/table[1]/tr[1]/td[1]/table[1]/tr[1]/td[1]/div[1]/table[2]/tr[1]/td[1]/table[1]");
           // DataTable dt = ConvertHtml2Table(nodes[0]);
            Console.WriteLine(DateTime.Now.ToLongTimeString() + " : Parsed: " + _URL);
           // Console.WriteLine(DateTime.Now.ToLongTimeString() + " : Parsed records: " + dt.Rows.Count.ToString());
            
        }
    }
}
