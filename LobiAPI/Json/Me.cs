using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LobiAPI.Json
{
    public class Me
    {
        public string contacted_date { get; set; }
        public int contacts_count { get; set; }
        public string cover { get; set; }
        public int @default { get; set; }
        public string description { get; set; }
        public int followers_count { get; set; }
        public string following_date { get; set; }
        public string icon { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public string located_date { get; set; }
        public int my_groups_count { get; set; }
        public string name { get; set; }
        public int premium { get; set; }
        public Group[] public_groups { get; set; }
        public string public_groups_next_cursor { get; set; }
        public string uid { get; set; }
        public object[] uesrs { get; set; }
        public int users_next_cursor { get; set; }
    }
}
