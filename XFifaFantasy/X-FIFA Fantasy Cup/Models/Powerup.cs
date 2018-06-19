using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace X_FIFA_Fantasy_Cup.Models
{
    public class Powerup
    {
        public string powerup_name { get; set; }
        public int powerup_id { get; set; }
        public int powerup_unique { get; set; }
        public int powerup_selection { get; set; }
        public int sponsor_id { get; set; }
        public int powerup_type_id { get; set; }
    }
}