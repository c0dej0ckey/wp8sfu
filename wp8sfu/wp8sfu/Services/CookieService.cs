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

        public static void DeleteCookies()
        {
            mCookies.Clear();
        }

        public static Cookie GetCookieWithName(string name)
        {
            return mCookies.Where(c => c.Name == name).FirstOrDefault();
        }

        public static void RemoveCookieWithName(string name)
        {
            Cookie cookie = mCookies.Where(c => c.Name == name).FirstOrDefault();
            mCookies.Remove(cookie);
        }

        public static bool CookieExists(string name)
        {
            return mCookies.Where(c => c.Name == name).FirstOrDefault() != null;
        }


    }
}
