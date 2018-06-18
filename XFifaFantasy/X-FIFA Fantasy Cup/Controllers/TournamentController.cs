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


namespace X_FIFA_Fantasy_Cup.Controllers
{
    public class TournamentController : ApiController
    {
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
            System.Diagnostics.Debug.WriteLine("llegó al post");
            DbConnection constructor = new DbConnection();
            System.Diagnostics.Debug.WriteLine("entrando al get");
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            System.Diagnostics.Debug.WriteLine("cargo base");
            myConnection.Open();
            SqlCommand sqlCmd = new SqlCommand("createtournament", myConnection);
            System.Diagnostics.Debug.WriteLine("cargo sqlcommand");
            sqlCmd.CommandType = CommandType.StoredProcedure;
            System.Diagnostics.Debug.WriteLine("COSO:" + tournament.tournament_name + "ID: " + tournament.tournament_id);
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
        [ActionName("Add")]
        public JsonResult<DbConnection> AddCountry(Tournament tournament)
        {
            int result = new int();
            DbConnection constructor = new DbConnection();

            int t_id = tournament.tournament_id;
            List<tourmamentxcountry> coun = tournament.countries;
            foreach (var i in coun)
            {
                SqlConnection myConnection = new SqlConnection();
                myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                myConnection.Open();
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
                myConnection.Close();
                result = (int)returnparam.Value;
                i.tournamentxcountry_id = (int)returnparam.Value;
                constructor.tournamentxcountry.Add(result);


                List<int> plays = i.players;
                foreach (var j in plays)
                {
                    myConnection.Open();
                    SqlCommand sqlCmdtmp = new SqlCommand("inserttourplayer", myConnection);
                    sqlCmdtmp.CommandType = CommandType.StoredProcedure;

                    sqlCmdtmp.Parameters.Add(new SqlParameter("@tourxcountry_id", i.tournamentxcountry_id));
                    sqlCmdtmp.Parameters.Add(new SqlParameter("@player_id", j));
                    var returnparamtmp = new SqlParameter { ParameterName = "@result", Direction = ParameterDirection.ReturnValue };
                    sqlCmdtmp.Parameters.Add(returnparamtmp);
                    sqlCmdtmp.ExecuteNonQuery();
                    


                }
            }
            return Json(constructor);         
        /*
        [HttpPost]
        [ActionName("AddMatch")]
        public JsonResult<DbConnection> AddMatch()
        {

        }
        */
        }
    }
}

