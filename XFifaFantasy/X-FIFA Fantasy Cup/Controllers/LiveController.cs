using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using X_FIFA_Fantasy_Cup.Logic;
using System.Web.Http.Results;
using X_FIFA_Fantasy_Cup.Models;

namespace X_FIFA_Fantasy_Cup.Controllers
{
    public class LiveController : ApiController
    {
        [HttpPost]
        [ActionName("AddLive")]
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

                constructor.detail = result.ToString();

                return Json(constructor);
            }
            else
            {
                constructor.success = "false";
                constructor.detail = "Error while inserting the tournament";
                return Json(constructor);

            }
        }

    }
}
