using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.IO;

using LobiAPI.HttpAPI.Header;

namespace LobiAPI.HttpAPI
{
    public class Http
    {
        public CookieContainer cookie = new CookieContainer();

        public async Task<string> get(string url, HttpRequestHeaders header){
            string result = "";
            using(HttpClientHandler handler = new HttpClientHandler())
            using(HttpClient client = new HttpClient()){
                if(header.Host != "")
                    client.DefaultRequestHeaders.Host = header.Host;
                if(header.Accept.Count > 0)
                    foreach(var accept in header.Accept)
                        client.DefaultRequestHeaders.Accept.Add(accept);
                if(header.UserAgent.Count > 0)
                    foreach(var useragent in header.UserAgent)
                        client.DefaultRequestHeaders.UserAgent.Add(useragent);
                if(header.Referer != null)
                    client.DefaultRequestHeaders.Referer = header.Referer;
                if(header.AcceptEncoding.Count > 0)
                    foreach(var encoding in header.AcceptEncoding)
                        client.DefaultRequestHeaders.AcceptEncoding.Add(encoding);
                if(header.AcceptLanguage.Count > 0)
                    foreach(var lang in header.AcceptLanguage)
                        client.DefaultRequestHeaders.AcceptLanguage.Add(lang);
                handler.CookieContainer = this.cookie;
                var response = await client.GetAsync(url);
                if(response.StatusCode == HttpStatusCode.OK){
                    result = await response.Content.ReadAsStringAsync();
                }
            }
            return result;
        }
        public async Task<string> post_x_www_form_urlencoded(string url, FormUrlEncodedContent content, HttpRequestHeaders header)
        {
            string result = "";
            using(HttpClientHandler handler = new HttpClientHandler())
            using(HttpClient client = new HttpClient()){
                if(header.Host != "")
                    client.DefaultRequestHeaders.Host = header.Host;
                if(header.Accept.Count > 0)
                    foreach(var accept in header.Accept)
                        client.DefaultRequestHeaders.Accept.Add(accept);
                if(header.UserAgent.Count > 0)
                    foreach(var useragent in header.UserAgent)
                        client.DefaultRequestHeaders.UserAgent.Add(useragent);
                if(header.Referer != null)
                    client.DefaultRequestHeaders.Referer = header.Referer;
                if(header.AcceptEncoding.Count > 0)
                    foreach(var encoding in header.AcceptEncoding)
                        client.DefaultRequestHeaders.AcceptEncoding.Add(encoding);
                if(header.AcceptLanguage.Count > 0)
                    foreach(var lang in header.AcceptLanguage)
                        client.DefaultRequestHeaders.AcceptLanguage.Add(lang);
                handler.CookieContainer = this.cookie;
                var response = await client.PostAsync(url, content);
                if(response.StatusCode == HttpStatusCode.OK){
                    result = await response.Content.ReadAsStringAsync();
                }
            }
            return result;
        }
    }
}
