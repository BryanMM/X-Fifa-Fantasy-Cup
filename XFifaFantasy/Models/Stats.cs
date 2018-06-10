using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XFifaFantasy.Models
{
    public class Stats
    {
        public int id { get; set; }
        public int played { get; set; }
        public int winner { get; set; }
        public int tie { get; set; }
        public int lost { get; set; }
        public int minutes { get; set; }
        public int goals { get; set; }
        public int assistance { get; set; }
        public int ball_back { get; set; }
        public int yellow_card { get; set; }
        public int red_card { get; set; }
        public int expulsion { get; set; }
        public int penalty_stop { get; set; }
        public int penalty_cause { get; set; }
        public int remate_save { get; set; }
    }
}