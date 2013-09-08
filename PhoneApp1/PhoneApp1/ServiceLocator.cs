using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneApp1
{
    public static class ServiceLocator
    {
        private static List<object> services;
        public static List<object> Instance
        {
            get
            {
                if (services == null)
                {
                    services = new List<object>();
                }
                return services;
            }
        }

        public static T GetService<T>()
        {
            return (T)Instance.Where(s => s.GetType() == typeof(T)).FirstOrDefault();
        }

        public static void AddService<T>(object service)
        {
            Instance.Add(service);
        }

        public static void RemoveService<T>()
        {
            object ser = Instance.Where(s => s.GetType() == typeof(T)).FirstOrDefault();
            Instance.Remove(ser);
        }

    }
}
