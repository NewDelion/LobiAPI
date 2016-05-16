using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LobiAPI.Json
{
    public class Chat
    {
        public Asset[] assets { get; set; }
        public int? assets_expired { get; set; }
        public int? booed { get; set; }
        public int? bookmarks_count { get; set; }
        public int? boos_count { get; set; }
        public long? created_date { get; set; }
        public string id { get; set; }
        public string image { get; set; }
        public int? is_group_bookmarked { get; set; }
        public int? is_me_bookmarked { get; set; }
        public int? liked { get; set; }
        public int likes_count { get; set; }
        public string message { get; set; }
        public Reply replies { get; set; }
        public string reply_to { get; set; }
        public string type { get; set; }
        public string[] urls { get; set; }
        public User user { get; set; }
    }
}
