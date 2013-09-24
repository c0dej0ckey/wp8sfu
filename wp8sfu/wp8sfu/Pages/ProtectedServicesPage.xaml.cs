using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Text;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Media;
using wp8sfu.Services;
using wp8sfu.VMs;
using wp8sfu.Utilities;

namespace wp8sfu.Pages
{
    public partial class ProtectedServicesPage : PhoneApplicationPage
    {
        private WebBrowser mbrowser;
    
        public ProtectedServicesPage()
        {
            InitializeComponent();
            if(this.DataContext == null)
            {
                this.DataContext = new ProtectedServicesVM();
            }
        }

        private void ServiceChanged(object sender, SelectionChangedEventArgs e)
        {

            if (LoginDetailsVM.Username.Equals(string.Empty) && LoginDetailsVM.Password.Equals(string.Empty))
            {
                NavigationService navigationService = ServiceLocator.GetService<NavigationService>();
                navigationService.Navigate(new Uri("/Pages/LoginDetailsPage.xaml", UriKind.Relative));
            }
            else
            {
               // SetBrowserNavigationUrl(service);
                NavigationService navigationService = ServiceLocator.GetService<NavigationService>();
                navigationService.Navigate(new Uri("/Pages/ProtectedServiceBrowserPage.xaml", UriKind.Relative));
                
            }
        }

        

      

        


    }
}