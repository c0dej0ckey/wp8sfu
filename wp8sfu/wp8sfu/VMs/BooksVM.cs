using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using wp8sfu.Entities;
using wp8sfu.Utilities;

namespace wp8sfu.VMs
{
    public class BooksVM : INotifyPropertyChanged
    {
        private ObservableCollection<Book> mBooks;

        public BooksVM()
        {
            Books = new ObservableCollection<Book>();
            List<Course> courses = Settings.LoadCourses();
            courses = courses.Where(c => c.Type == "Lecture").ToList();
            if(courses == null || courses.Count() == 0)
            {
                //error refresh schedule
            }
            else
            {
                HttpWebRequest request = null;
                foreach(Course course in courses)
                {
                request = (HttpWebRequest)HttpWebRequest.Create(string.Format("http://sfu.collegestoreonline.com/ePOS?form=shared3/textbooks/json/json_books.html&term={0}&dept={1}&crs={2}&sec={3}&go=Go", SemesterHelper.GetSemesterId(), Regex.Split(course.ClassName, @"(\w+)(\d)")[0].Trim().ToLower(), Regex.Split(course.ClassName, @"(\d+)")[1], course.Section));
                    request.Method = "GET";
                    request.BeginGetResponse(new AsyncCallback(GetBookResponse), request);
                }

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {

                    OnPropertyChanged("Books");
                });

            }


        }

        public ObservableCollection<Book> Books
        {
            get { return this.mBooks; }
            set { this.mBooks = value; }
        }


        private void GetBookResponse(IAsyncResult result)
        {
            HttpWebRequest request = (HttpWebRequest)result.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
            Stream stream = response.GetResponseStream();
            using(StreamReader reader = new StreamReader(stream))
            {
                string json = reader.ReadToEnd();
                JObject jsonClass = JObject.Parse(json);

                JObject course = (JObject)jsonClass["course"];

                JArray bookArray = (JArray)course["books"];
                foreach(JObject book in bookArray)
                {
                    string title = book["title"].ToString();
                    if(title == "No Books Found")
                    {
                        continue;
                    }
                    string className = course["courseAcdeptcode"].ToString();
                    string classNumber = course["courseClass"].ToString();
                    
                    string author = book["author"].ToString();
                    string status = book["bookstatus"].ToString();
                    string isbn = book["isbn"].ToString();
                    JArray detailsArray = (JArray)book["details"];
                    string newPrice = string.Empty;
                    string usedPrice = string.Empty;
                    foreach(JObject detail in detailsArray)
                    {
                        if(detail["isNew"].ToString() == "1")
                        {
                             newPrice = detail["price"].ToString();
                        }
                        else if(detail["isUsed"].ToString() == "1")
                        {
                             usedPrice = detail["price"].ToString();
                        }
                    }
                    float newP;
                    float.TryParse(newPrice, out newP);
                    float usedP;
                    float.TryParse(usedPrice, out usedP);
                    Book bk = new Book(className, classNumber, title, author, status, isbn, newP, usedP);
                    
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {

                        Books.Add(bk);
                        OnPropertyChanged("Books");
                    });
                    
                }
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
