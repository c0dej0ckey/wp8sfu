using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Text.RegularExpressions;
using wp8sfu.VMs;
using wp8sfu.Services;

namespace wp8sfu.Pages
{
    public partial class ProtectedServiceBrowserPage : PhoneApplicationPage
    {
        private string mService;

        public ProtectedServiceBrowserPage()
        {
            InitializeComponent();
            mService = ProtectedServicesVM.SelectedService;
            SetBrowserNavigationUrl(mService, this.FindName("Browser") as WebBrowser);
        }

        private void Browser_Navigated(object sender, NavigationEventArgs e)
        {
            WebBrowser browser = sender as WebBrowser;
            if (browser.IsEnabled)
            {
                Uri uri = browser.Source;
                string uriString = uri.ToString();
                if (Regex.IsMatch(uriString, "https:\\/\\/cas\\.sfu\\.ca.*"))
                {
                    browser.InvokeScript(
                        "eval", string.Format("document.getElementById('computingId').value='{0}'; document.getElementById('password').value='{1}';document.forms[0].submit();", LoginDetailsVM.Username, LoginDetailsVM.Password));
                }

            }
        }


        private void SetBrowserNavigationUrl(string service, WebBrowser browser)
        {
            switch (service)
            {
                case "webct":
                    browser.Navigate(new Uri("https://webct.sfu.ca/webct/urw/ssinboundCAS.siURN:X-WEBCT-VISTA-V1:ae0c1f73-8e3a-65d6-001c-5fd50753fb4e.snWebCT/cobaltMainFrame.dowebct?&allow=sfu,apache&app=WebCT"));
                    break;
                case "go sfu":
                    browser.Navigate(new Uri("https://go.sfu.ca/psp/goprd/SFU_SITE/ENTP/h/?tab=DEFAULT&allow=sfu,alumni,apache&renew=false"));
                    break;
                case "sfu connect":
                    browser.Navigate(new Uri("https://connect.sfu.ca/zimbra/mail#1"));
                    break;
                case "sims":
                    browser.Navigate(new Uri("http://sakai.sfu.ca/portal/login"));
                    break;
                case "coursys":
                    browser.Navigate(new Uri("https://courses.cs.sfu.ca/"));
                    break;
            }
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            NavigationService navigationService = ServiceLocator.GetService<NavigationService>();
            navigationService.Navigate(new Uri("/Pages/ProtectedServices.xaml", UriKind.Relative));
            base.OnBackKeyPress(e);
        }

    }
}