using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Xml;

namespace PhoneApp1
{
    public class LoginDetailsVM : INotifyPropertyChanged
    {
        private string mUsername;
        private string mPassword;
        private DelegateCommand mLoginCommand;
        private string mKey;
        private bool mIsLoggingIn;

        public LoginDetailsVM()
        {
            mUsername = string.Empty;
            mPassword = string.Empty;
            mLoginCommand = new DelegateCommand(ExecuteLogin, CanExecuteLogin);
        }

        public string Username
        {
            get { return this.mUsername; }
            set 
            { 
                this.mUsername = value;
                LoginCommand.RaiseCanExecuteChanged();
            }
        }

        public string Password
        {
            get { return this.mPassword; }
            set 
            {
                this.mPassword = value;
                LoginCommand.RaiseCanExecuteChanged();
            }
        }

        public Visibility Loading
        {
            get
            {
                if (mIsLoggingIn == false)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Visible;
                }
            }

        }

        public DelegateCommand LoginCommand
        {
            get { return mLoginCommand; }
        }

        private bool CanExecuteLogin(object parameter)
        {
            if(mUsername.Length == 0 || mPassword.Length == 0)
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
            mIsLoggingIn = true;

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
            string loginData = "username=" + mUsername + "&password=" + mPassword + "&lt=" + mKey;
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
                    Settings.AddSetting("LoggedIn", "True");
                    Settings.AddSetting("Username", mUsername);
                    Settings.AddSetting("Password", mPassword);
                    ServiceLocator.AddService<CookieCollection>(cookies);
                    mIsLoggingIn = false;
                    break;
                    
                }
            }

            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {

                    OnPropertyChanged("Loading");
                    NavigationService service = ServiceLocator.GetService<NavigationService>();
                    service.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                });
            

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
