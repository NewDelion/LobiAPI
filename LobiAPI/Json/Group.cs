using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LobiAPI.Json
{
    public class Group
    {
        public long created_date { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
        public int is_archived { get; set; }
        public int is_authorized { get; set; }
        public int is_notice { get; set; }
        public int is_official { get; set; }
        public int is_online { get; set; }
        public int is_public { get; set; }
        public string last_chat_at { get; set; }
        public string name { get; set; }
        public int online_users { get; set; }
        public int push_enabled { get; set; }
        public string stream_host { get; set; }
        public long total_users { get; set; }
        public string type { get; set; }
        public string uid { get; set; }
        public string wallpaper { get; set; }
        public object game { get; set; }//いらない情報だからオブジェクトにしてるけどもしかしたらパースできないかも
        public GroupCan can { get; set; }
    }
}
