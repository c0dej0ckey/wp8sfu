﻿using HtmlAgilityPack;
using Microsoft.Phone.Net.NetworkInformation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using wp8sfu.Entities;
using wp8sfu.Services;
using wp8sfu.UI;
using wp8sfu.Utilities;

namespace wp8sfu.VMs
{
    public class ScheduleVM : INotifyPropertyChanged
    {
        private ObservableCollection<Course> mCourses;
        private bool mIsLoadingCourses;

        public ObservableCollection<Course> Courses
        {
            get { return this.mCourses; }
            set { this.mCourses = value; }
        }

        public ScheduleVM()
        {
            
            
            //code for semester change
            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();
            if (fileStorage.FileExists("classes.json"))
            {
                Courses = new ObservableCollection<Course>(Settings.LoadCourses());
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        OnPropertyChanged("Courses");
                    });
            }
            else
            {
                var available = NetworkInterface.GetIsNetworkAvailable();
#if DEBUG
                    available = true;
#endif
                if (available)
                {
                    mIsLoadingCourses = true;
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        OnPropertyChanged("Loading");
                    });
            
                    GetClasses();
                }
                else
                {
                    MessageBox.Show("No internet connection available. Courses will not load without it.");
                }
            }
        }

        public ICommand RefreshCommand
        {
            get { return new DelegateCommand(ExecuteRefreshSchedule, CanExecuteRefreshSchedule); }
        }

        public Visibility Loading
        {
            get
            {
                if(mIsLoadingCourses)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }

        private bool CanExecuteRefreshSchedule(object parameter)
        {
            return true;
        }

        private void ExecuteRefreshSchedule(object parameter)
        {
            Settings.DeleteCourses();
            Courses = new ObservableCollection<Course>();

            //TODO : CHECK FOR RIGHT COOKIE
            Cookie simsCookie = CookieService.GetCookieWithName("https%3a%2f%2fgo.sfu.ca%2fpsp%2fgoprd%2fsfu_site%2fentp%2frefresh");

            if (simsCookie != null)
            {
                if (simsCookie.Expired)
                {
                    var available = NetworkInterface.GetIsNetworkAvailable();
#if DEBUG
                    available = true;
#endif
                    if (available)
                    {
                        mIsLoadingCourses = true;
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            OnPropertyChanged("Loading");
                        });
            
                        GetClasses();
                    }
                    else
                    {
                        MessageBox.Show("No internet connection available. Courses will not load without it.");
                    }
                }
                else
                {
                    //Cookie 
                    //check that one of the sims cookies hasnt expired as well
                    GetSIMSResponseWithCookies();
                }
            }
            else
            {
                GetClasses();
            }

        }


        private void GetClasses()
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://go.sfu.ca/psp/paprd/?cmd=login&languageCd=ENG");
            request.CookieContainer = new CookieContainer();
            foreach (Cookie cookie in CookieService.GetCookies().Where(c => !c.Name.Equals("CASTGC")))
            {
                if (cookie.Domain == ".sfu.ca")
                    request.CookieContainer.Add(new Uri("http://www" + cookie.Domain + cookie.Path), cookie);
                else
                    request.CookieContainer.Add(new Uri("http://" + cookie.Domain + cookie.Path), cookie);
            }
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            request.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows Phone 8.0; Trident/6.0; IEMobile/10.0; ARM; Touch; NOKIA; Lumia 920)";
            request.BeginGetRequestStream(new AsyncCallback(GetSIMSRequestStream), request);


        }

        private void GetSIMSRequestStream(IAsyncResult asyncResult)
        {
            HttpWebRequest request = (HttpWebRequest)asyncResult.AsyncState;
            Stream stream = request.EndGetRequestStream(asyncResult);
            string loginData = string.Format("user={0}&pwd={1}&userid={2}&Submit=Login", Settings.ComputingId, Settings.Password, Settings.ComputingId.ToUpper());
            byte[] bytes = Encoding.UTF8.GetBytes(loginData);
            stream.Write(bytes, 0, loginData.Length);
            stream.Close();
            request.BeginGetResponse(new AsyncCallback(GetSIMSResponse), request);

        }

        private void GetSIMSResponse(IAsyncResult result)
        {
            HttpWebRequest request = (HttpWebRequest)result.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);

            Cookie casCookie = CookieService.GetCookieWithName("CASTGC");

            CookieService.DeleteCookies();
            if (casCookie != null)
            {
                CookieService.AddCookie(casCookie);
            }
            CookieCollection cookies = request.CookieContainer.GetCookies(new Uri("https://go.sfu.ca"));
            foreach (Cookie cookie in cookies)
                CookieService.AddCookie(cookie);
            //Settings.GetStudentId();
            string request2String = string.Format("https://sims-prd.sfu.ca/psc/csprd_1/EMPLOYEE/HRMS/c/SA_LEARNER_SERVICES.SS_ES_STUDY_LIST.GBL?Page=SS_ES_STUDY_LIST&Action=U&ACAD_CAREER=UGRD&EMPLID=&INSTITUTION=SFUNV&STRM={0}", SemesterHelper.GetSemesterId());
            HttpWebRequest request2 = (HttpWebRequest)HttpWebRequest.Create(request2String);
            request2.Method = "GET";
            request2.CookieContainer = new CookieContainer();
            
            foreach(Cookie cookie in cookies)
            {
                if (cookie.Domain == ".sfu.ca")
                    request2.CookieContainer.Add(new Uri("http://www" + cookie.Domain), cookie);
                else
                    request2.CookieContainer.Add(new Uri("https://" + cookie.Domain), cookie);
            }
            
            
            request2.BeginGetResponse(new AsyncCallback(GetClassesResponse), request2);
        }

        private void GetSIMSResponseWithCookies()
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(string.Format("https://sims-prd.sfu.ca/psc/csprd_1/EMPLOYEE/HRMS/c/SA_LEARNER_SERVICES.SS_ES_STUDY_LIST.GBL?Page=SS_ES_STUDY_LIST&Action=U&ACAD_CAREER=UGRD&EMPLID=&INSTITUTION=SFUNV&STRM={0}", SemesterHelper.GetSemesterId()));
            
            request.CookieContainer = new CookieContainer();
            foreach (Cookie cookie in CookieService.GetCookies().Where(c => c.Domain != "cas.sfu.ca"))
            {
                if (cookie.Domain == ".sfu.ca")
                    request.CookieContainer.Add(new Uri("http://www" + cookie.Domain + cookie.Path), cookie);
                else
                    request.CookieContainer.Add(new Uri("http://" + cookie.Domain + cookie.Path), cookie);
            }
            request.BeginGetResponse(new AsyncCallback(GetClassesResponse), request);
        }

        private void GetClassesResponse(IAsyncResult result)
        {
            HttpWebRequest request = (HttpWebRequest)result.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string classString = reader.ReadToEnd();
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(classString);
            ParseClasses(document);

        }

        private void ParseClasses(HtmlDocument document)
        {
            int classIndex = 0;
            int detailsIndex = 0;
            int profCount = 0;
            List<Course> courses = new List<Course>();
            while(document.GetElementbyId("CLASS_TBL_VW_CLASS_SECTION$" + classIndex) != null)
            {
                HtmlNode classDescription = document.GetElementbyId("win1divDERIVED_SSE_DSP_CLASS_DESCR$" + classIndex);
                
                string className = classDescription.FirstChild.InnerText;

                string section = document.GetElementbyId("CLASS_TBL_VW_CLASS_SECTION$" + classIndex).InnerText;
                string type = document.GetElementbyId("PSXLATITEM_XLATSHORTNAME$95$$" + classIndex).InnerText;
                string credits = document.GetElementbyId("STDNT_ENRL_SSVW_UNT_TAKEN$" + classIndex).InnerText;
                string classStatus = document.GetElementbyId("PSXLATITEM_XLATSHORTNAME$" + classIndex).InnerText;
                string profName = string.Empty;

                

                try
                {
                    profName = document.GetElementbyId("PERSONAL_VW_NAME$135$$" + profCount).InnerText;
                }
                catch (NullReferenceException) { }
                
                Course course = new Course(className, section, credits, classStatus, profName, type);

                if(!string.IsNullOrEmpty(profName))
                {
                    profCount++;
                }
                HtmlNode node = document.GetElementbyId("CLASS_TBL_VW_CLASS_SECTION$" + (classIndex + 1));
                if (node != null)
                {
                    while (document.GetElementbyId("CLASS_MTG_VW_MEETING_TIME_START$" + detailsIndex) != null && document.GetElementbyId("win1divCLASS_MTG_VW_MEETING_TIME_START$" + detailsIndex).Line < document.GetElementbyId("CLASS_TBL_VW_CLASS_SECTION$" + (classIndex + 1)).Line)
                    {
                        string startTime = string.Empty;
                        string endTime = string.Empty;
                        string location = string.Empty;
                        string days = string.Empty;
                        string date = string.Empty;
                        try
                        {
                            startTime = document.GetElementbyId("CLASS_MTG_VW_MEETING_TIME_START$" + detailsIndex).InnerText;
                            endTime = document.GetElementbyId("CLASS_MTG_VW_MEETING_TIME_END$" + detailsIndex).InnerText;
                            location = document.GetElementbyId("DERIVED_SSE_DSP_DESCR40$" + detailsIndex).InnerText;
                            days = document.GetElementbyId("DERIVED_SSE_DSP_CLASS_MTG_DAYS$" + detailsIndex).InnerText;
                            date = document.GetElementbyId("DERIVED_SSE_DSP_START_DT$" + detailsIndex).InnerText;
                        }
                        catch (NullReferenceException) { }
                        
                        CourseOffering courseOffering = new CourseOffering(startTime, endTime, location, days, date);
                        course.AddCourseOffering(courseOffering);

                        detailsIndex++;
                    }
                    
                }
                classIndex++;
                courses.Add(course);
                mIsLoadingCourses = false;
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    OnPropertyChanged("Courses");
                });
            

            }
            foreach(Course course in courses)
            {
                if (course.Type == "Lecture")
                {
                    List<CourseOffering> offerings = course.CourseOfferings;
                    offerings.RemoveAt(offerings.Count - 1);
                }
                
            }
            Courses =  new ObservableCollection<Course>(courses);
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                OnPropertyChanged("Courses");
            });
            

            Settings.SaveCourses(courses);

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
