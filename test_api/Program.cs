using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LobiAPI;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace test_api
{
    class Program
    {
        static void Main(string[] args)
        {
            BasicAPI api = new BasicAPI();
login:      Console.Write("Mail: ");
            string mail = Console.ReadLine();
            Console.Write("Password: ");
            string password = Console.ReadLine();
            if (!api.Login(mail, password).Result)
            {
                Console.WriteLine("ログインに失敗しました。");
                goto login;
            }

            LobiAPI.Json.Me me = api.GetMe().Result;

            var TaskPublicGroups = api.GetPublicGroupList();
            TaskPublicGroups.Wait();
            var TaskPrivateGroups = api.GetPrivateGroupList();
            TaskPrivateGroups.Wait();

            LobiAPI.Json.PublicGroups[] PublicGroups = TaskPublicGroups.Result;
            LobiAPI.Json.PrivateGroups[] PrivateGroups = TaskPrivateGroups.Result;

            /*LobiAPI.Json.Notifications Notifications = api.GetNotifications();
            LobiAPI.Json.Followers Followers = api.GetFollowers(me.uid);
            LobiAPI.Json.Contacts Contacts = api.GetContacts(me.uid);
            LobiAPI.Json.Group Group = api.GetGroup(PublicGroups[0].items[0].uid);
            LobiAPI.Json.User[] users = api.GetGroupMembers(PublicGroups[0].items[0].uid);
            LobiAPI.Json.Chat[] chats = api.GetThreads(PublicGroups[0].items[0].uid);*/
            
            Console.ReadLine();
        }
    }
}
