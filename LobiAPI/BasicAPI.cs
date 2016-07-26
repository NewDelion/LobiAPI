using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

using LobiAPI.HttpAPI;
using LobiAPI.Json;

using Newtonsoft.Json;

namespace LobiAPI
{
    public class BasicAPI
    {
        private Http NetworkAPI = null;
        private HttpRequestHeader GetHeader = null;
        private HttpRequestHeader PostHeader = null;

        public BasicAPI()
        {
            NetworkAPI = new Http();
            
            GetHeader = new HttpRequestHeader();
            GetHeader.Host = "lobi.co";
            GetHeader.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8"));
            GetHeader.UserAgent.Add(new ProductInfoHeaderValue("Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.110 Safari/537.36"));
            GetHeader.AcceptLanguage("ja,en-US;q=0.8,en;q=0.6");
            
            PostHeader = new HttpRequestHeader();
            PostHeader.Host = "lobi.co";
            PostHeader.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8"));
            PostHeader.UserAgent.Add(new ProductInfoHeaderValue("Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.110 Safari/537.36"));
            PostHeader.AcceptLanguage("ja,en-US;q=0.8,en;q=0.6");
            
        }

        public async Task<bool> Login(string mail, string password)
        {
            string source = await this.NetworkAPI.get("https://lobi.co/signin", GetHeader);
            string csrf_token = Pattern.get_string(source, Pattern.csrf_token, "\"");
            FormUrlEncodedContent post_data = new FormUrlEncodedContent(new Dictionary<string, string>{
               { "csrf_token", csrf_token },
               { "email", mail },
               { "password", password }
            });
            string source2 = await this.NetworkAPI.post_x_www_form_urlencoded("https://lobi.co/signin", post_data, PostHeader);
            bool result = source2.IndexOf("ログインに失敗しました") == -1;
            if(result){
                GetHeader.Host = "web.lobi.co";
                PostHeader.Host = "web.lobi.co";
            }
            return result;
        }

        public async Task<bool> TwitterLogin(string mail, string password)
        {
            string source = await this.NetworkAPI.get("https://lobi.co/signup/twitter", GetHeader);
            string authenticity_token = Pattern.get_string(source, Pattern.authenticity_token, "\"");
            string redirect_after_login = Pattern.get_string(source, Pattern.redirect_after_login, "\"");
            string oauth_token = Pattern.get_string(source, Pattern.oauth_token, "\"");
            FormUrlEncodedContent post_data = new FormUrlEncodedContent(new Dictionary<string, string>{
               { "authenticity_token", oauth_token },
               { "redirect_after_login", redirect_after_login },
               { "oauth_token", oauth_token },
               { "session[username_or_email]", mail },
               { "session[password]", password }
            });
            PostHeader.Host = "api.twitter.com";
            string source2 = await this.NetworkAPI.post_x_www_form_urlencoded("https://api.twitter.com/oauth/authorize", post_data, PostHeader);
            if (source2.IndexOf("Twitterにログイン") > -1)
                return false;
            string source3 = this.NetworkAPI.get(Pattern.get_string(source2, Pattern.twitter_redirect_to_lobi, "\""), GetHeader);
            bool result = source3.IndexOf("ログインに失敗しました") == -1;
            if(result){
                GetHeader.Host = "web.lobi.co";
                PostHeader.Host = "web.lobi.co";
            }
            return result;
        }

        public Me GetMe()
        {
            return JsonConvert.DeserializeObject<Me>(this.NetworkAPI.get("https://web.lobi.co/api/me?fields=premium", GetHeader));
        }

        public PublicGroups[] GetPublicGroupList()
        {
            List<PublicGroups> result = new List<PublicGroups>();

            int index = 1;
            while (true)
            {
                PublicGroups[] pg = JsonConvert.DeserializeObject<PublicGroups[]>(this.NetworkAPI.get("https://web.lobi.co/api/public_groups?count=1000&page=" + index.ToString() + "&with_archived=1", GetHeader));
                index++;
                if (pg[0].items.Length == 0)
                    break;
                result.AddRange(pg);
            }

            return result.ToArray();
        }

