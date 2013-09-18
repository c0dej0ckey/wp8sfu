using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using wp8sfu.Services;

namespace wp8sfu.VMs
{
    public class ProtectedServicesVM : INotifyPropertyChanged
    {
        private List<string> mServices = new List<string>();
        private Visibility mBrowserVisibility;
        private Visibility mServiceListVisibility;
        private string mSelectedService;

        public ProtectedServicesVM()
        {
            mServices.Add("webct");
            mServices.Add("sims");
            mServices.Add("go sfu");
            mServices.Add("sfu connect");
            mServices.Add("coursys");
            BrowserVisibility = Visibility.Collapsed;
            ServiceListVisibility = Visibility.Visible;

        }

        public Visibility BrowserVisibility
        {
            get { return mBrowserVisibility; }
            set { mBrowserVisibility = value; }
        }

        public Visibility ServiceListVisibility
        {
            get { return mServiceListVisibility; }
            set { mServiceListVisibility = value; }
        }

        public List<string> Services
        {
            get { return mServices; }
        }

        public string SelectedService
        {
            get { return this.mSelectedService; }
            set { this.mSelectedService = value; }
        }

        public void OpenService(string service)
        {
            NavigationService navigationService = ServiceLocator.GetService<NavigationService>();
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    mBrowserVisibility = Visibility.Visible;
                    mServiceListVisibility = Visibility.Collapsed;
                    OnPropertyChanged("BrowserVisibility");
                    OnPropertyChanged("ServiceListVisibility");
                });
            CookieContainer cookies = ServiceLocator.GetService<CookieContainer>();
            if(cookies == null)
            {
                //login
            }
            if(service.Equals("webct"))
            {
                //CookieAwareClient client = new CookieAwareClient(cookies);

            }
            else if(service.Equals("sims"))
            {
                
            }
            else if(service.Equals("go sfu"))
            {
                
            }
            else if(service.Equals("sfu connect"))
            {
                
            }
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
