using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace X_FIFA_Fantasy_Cup.Models
{
    public class Tournament
    {
        public int tournament_id { get; set; }
        public string tournament_name { get; set; }
        public int sponsor_id { get; set; }
        public List<Match> matches = new List<Match>();

        public List<tourmamentxcountry> countries = new List<tourmamentxcountry>();
    }
}