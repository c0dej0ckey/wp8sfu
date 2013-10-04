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
        private static List<string> sBurnabyStops = new List<string>() { "53096", "51861", "52998", "52807" };
        private static List<string> sSurreyStops = new List<string>() {"55836", "55738", "61035", "55070", "61787", "55210", "55713", "54993", "55714", "56406", "55441", "55612" };
        private static string apiKey = "axidqpmv6rg7WgIOsYjt";
        private ObservableCollection<BusRoute> mBurnabyBusRoutes = new ObservableCollection<BusRoute>();
        private ObservableCollection<BusRoute> mSurreyBusRoutes = new ObservableCollection<BusRoute>();

        public TransitVM()
        {
            GetTransitTimes();
        }

        public ObservableCollection<BusRoute> BurnabyBusRoutes
        {
            get { return this.mBurnabyBusRoutes; }
            set { this.mBurnabyBusRoutes = value; }
        }

        public ObservableCollection<BusRoute> SurreyBusRoutes
        {
            get { return this.mSurreyBusRoutes; }
            set { this.mSurreyBusRoutes = value; }
        }

        private void GetTransitTimes()
        {
            foreach (string stop in sBurnabyStops)
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(string.Format("http://api.translink.ca/RTTIAPI/V1/stops/{0}/estimates?apiKey={1}", stop, apiKey));
                request.Method = "GET";
                request.Accept = "application/json";
                request.BeginGetResponse(new AsyncCallback(GetBurnabyStopResponse), request);
            }

            foreach (string stop in sSurreyStops)
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(string.Format("http://api.translink.ca/RTTIAPI/V1/stops/{0}/estimates?apiKey={1}", stop, apiKey));
                request.Method = "GET";
                request.Accept = "application/json";
                request.BeginGetResponse(new AsyncCallback(GetBurnabyStopResponse), request);
            }

        }

        private void GetBurnabyStopResponse(IAsyncResult result)
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
            
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    BurnabyBusRoutes.Add(route);
                    OnPropertyChanged("BusRoutes");
                });

        }

        private void GetSurreyStopResponse(IAsyncResult result)
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
            foreach (JObject obj in times)
            {
                string time = obj["ExpectedLeaveTime"].ToString();
                route.AddRouteTime(time);
            }

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                BurnabyBusRoutes.Add(route);
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
