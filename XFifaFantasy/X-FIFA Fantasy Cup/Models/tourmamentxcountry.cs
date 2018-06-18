using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace X_FIFA_Fantasy_Cup.Models
{
    public class tourmamentxcountry
    {
        public int country_id { get; set; }
        public int tournamentxcountry_id { get; set; }
        public List<int> players = new List<int>();
    }
}