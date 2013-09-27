using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using wp8sfu.Entities;

namespace wp8sfu.Services
{
    public static class CookieService
    {
        private static List<Cookie> mCookies;

        static CookieService()
        {
            mCookies = new List<Cookie>();
        }

        public static void AddCookie(Cookie cookie)
        {
            mCookies.Add(cookie);
        }

        public static void RemoveCookie(Cookie cookie)
        {
            mCookies.Remove(cookie);
        }

        public static List<Cookie> GetCookies()
        {
            return mCookies;
        }

        public static Cookie GetCookieWithName(string name)
        {
            return mCookies.Where(c => c.Name == name).FirstOrDefault();
        }

        public static void LoadCookies()
        {
            try
            {
                List<SerializationCookie> serializedCookies = new List<SerializationCookie>();
                IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();
                JsonSerializer serializer = new JsonSerializer();
                using (StreamReader reader = new StreamReader(new IsolatedStorageFileStream("cookies.json", System.IO.FileMode.Open, fileStorage)))
                {
                    using (JsonReader jr = new JsonTextReader(reader))
                    {
                        List<SerializationCookie> cookies = new List<SerializationCookie>();
                        serializedCookies = serializer.Deserialize<List<SerializationCookie>>(jr);
                    }
                    foreach(SerializationCookie cookie in serializedCookies)
                    {
                        mCookies.Add(cookie.Cookie);
                    }
                }
            }
            catch
            {

            }
        }

        public static void SaveCookies()
        {
            try
            {
                IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();
                JsonSerializer serializer = new JsonSerializer();

                List<SerializationCookie> serializedCookies = new List<SerializationCookie>();
                foreach(Cookie cookie in mCookies)
                {
                    serializedCookies.Add(new SerializationCookie(cookie));
                }

                using (StreamWriter sw = new StreamWriter(new IsolatedStorageFileStream("cookies.json", System.IO.FileMode.OpenOrCreate, fileStorage)))
                {
                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                        serializer.Serialize(writer, serializedCookies);
                    }
                }
            }
            catch
            {

            }
        }

        public static void DeleteCookies()
        {
            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();
            if (fileStorage.FileExists("cookies.json"))
                fileStorage.DeleteFile("cookies.json");
            Cookie casCookie = mCookies.Where(c => c.Domain == "cas.sfu.ca").FirstOrDefault();
            mCookies.Clear();
            mCookies.Add(casCookie);
        }

    }
}
