using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace wp8sfu.Entities
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SerializationCookie
    {
        [JsonProperty]
        public Cookie Cookie;

        public SerializationCookie(Cookie cookie)
        {
            Cookie = cookie;
        }
    }
}
