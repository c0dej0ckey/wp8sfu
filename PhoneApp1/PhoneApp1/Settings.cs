using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneApp1
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

    }
}
