using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using wp8sfu.Services;
using wp8sfu.Utilities;
using wp8sfu.UI;
using System.Net;
using System.IO;
using HtmlAgilityPack;
using System.Windows;
using Microsoft.Phone.Controls;

namespace wp8sfu.VMs
{
    public class MainPageVM : INotifyPropertyChanged
    {
        private NavigationService mNavigationService;
        private string mKey;

        public MainPageVM()
        {

            if (!CookieService.CookieExists("CASTGC"))
            {
                if (Settings.ComputingId != string.Empty && Settings.Password != string.Empty)
                {
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://cas.sfu.ca/cgi-bin/WebObjects/cas.woa/wa/login");
                    IAsyncResult response = request.BeginGetResponse(new AsyncCallback(GetLoginResponseCallback), request);
                }
            }
        }

       

        public string LoginStatus
        {
            get
            {
                if (CookieService.GetCookieWithName("CASTGC") != null)
                    return "LOGOUT";
                return "LOGIN";
            }
        }

        public NavigationService NavigationService
        {
            get { return this.mNavigationService; }
            set { this.mNavigationService = value; }
        }

        public ICommand LoginCommand
        {
            get { return new DelegateCommand(ExecuteLogin, CanExecuteLogin); }
        }

        public ICommand ProtectedServicesCommand
        {
            get { return new DelegateCommand(ExecuteProtectedServices, CanExecuteProtectedServices); }
        }

        public ICommand CoursesCommand
        {
            get { return new DelegateCommand(ExecuteCourses, CanExecuteCourses); }
        }

        public ICommand BooksCommand
        {
            get { return new DelegateCommand(ExecuteBooks, CanExecuteBooks); }
        }

        public ICommand MapsCommand
        {
            get { return new DelegateCommand(ExecuteMaps, CanExecuteMaps); }
        }

        public ICommand TransitCommand
        {
            get { return new DelegateCommand(ExecuteTransit, CanExecuteTransit); }
        }

        private bool CanExecuteLogin(object parameter)
        {
            return true;
        }

        private void ExecuteLogin(object parameter)
        {
            if (LoginStatus.Equals("LOGOUT"))
            {
                CookieService.DeleteCookies();
                Settings.ComputingId = "";
                Settings.Password = "";
                Settings.DeleteCourses();

                OnPropertyChanged("LoginStatus");

            }
            else
            {
                NavigationService navigationService = ServiceLocator.GetService<NavigationService>();
                navigationService.Navigate(new Uri("/Pages/LoginDetailsPage.xaml", UriKind.Relative));
            }
        }

        private bool CanExecuteProtectedServices(object parameter)
        {
            return true;
        }

        private void ExecuteProtectedServices(object parameter)
        {
            
            if(Settings.ComputingId == string.Empty && Settings.Password == string.Empty)
            {
                MessageBox.Show("Please Login.", "Login Status", MessageBoxButton.OK);
            }
            else if (!CookieService.CookieExists("CASTGC"))
            {
                MessageBox.Show("Wrong Computing Id or Password or Not Logged In", "Login Status", MessageBoxButton.OK);
            }
            else
            {

                NavigationService navigationService = ServiceLocator.GetService<NavigationService>();
                navigationService.Navigate(new Uri("/Pages/ProtectedServicesPage.xaml", UriKind.Relative));
            }
        }

        private bool CanExecuteCourses(object parameter)
        {
            return true;
        }

        private void ExecuteCourses(object parameter)
        {
            if (Settings.ComputingId == string.Empty && Settings.Password == string.Empty)
            {
                MessageBox.Show("Please Login.", "Login Status", MessageBoxButton.OK);
            }
            else if (!CookieService.CookieExists("CASTGC"))
            {
                MessageBox.Show("Wrong Computing Id or Password or Not Logged In", "Login Status", MessageBoxButton.OK);
            }
            else
            {

                NavigationService navigationService = ServiceLocator.GetService<NavigationService>();
                navigationService.Navigate(new Uri("/Pages/SchedulePage.xaml", UriKind.Relative));
            }
        }

        private bool CanExecuteBooks(object parameter)
        {
            return true;
        }

        private void ExecuteBooks(object parameter)
        {
            List<wp8sfu.Entities.Course> courses = Settings.LoadCourses();
            if (courses == null || Settings.LoadCourses().Count == 0)
            {
                MessageBox.Show("No courses found, please refresh courses before retrieving books", "No Courses Found", MessageBoxButton.OK);
            }
            else
            {

                NavigationService navigationService = ServiceLocator.GetService<NavigationService>();
                navigationService.Navigate(new Uri("/Pages/BooksPage.xaml", UriKind.Relative));
            }
        }

        private bool CanExecuteMaps(object parameter)
        {
            return true;

        }

        private void ExecuteMaps(object parameter)
        {
            NavigationService navigationService = ServiceLocator.GetService<NavigationService>();
            navigationService.Navigate(new Uri("/Pages/MapsPage.xaml", UriKind.Relative));
        }

        private bool CanExecuteTransit(object parameter)
        {
            return true;
        }

        private void ExecuteTransit(object parameter)
        {
            NavigationService navigationService = ServiceLocator.GetService<NavigationService>();
            navigationService.Navigate(new Uri("/Pages/TransitPage.xaml", UriKind.Relative));
        }

        #region AutoLogin 
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
            string loginData = "username=" + Settings.ComputingId + "&password=" + Settings.Password + "&lt=" + mKey;
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
            foreach (Cookie cookie in cookies)
            {

                if (cookie.Name == "CASTGC")
                {
                    CookieService.AddCookie(cookie);


                }
            }
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    OnPropertyChanged("LoginStatus");
                });
        }

        private static HtmlNode CheckLine(HtmlNode node)
        {
            if (node.Line == 55)
            {
                return node;
            }
            foreach (HtmlNode nd in node.ChildNodes)
            {
                var val = CheckLine(nd);
                if (val != null)
                    return val;
            }


            return null;
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
