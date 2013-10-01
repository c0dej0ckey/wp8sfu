using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace wp8sfu.VMs
{
    public class TransitVM
    {
        private static List<string> sBurnabyStops = new List<string>() { "55612" };
        private static string apiKey = "axidqpmv6rg7WgIOsYjt";
        private List<string> mStopTimes = new List<string>();

        public TransitVM()
        {
            GetTransitTimes();
        }

        private void GetTransitTimes()
        {
            foreach(string stop in sBurnabyStops)
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(string.Format("http://api.translink.ca/RTTIAPI/V1/stops/55612?apiKey={0}", apiKey));
                request.Method = "GET";
                request.Accept = "application/json";
                request.BeginGetResponse(new AsyncCallback(GetStopResponse), request);
            }
        }

        private void GetStopResponse(IAsyncResult result)
        {
            HttpWebRequest request = (HttpWebRequest)result.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string json = reader.ReadToEnd();
            JObject stopObject = JObject.Parse(json);



        }
    }
}
