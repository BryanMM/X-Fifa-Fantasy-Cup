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
        [HttpGet]
        [ActionName("players")]
        public JsonResult<List<Player>> players(Player player)
        {
            List<Player> results = new List<Player>();
            Player tmp = null;
            string action = "select * from playerxinfo full outer join player on playerxinfo.player_id = player.player_id where playerxinfo.country_id"+" = "+ player.country_id+" and player.player_active = "+player.player_active;
            SqlConnection myConnection = new SqlConnection();
            myConnection.Open();
            SqlCommand sqlCmd = new SqlCommand(action, myConnection);
            sqlCmd.CommandType = CommandType.Text;
            var reader = sqlCmd.ExecuteReader();
            while (reader.Read())
            {
                tmp = new Player();
                tmp.player_id = (int)reader["player_id"];
                tmp.player_name = (string)reader["player_name"]+" "+(string)reader["player_last_name"];
                results.Add(tmp);
            }
            myConnection.Close();
            return Json(results);
        }
    }
    }
