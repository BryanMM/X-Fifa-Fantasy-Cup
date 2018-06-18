using System;
using System.Globalization;
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
            DbConnection constructor = new DbConnection();
            if (data.username == "" | data.password == "")
            {
                constructor.success = "false";
                constructor.detail_type = "The given data is not complete";
                return Json(constructor);
            }            
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
            SqlDataReader dr = sqlCmd.ExecuteReader();
            var result = (int)returnparam.Value;
           // System.Diagnostics.Debug.WriteLine();
            
           
            while (dr.Read())
            {
                constructor.success = "";
                constructor.detail_type = (string) dr["user_type"].ToString();
                constructor.detail_xinfo = (string)dr["user_login"].ToString();
                constructor.detail_status = (string)dr["user_active"].ToString();
            }
            if (result >= 0)
            {
                constructor.success = "true";
                
                return Json(constructor);
            }
            else if (result == -3)
            {
                constructor.success = "false";
                
                return Json(constructor);
            }
            else
            {
                constructor.success = "false";
                
                return Json(constructor);

            }
        }
        [HttpPost]
        [ActionName("register")]
        public JsonResult<DbConnection> Register(Fanatic fanatic)
        {
            DbConnection constructor = new DbConnection();
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;           
            myConnection.Open();
            SqlCommand sqlCmd = new SqlCommand("insertfanatic", myConnection);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add(new SqlParameter("@f_login", fanatic.fanatic_id));
            sqlCmd.Parameters.Add(new SqlParameter("@f_name", fanatic.fanatic_name));
            sqlCmd.Parameters.Add(new SqlParameter("@f_last_name", fanatic.fanatic_last_name));
            sqlCmd.Parameters.Add(new SqlParameter("@f_email", fanatic.fanatic_email));
            sqlCmd.Parameters.Add(new SqlParameter("@f_phone", fanatic.fanatic_phone));
            System.Diagnostics.Debug.WriteLine("FECHAAAA="+fanatic.fanatic_password);
            sqlCmd.Parameters.Add(new SqlParameter("@f_birth", Convert.ToDateTime(fanatic.fanatic_birth, new CultureInfo("ru-RU"))));
            sqlCmd.Parameters.Add(new SqlParameter("@f_password", fanatic.fanatic_password));
            
            sqlCmd.Parameters.Add(new SqlParameter("@f_about", fanatic.fanatic_description));
            sqlCmd.Parameters.Add(new SqlParameter("@f_country", fanatic.fanatic_country));
            if (fanatic.fanatic_photo != null)
            {
                sqlCmd.Parameters.Add(new SqlParameter("@f_photo", fanatic.fanatic_photo));
                var returnparam = new SqlParameter { ParameterName = "@result", Direction = ParameterDirection.ReturnValue };
                sqlCmd.Parameters.Add(returnparam);
                sqlCmd.ExecuteNonQuery();
                int result = (int)returnparam.Value;
                myConnection.Close();
                if (result > 0)
                {
                    constructor.success = "true";

                    constructor.detail_type = result.ToString();

                    return Json(constructor);
                }
                else if (result == -3)
                {
                    constructor.success = "false";
                    constructor.detail_type = "The username and password don't match";
                    return Json(constructor);
                }
                else
                {
                    constructor.success = "false";
                    constructor.detail_type = "The user doesn't exist";
                    return Json(constructor);
                }
            }
            else
            {
                var returnparam = new SqlParameter { ParameterName = "@result", Direction = ParameterDirection.ReturnValue };
                sqlCmd.Parameters.Add(returnparam);
                sqlCmd.ExecuteNonQuery();
                int result = (int)returnparam.Value;
                myConnection.Close();
                if (result > 0)
                {
                    constructor.success = "true";

                    constructor.detail_type = "";

                    return Json(constructor);
                }
                else
                {
                    constructor.success = "false";
                    constructor.detail_type = "User already exists";
                    return Json(constructor);

                }
            }
        }
        [HttpPost]
        [ActionName("adminregister")]
        public JsonResult<DbConnection> adminregister(Admin admin)
        {
            DbConnection constructor = new DbConnection();
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            System.Diagnostics.Debug.WriteLine("cargo base");
            myConnection.Open();
            SqlCommand sqlCmd = new SqlCommand("insertadmin", myConnection);
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
            myConnection.Close();
            if (result > 0)
            {
                constructor.success = "true";
                constructor.detail_type = "";
                return Json(constructor);
            }
            else
            {
                constructor.success = "false";
                constructor.detail_type = "The user already exist";
                return Json(constructor);

            }
            

        }

        [HttpPost]
        public JsonResult<Fanatic> getinfo(DbConnection dbConnection)
        {
            Fanatic result = new Fanatic();
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            if (dbConnection.detail_type == "2")
            {
                string action = "select * from userxinfo full outer join fanatic on userxinfo.fanatic_login = fanatic.fanatic_login where userxinfo_id ="+ Int32.Parse(dbConnection.detail_xinfo);

                myConnection.Open();
                SqlCommand sqlCmd = new SqlCommand(action, myConnection);


                sqlCmd.CommandType = CommandType.Text;
                var reader = sqlCmd.ExecuteReader();
                while (reader.Read())
                {
                    DateTime datetmp = (DateTime)reader["fanatic_date_create"];   
                    result.fanatic_name = (string)reader["fanatic_name"];
                    result.fanatic_last_name = (string)reader["fanatic_last_name"];
                    //result.fanatic_birth = reader["fanatic_birth"].ToString();
                    result.fanatic_date_create = datetmp.ToString("dd-mm-yyyy hh:mm");
                    result.fanatic_description = (string)reader["fanatic_description"];
                    result.fanatic_email = (string)reader["fanatic_email"];
                    result.fanatic_id = (string)reader["fanatic_login"];                   
                }
                myConnection.Close();
                return Json(result);
            }
            else
            {
                string action = "select * from adminxinfo full outer join admin on adminxinfo.admin_username = admin.admin_username where adminxinfo_id =" + Int16.Parse(dbConnection.detail_xinfo);
                myConnection.Open();
                SqlCommand sqlCmd = new SqlCommand(action, myConnection);


                sqlCmd.CommandType = CommandType.Text;
                var reader = sqlCmd.ExecuteReader();
                while (reader.Read())
                {
                    DateTime datetmp = (DateTime)reader["fanatic_date_create"];
                    result.fanatic_name = (string)reader["admin_name"];
                    result.fanatic_last_name = (string)reader["admin_last_name"];
                    //result.fanatic_birth = reader["fanatic_birth"].ToString();
                    result.fanatic_date_create = datetmp.ToString("dd-mm-yyyy hh:mm");                    
                    result.fanatic_email = (string)reader["admin_email"];
                    result.fanatic_id = (string)reader["admin_username"];
                }
                myConnection.Close();
                return Json(result);
            }            
        }



    }
}
