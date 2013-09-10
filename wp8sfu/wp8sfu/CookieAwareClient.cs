using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace wp8sfu
{
    public class CookieAwareClient : WebClient
    {

        private CookieContainer cookie;

    public CookieContainer Cookie { get { return cookie; } }

    public CookieAwareClient(CookieContainer givenContainer) {
        cookie = givenContainer;
    }

        [System.Security.SecuritySafeCritical]
        public CookieAwareClient()
            : base()
        {
        }
        private CookieContainer m_container = new CookieContainer();
        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            if (request is HttpWebRequest)
            {
                (request as HttpWebRequest).CookieContainer = m_container;
            }
            return request;
        }

    }
}
