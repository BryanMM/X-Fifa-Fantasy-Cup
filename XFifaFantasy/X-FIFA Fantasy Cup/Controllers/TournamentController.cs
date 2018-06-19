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
        [ActionName("gettournaments")]
        public JsonResult<List<Tournament>> tournaments()
        {
            List<Tournament> results = new List<Tournament>();
            Tournament tmp = null;
            string action = "SELECT * FROM TOURNAMENT";
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            myConnection.Open();
            SqlCommand sqlCmd = new SqlCommand(action, myConnection);
            sqlCmd.CommandType = CommandType.Text;
            var reader = sqlCmd.ExecuteReader();
            while (reader.Read())
            {
                tmp = new Tournament();
                tmp.tournament_id = (int)reader["tournament_id"];
                tmp.tournament_name = (string)reader["tournament_name"];
                
                string actiontmp = "select * from tournamentxstage full outer join stagexmatch on stagexmatch.txs_id = tournamentxstage.txs_id where tournamentxstage.tournament_id = "+tmp.tournament_id;

                SqlCommand sqlCmdtmp = new SqlCommand(actiontmp, myConnection);
                sqlCmdtmp.CommandType = CommandType.Text;
                var readertmp = sqlCmdtmp.ExecuteReader();
                Match matchtmp = null;
                while (readertmp.Read())
                {
                    if (!string.IsNullOrEmpty(readertmp["match_id"].ToString()))
                    {
                        matchtmp = new Match();
                        System.Diagnostics.Debug.WriteLine(readertmp["match_id"]);
                        matchtmp.match_id = Int32.Parse(readertmp["match_id"].ToString());
                        string action2 = "SELECT * FROM MATCH WHERE MATCH_ID = " + matchtmp.match_id;
                        SqlCommand sqlCmd2 = new SqlCommand(action2, myConnection);
                        sqlCmd2.CommandType = CommandType.Text;
                        var reader2 = sqlCmd2.ExecuteReader();
                        while (reader2.Read())
                        {
                            matchtmp.match_name = (string)reader2["match_date"].ToString();
                        }
                        tmp.matches.Add(matchtmp);
                    }
                    else
                    {
                        break;
                    }
                }

                results.Add(tmp);
            }
            myConnection.Close();
            return Json(results);
        }
        [HttpGet]
        public JsonResult<List<Sponsor>> sponsor()
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
            sqlCmd.Parameters.Add(new SqlParameter("@sponsor_id", tournament.sponsor_id));
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
        public JsonResult<Match> AddMatch(AdminMatch match)
        {
            Match constructor = new Match();
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            System.Diagnostics.Debug.WriteLine("cargo base");
            myConnection.Open();
            SqlCommand sqlCmd = new SqlCommand("insertadminmatch", myConnection);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            System.Diagnostics.Debug.WriteLine(match.stage_id +" "+ match.sxm_winner1 +" "+ match.sxm_winner2+" "  + match.tournament_id+" " + match.txc_team1+" " + match.txc_team2+" " + match.match_date + match.match_location);
            sqlCmd.Parameters.Add(new SqlParameter("@match_date", match.match_date));
            sqlCmd.Parameters.Add(new SqlParameter("@match_location", match.match_location));
            sqlCmd.Parameters.Add(new SqlParameter("@stage_id", match.stage_id));
            sqlCmd.Parameters.Add(new SqlParameter("@tournament_id", match.tournament_id));
            sqlCmd.Parameters.Add(new SqlParameter("@txc_team_1", match.txc_team1));
            sqlCmd.Parameters.Add(new SqlParameter("@txc_team_2", match.txc_team2));
            if (match.sxm_winner1 != "")
            {
                System.Diagnostics.Debug.WriteLine("entrando al coso");
                sqlCmd.Parameters.Add(new SqlParameter("@sxm_winner1", match.sxm_winner1));
                sqlCmd.Parameters.Add(new SqlParameter("@sxm_winner2", match.sxm_winner2));
                var returnparam = new SqlParameter { ParameterName = "@result", Direction = ParameterDirection.ReturnValue };
                sqlCmd.Parameters.Add(returnparam);
                sqlCmd.ExecuteNonQuery();
                int result = (int)returnparam.Value;
                if (result > 0)
                {
                    constructor.Success = "true";
                    constructor.stagexmatch_id = result;
                    constructor.stagexmatch_name = "winner of match "+match.match_number;
                    return Json(constructor);
                }
                else
                {
                    
                    constructor.Success = "false";
                    return Json(constructor);
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("no entro al coso");
                var returnparam = new SqlParameter { ParameterName = "@result", Direction = ParameterDirection.ReturnValue };
                sqlCmd.Parameters.Add(returnparam);
                sqlCmd.ExecuteNonQuery();
                int result = (int)returnparam.Value;
                if (result > 0)
                {
                    constructor.Success = "true";
                    constructor.stagexmatch_id= result;
                    constructor.stagexmatch_name = "winner of match " + match.match_number;
                    return Json(constructor);
                }
                else
                {
                    constructor.Success = "false";
                    return Json(constructor);
                }
            }

        }
        /*
        [HttpPost]
        [ActionName("GetStage")]
        public JsonResult<AdminMatch> getstage(AdminMatch match)
        {
            AdminMatch constructor = new AdminMatch();
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            System.Diagnostics.Debug.WriteLine("cargo base");
            myConnection.Open();
            if (match.stage_id > 1)
            {
                SqlCommand sqlCmd = new SqlCommand("getnextstage", myConnection);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                System.Diagnostics.Debug.WriteLine(DateTime.Parse(match.match_date));
                sqlCmd.Parameters.Add(new SqlParameter("@next_stage", match.stage_id));
                sqlCmd.Parameters.Add(new SqlParameter("@tournament_id", match.tournament_id));
                var returnparam = new SqlParameter { ParameterName = "@result", Direction = ParameterDirection.ReturnValue };
                sqlCmd.Parameters.Add(returnparam);
                sqlCmd.ExecuteNonQuery();
                int result = (int)returnparam.Value;
                sqlCmd.ExecuteNonQuery();
                SqlDataReader dr = sqlCmd.ExecuteReader();
                // System.Diagnostics.Debug.WriteLine();
               while (dr.Read())
                {
                    constructor.match_id =(int) dr["match_id"] ;
                    constructor.name_team_1 = (string)dr["name_team_1"];
                    constructor.name_team_2 = (string)dr["name_team_2"];
                    constructor.team_1 = (string)dr["team_1"];
                    constructor.team_2 = (string)dr["team_2"];
                }
            }
            else
            {

            }
            */
            

        }






    }


