using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace X_FIFA_Fantasy_Cup.Models
{
    public class Match
    {
        public string match_name { get; set; }
        public int match_id { get; set; }
        public string match_date { get; set; }
        public string match_location { get; set; }
        public int stage_id { get; set; }
        public int tournament_id { get; set; }
        public int txc_team_1 { get; set; }
        public int txc_team_2 { get; set; }
        public int sxm_winner1 { get; set; }
        public int sxm_winner2 { get; set; }
        public int match_number { get; set; }

    }
}