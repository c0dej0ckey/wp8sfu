using HtmlAgilityPack;
using Microsoft.Phone.Net.NetworkInformation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Xml;
using wp8sfu.Services;
using wp8sfu.UI;
using wp8sfu.Utilities;

namespace wp8sfu.VMs
{
    public class LoginDetailsVM : INotifyPropertyChanged
    {
        private static DelegateCommand mLoginCommand;
        private string mKey;
        private static bool sLogInStatus = false;
        private Visibility mErrorVisibility;
        private Visibility mLoadingVisibility;

        public LoginDetailsVM()
        {
            mLoginCommand = new DelegateCommand(ExecuteLogin, CanExecuteLogin);
            mErrorVisibility = Visibility.Collapsed;
        }

        public static string ComputingId
        {
            get 
            {
                return Settings.ComputingId;
            }
            set 
            {
                Settings.ComputingId = value; 
            }
        }

        public static string Password
        {
            get 
            {
                return Settings.Password;
            }
            set 
            {
                Settings.Password = value;
            }
        }

        public static bool LoginStatus
        {
            get { return sLogInStatus; }
        }

        public Visibility Loading
        {
            get
            {
                if (sLogInStatus == false)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
            set
            {
                mLoadingVisibility = value;
            }

        }

        public Visibility ErrorVisibility
        {
            get { return mErrorVisibility; }
            set { mErrorVisibility = value; }
        }


        public static DelegateCommand LoginCommand
        {
            get { return mLoginCommand; }
        }

        private bool CanExecuteLogin(object parameter)
        {
                return true;
        }

        private void ExecuteLogin(object parameter)
        {
            sLogInStatus = true;
            ErrorVisibility = Visibility.Collapsed;
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {

                    OnPropertyChanged("Loading");
                });

            var available = NetworkInterface.GetIsNetworkAvailable();
#if DEBUG
            available = true;
#endif
            if (available)
            {

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://cas.sfu.ca/cgi-bin/WebObjects/cas.woa/wa/login");
                IAsyncResult response = request.BeginGetResponse(new AsyncCallback(GetLoginResponseCallback), request);
            }
            else
            {
                MessageBox.Show("No internet connection is available.");
            }


        }

        private void GetLoginResponseCallback(IAsyncResult asyncResult)
        {
            HttpWebRequest request = (HttpWebRequest)asyncResult.AsyncState;
            //post logout
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asyncResult);
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string responseString = reader.ReadToEnd();
            HtmlDocument document = new HtmlDocument();
            document.OptionFixNestedTags = true;
            document.LoadHtml(responseString);
            HtmlNode node = CheckLine(document.DocumentNode);
            HtmlAttribute attribute = node.Attributes[1];
            mKey = attribute.Value;
            LoginUser();
        }

        private void LoginUser()
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://cas.sfu.ca/cgi-bin/WebObjects/cas.woa/wa/login");
            request.CookieContainer = new CookieContainer();
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            request.BeginGetRequestStream(new AsyncCallback(GetLoginRequestStreamCallback), request);
        }

        private void GetLoginRequestStreamCallback(IAsyncResult asyncResult)
        {
            HttpWebRequest request = (HttpWebRequest)asyncResult.AsyncState;
            
            Stream stream = request.EndGetRequestStream(asyncResult);
            string loginData = "username=" + ComputingId + "&password=" + Password + "&lt=" + mKey;
            byte[] bytes = Encoding.UTF8.GetBytes(loginData);
            stream.Write(bytes, 0, loginData.Length);
            stream.Close();

            request.BeginGetResponse(new AsyncCallback(GetLoggedInCallback), request);
        }

        private void GetLoggedInCallback(IAsyncResult asyncResult)
        {
            HttpWebRequest request = (HttpWebRequest)asyncResult.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asyncResult);
            Stream stream = stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            CookieCollection cookies = request.CookieContainer.GetCookies(new Uri("https://cas.sfu.ca/cgi-bin/WebObjects/cas.woa/wa/login"));

            
            if (CookieService.CookieExists("CASTGC"))
            {
                CookieService.RemoveCookieWithName("CASTGC");
            }
            foreach(Cookie cookie in cookies)
            {
                CookieService.AddCookie(cookie);
                if (cookie.Name == "CASTGC")
                {
                    //ServiceLocator.AddService<CookieCollection>(cookies);
                    FlurryWP8SDK.Api.SetUserId(Settings.ComputingId);
                    sLogInStatus = false;
                    
                    
                }
            }
            //getting cookie failed
            if (sLogInStatus == true)
            {
                ErrorVisibility = Visibility.Visible;
                Loading = Visibility.Collapsed;
                sLogInStatus = false;
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    OnPropertyChanged("Loading");
                    OnPropertyChanged("ErrorVisibility");
                });

            }
            else
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {

                        OnPropertyChanged("Loading");
                        NavigationService service = ServiceLocator.GetService<NavigationService>();
                        if (service.BackStack.First().Source.OriginalString == "/Pages/ProtectedServicesPage.xaml")
                        {
                            service.Navigate(new Uri("/Pages/ProtectedServiceBrowserPage.xaml", UriKind.Relative));
                        }
                        else
                        {
                            service.GoBack();
                        }
                    });
            }

        }

        private static HtmlNode CheckLine(HtmlNode node)
        {
            if(node.Line == 55)
            {
                return node;
            }
            foreach(HtmlNode nd in node.ChildNodes)
            {
                var val = CheckLine(nd);
                if (val != null)
                    return val;
            }


            return null;
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
