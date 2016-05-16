using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LobiAPI;

namespace test_api
{
    class Program
    {
        static void Main(string[] args)
        {
            BasicAPI api = new BasicAPI();
            Console.Write("Mail: ");
            string mail = System.Net.WebUtility.UrlEncode(Console.ReadLine());
            Console.Write("Password: ");
            string password = System.Net.WebUtility.UrlEncode(Console.ReadLine());
            api.Login(mail, password);
            LobiAPI.Json.Me me = api.GetMe();
            LobiAPI.Json.PublicGroups[] PublicGroups = api.GetPublicGroupList();
            LobiAPI.Json.PrivateGroups[] PrivateGroups = api.GetPrivateGroupList();
            LobiAPI.Json.Notifications Notifications = api.GetNotifications();
            LobiAPI.Json.Followers Followers = api.GetFollowers(me.uid);
            LobiAPI.Json.Contacts Contacts = api.GetContacts(me.uid);
            LobiAPI.Json.Group Group = api.GetGroup(PublicGroups[0].items[0].uid);
            LobiAPI.Json.User[] users = api.GetGroupMembers(PublicGroups[0].items[0].uid);
            LobiAPI.Json.Chat[] chats = api.GetThreads(PublicGroups[0].items[0].uid);

            
            Console.ReadLine();
        }
    }
}
