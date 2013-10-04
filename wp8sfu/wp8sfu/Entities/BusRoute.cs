using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wp8sfu.Entities
{
    public class BusRoute
    {
        private string mRouteNumber;
        private string mRouteName;
        private List<string> mBusRouteTimes;

        public BusRoute(string routeNumber, string routeName)
        {
            this.mBusRouteTimes = new List<string>();
            this.mRouteName = routeName;
            this.mRouteNumber = routeNumber;
        }

        public string RouteNumber
        {
            get { return this.mRouteNumber; }
            set { this.mRouteNumber = value; }
        }

        public string RouteName
        {
            get { return this.mRouteName; }
            set { this.mRouteName = value; }
        }

        public List<string> BusRouteTimes
        {
            get { return this.mBusRouteTimes; }
        }

        public void AddRouteTime(string time)
        {
            mBusRouteTimes.Add(time);
        }

        public void RemoveRouteTimes()
        {
            mBusRouteTimes.Clear();
        }
    }

}
