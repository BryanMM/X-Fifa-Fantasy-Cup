using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XFifaFantasy.Models
{
    public class Fanatic
    {
        public int fanatic_id { get; set; }
        public string fanatic_login { get; set; }
        public string fanatic_name { get; set; }
        public string last_name { get; set; }
        public string user_email { get; set; }
        public int user_phone { get; set; }
        public DateTime user_birth { get; set; }
        public string user_photo { get; set; }
        public DateTime user_date_create { get; set; }
        public string user_password { get; set; }
        public string user_description { get; set; } 
        public bool active { get; set; }
    }
}