        public PrivateGroups[] GetPrivateGroupList()
        {
            List<PrivateGroups> result = new List<PrivateGroups>();
            int index = 1;
            while (true)
            {
                PrivateGroups[] pg = JsonConvert.DeserializeObject<PrivateGroups[]>(this.NetworkAPI.get("https://web.lobi.co/api/groups?count=1000&page=" + index.ToString(), GetHeader));
                index++;
                if (pg[0].items.Length == 0)
                    break;
                result.AddRange(pg);
            }

            return result.ToArray();
        }

        public Notifications GetNotifications()
        {
            return JsonConvert.DeserializeObject<Notifications>(this.NetworkAPI.get("https://web.lobi.co/api/info/notifications?platform=any&last_cursor=0", GetHeader));
        }

        public Contacts GetContacts(string uid)
        {
            return JsonConvert.DeserializeObject<Contacts>(this.NetworkAPI.get("https://web.lobi.co/api/user/" + uid + "/contacts", GetHeader));
        }

        public Followers GetFollowers(string uid)
        {
            return JsonConvert.DeserializeObject<Followers>(this.NetworkAPI.get("https://web.lobi.co/api/user/" + uid + "/followers", GetHeader));
        }

        public Group GetGroup(string uid)
        {
            return JsonConvert.DeserializeObject<Group>(this.NetworkAPI.get("https://web.lobi.co/api/group/" + uid + "?error_flavor=json2&fields=group_bookmark_info%2Capp_events_info", GetHeader));
        }

        public int GetGroupMembersCount(string uid)
        {
            int? result =JsonConvert.DeserializeObject<Group>(this.NetworkAPI.get("https://web.lobi.co/api/group/"+uid, GetHeader)).members_count;
            return result == null ? 0 : (int)result;
        }

        public User[] GetGroupMembers(string uid)
        {
            List<User> result = new List<User>();
            string next = "0";
            int limit = 10000;
            while (limit-- > 0)
            {
                Group g = JsonConvert.DeserializeObject<Group>(this.NetworkAPI.get("https://web.lobi.co/api/group/" + uid + "?members_cursor=" + next, GetHeader));
                result.AddRange(g.members);
                if (g.members_next_cursor == 0)
                    break;
                next = g.members_next_cursor.ToString();
            }

            return result.ToArray();
        }

        public Chat[] GetThreads(string uid, int count = 20)
        {
            return JsonConvert.DeserializeObject<Chat[]>(this.NetworkAPI.get("https://web.lobi.co/api/group/" + uid + "/chats?count=" + count.ToString(), GetHeader));
        }

        public Pokes GetPokes(string group_id, string chat_id)
        {
            return JsonConvert.DeserializeObject<Pokes>(this.NetworkAPI.get("https://web.lobi.co/api/group/" + group_id + "/chats/pokes?id=" + chat_id, GetHeader));
        }

        public Bookmarks GetBookmarks()
        {
            return JsonConvert.DeserializeObject<Bookmarks>(this.NetworkAPI.get("https://web.lobi.co/api/me/bookmarks", GetHeader));
        }

        public void Goo(string group_id, string chat_id)
        {
            List<string> data = new List<string>();
            data.Add("Content-Disposition: form-data; name=\"id\"\r\n\r\n"+chat_id);

            this.NetworkAPI.post_form_data("https://web.lobi.co/api/group/" + group_id + "/chats/like", "----WebKitFormBoundary" + Guid.NewGuid().ToString("N").Substring(0, 16), new string[] { "Content-Disposition: form-data; name=\"id\"\r\n\r\n" + chat_id }, PostHeader);
        }

        public void UnGoo(string group_id, string chat_id)
        {
            List<string> data = new List<string>();
            data.Add("Content-Disposition: form-data; name=\"id\"\r\n\r\n" + chat_id);

            this.NetworkAPI.post_form_data("https://web.lobi.co/api/group/" + group_id + "/chats/unlike", "----WebKitFormBoundary" + Guid.NewGuid().ToString("N").Substring(0, 16), new string[] { "Content-Disposition: form-data; name=\"id\"\r\n\r\n" + chat_id }, PostHeader);
        }

