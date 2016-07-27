using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

using LobiAPI.HttpAPI;
using LobiAPI.Json;

using Newtonsoft.Json;

namespace LobiAPI
{
    public class BasicAPI
    {
        private Http NetworkAPI = null;
        private HttpRequestMessage Header = null;

        public BasicAPI()
        {
            NetworkAPI = new Http();

            Header = new HttpRequestMessage();
            Header.Headers.Host = "lobi.co";
            Header.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
            Header.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
            Header.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            Header.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            Header.Headers.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
            Header.Headers.UserAgent.Add(new ProductInfoHeaderValue("AppleWebKit", "537.36"));
            Header.Headers.UserAgent.Add(new ProductInfoHeaderValue("Chrome", "49.0.2623.110"));
            Header.Headers.UserAgent.Add(new ProductInfoHeaderValue("Safari", "537.36"));
            Header.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue("ja"));
            Header.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-US"));
            
        }

        public async Task<bool> Login(string mail, string password)
        {
            string source = await this.NetworkAPI.get("https://lobi.co/signin", Header);
            string csrf_token = Pattern.get_string(source, Pattern.csrf_token, "\"");
            FormUrlEncodedContent post_data = new FormUrlEncodedContent(new Dictionary<string, string>{
               { "csrf_token", csrf_token },
               { "email", mail },
               { "password", password }
            });
            string source2 = await this.NetworkAPI.post_x_www_form_urlencoded("https://lobi.co/signin", post_data, Header);
            bool result = source2.IndexOf("ログインに失敗しました") == -1;
            if(result){
                Header.Headers.Host = "web.lobi.co";
                Header.Headers.Host = "web.lobi.co";
            }
            return result;
        }

        public async Task<bool> TwitterLogin(string mail, string password)
        {
            string source = await this.NetworkAPI.get("https://lobi.co/signup/twitter", Header);
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
            Header.Headers.Host = "api.twitter.com";
            string source2 = await this.NetworkAPI.post_x_www_form_urlencoded("https://api.twitter.com/oauth/authorize", post_data, Header);
            if (source2.IndexOf("Twitterにログイン") > -1)
                return false;
            string source3 = await this.NetworkAPI.get(Pattern.get_string(source2, Pattern.twitter_redirect_to_lobi, "\""), Header);
            bool result = source3.IndexOf("ログインに失敗しました") == -1;
            if(result){
                Header.Headers.Host = "web.lobi.co";
                Header.Headers.Accept.Clear();
                Header.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                Header.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
                Header.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            }
            return result;
        }

        public async Task<Me> GetMe()
        {
            return JsonConvert.DeserializeObject<Me>(await this.NetworkAPI.get("https://web.lobi.co/api/me?fields=premium", Header));
        }

        public async Task<PublicGroups[]> GetPublicGroupList()
        {
            List<PublicGroups> result = new List<PublicGroups>();

            int index = 1;
            while (true)
            {
                PublicGroups[] pg = JsonConvert.DeserializeObject<PublicGroups[]>(await this.NetworkAPI.get("https://web.lobi.co/api/public_groups?count=1000&page=" + index.ToString() + "&with_archived=1", Header));
                index++;
                if (pg[0].items.Length == 0)
                    break;
                result.AddRange(pg);
            }

            return result.ToArray();
        }

        public async Task<PrivateGroups[]> GetPrivateGroupList()
        {
            List<PrivateGroups> result = new List<PrivateGroups>();
            int index = 1;
            while (true)
            {
                PrivateGroups[] pg = JsonConvert.DeserializeObject<PrivateGroups[]>(await this.NetworkAPI.get("https://web.lobi.co/api/groups?count=1000&page=" + index.ToString(), Header));
                index++;
                if (pg[0].items.Length == 0)
                    break;
                result.AddRange(pg);
            }

            return result.ToArray();
        }

        public async Task<Notifications> GetNotifications()
        {
            return JsonConvert.DeserializeObject<Notifications>(await this.NetworkAPI.get("https://web.lobi.co/api/info/notifications?platform=any&last_cursor=0", Header));
        }

        public async Task<Contacts> GetContacts(string uid)
        {
            return JsonConvert.DeserializeObject<Contacts>(await this.NetworkAPI.get("https://web.lobi.co/api/user/" + uid + "/contacts", Header));
        }

        public async Task<Followers> GetFollowers(string uid)
        {
            return JsonConvert.DeserializeObject<Followers>(await this.NetworkAPI.get("https://web.lobi.co/api/user/" + uid + "/followers", Header));
        }

        public async Task<Group> GetGroup(string uid)
        {
            return JsonConvert.DeserializeObject<Group>(await this.NetworkAPI.get("https://web.lobi.co/api/group/" + uid + "?error_flavor=json2&fields=group_bookmark_info%2Capp_events_info", Header));
        }

        public async Task<int> GetGroupMembersCount(string uid)
        {
            int? result =JsonConvert.DeserializeObject<Group>(await this.NetworkAPI.get("https://web.lobi.co/api/group/"+uid, Header)).members_count;
            return result == null ? 0 : (int)result;
        }

