using NgHTTP.Proxies.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Models {
    public class ProxyItem {

        public string Host { get; }

        public string Port { get; }

        public ProxyCredentials ProxyCreds { get; }

    }

}
