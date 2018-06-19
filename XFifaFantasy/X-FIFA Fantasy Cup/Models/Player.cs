using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace X_FIFA_Fantasy_Cup.Models
{
    public class Player
    {
        public string player_id { get; set; }
        public int player_grade { get; set; }
        public string playerxposition_name { get; set; }
        public int country_id { get; set; }
        public int playerxinfo_id { get; set; }
        public string player_name { get; set; }
        public string player_last_name { get; set; }
        public string player_birth { get; set; }
        public int player_height { get; set; }
        public int player_weight { get; set; }
        public string player_team { get; set; }
        public float player_price { get; set; }
        public int player_active { get; set; }
        public string player_photo { get; set; }

 
    }
}