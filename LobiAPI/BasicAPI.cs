using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

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

        public bool Login(string mail, string password)
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

            return this.NetworkAPI.post_x_www_form_urlencoded("https://lobi.co/signin", post_data, header2).IndexOf("ログインに失敗しました") == -1;

        }

        public bool TwitterLogin(string mail, string password)
        {
            GetHeader header1 = new GetHeader()
                .setHost("lobi.co")
                .setConnection(true)
                .setAccept("text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8")
                .setUserAgent("Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.110 Safari/537.36")
                .setAcceptLanguage("ja,en-US;q=0.8,en;q=0.6");

            string source = this.NetworkAPI.get("https://lobi.co/signup/twitter", header1);
            string authenticity_token = Pattern.get_string(source, Pattern.authenticity_token, "\"");
            string redirect_after_login = Pattern.get_string(source, Pattern.redirect_after_login, "\"");
            string oauth_token = Pattern.get_string(source, Pattern.oauth_token, "\"");

            string post_data = string.Format("authenticity_token={0}&redirect_after_login={1}&oauth_token={2}&session%5Busername_or_email%5D={3}&session%5Bpassword%5D={4}", authenticity_token, WebUtility.UrlEncode(redirect_after_login), oauth_token, mail, password);
            PostHeader header2 = new PostHeader()
                .setHost("api.twitter.com")
                .setConnection(true)
                .setAccept("text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8")
                .setUserAgent("Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.110 Safari/537.36")
                .setAcceptLanguage("ja,en-US;q=0.8,en;q=0.6")
                .setOrigin("https://api.twitter.com");

            string source2 = this.NetworkAPI.post_x_www_form_urlencoded("https://api.twitter.com/oauth/authorize", post_data, header2);
            if (source2.IndexOf("Twitterにログイン") > -1)
                return false;

            return this.NetworkAPI.get(Pattern.get_string(source2, Pattern.twitter_redirect_to_lobi, "\""), header1).IndexOf("ログインに失敗しました") == -1;
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

            List<PublicGroups> result = new List<PublicGroups>();

            int index = 1;
            while (true)
            {
                PublicGroups[] pg = JsonConvert.DeserializeObject<PublicGroups[]>(this.NetworkAPI.get("https://web.lobi.co/api/public_groups?count=1000&page=" + index.ToString() + "&with_archived=1", header));
                index++;
                if (pg[0].items.Length == 0)
                    break;
                result.AddRange(pg);
            }

            return result.ToArray();
        }

        public PrivateGroups[] GetPrivateGroupList()
        {
            GetHeader header = new GetHeader()
                .setHost("web.lobi.co")
                .setConnection(true)
                .setAccept("application/json, text/plain, */*")
                .setUserAgent("Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.110 Safari/537.36")
                .setAcceptLanguage("ja,en-US;q=0.8,en;q=0.6");

            List<PrivateGroups> result = new List<PrivateGroups>();
            int index = 1;
            while (true)
            {
                PrivateGroups[] pg = JsonConvert.DeserializeObject<PrivateGroups[]>(this.NetworkAPI.get("https://web.lobi.co/api/groups?count=1000&page=" + index.ToString(), header));
                index++;
                if (pg[0].items.Length == 0)
                    break;
                result.AddRange(pg);
            }

            return result.ToArray();
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

        public Contacts GetContacts(string uid)
        {
            GetHeader header = new GetHeader()
                .setHost("web.lobi.co")
                .setConnection(true)
                .setAccept("application/json, text/plain, */*")
                .setUserAgent("Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.110 Safari/537.36")
                .setAcceptLanguage("ja,en-US;q=0.8,en;q=0.6");

            return JsonConvert.DeserializeObject<Contacts>(this.NetworkAPI.get("https://web.lobi.co/api/user/" + uid + "/contacts", header));
        }

        public Followers GetFollowers(string uid)
        {
            GetHeader header = new GetHeader()
                .setHost("web.lobi.co")
                .setConnection(true)
                .setAccept("application/json, text/plain, */*")
                .setUserAgent("Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.110 Safari/537.36")
                .setAcceptLanguage("ja,en-US;q=0.8,en;q=0.6");

            return JsonConvert.DeserializeObject<Followers>(this.NetworkAPI.get("https://web.lobi.co/api/user/" + uid + "/followers", header));
        }

        public Group GetGroup(string uid)
        {
            GetHeader header = new GetHeader()
                .setHost("web.lobi.co")
                .setConnection(true)
                .setAccept("application/json, text/plain, */*")
                .setUserAgent("Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.110 Safari/537.36")
                .setAcceptLanguage("ja,en-US;q=0.8,en;q=0.6");

            return JsonConvert.DeserializeObject<Group>(this.NetworkAPI.get("https://web.lobi.co/api/group/" + uid + "?error_flavor=json2&fields=group_bookmark_info%2Capp_events_info", header));
        }

        public int GetGroupMembersCount(string uid)
        {
            GetHeader header = new GetHeader()
                .setHost("web.lobi.co")
                .setConnection(true)
                .setAccept("application/json, text/plain, */*")
                .setUserAgent("Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.110 Safari/537.36")
                .setAcceptLanguage("ja,en-US;q=0.8,en;q=0.6");

            int? result =JsonConvert.DeserializeObject<Group>(this.NetworkAPI.get("https://web.lobi.co/api/group/"+uid,header)).members_count;
            return result == null ? 0 : (int)result;
        }

        public User[] GetGroupMembers(string uid)
        {
            GetHeader header = new GetHeader()
                .setHost("web.lobi.co")
                .setConnection(true)
                .setAccept("application/json, text/plain, */*")
                .setUserAgent("Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.110 Safari/537.36")
                .setAcceptLanguage("ja,en-US;q=0.8,en;q=0.6");

            List<User> result = new List<User>();
            string next = "0";
            int limit = 10000;
            while (limit-- > 0)
            {
                Group g = JsonConvert.DeserializeObject<Group>(this.NetworkAPI.get("https://web.lobi.co/api/group/" + uid + "?members_cursor=" + next, header));
                result.AddRange(g.members);
                if (g.members_next_cursor == 0)
                    break;
                next = g.members_next_cursor.ToString();
            }

            return result.ToArray();
        }

        public Chat[] GetThreads(string uid, int count = 20)
        {
            GetHeader header = new GetHeader()
                .setHost("web.lobi.co")
                .setConnection(true)
                .setAccept("application/json, text/plain, */*")
                .setUserAgent("Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.110 Safari/537.36")
                .setAcceptLanguage("ja,en-US;q=0.8,en;q=0.6");

            return JsonConvert.DeserializeObject<Chat[]>(this.NetworkAPI.get("https://web.lobi.co/api/group/" + uid + "/chats?count=" + count.ToString(), header));
        }

        public Pokes GetPokes(string group_id, string chat_id)
        {
            GetHeader header = new GetHeader()
                .setHost("web.lobi.co")
                .setConnection(true)
                .setAccept("application/json, text/plain, */*")
                .setUserAgent("Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.110 Safari/537.36")
                .setAcceptLanguage("ja,en-US;q=0.8,en;q=0.6");

            return JsonConvert.DeserializeObject<Pokes>(this.NetworkAPI.get("https://web.lobi.co/api/group/" + group_id + "/chats/pokes?id=" + chat_id, header));
        }

        public Bookmarks GetBookmarks()
        {
            GetHeader header = new GetHeader()
                .setHost("web.lobi.co")
                .setConnection(true)
                .setAccept("application/json, text/plain, */*")
                .setUserAgent("Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.110 Safari/537.36")
                .setAcceptLanguage("ja,en-US;q=0.8,en;q=0.6");

            return JsonConvert.DeserializeObject<Bookmarks>(this.NetworkAPI.get("https://web.lobi.co/api/me/bookmarks", header));
        }

        public void Goo(string group_id, string chat_id)
        {
            PostHeader header =new PostHeader()
                .setHost("web.lobi.co")
                .setConnection(true)
                .setAccept("application/json, text/plain, */*")
                .setOrigin("https://web.lobi.co")
                .setUserAgent("Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.110 Safari/537.36")
                .setAcceptLanguage("ja,en-US;q=0.8,en;q=0.6");

            List<string> data = new List<string>();
            data.Add("Content-Disposition: form-data; name=\"id\"\r\n\r\n"+chat_id);

            this.NetworkAPI.post_form_data("https://web.lobi.co/api/group/" + group_id + "/chats/like", "----WebKitFormBoundary" + Guid.NewGuid().ToString("N").Substring(0, 16), new string[] { "Content-Disposition: form-data; name=\"id\"\r\n\r\n" + chat_id }, header);
        }

        public void UnGoo(string group_id, string chat_id)
        {
            PostHeader header = new PostHeader()
                .setHost("web.lobi.co")
                .setConnection(true)
                .setAccept("application/json, text/plain, */*")
                .setOrigin("https://web.lobi.co")
                .setUserAgent("Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.110 Safari/537.36")
                .setAcceptLanguage("ja,en-US;q=0.8,en;q=0.6");

            List<string> data = new List<string>();
            data.Add("Content-Disposition: form-data; name=\"id\"\r\n\r\n" + chat_id);

            this.NetworkAPI.post_form_data("https://web.lobi.co/api/group/" + group_id + "/chats/unlike", "----WebKitFormBoundary" + Guid.NewGuid().ToString("N").Substring(0, 16), new string[] { "Content-Disposition: form-data; name=\"id\"\r\n\r\n" + chat_id }, header);
        }

        public void Boo(string group_id, string chat_id)
        {
            PostHeader header = new PostHeader()
                .setHost("web.lobi.co")
                .setConnection(true)
                .setAccept("application/json, text/plain, */*")
                .setOrigin("https://web.lobi.co")
                .setUserAgent("Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.110 Safari/537.36")
                .setAcceptLanguage("ja,en-US;q=0.8,en;q=0.6");

            List<string> data = new List<string>();
            data.Add("Content-Disposition: form-data; name=\"id\"\r\n\r\n" + chat_id);

            this.NetworkAPI.post_form_data("https://web.lobi.co/api/group/" + group_id + "/chats/boo", "----WebKitFormBoundary" + Guid.NewGuid().ToString("N").Substring(0, 16), new string[] { "Content-Disposition: form-data; name=\"id\"\r\n\r\n" + chat_id }, header);
        }

        public void UnBoo(string group_id, string chat_id)
        {
            PostHeader header = new PostHeader()
                .setHost("web.lobi.co")
                .setConnection(true)
                .setAccept("application/json, text/plain, */*")
                .setOrigin("https://web.lobi.co")
                .setUserAgent("Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.110 Safari/537.36")
                .setAcceptLanguage("ja,en-US;q=0.8,en;q=0.6");

            this.NetworkAPI.post_form_data("https://web.lobi.co/api/group/" + group_id + "/chats/unboo", "----WebKitFormBoundary" + Guid.NewGuid().ToString("N").Substring(0, 16), new string[] { "Content-Disposition: form-data; name=\"id\"\r\n\r\n" + chat_id }, header);
        }

        public void Follow(string user_id)
        {
            PostHeader header = new PostHeader()
                .setHost("web.lobi.co")
                .setConnection(true)
                .setAccept("application/json, text/plain, */*")
                .setOrigin("https://web.lobi.co")
                .setUserAgent("Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.110 Safari/537.36")
                .setAcceptLanguage("ja,en-US;q=0.8,en;q=0.6");

            this.NetworkAPI.post_form_data("https://web.lobi.co/api/me/contacts", "----WebKitFormBoundary" + Guid.NewGuid().ToString("N").Substring(0, 16), new string[] { "Content-Disposition: form-data; name=\"users\"\r\n\r\n" + user_id }, header);
        }

        public void UnFollow(string user_id)
        {
            PostHeader header = new PostHeader()
                .setHost("web.lobi.co")
                .setConnection(true)
                .setAccept("application/json, text/plain, */*")
                .setOrigin("https://web.lobi.co")
                .setUserAgent("Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.110 Safari/537.36")
                .setAcceptLanguage("ja,en-US;q=0.8,en;q=0.6");

            this.NetworkAPI.post_form_data("https://web.lobi.co/api/me/contacts/remove", "----WebKitFormBoundary" + Guid.NewGuid().ToString("N").Substring(0, 16), new string[] { "Content-Disposition: form-data; name=\"users\"\r\n\r\n" + user_id }, header);
        }

        public void MakeThread(string group_id, string message, bool shout = false)
        {
            PostHeader header = new PostHeader()
                .setHost("web.lobi.co")
                .setConnection(true)
                .setAccept("application/json, text/plain, */*")
                .setOrigin("https://web.lobi.co")
                .setUserAgent("Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.110 Safari/537.36")
                .setAcceptLanguage("ja,en-US;q=0.8,en;q=0.6");


            this.NetworkAPI.post_form_data("https://web.lobi.co/api/group/" + group_id + "/chats", "----WebKitFormBoundary" + Guid.NewGuid().ToString("N").Substring(0, 16), new string[] { "Content-Disposition: form-data; name=\"type\"\r\n\r\n" + (shout ? "shout" : "normal"), "Content-Disposition: form-data; name=\"lang\"\r\n\r\nja", "Content-Disposition: form-data; name=\"message\"\r\n\r\n" + message }, header);
        }

        public void Reply(string group_id, string thread_id, string message)
        {
            PostHeader header = new PostHeader()
                .setHost("web.lobi.co")
                .setConnection(true)
                .setAccept("application/json, text/plain, */*")
                .setOrigin("https://web.lobi.co")
                .setUserAgent("Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.110 Safari/537.36")
                .setAcceptLanguage("ja,en-US;q=0.8,en;q=0.6");


            this.NetworkAPI.post_form_data("https://web.lobi.co/api/group/" + group_id + "/chats", "----WebKitFormBoundary" + Guid.NewGuid().ToString("N").Substring(0, 16), new string[] { "Content-Disposition: form-data; name=\"type\"\r\n\r\nnormal", "Content-Disposition: form-data; name=\"lang\"\r\n\r\nja", "Content-Disposition: form-data; name=\"message\"\r\n\r\n" + message, "Content-Disposition: form-data; name=\"reply_to\"\r\n\r\n" + thread_id }, header);
        }

        public void RemoveGroup(string group_id)
        {
            PostHeader header = new PostHeader()
                .setHost("web.lobi.co")
                .setConnection(true)
                .setAccept("application/json, text/plain, */*")
                .setOrigin("https://web.lobi.co")
                .setUserAgent("Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.110 Safari/537.36")
                .setAcceptLanguage("ja,en-US;q=0.8,en;q=0.6");

            this.NetworkAPI.post_x_www_form_urlencoded("https://web.lobi.co/api/group/" + group_id + "/remove", "", header);
        }

        public MakePrivateGroupResult MakePrivateGroup(string user_id)
        {
            PostHeader header = new PostHeader()
                .setHost("web.lobi.co")
                .setConnection(true)
                .setAccept("application/json, text/plain, */*")
                .setOrigin("https://web.lobi.co")
                .setUserAgent("Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.110 Safari/537.36")
                .setAcceptLanguage("ja,en-US;q=0.8,en;q=0.6");

            return JsonConvert.DeserializeObject<MakePrivateGroupResult>(this.NetworkAPI.post_form_data("https://web.lobi.co/api/groups/1on1s", "----WebKitFormBoundary" + Guid.NewGuid().ToString("N").Substring(0, 16), new string[] { "Content-Disposition: form-data; name=\"user\"\r\n\r\n" + user_id }, header));
        }

        public void ChangeProfile(string name, string description)
        {
            PostHeader header = new PostHeader()
                .setHost("web.lobi.co")
                .setConnection(true)
                .setAccept("application/json, text/plain, */*")
                .setOrigin("https://web.lobi.co")
                .setUserAgent("Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.110 Safari/537.36")
                .setAcceptLanguage("ja,en-US;q=0.8,en;q=0.6");

            this.NetworkAPI.post_form_data("https://web.lobi.co/api/me/profile", "----WebKitFormBoundary" + Guid.NewGuid().ToString("N").Substring(0, 16), new string[] { "Content-Disposition: form-data; name=\"name\"\r\n\r\n" + name, "Content-Disposition: form-data; name=\"description\"\r\n\r\n" + description }, header);
        }

        private class Pattern
        {
            public static string csrf_token = "<input type=\"hidden\" name=\"csrf_token\" value=\"";
            public static string authenticity_token = "<input name=\"authenticity_token\" type=\"hidden\" value=\"";
            public static string redirect_after_login = "<input name=\"redirect_after_login\" type=\"hidden\" value=\"";
            public static string oauth_token = "<input id=\"oauth_token\" name=\"oauth_token\" type=\"hidden\" value=\"";
            public static string twitter_redirect_to_lobi = "<a class=\"maintain-context\" href=\"";
            public static string get_string(string source, string pattern, string end_pattern)
            {
                int start = source.IndexOf(pattern) + pattern.Length;
                int end = source.IndexOf(end_pattern, start + 1);
                return source.Substring(start, end - start);
            }
        }
    }
}
