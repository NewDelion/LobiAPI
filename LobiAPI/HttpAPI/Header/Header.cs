using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LobiAPI.HttpAPI.Header
{
    public class BasicHeader
    {
        public string Host;
        public bool Connection = true;
        public string Accept;
        public string UserAgent;
        public string Referer;
        public string AcceptEncoding;
        public string AcceptLanguage;
        public string Origin;

        public BasicHeader setHost(string host)
        {
            this.Host = host;
            return this;
        }
        public BasicHeader setConnection(bool connection)
        {
            this.Connection = connection;
            return this;
        }
        public BasicHeader setAccept(string accept)
        {
            this.Accept = accept;
            return this;
        }
        public BasicHeader setUserAgent(string useragent)
        {
            this.UserAgent = useragent;
            return this;
        }
        public BasicHeader setReferer(string referer)
        {
            this.Referer = referer;
            return this;
        }
        public BasicHeader setAcceptEncoding(string acceptencoding)
        {
            this.AcceptEncoding = acceptencoding;
            return this;
        }
        public BasicHeader setAcceptLanguage(string acceptlanguage)
        {
            this.AcceptLanguage = acceptlanguage;
            return this;
        }
        public BasicHeader setOrigin(string origin)
        {
            this.Origin = origin;
            return this;
        }
    }
    public class GetHeader : BasicHeader
    {
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
    public class PostHeader : BasicHeader
    {
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
