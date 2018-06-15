using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using X_FIFA_Fantasy_Cup.Models;

namespace X_FIFA_Fantasy_Cup.Controllers
{
    public class CountryController : ApiController
    {

        [HttpGet]
        public JsonResult<List<Country>> Countries()
        {
            List <Country> results = new List<Country>();
            Country tmp = null;

            System.Diagnostics.Debug.WriteLine("llegó al post");
            System.Diagnostics.Debug.WriteLine("entrando al get");
             
            string action = "SELECT * FROM COUNTRY";
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
  
            myConnection.Open();
            SqlCommand sqlCmd = new SqlCommand(action, myConnection);


            sqlCmd.CommandType = CommandType.Text;
             var reader = sqlCmd.ExecuteReader();
            while (reader.Read())
            {
                tmp = new Country();
                tmp.Id = (int)reader["country_id"];
                tmp.Name = (string)reader["country_name"];
                results.Add(tmp);

            }
    
            myConnection.Close();
            return Json(results);
            
        }
    }
    }
