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

namespace wp8sfu
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

        private void ServiceChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox serviceBox = (ListBox)sender;
            WebBrowser browser = (WebBrowser)serviceBox.FindName("Browser");
            ServiceLocator.AddService<ListBox>(serviceBox);
            ServiceLocator.AddService<WebBrowser>(browser);
            string service = (string)serviceBox.SelectedItem;
            serviceBox.Visibility = System.Windows.Visibility.Collapsed;
            browser.Visibility = System.Windows.Visibility.Visible;
            
            if(service == "webct")
            {
                browser.Navigate(new Uri("https://webct.sfu.ca/webct/urw/ssinboundCAS.siURN:X-WEBCT-VISTA-V1:ae0c1f73-8e3a-65d6-001c-5fd50753fb4e.snWebCT/cobaltMainFrame.dowebct?&allow=sfu,apache&app=WebCT"));
            }
            else if(service == "go sfu")
            {
                browser.Navigate(new Uri("https://go.sfu.ca/psp/goprd/SFU_SITE/ENTP/h/?tab=DEFAULT&allow=sfu,alumni,apache&renew=false"));
            }
            else if(service == "sfu connect")
            {
                browser.Navigate(new Uri("https://connect.sfu.ca/zimbra/mail#1"));
            }
            else if(service == "sims")
            {
               browser.Navigate(new Uri( "http://sakai.sfu.ca/portal/login"));
            }
            else if(service == "coursys")
            {
                browser.Navigate(new Uri("https://courses.cs.sfu.ca/"));
            }
        }

        private void Browser_Navigated(object sender, NavigationEventArgs e)
        {
            WebBrowser browser =(WebBrowser)sender;
            Uri uri = browser.Source;
            string uriString = uri.ToString();
            if(Regex.IsMatch(uriString, "https:\\/\\/cas\\.sfu\\.ca.*"))
            {
                browser.InvokeScript(
                    "eval", string.Format("document.getElementById('computingId').value='{0}'; document.getElementById('password').value='{1}';document.forms[0].submit();", Settings.GetSetting("Username"), Settings.GetSetting("Password")));
            }
        }


    }
}