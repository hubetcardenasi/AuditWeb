using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AuditWeb.api
{
    public partial class values : System.Web.UI.Page
    {
        string UserID = "", Password = "";
        string sMessage = null, connetionString = null;
        SqlConnection connection = null;
        SqlCommand command = null;
        SqlDataReader dataReader = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            connetionString = "Data Source=127.0.0.1;Initial Catalog=BD_Android;User ID=Android;Password=123";

            if ((Request.QueryString["UserID"] ?? "") != "")
            { UserID = Request.QueryString["UserID"]; }

            if ((Request.QueryString["Password"] ?? "") != "")
            { Password = Request.QueryString["Password"]; }

            if (UserID.Length > 0)
            {
                var obj = new Login
                {
                    UserID = UserID,
                    ValidAccess = FLogin(UserID, Password)
                };
                var json = new JavaScriptSerializer().Serialize(obj);
                json = "{ \"login\" : " + json + "}";
                Response.Write(json.Replace("\n", ""));
            }
        }

        private string FLogin(string UserID, string Password)
        {
            string sResult = "";

            sMessage = "";

            
            connection = new SqlConnection(connetionString);
            try
            {
                connection.Open();
                command = new SqlCommand("[dbo].[sp_GetLogin]", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@pUserID", UserID));
                command.Parameters.Add(new SqlParameter("@pPassword", FGetPassword(Password)));

                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                { sResult = dataReader.GetValue(0).ToString(); }
            }
            catch (Exception ex)
            { sMessage = "Error: " + ex.Message; }
            finally
            {
                if (sMessage.Length > 0)
                { sResult = sMessage; }
                if (command != null)
                { command.Dispose(); }
                if (dataReader != null)
                { dataReader.Close(); }
                if (connection != null)
                { connection.Close(); }
            }
            return sResult;
        }

        private string FGetPassword(string sPassword)
        {
            string _return = "", _error_message = "";
            try
            {
                byte[] _data = System.Text.Encoding.ASCII.GetBytes(sPassword);
                _data = new System.Security.Cryptography.SHA256Managed().ComputeHash(_data);
                String _hash = System.Text.Encoding.ASCII.GetString(_data);
                _return = _hash;
            }
            catch (Exception exp)
            { _error_message = exp.Message; }
            return _return;
        }
    }

    public class Login
    {
        public string UserID;
        public string ValidAccess;
    }
}