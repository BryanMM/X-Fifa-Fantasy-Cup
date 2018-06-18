using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace X_FIFA_Fantasy_Cup.Logic
{
    public class DbConnection
    {
        public string success { get; set; }
        public int id { get; set; }
        public string detail_type { get; set; }
        public string detail_xinfo { get; set; }
        public string detail_status { get; set; }
        public List<int> tournamentxcountry = new List<int>();
    }
}