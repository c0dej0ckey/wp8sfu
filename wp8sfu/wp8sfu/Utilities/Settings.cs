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
        public static string ComputingId { get; set; }
        public static string Password { get; set; }
        public static int StudentId { get; set; }

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
