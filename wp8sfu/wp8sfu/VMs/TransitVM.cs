using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using wp8sfu.Entities;

namespace wp8sfu.VMs
{
    public class TransitVM : INotifyPropertyChanged
    {
        private static List<string> sBurnabyStops = new List<string>() { "55612", "51861", "52998", "52807" };
        private static string apiKey = "axidqpmv6rg7WgIOsYjt";
        private List<BusRoute> mBusRoutes = new List<BusRoute>();

        public TransitVM()
        {
            GetTransitTimes();
        }

        public List<BusRoute> BusRoutes
        {
            get { return this.mBusRoutes; }
            set { this.mBusRoutes = value; }
        }

        private void GetTransitTimes()
        {
            foreach(string stop in sBurnabyStops)
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(string.Format("http://api.translink.ca/RTTIAPI/V1/stops/53096/estimates?apiKey={0}", apiKey));
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
            JArray stopObject = JArray.Parse(json);
            string routeNo = stopObject[0]["RouteNo"].ToString();
            string routeName = stopObject[0]["RouteName"].ToString();

            BusRoute route = new BusRoute(routeNo, routeName);
            JArray times = stopObject[0]["Schedules"] as JArray;
            foreach(JObject obj in times)
            {
                string time = obj["ExpectedLeaveTime"].ToString();
                route.AddRouteTime(time);
            }
            mBusRoutes.Add(route);
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    OnPropertyChanged("BusRoutes");
                });

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
