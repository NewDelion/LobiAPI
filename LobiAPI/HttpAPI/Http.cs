using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Net;
using System.IO;

using LobiAPI.HttpAPI.Header;

namespace LobiAPI.HttpAPI
{
    public class Http
    {
        public CookieContainer cookie = new CookieContainer();

        public string get(string url, GetHeader header)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "GET";
            if (header.Host != "")
                req.Host = header.Host;
            req.KeepAlive = header.Connection;
            if (header.Accept != "")
                req.Accept = header.Accept;
            if (header.UserAgent != "")
                req.UserAgent = header.UserAgent;
            if (header.Referer != "")
                req.Referer = header.Referer;
            if (header.AcceptEncoding != "")
                req.Headers.Add("Accept-Encoding", header.AcceptEncoding);
            if (header.AcceptLanguage != "")
                req.Headers.Add("Accept-Language", header.AcceptLanguage);
            req.CookieContainer = this.cookie;

            string result = "";
            using (HttpWebResponse res = (HttpWebResponse)req.GetResponse())
            {
                using (StreamReader reader = new StreamReader(res.GetResponseStream(), Encoding.UTF8))
                    result = reader.ReadToEnd();
            }

            return result;
        }
        public string post(string url, PostHeader header, string data)
        {
            byte[] data_bytes = Encoding.UTF8.GetBytes(data);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            if (header.Host != "")
                req.Host = header.Host;
            req.KeepAlive = header.Connection;
            if (header.Accept != "")
                req.Accept = header.Accept;
            if (header.Origin != "")
                req.Headers.Add("Origin", header.Origin);
            if (header.UserAgent != "")
                req.UserAgent = header.UserAgent;
            if (header.ContentType != "")
                req.ContentType = header.ContentType;
            if (header.Referer != "")
                req.Referer = header.Referer;
            if (header.AcceptEncoding != "")
                req.Headers.Add("Accept-Encoding", header.AcceptEncoding);
            if (header.AcceptLanguage != "")
                req.Headers.Add("Accept-Language", header.AcceptLanguage);
            req.CookieContainer = this.cookie;
            using (Stream stream = req.GetRequestStream())
                stream.Write(data_bytes, 0, data_bytes.Length);

            string result = "";
            using (HttpWebResponse res = (HttpWebResponse)req.GetResponse())
            {
                using (StreamReader reader = new StreamReader(res.GetResponseStream(), Encoding.UTF8))
                    result = reader.ReadToEnd();
            }

            return result;
        }
        public string post_x_www_form_urlencoded(string url, string data, PostHeader header)
        {
            byte[] data_bytes = Encoding.UTF8.GetBytes(data);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            if (header.Host != "")
                req.Host = header.Host;
            req.KeepAlive = header.Connection;
            if (header.Accept != "")
                req.Accept = header.Accept;
            if (header.Origin != "")
                req.Headers.Add("Origin", header.Origin);
            if (header.UserAgent != "")
                req.UserAgent = header.UserAgent;
            req.ContentType = "application/x-www-form-urlencoded";
            if (header.Referer != "")
                req.Referer = header.Referer;
            if (header.AcceptEncoding != "")
                req.Headers.Add("Accept-Encoding", header.AcceptEncoding);
            if (header.AcceptLanguage != "")
                req.Headers.Add("Accept-Language", header.AcceptLanguage);
            req.CookieContainer = this.cookie;
            using (Stream stream = req.GetRequestStream())
                stream.Write(data_bytes, 0, data_bytes.Length);

            string result = "";
            using (HttpWebResponse res = (HttpWebResponse)req.GetResponse())
            {
                using (StreamReader reader = new StreamReader(res.GetResponseStream(), Encoding.UTF8))
                    result = reader.ReadToEnd();
            }

            return result;
        }
        public string post_form_data(string url, string boundary, string[] data, PostHeader header)
        {
            string post_data = "";
            foreach (string cdata in data)
                post_data += "--" + boundary + "\n" + cdata + "\n";
            post_data += "--" + boundary + "--";

            byte[] data_bytes = Encoding.UTF8.GetBytes(post_data);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            if (header.Host != "")
                req.Host = header.Host;
            req.KeepAlive = header.Connection;
            if (header.Accept != "")
                req.Accept = header.Accept;
            if (header.Origin != "")
                req.Headers.Add("Origin", header.Origin);
            if (header.UserAgent != "")
                req.UserAgent = header.UserAgent;
            req.ContentType = "multipart/form-data; boundary=" + boundary;
            if (header.Referer != "")
                req.Referer = header.Referer;
            if (header.AcceptEncoding != "")
                req.Headers.Add("Accept-Encoding", header.AcceptEncoding);
            if (header.AcceptLanguage != "")
                req.Headers.Add("Accept-Language", header.AcceptLanguage);
            req.CookieContainer = this.cookie;
            using (Stream stream = req.GetRequestStream())
                stream.Write(data_bytes, 0, data_bytes.Length);

            string result = "";
            using (HttpWebResponse res = (HttpWebResponse)req.GetResponse())
            {
                using (StreamReader reader = new StreamReader(res.GetResponseStream(), Encoding.UTF8))
                    result = reader.ReadToEnd();
            }

            return result;
        }
    }
}
