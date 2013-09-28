using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using wp8sfu.Entities;
using wp8sfu.Services;

namespace wp8sfu.Utilities
{
    public static class Settings
    {
        private static string sComputingId = "COMPUTINGID";
        private static string sPassword = "PASSWORD";
        private static string sStudentId = "STUDENTID";

        public static string ComputingId
        {
            get
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains(sComputingId))
                {
                    var bytes = IsolatedStorageSettings.ApplicationSettings[sComputingId] as byte[];
                    var unEncryptedBytes = ProtectedData.Unprotect(bytes, null);
                    return Encoding.UTF8.GetString(unEncryptedBytes, 0, unEncryptedBytes.Length);
                }
                else
                    return string.Empty;
            }
            set
            {
                var encryptedBytes = ProtectedData.Protect(Encoding.UTF8.GetBytes(value), null);
                IsolatedStorageSettings.ApplicationSettings[sComputingId] = encryptedBytes;
                IsolatedStorageSettings.ApplicationSettings.Save();
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
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
        }

        //public static string StudentId
        //{
        //    get
        //    {
        //        if (IsolatedStorageSettings.ApplicationSettings.Contains(sStudentId))
        //        {
        //            var bytes = IsolatedStorageSettings.ApplicationSettings[sStudentId] as byte[];
        //            var unEncryptedBytes = ProtectedData.Unprotect(bytes, null);
        //            return Encoding.UTF8.GetString(unEncryptedBytes, 0, unEncryptedBytes.Length);
        //        }
        //        else
        //            return string.Empty;
        //    }
        //    set
        //    {
        //        var encryptedBytes = ProtectedData.Protect(Encoding.UTF8.GetBytes(value), null);
        //        IsolatedStorageSettings.ApplicationSettings[sStudentId] = encryptedBytes;
        //        IsolatedStorageSettings.ApplicationSettings.Save();
        //    }
        //}


        //public static void GetStudentId()
        //{
        //    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://go.sfu.ca/psp/goprd_2/SFU_SITE/SA/c/SA_LEARNER_SERVICES.SSS_STUDENT_CENTER.GBL");
        //    request.CookieContainer = new CookieContainer();
        //    foreach (Cookie cookie in CookieService.GetCookies().Where(c => c.Domain != "cas.sfu.ca"))
        //    {
        //        if (cookie.Domain == ".sfu.ca")
        //            request.CookieContainer.Add(new Uri("http://www" + cookie.Domain + cookie.Path), cookie);
        //        else
        //            request.CookieContainer.Add(new Uri("http://" + cookie.Domain + cookie.Path), cookie);
        //    }
        //    request.Method = "GET";
        //    request.BeginGetResponse(new AsyncCallback(GetStudentIdResponse), request);
        //}

        //private static void GetStudentIdResponse(IAsyncResult result)
        //{
        //    HttpWebRequest request = (HttpWebRequest)result.AsyncState;
        //    HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
        //    Stream stream = response.GetResponseStream();
        //    StreamReader reader = new StreamReader(stream);
        //    StringBuilder sb = new StringBuilder();
        //    string line = null;
        //    while ((line = reader.ReadLine()) != null)
        //    {
        //        sb.Append(line);
        //    }
        //    HtmlDocument document = new HtmlDocument();
        //    document.LoadHtml(sb.ToString());
        //    HtmlNode idNode = document.GetElementbyId("DERIVED_SSS_SCL_EMPLID");
        //    StudentId = idNode.InnerText;


        //}



        public static void SaveCourses(List<Course> courses)
        {
            try
            {
                IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();
                JsonSerializer serializer = new JsonSerializer();
                using (StreamWriter sw = new StreamWriter(new IsolatedStorageFileStream("classes.json", System.IO.FileMode.OpenOrCreate, fileStorage)))
                {
                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                        serializer.Serialize(writer, courses);
                    }
                }
            }
            catch
            {
                
            }

        }

        public static List<Course> LoadCourses()
        {
            try
            {
                IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();
                JsonSerializer serializer = new JsonSerializer();
                using(StreamReader reader = new StreamReader(new IsolatedStorageFileStream("classes.json", System.IO.FileMode.Open, fileStorage)))
                {
                    using(JsonReader jr = new JsonTextReader(reader))
                    {
                        List<Course> courses = new List<Course>();
                        courses = serializer.Deserialize<List<Course>>(jr);
                        return courses;
                    }
                }
            }
            catch
            {
                
            }
            return null;
        }

        public static void DeleteCourses()
        {
            try
            {
                IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();
                if(fileStorage.FileExists("classes.json"))
                {
                    fileStorage.DeleteFile("classes.json");
                }
            }
            catch
            {
                
            }
        }


    }
}
