using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using wp8sfu.Services;
using wp8sfu.Utilities;

namespace wp8sfu.VMs
{
    public class ProtectedServicesVM : INotifyPropertyChanged
    {
        private static ObservableCollection<string> mServices = new ObservableCollection<string>() { "webct", "sims", "go sfu", "sfu connect", "coursys" };
        private static string mSelectedService;

        public ProtectedServicesVM()
        {
        }

        public static ObservableCollection<string> Services
        {
            get { return mServices; }
        }

        public static string SelectedService
        {
            get { return mSelectedService; }
            set { mSelectedService = value; }
        }

        public void OpenService()
        {
            NavigationService navigationService = ServiceLocator.GetService<NavigationService>();
            if (Settings.ComputingId.Equals(string.Empty) && Settings.Password.Equals(string.Empty))
            {
                navigationService.Navigate(new Uri("/Pages/LoginDetailsPage.xaml", UriKind.Relative));
            }
            else
            {
                navigationService.Navigate(new Uri("/Pages/ProtectedServiceBrowserPage.xaml", UriKind.Relative));
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
