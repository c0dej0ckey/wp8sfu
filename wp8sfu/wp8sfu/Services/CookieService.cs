using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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

    }
}
