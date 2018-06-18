using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace X_FIFA_Fantasy_Cup.Models
{
    public class PlayerxCountry
    {
        public int tournamentxcountry_id { get; set; }
        public List<string> Players = new List<string>();
    }
}