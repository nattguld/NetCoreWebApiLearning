using NgHTTP.Proxies.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Models {
    public class PornhubVideoCommentItem {

        public int Id { get; set; }

        public string ProxyHost { get; set; }

        public int ProxyPort { get; set; }

        public string ProxyUsername { get; set; }

        public string ProxyPassword { get; set; }

        public string PornhubUsername { get; set; }

        public string PornhubPassword { get; set; }

        public string VideoUrl { get; set; }

        public string CommentText { get; set; }

    }

}
