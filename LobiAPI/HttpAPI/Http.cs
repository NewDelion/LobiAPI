using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Net.Http.Headers;

namespace LobiAPI.HttpAPI
{
    public class Http
    {
        public CookieContainer cookie = new CookieContainer();

        public async Task<string> get(string url, HttpRequestMessage header){
            string result = "";
            using(HttpClientHandler handler = new HttpClientHandler())
            using(HttpClient client = new HttpClient(handler)){
                if(header.Headers.Host != "")
                    client.DefaultRequestHeaders.Host = header.Headers.Host;
                if(header.Headers.Accept.Count > 0)
                    foreach(var accept in header.Headers.Accept)
                        client.DefaultRequestHeaders.Accept.Add(accept);
                if(header.Headers.UserAgent.Count > 0)
                    foreach(var useragent in header.Headers.UserAgent)
                        client.DefaultRequestHeaders.UserAgent.Add(useragent);
                if(header.Headers.Referrer != null)
                    client.DefaultRequestHeaders.Referrer = header.Headers.Referrer;
                if(header.Headers.AcceptEncoding.Count > 0)
                    foreach(var encoding in header.Headers.AcceptEncoding)
                        client.DefaultRequestHeaders.AcceptEncoding.Add(encoding);
                if(header.Headers.AcceptLanguage.Count > 0)
                    foreach(var lang in header.Headers.AcceptLanguage)
                        client.DefaultRequestHeaders.AcceptLanguage.Add(lang);
                handler.CookieContainer = this.cookie;
                var response = await client.GetAsync(url);
                result = await response.Content.ReadAsStringAsync();
            }
            return result;
        }
        public async Task<string> post_x_www_form_urlencoded(string url, FormUrlEncodedContent content, HttpRequestMessage header)
        {
            string result = "";
            using(HttpClientHandler handler = new HttpClientHandler())
            using(HttpClient client = new HttpClient(handler)){
                if(header.Headers.Host != "")
                    client.DefaultRequestHeaders.Host = header.Headers.Host;
                if(header.Headers.Accept.Count > 0)
                    foreach(var accept in header.Headers.Accept)
                        client.DefaultRequestHeaders.Accept.Add(accept);
                if(header.Headers.UserAgent.Count > 0)
                    foreach(var useragent in header.Headers.UserAgent)
                        client.DefaultRequestHeaders.UserAgent.Add(useragent);
                if(header.Headers.Referrer != null)
                    client.DefaultRequestHeaders.Referrer = header.Headers.Referrer;
                if(header.Headers.AcceptEncoding.Count > 0)
                    foreach(var encoding in header.Headers.AcceptEncoding)
                        client.DefaultRequestHeaders.AcceptEncoding.Add(encoding);
                if(header.Headers.AcceptLanguage.Count > 0)
                    foreach(var lang in header.Headers.AcceptLanguage)
                        client.DefaultRequestHeaders.AcceptLanguage.Add(lang);
                handler.CookieContainer = this.cookie;
                var response = await client.PostAsync(url, content);
                result = await response.Content.ReadAsStringAsync();
            }
            return result;
        }
    }
}
