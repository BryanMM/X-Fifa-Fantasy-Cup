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
            sqlCmd.Parameters.Add(new SqlParameter("@tournament_name", tournament.tournament_name));
            sqlCmd.Parameters.Add(new SqlParameter("@sponsor_id", tournament.tournament_id));
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
        public JsonResult<DbConnection> AddCountry(Country country)
        {            
            DbConnection constructor = new DbConnection();            
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;          
            myConnection.Open();
            SqlCommand sqlCmd = new SqlCommand("inserttourxcountry", myConnection);            
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add(new SqlParameter("@tournament_id", country.tournament_id));
            sqlCmd.Parameters.Add(new SqlParameter("@country_id", country.Id));            
            var returnparam = new SqlParameter { ParameterName = "@result", Direction = ParameterDirection.ReturnValue };
            sqlCmd.Parameters.Add(returnparam);
            sqlCmd.ExecuteNonQuery();
            int result = (int)returnparam.Value;
            if (result > 0)
            {
                constructor.success = "true";

                constructor.detail_type = "Successfully added the country";

                return Json(constructor);
            }
           
            else
            {
                constructor.success = "false";
                constructor.detail_type = "Error while trying to add the country";
                return Json(constructor);

            }

        }
        /*
        [HttpPost]
        [ActionName("AddMatch")]
        public JsonResult<DbConnection> AddMatch()
        {

        }
        */
    }
}