        public void Boo(string group_id, string chat_id)
        {
            List<string> data = new List<string>();
            data.Add("Content-Disposition: form-data; name=\"id\"\r\n\r\n" + chat_id);

            this.NetworkAPI.post_form_data("https://web.lobi.co/api/group/" + group_id + "/chats/boo", "----WebKitFormBoundary" + Guid.NewGuid().ToString("N").Substring(0, 16), new string[] { "Content-Disposition: form-data; name=\"id\"\r\n\r\n" + chat_id }, PostHeader);
        }

        public void UnBoo(string group_id, string chat_id)
        {
            this.NetworkAPI.post_form_data("https://web.lobi.co/api/group/" + group_id + "/chats/unboo", "----WebKitFormBoundary" + Guid.NewGuid().ToString("N").Substring(0, 16), new string[] { "Content-Disposition: form-data; name=\"id\"\r\n\r\n" + chat_id }, PostHeader);
        }

        public void Follow(string user_id)
        {
            this.NetworkAPI.post_form_data("https://web.lobi.co/api/me/contacts", "----WebKitFormBoundary" + Guid.NewGuid().ToString("N").Substring(0, 16), new string[] { "Content-Disposition: form-data; name=\"users\"\r\n\r\n" + user_id }, PostHeader);
        }

        public void UnFollow(string user_id)
        {
            this.NetworkAPI.post_form_data("https://web.lobi.co/api/me/contacts/remove", "----WebKitFormBoundary" + Guid.NewGuid().ToString("N").Substring(0, 16), new string[] { "Content-Disposition: form-data; name=\"users\"\r\n\r\n" + user_id }, PostHeader);
        }

        public void MakeThread(string group_id, string message, bool shout = false)
        {
            this.NetworkAPI.post_form_data("https://web.lobi.co/api/group/" + group_id + "/chats", "----WebKitFormBoundary" + Guid.NewGuid().ToString("N").Substring(0, 16), new string[] { "Content-Disposition: form-data; name=\"type\"\r\n\r\n" + (shout ? "shout" : "normal"), "Content-Disposition: form-data; name=\"lang\"\r\n\r\nja", "Content-Disposition: form-data; name=\"message\"\r\n\r\n" + message }, PostHeader);
        }

        public void Reply(string group_id, string thread_id, string message)
        {
            this.NetworkAPI.post_form_data("https://web.lobi.co/api/group/" + group_id + "/chats", "----WebKitFormBoundary" + Guid.NewGuid().ToString("N").Substring(0, 16), new string[] { "Content-Disposition: form-data; name=\"type\"\r\n\r\nnormal", "Content-Disposition: form-data; name=\"lang\"\r\n\r\nja", "Content-Disposition: form-data; name=\"message\"\r\n\r\n" + message, "Content-Disposition: form-data; name=\"reply_to\"\r\n\r\n" + thread_id }, PostHeader);
        }

        public void RemoveGroup(string group_id)
        {
            this.NetworkAPI.post_x_www_form_urlencoded("https://web.lobi.co/api/group/" + group_id + "/remove", "", PostHeader);
        }

        public MakePrivateGroupResult MakePrivateGroup(string user_id)
        {
            return JsonConvert.DeserializeObject<MakePrivateGroupResult>(this.NetworkAPI.post_form_data("https://web.lobi.co/api/groups/1on1s", "----WebKitFormBoundary" + Guid.NewGuid().ToString("N").Substring(0, 16), new string[] { "Content-Disposition: form-data; name=\"user\"\r\n\r\n" + user_id }, PostHeader));
        }

        public void ChangeProfile(string name, string description)
        {
            this.NetworkAPI.post_form_data("https://web.lobi.co/api/me/profile", "----WebKitFormBoundary" + Guid.NewGuid().ToString("N").Substring(0, 16), new string[] { "Content-Disposition: form-data; name=\"name\"\r\n\r\n" + name, "Content-Disposition: form-data; name=\"description\"\r\n\r\n" + description }, PostHeader);
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
