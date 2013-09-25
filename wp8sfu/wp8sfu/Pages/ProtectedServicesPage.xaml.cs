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
    
        public ProtectedServicesPage()
        {
            InitializeComponent();
            if(this.DataContext == null)
            {
                this.DataContext = new ProtectedServicesVM();
            }
        }

        private void Service_Changed(object sender, SelectionChangedEventArgs e)
        {
            ProtectedServicesVM vm = this.DataContext as ProtectedServicesVM;
            vm.OpenService();
        }

        //private void ServiceChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    ListBox serviceBox = (ListBox)sender;
        //    WebBrowser browser = (WebBrowser)serviceBox.FindName("Browser");
        //    mbrowser = browser;
        //    ServiceLocator.AddService<ListBox>(serviceBox);
        //    ServiceLocator.AddService<WebBrowser>(browser);
        //    string service = (string)serviceBox.SelectedItem;
        //    serviceBox.Visibility = System.Windows.Visibility.Collapsed;
        //   // browser.Visibility = System.Windows.Visibility.Visible;

        //    if (LoginDetailsVM.Username.Equals(string.Empty) && LoginDetailsVM.Password.Equals(string.Empty))
        //    {
        //        NavigationService navigationService = ServiceLocator.GetService<NavigationService>();
        //        navigationService.Navigate(new Uri("/Pages/LoginDetailsPage.xaml", UriKind.Relative));
        //    }
        //    else
        //    {
        //       // SetBrowserNavigationUrl(service);
        //        NavigationService navigationService = ServiceLocator.GetService<NavigationService>();
        //        navigationService.Navigate(new Uri("/Pages/ProtectedServiceBrowserPage.xaml", UriKind.Relative));
                
        //    }
        //}

        

        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    if(e.Uri.Equals("/Pages/ProtectedServicesPage.xaml") && e.NavigationMode.Equals(NavigationMode.Back))
        //    {
        //        ProtectedServicesVM vm = this.DataContext as ProtectedServicesVM;
        //       // SetBrowserNavigationUrl(vm.SelectedService);
        //    }
        //    base.OnNavigatedTo(e);
        //}

        //private void Browser_Navigated(object sender, NavigationEventArgs e)
        //{
        //    if (mbrowser.IsEnabled)
        //    {
        //        Uri uri = mbrowser.Source;
        //        string uriString = uri.ToString();
        //        if (Regex.IsMatch(uriString, "https:\\/\\/cas\\.sfu\\.ca.*"))
        //        {
        //            mbrowser.InvokeScript(
        //                "eval", string.Format("document.getElementById('computingId').value='{0}'; document.getElementById('password').value='{1}';document.forms[0].submit();", LoginDetailsVM.Username, LoginDetailsVM.Password));
        //        }

        //    }   
        // }
            
       
            
            
        


    }
}