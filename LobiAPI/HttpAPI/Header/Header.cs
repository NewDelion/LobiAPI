using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LobiAPI.HttpAPI.Header
{
    public class GetHeader
    {
        public string Host;
        public bool Connection = true;
        public string Accept;
        public string UserAgent;
        public string Referer;
        public string AcceptEncoding;
        public string AcceptLanguage;
        public string Origin;
        
        public GetHeader setHost(string host)
        {
            this.Host = host;
            return this;
        }
        public GetHeader setConnection(bool connection)
        {
            this.Connection = connection;
            return this;
        }
        public GetHeader setAccept(string accept)
        {
            this.Accept = accept;
            return this;
        }
        public GetHeader setUserAgent(string useragent)
        {
            this.UserAgent = useragent;
            return this;
        }
        public GetHeader setReferer(string referer)
        {
            this.Referer = referer;
            return this;
        }
        public GetHeader setAcceptEncoding(string acceptencoding)
        {
            this.AcceptEncoding = acceptencoding;
            return this;
        }
        public GetHeader setAcceptLanguage(string acceptlanguage)
        {
            this.AcceptLanguage = acceptlanguage;
            return this;
        }
        public GetHeader setOrigin(string origin)
        {
            this.Origin = origin;
            return this;
        }
    }
    public class PostHeader
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

        public PostHeader setHost(string host)
        {
            this.Host = host;
            return this;
        }
        public PostHeader setConnection(bool connection)
        {
            this.Connection = connection;
            return this;
        }
        public PostHeader setAccept(string accept)
        {
            this.Accept = accept;
            return this;
        }
        public PostHeader setUserAgent(string useragent)
        {
            this.UserAgent = useragent;
            return this;
        }
        public PostHeader setReferer(string referer)
        {
            this.Referer = referer;
            return this;
        }
        public PostHeader setAcceptEncoding(string acceptencoding)
        {
            this.AcceptEncoding = acceptencoding;
            return this;
        }
        public PostHeader setAcceptLanguage(string acceptlanguage)
        {
            this.AcceptLanguage = acceptlanguage;
            return this;
        }
        public PostHeader setOrigin(string origin)
        {
            this.Origin = origin;
            return this;
        }

        public PostHeader setContentType(string contenttype)
        {
            this.ContentType = contenttype;
            return this;
        }

    }
}
