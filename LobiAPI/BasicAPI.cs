using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LobiAPI.HttpAPI;
using LobiAPI.HttpAPI.Header;
using LobiAPI.Json;

using Newtonsoft.Json;

namespace LobiAPI
{
    public class BasicAPI
    {
        private Http NetworkAPI = null;

        public BasicAPI()
        {
            this.NetworkAPI = new Http();
        }

        public void Login(string mail, string password)
        {
            GetHeader header1 = new GetHeader()
                .setHost("lobi.co")
                .setConnection(true)
                .setAccept("text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8")
                .setUserAgent("Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.110 Safari/537.36")
                .setAcceptLanguage("ja,en-US;q=0.8,en;q=0.6");

            string source = this.NetworkAPI.get("https://lobi.co/signin", header1);
            string csrf_token = Pattern.get_string(source, Pattern.csrf_token, "\"");

            string post_data = string.Format("csrf_token={0}&email={1}&password={2}", csrf_token, mail, password);
            PostHeader header2 = new PostHeader()
                .setHost("lobi.co")
                .setConnection(true)
                .setAccept("text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8")
                .setUserAgent("Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.110 Safari/537.36")
                .setAcceptLanguage("ja,en-US;q=0.8,en;q=0.6")
                .setOrigin("https://lobi.co")
                .setReferer("https://lobi.co/signin");

            this.NetworkAPI.post_x_www_form_urlencoded("https://lobi.co/signin", post_data, header2);
        }

        public Me GetMe()
        {
            GetHeader header = new GetHeader()
                .setHost("web.lobi.co")
                .setConnection(true)
                .setAccept("application/json, text/plain, */*")
                .setUserAgent("Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.110 Safari/537.36")
                .setAcceptLanguage("ja,en-US;q=0.8,en;q=0.6");

            return JsonConvert.DeserializeObject<Me>(this.NetworkAPI.get("https://web.lobi.co/api/me?fields=premium", header));
        }

        public PublicGroups[] GetPublicGroupList()
        {
            GetHeader header = new GetHeader()
                .setHost("web.lobi.co")
                .setConnection(true)
                .setAccept("application/json, text/plain, */*")
                .setUserAgent("Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.110 Safari/537.36")
                .setAcceptLanguage("ja,en-US;q=0.8,en;q=0.6");

            return JsonConvert.DeserializeObject<PublicGroups[]>(this.NetworkAPI.get("https://web.lobi.co/api/public_groups?count=1000&page=1&with_archived=1", header));
        }

        public PrivateGroups[] GetPrivateGroupList()
        {
            GetHeader header = new GetHeader()
                .setHost("web.lobi.co")
                .setConnection(true)
                .setAccept("application/json, text/plain, */*")
                .setUserAgent("Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.110 Safari/537.36")
                .setAcceptLanguage("ja,en-US;q=0.8,en;q=0.6");

            return JsonConvert.DeserializeObject<PrivateGroups[]>(this.NetworkAPI.get("https://web.lobi.co/api/groups?count=1000&page=1", header));
        }

        public Notifications GetNotifications()
        {
            GetHeader header = new GetHeader()
                .setHost("web.lobi.co")
                .setConnection(true)
                .setAccept("application/json, text/plain, */*")
                .setUserAgent("Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.110 Safari/537.36")
                .setAcceptLanguage("ja,en-US;q=0.8,en;q=0.6");

            return JsonConvert.DeserializeObject<Notifications>(this.NetworkAPI.get("https://web.lobi.co/api/info/notifications?platform=any&last_cursor=0", header));
        }
        


        private class Pattern
        {
            public static string csrf_token = "<input type=\"hidden\" name=\"csrf_token\" value=\"";
            public static string get_string(string source, string pattern, string end_pattern)
            {
                int start = source.IndexOf(pattern) + pattern.Length;
                int end = source.IndexOf(end_pattern, start + 1);
                return source.Substring(start, end - start);
            }
        }
    }
}
