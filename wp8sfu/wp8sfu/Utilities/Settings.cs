using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using wp8sfu.Entities;

namespace wp8sfu.Utilities
{
    public static class Settings
    {
        private static Dictionary<string, string> settings;
        public static Dictionary<string, string> Instance
        {
            get
            {
                if (settings == null)
                {
                    settings = new Dictionary<string, string>(); 
                }
                return settings;
            }
        }

        public static void AddSetting(string key, string value)
        {
            Instance.Add(key, value);
        }

        public static string GetSetting(string key)
        {
            string data;
            Instance.TryGetValue(key, out data);
            return data;
        }

        public static void RemoveSetting(string key)
        {
            Instance.Remove(key);
        }

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

        public static void LoadSettings()
        {
            
        }

    }
}
