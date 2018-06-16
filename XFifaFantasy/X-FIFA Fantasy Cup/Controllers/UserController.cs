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
    public class UserController : ApiController
    {

        
        [ActionName("login")]
        public JsonResult<DbConnection> login(LoginDetails data)
        {
            System.Diagnostics.Debug.WriteLine("llegó al post");

            DbConnection constructor = new DbConnection();
            if (data.username == "" | data.password == "")
            {
                constructor.success = "false";
                constructor.detail = "The given data is not complete";
                return Json(constructor);

            }
            System.Diagnostics.Debug.WriteLine("entrando al get");
            Object reader = null;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            myConnection.Open();
            SqlCommand sqlCmd = new SqlCommand("checkuser", myConnection);


            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add(new SqlParameter("@user_username", data.username));
            sqlCmd.Parameters.Add(new SqlParameter("@user_password", data.password));
            var returnparam = new SqlParameter { ParameterName = "@result", Direction = ParameterDirection.ReturnValue };
            sqlCmd.Parameters.Add(returnparam);
            sqlCmd.ExecuteNonQuery();
            int result = (int)returnparam.Value;
            System.Diagnostics.Debug.WriteLine(result);
            myConnection.Close();
            if (result > 0)
            {
                constructor.success = "true";

                constructor.detail = result.ToString();

                return Json(constructor);
            }
            else if (result == -3)
            {
                constructor.success = "false";
                constructor.detail = "The username and password don't match";
                return Json(constructor);
            }
            else
            {
                constructor.success = "false";
                constructor.detail = "The user doesn't exist";
                return Json(constructor);

            }
        }

        [HttpPost]
        [ActionName("register")]
        public JsonResult<DbConnection> register(Fanatic fanatic)
        {
            System.Diagnostics.Debug.WriteLine("llegó al post");
            DbConnection constructor = new DbConnection();
            System.Diagnostics.Debug.WriteLine("entrando al get");
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            System.Diagnostics.Debug.WriteLine("cargo base");
            myConnection.Open();
            SqlCommand sqlCmd = new SqlCommand("insertfanatic", myConnection);
            System.Diagnostics.Debug.WriteLine("cargo sqlcommand");
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add(new SqlParameter("@f_username", fanatic.fanatic_id));
            sqlCmd.Parameters.Add(new SqlParameter("@f_name", fanatic.fanatic_name));
            sqlCmd.Parameters.Add(new SqlParameter("@f_last_name", fanatic.fanatic_last_name));
            sqlCmd.Parameters.Add(new SqlParameter("@f_email", fanatic.fanatic_email));
            sqlCmd.Parameters.Add(new SqlParameter("@f_phone", fanatic.fanatic_phone));
            System.Diagnostics.Debug.WriteLine(fanatic.fanatic_birth.Date);
            sqlCmd.Parameters.Add(new SqlParameter("@f_birth", fanatic.fanatic_birth.Date));
            sqlCmd.Parameters.Add(new SqlParameter("@f_password", fanatic.fanatic_password));
            sqlCmd.Parameters.Add(new SqlParameter("@f_active", fanatic.fanatic_active));
            sqlCmd.Parameters.Add(new SqlParameter("@f_about", fanatic.fanatic_description));
            sqlCmd.Parameters.Add(new SqlParameter("@f_country", fanatic.fanatic_country));
            if (fanatic.fanatic_photo != null)
            {
                sqlCmd.Parameters.Add(new SqlParameter("@f_photo", fanatic.fanatic_photo));
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
                else if (result == -3)
                {
                    constructor.success = "false";
                    constructor.detail = "The username and password don't match";
                    return Json(constructor);
                }
                else
                {
                    constructor.success = "false";
                    constructor.detail = "The user doesn't exist";
                    return Json(constructor);

                }
            }
            else
            {
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
                else if (result == -3)
                {
                    constructor.success = "false";
                    constructor.detail = "The username and password don't match";
                    return Json(constructor);
                }
                else
                {
                    constructor.success = "false";
                    constructor.detail = "The user doesn't exist";
                    return Json(constructor);

                }
            }
        }

            [HttpPost]
        [ActionName("register")]
        public JsonResult<DbConnection> adminregister(Admin admin)
        {
            System.Diagnostics.Debug.WriteLine("llegó al post");
            DbConnection constructor = new DbConnection();
            System.Diagnostics.Debug.WriteLine("entrando al get");
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            System.Diagnostics.Debug.WriteLine("cargo base");
            myConnection.Open();
            SqlCommand sqlCmd = new SqlCommand("insertadmin", myConnection);
            System.Diagnostics.Debug.WriteLine("cargo sqlcommand");
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add(new SqlParameter("@a_username", admin.admin_id));
            sqlCmd.Parameters.Add(new SqlParameter("@a_name", admin.admin_name));
            sqlCmd.Parameters.Add(new SqlParameter("@a_last_name", admin.admin_last_name));
            sqlCmd.Parameters.Add(new SqlParameter("@a_email", admin.admin_email));                                   
            sqlCmd.Parameters.Add(new SqlParameter("@a_password", admin.admin_password));            
            
            

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
            else if (result == -3)
            {
                constructor.success = "false";
                constructor.detail = "The username and password don't match";
                return Json(constructor);
            }
            else
            {
                constructor.success = "false";
                constructor.detail = "The user doesn't exist";
                return Json(constructor);

            }
            

        }
        
    }
}
