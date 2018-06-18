using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using X_FIFA_Fantasy_Cup.Models;
using X_FIFA_Fantasy_Cup.Logic;
using System.Data;
using System.Globalization;

namespace X_FIFA_Fantasy_Cup.Controllers
{
    public class TournamentController : ApiController
    {
        [HttpGet]
        public JsonResult<List<Sponsor>> Sponsor()
        {
            List<Sponsor> results = new List<Sponsor>();
            Sponsor tmp = null;
            string action = "SELECT * FROM SPONSOR";
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            myConnection.Open();
            SqlCommand sqlCmd = new SqlCommand(action, myConnection);
            sqlCmd.CommandType = CommandType.Text;
            var reader = sqlCmd.ExecuteReader();
            while (reader.Read())
            {
                tmp = new Sponsor();
                tmp.sponsor_id = (int)reader["sponsor_id"];
                tmp.sponsor_name = (string)reader["sponsor_name"];
                results.Add(tmp);
            }
            myConnection.Close();
            return Json(results);
        }
        [HttpPost]
        [ActionName("AddPowerup")]
        public JsonResult<DbConnection> AddPowerup(Powerup powerup)
        {
            DbConnection constructor = new DbConnection();
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            myConnection.Open();
            SqlCommand sqlCmd = new SqlCommand("createpowerup", myConnection);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add(new SqlParameter("@pu_name", powerup.powerup_name));
            sqlCmd.Parameters.Add(new SqlParameter("@pu_name", powerup.powerup_name));
            sqlCmd.Parameters.Add(new SqlParameter("@pu_selection", powerup.powerup_selection));
            sqlCmd.Parameters.Add(new SqlParameter("@pu_sponsor", powerup.sponsor_id));
            sqlCmd.Parameters.Add(new SqlParameter("@pu_type", powerup.powerup_type_id));
            var returnparam = new SqlParameter { ParameterName = "@result", Direction = ParameterDirection.ReturnValue };
            sqlCmd.Parameters.Add(returnparam);
            sqlCmd.ExecuteNonQuery();
            int result = (int)returnparam.Value;
            if (result > 0)
            {
                constructor.success = "true";

                constructor.detail_type = result.ToString();

                return Json(constructor);
            }
            else
            {
                constructor.success = "false";
                constructor.detail_type = "Error while inserting the tournament";
                return Json(constructor);

            }
        }
        [HttpPost]
        [ActionName("Create")]
        public JsonResult<DbConnection> Create(Tournament tournament)
        {
            DbConnection constructor = new DbConnection();
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            System.Diagnostics.Debug.WriteLine("cargo base");
            myConnection.Open();
            SqlCommand sqlCmd = new SqlCommand("createtournament", myConnection);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add(new SqlParameter("@tournament_name", tournament.tournament_name));
            sqlCmd.Parameters.Add(new SqlParameter("@sponsor_id",tournament.sponsor_id));
            var returnparam = new SqlParameter { ParameterName = "@result", Direction = ParameterDirection.ReturnValue };
            sqlCmd.Parameters.Add(returnparam);
            sqlCmd.ExecuteNonQuery();
            int result = (int)returnparam.Value;
            if (result > 0)
            {
                constructor.success = "true";

                constructor.detail_type = result.ToString();

                return Json(constructor);
            }

            else
            {
                constructor.success = "false";
                constructor.detail_type = "Error while inserting the tournament";
                return Json(constructor);

            }

        }
        [HttpPost]
        [ActionName("addcountry")]
        public JsonResult<DbConnection> AddCountry(Tournament tournament)
        {
            int result = new int();
            DbConnection constructor = new DbConnection();
            int t_id = (int)Int32.Parse(tournament.tournament_id.ToString());
            List<tourmamentxcountry> coun = tournament.countries;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            myConnection.Open();
            foreach (var i in coun)
            {
                SqlCommand sqlCmd = new SqlCommand("inserttourxcountry", myConnection);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                System.Diagnostics.Debug.WriteLine("cosoo:" + i.country_id + "tid:" + t_id);
                sqlCmd.Parameters.Add(new SqlParameter("@tournament_id", t_id));
                sqlCmd.Parameters.Add(new SqlParameter("@country_id", i.country_id));
                var returnparam = new SqlParameter { ParameterName = "@result", Direction = ParameterDirection.ReturnValue };
                sqlCmd.Parameters.Add(returnparam);
                try
                {
                    sqlCmd.ExecuteNonQuery();

                }
                catch (SqlException)
                {
                }
                
                result = (int)returnparam.Value;
                i.tournamentxcountry_id = (int)returnparam.Value;
                constructor.tournamentxcountry.Add(result);
                List<int> plays = i.players;
                foreach (var j in plays)
                {
                    
                    SqlCommand sqlCmdtmp = new SqlCommand("inserttourplayer", myConnection);
                    sqlCmdtmp.CommandType = CommandType.StoredProcedure;

                    sqlCmdtmp.Parameters.Add(new SqlParameter("@tourxcountry_id", i.tournamentxcountry_id));
                    sqlCmdtmp.Parameters.Add(new SqlParameter("@player_id", j));
                    var returnparamtmp = new SqlParameter { ParameterName = "@result", Direction = ParameterDirection.ReturnValue };
                    sqlCmdtmp.Parameters.Add(returnparamtmp);
                    sqlCmdtmp.ExecuteNonQuery();



                }
            }
            myConnection.Close();
            constructor.success = "true";
            return Json(constructor);
        }



        [HttpPost]
        [ActionName("AddMatch")]
        public JsonResult<DbConnection> AddMatch(Match match)
        {
            DbConnection constructor = new DbConnection();
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            System.Diagnostics.Debug.WriteLine("cargo base");
            myConnection.Open();
            SqlCommand sqlCmd = new SqlCommand("insertadminmatch", myConnection);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            //System.Diagnostics.Debug.WriteLine(DateTime.ParseExact(match.match_date, "yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture));
            sqlCmd.Parameters.Add(new SqlParameter("@match_date", Convert.ToDateTime(match.match_date, new CultureInfo("ru-RU"))));
            sqlCmd.Parameters.Add(new SqlParameter("@match_location", match.match_location));
            sqlCmd.Parameters.Add(new SqlParameter("@stage_id", match.stage_id));
            sqlCmd.Parameters.Add(new SqlParameter("@tournament_id", match.tournament_id));
            sqlCmd.Parameters.Add(new SqlParameter("@txc_team_1", match.txc_team_1));
            sqlCmd.Parameters.Add(new SqlParameter("@txc_team_2", match.txc_team_2));
            sqlCmd.Parameters.Add(new SqlParameter("@sxm_winner1", match.sxm_winner1));
            sqlCmd.Parameters.Add(new SqlParameter("@sxm_winner2", match.sxm_winner2));
            var returnparam = new SqlParameter { ParameterName = "@result", Direction = ParameterDirection.ReturnValue };
            sqlCmd.Parameters.Add(returnparam);
            sqlCmd.ExecuteNonQuery();
            int result = (int)returnparam.Value;
            if (result > 0)
            {
                constructor.success = "true";
                constructor.detail_type = result.ToString();
                return Json(constructor);
            }
            else
            {
                constructor.success = "false";
                constructor.detail_type = "Error while inserting the tournament";
                return Json(constructor);
            }

        }

        
        
    }
}

