using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace X_FIFA_Fantasy_Cup.Models
{
    public class UserxFantasy
    {
        public int userxinfo_id { get; set; }
        public int tournament_id { get; set; }
        public List<int> players { get; set; }
    }
}