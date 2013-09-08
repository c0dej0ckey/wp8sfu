using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace PhoneApp1
{
    public static class Extensions
    {
            private static Dictionary<object, object> _navigationData = null;

            public static void Navigate(this NavigationService service, string page, object key, object value)
            {
                _navigationData.Add(key, value);
                service.Navigate(new Uri(page, UriKind.Relative));
            }

            public static object GetLastNavigationData(this NavigationService service, object key)
            {
                object data;
                _navigationData.TryGetValue(key, out data);
                _navigationData = null;
                return data;
            }
    }
}