        public async Task<User[]> GetGroupMembers(string uid)
        {
            List<User> result = new List<User>();
            string next = "0";
            int limit = 10000;
            while (limit-- > 0)
            {
                Group g = JsonConvert.DeserializeObject<Group>(await this.NetworkAPI.get("https://web.lobi.co/api/group/" + uid + "?members_cursor=" + next, Header));
                result.AddRange(g.members);
                if (g.members_next_cursor == 0)
                    break;
                next = g.members_next_cursor.ToString();
            }

            return result.ToArray();
        }

        public async Task<Chat[]> GetThreads(string uid, int count = 20)
        {
            return JsonConvert.DeserializeObject<Chat[]>(await this.NetworkAPI.get("https://web.lobi.co/api/group/" + uid + "/chats?count=" + count.ToString(), Header));
        }

        public async Task<Pokes> GetPokes(string group_id, string chat_id)
        {
            return JsonConvert.DeserializeObject<Pokes>(await this.NetworkAPI.get("https://web.lobi.co/api/group/" + group_id + "/chats/pokes?id=" + chat_id, Header));
        }

        public async Task<Bookmarks> GetBookmarks()
        {
            return JsonConvert.DeserializeObject<Bookmarks>(await this.NetworkAPI.get("https://web.lobi.co/api/me/bookmarks", Header));
        }

        public async void Goo(string group_id, string chat_id)
        {
            FormUrlEncodedContent post_data = new FormUrlEncodedContent(new Dictionary<string, string>{
               { "id", chat_id }
            });
            await this.NetworkAPI.post_x_www_form_urlencoded("https://web.lobi.co/api/group/" + group_id + "/chats/like", post_data, Header);
        }

        public async void UnGoo(string group_id, string chat_id)
        {
            FormUrlEncodedContent post_data = new FormUrlEncodedContent(new Dictionary<string, string>{
               { "id", chat_id }
            });
            await this.NetworkAPI.post_x_www_form_urlencoded("https://web.lobi.co/api/group/" + group_id + "/chats/unlike", post_data, Header);
        }

        public async void Boo(string group_id, string chat_id)
        {
            FormUrlEncodedContent post_data = new FormUrlEncodedContent(new Dictionary<string, string>{
               { "id", chat_id }
            });
            await this.NetworkAPI.post_x_www_form_urlencoded("https://web.lobi.co/api/group/" + group_id + "/chats/boo", post_data, Header);
        }

        public async void UnBoo(string group_id, string chat_id)
        {
            FormUrlEncodedContent post_data = new FormUrlEncodedContent(new Dictionary<string, string>{
               { "id", chat_id }
            });
            await this.NetworkAPI.post_x_www_form_urlencoded("https://web.lobi.co/api/group/" + group_id + "/chats/unboo", post_data, Header);
        }

        public async void Follow(string user_id)
        {
            FormUrlEncodedContent post_data = new FormUrlEncodedContent(new Dictionary<string, string>{
               { "users", user_id }
            });
            await this.NetworkAPI.post_x_www_form_urlencoded("https://web.lobi.co/api/me/contacts", post_data, Header);
        }

        public async void UnFollow(string user_id)
        {
            FormUrlEncodedContent post_data = new FormUrlEncodedContent(new Dictionary<string, string>{
               { "users", user_id }
            });
            await this.NetworkAPI.post_x_www_form_urlencoded("https://web.lobi.co/api/me/contacts/remove", post_data, Header);
        }

        public async void MakeThread(string group_id, string message, bool shout = false)
        {
            FormUrlEncodedContent post_data = new FormUrlEncodedContent(new Dictionary<string, string>{
               { "type", shout ? "shout" : "normal" },
               { "lang", "ja" },
               { "message", message }
            });
            await this.NetworkAPI.post_x_www_form_urlencoded("https://web.lobi.co/api/group/" + group_id + "/chats", post_data, Header);
        }

        public async void Reply(string group_id, string thread_id, string message)
        {
            FormUrlEncodedContent post_data = new FormUrlEncodedContent(new Dictionary<string, string>{
               { "type", "normal" },
               { "lang", "ja" },
               { "message", message },
               { "reply_to", thread_id }
            });
            await this.NetworkAPI.post_x_www_form_urlencoded("https://web.lobi.co/api/group/" + group_id + "/chats", post_data, Header);
        }

        public async void RemoveGroup(string group_id)
        {
            FormUrlEncodedContent post_data = new FormUrlEncodedContent(new Dictionary<string, string>());
            await this.NetworkAPI.post_x_www_form_urlencoded("https://web.lobi.co/api/group/" + group_id + "/remove", post_data, Header);
        }

        public async Task<MakePrivateGroupResult> MakePrivateGroup(string user_id)
        {
            FormUrlEncodedContent post_data = new FormUrlEncodedContent(new Dictionary<string, string>{
               { "user", user_id }
            });
            return JsonConvert.DeserializeObject<MakePrivateGroupResult>(await this.NetworkAPI.post_x_www_form_urlencoded("https://web.lobi.co/api/groups/1on1s", post_data, Header));
        }

        public async void ChangeProfile(string name, string description)
        {
            FormUrlEncodedContent post_data = new FormUrlEncodedContent(new Dictionary<string, string>{
               { "name", name },
               { "description", description }
            });
            await this.NetworkAPI.post_x_www_form_urlencoded("https://web.lobi.co/api/me/profile", post_data, Header);
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
