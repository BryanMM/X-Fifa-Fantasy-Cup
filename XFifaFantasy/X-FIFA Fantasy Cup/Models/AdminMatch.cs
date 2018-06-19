using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace X_FIFA_Fantasy_Cup.Models
{
    public class AdminMatch
    {
        public int tournament_id { get; set; }
        public string name_team_1 { get; set; }
        public string name_team_2 { get; set; }
        public string match_date { get; set; }
        public string match_location { get; set; }
        public int stage_id { get; set; }
        public string txc_team1 { get; set; }
        public string team_1 { get; set; }
        public string team_2 { get; set; }
        public string txc_team2 { get; set; }
        public string sxm_winner1 { get; set; }
        public string sxm_winner2 { get; set; }
        public int match_number { get; set; }
    }
}