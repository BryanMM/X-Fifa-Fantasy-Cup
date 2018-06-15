using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XFifaFantasy.Logic
{
    public class DbConnection
    {
        public string success { get; set; }
        public string detail { get; set; }
        static private string GetConnectionString()
        {
            return @"Data Source=DESKTOP-GD88K2B\bryan;Initial Catalog=xfifafantasycup;"
                + "Integrated Security=true;";
        }
       
    }
}