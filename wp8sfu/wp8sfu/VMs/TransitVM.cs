using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private static List<string> sStops = new List<string>() { "53096", "51861", "52998", "52807", "55836", "55738", "61035", "55070", "61787", "55210", "55713", "54993", "55714", "56406", "55441", "55612" };
        private static string apiKey = "axidqpmv6rg7WgIOsYjt";
        private ObservableCollection<BusRoute> mBusRoutes = new ObservableCollection<BusRoute>();

        public TransitVM()
        {
            GetTransitTimes();
        }

        public ObservableCollection<BusRoute> BusRoutes
        {
            get { return this.mBusRoutes; }
            set { this.mBusRoutes = value; }
        }


        private void GetTransitTimes()
        {
            foreach (string stop in sStops)
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(string.Format("http://api.translink.ca/RTTIAPI/V1/stops/{0}/estimates?apiKey={1}", stop, apiKey));
                request.Method = "GET";
                request.Accept = "application/json";
                request.BeginGetResponse(new AsyncCallback(GetBurnabyStopResponse), request);
            }


        }

        private void GetBurnabyStopResponse(IAsyncResult result)
        {
            string json = string.Empty;
            try
            {
                HttpWebRequest request = (HttpWebRequest)result.AsyncState;
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                json = reader.ReadToEnd();
            }
            catch(Exception)
            {
                return;
            }
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
            
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    BusRoutes.Add(route);
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
