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
        [HttpPost]
        [ActionName("players")]
        public JsonResult<List<Player>> players(Player player)
        {
            List<Player> results = new List<Player>();
            Player tmp = null;
            string action = "select py.player_id,py.player_name,py.player_last_name,py.player_price,py.player_grade,pxi.playerxinfo_id,pxp.playerxposition_name from player as py left outer join playerxinfo as pxi on(py.player_id = pxi.player_id)left outer join playerxposition as pxp on(pxi.playerxposition_id = pxp.playerxposition_id) where pxi.country_id =" + player.country_id+" and py.player_active = "+player.player_active;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            SqlCommand sqlCmd = new SqlCommand(action, myConnection);
            myConnection.Open();
            sqlCmd.CommandType = CommandType.Text;
            var reader = sqlCmd.ExecuteReader();
            while (reader.Read())
            {
                tmp = new Player();
                tmp.player_id = (string)reader["player_id"];
                System.Diagnostics.Debug.WriteLine(reader["player_price"]);
                tmp.player_name = (string)reader["player_name"]+" "+(string)reader["player_last_name"];
                tmp.player_price = float.Parse(reader["player_price"].ToString());
                tmp.playerxinfo_id = (int)reader["playerxinfo_id"];
                tmp.player_grade = (int)reader["player_grade"];
                tmp.playerxposition_name = (string)reader["playerxposition_name"];

                results.Add(tmp);
            }
            myConnection.Close();
            return Json(results);
        }
    }
    }
