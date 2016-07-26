using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LobiAPI.HttpAPI.Header
{
    public class Header
    {
        public string Host;
        public bool Connection = true;
        public string Accept;
        public string UserAgent;
        public string Referer;
        public string AcceptEncoding;
        public string AcceptLanguage;
        public string Origin;
        public string ContentType;
        
        public Header setHost(string host){
            this.Host = host;
            return this;
        }
        public Header setConnection(bool connection){
            this.Connection = connection;
            return this;
        }
        public Header setAccept(string accept){
            this.Accept = accept;
            return this;
        }
        public Header setUserAgent(string useragent){
            this.UserAgent = useragent;
            return this;
        }
        public Header setReferer(string referer){
            this.Referer = referer;
            return this;
        }
        public Header setAcceptEncoding(string encoding){
            this.AcceptEncoding = encoding;
            return this;
        }
        public Header setAcceptLanguage(string lang){
            this.AcceptLanguage = lang;
            return this;
        }
        public Header setOrigin(string origin){
            this.Origin = origin;
            return this;
        }
        public Header setContentType(string contenttype){
            this.ContentType = contenttype;
            return this;
        }
    }
}
