using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using XFifaFantasy.Models;
using XFifaFantasy.Logic;
using System.Data;
using System.Web.Http.Cors;

namespace XFifaFantasy.Controllers
{
    public class UserController : ApiController
    {

        [HttpPost]
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
            System.Diagnostics.Debug.WriteLine("cargo base");
            SqlCommand sqlCmd = new SqlCommand();
            System.Diagnostics.Debug.WriteLine("cargo sqlcommand");


            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@username", SqlDbType.VarChar).Value = data.username;
            sqlCmd.Parameters.Add("@password", SqlDbType.VarChar).Value = data.password;
            System.Diagnostics.Debug.WriteLine("cargo comando");
            sqlCmd.Connection = myConnection;
            myConnection.Open();
            System.Diagnostics.Debug.WriteLine("estado " + myConnection.State);
            reader = sqlCmd.ExecuteScalar();

            if (Int32.Parse(reader.ToString()) > 0)
            {
                constructor.success = "true";
                constructor.detail = "";
                myConnection.Close();
                return Json(constructor);
            }
            else
            {
                constructor.success = "false";
                constructor.detail = "The username and password don't match";
                return Json(constructor);
            }
        }

        [HttpPost]
        public JsonResult<DbConnection> Post(Fanatic fanatic)
        {
            System.Diagnostics.Debug.WriteLine("llegó al post");
            DbConnection constructor = new DbConnection();
            
            System.Diagnostics.Debug.WriteLine("entrando al get");
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            System.Diagnostics.Debug.WriteLine("cargo base");
            SqlCommand sqlCmd = new SqlCommand();
            System.Diagnostics.Debug.WriteLine("cargo sqlcommand");


            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@", SqlDbType.VarChar).Value = fanatic.name;
            sqlCmd.Parameters.Add("@password", SqlDbType.VarChar).Value = fanatic.user_password;
            System.Diagnostics.Debug.WriteLine("cargo comando");
            sqlCmd.Connection = myConnection;
            myConnection.Open();
            System.Diagnostics.Debug.WriteLine("estado " + myConnection.State);
            reader = sqlCmd.ExecuteScalar();

            if (Int32.Parse(reader.ToString()) > 0)
            {
                constructor.success = "true";
                constructor.detail = "";
                myConnection.Close();
                return Json(constructor);
            }
            else
            {
                constructor.success = "false";
                constructor.detail = "The username and password don't match";
                return Json(constructor);
            }
        }
    }
}
