using HtmlAgilityPack;
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
        private static string sUsername = "USERNAME";
        private static string sPassword = "PASSWORD";
        private static DelegateCommand mLoginCommand;
        private string mKey;
        private static bool sIsLoggingIn = false;
        private Visibility mErrorVisibility;
        private Visibility mLoadingVisibility;

        public LoginDetailsVM()
        {
            mLoginCommand = new DelegateCommand(ExecuteLogin, CanExecuteLogin);
            mErrorVisibility = Visibility.Collapsed;
        }

        public static string Username
        {
            get 
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains(sUsername))
                {
                    var bytes = IsolatedStorageSettings.ApplicationSettings[sUsername] as byte[];
                    var unEncryptedBytes = ProtectedData.Unprotect(bytes, null);
                    return Encoding.UTF8.GetString(unEncryptedBytes, 0, unEncryptedBytes.Length);
                }
                else
                    return string.Empty;
            }
            set 
            {
                var encryptedBytes = ProtectedData.Protect(Encoding.UTF8.GetBytes(value), null);
                IsolatedStorageSettings.ApplicationSettings[sUsername] = encryptedBytes;
                LoginCommand.RaiseCanExecuteChanged();
            }
        }

        public static string Password
        {
            get 
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains(sPassword))
                {
                    var bytes = IsolatedStorageSettings.ApplicationSettings[sPassword] as byte[];
                    var unEncryptedBytes = ProtectedData.Unprotect(bytes, null);
                    return Encoding.UTF8.GetString(unEncryptedBytes, 0, unEncryptedBytes.Length);
                }
                else 
                    return string.Empty; 
            }
            set 
            {
                var encryptedBytes = ProtectedData.Protect(Encoding.UTF8.GetBytes(value), null);
                IsolatedStorageSettings.ApplicationSettings[sPassword] = encryptedBytes;
                LoginCommand.RaiseCanExecuteChanged();
            }
        }

        public Visibility Loading
        {
            get
            {
                if (sIsLoggingIn == false)
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
            if(Username.Length == 0 || Password.Length == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
            
        }

        private void ExecuteLogin(object parameter)
        {
            sIsLoggingIn = true;
            ErrorVisibility = Visibility.Collapsed;
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {

                    OnPropertyChanged("Loading");
                });
            
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://cas.sfu.ca/cgi-bin/WebObjects/cas.woa/wa/login");
            IAsyncResult response = request.BeginGetResponse(new AsyncCallback(GetLoginResponseCallback), request);


        }

        private void GetLoginResponseCallback(IAsyncResult asyncResult)
        {
            HttpWebRequest request = (HttpWebRequest)asyncResult.AsyncState;
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
            string loginData = "username=" + Username + "&password=" + Password + "&lt=" + mKey;
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
            
            foreach(Cookie cookie in cookies)
            {
                if (cookie.Name == "CASTGC")
                {
                    ServiceLocator.AddService<CookieCollection>(cookies);
                    sIsLoggingIn = false;
                    break;
                    
                }
            }
            //getting cookie failed
            if (sIsLoggingIn == true)
            {
                ErrorVisibility = Visibility.Visible;
                Loading = Visibility.Collapsed;
                sIsLoggingIn = false;
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
                        if (service.BackStack.First().Source.ToString() == "/Pages/ProtectedServicesPage.xaml")
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
