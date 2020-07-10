using System;
using System.Collections;
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
    public partial class audit : System.Web.UI.Page
    {
        string sMessage = null, connetionString = null;
        SqlConnection connection = null;
        SqlCommand command = null;
        SqlDataReader dataReader = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            connetionString = "Data Source=127.0.0.1;Initial Catalog=BD_Android;User ID=Android;Password=123";

            string sOption = "", sSearch = "", sAuditID = "";
            if ((Request.QueryString["Option"] ?? "") != "")
            { sOption = Request.QueryString["Option"]; }

            if ((Request.QueryString["Search"] ?? "") != "")
            { sSearch = Request.QueryString["Search"]; }

            if ((Request.QueryString["AuditID"] ?? "") != "")
            { sAuditID = Request.QueryString["AuditID"]; }

            //sOption = "AuditInfo";
            //sAuditID = "1";

            switch (sOption)
            {
                case "AuditList":
                    Response.Write(FAuditList(sSearch).Replace("\n", ""));
                    break;
                case "AuditDetail":
                    int iAuditIDDetail = 0;
                    if (int.TryParse(sAuditID, out iAuditIDDetail))
                    { Response.Write(FAuditDetail(iAuditIDDetail).Replace("\n", "")); }
                    break;
                case "AuditInfo":
                    int iAuditIDInfo = 0;
                    if (int.TryParse(sAuditID, out iAuditIDInfo))
                    { Response.Write(FAuditInfo(iAuditIDInfo).Replace("\n", "")); }
                    break;
                default:
                    break;
            }
        }

        private string FAuditList(string Search)
        {
            string sResult = "";

            sMessage = "";

            connection = new SqlConnection(connetionString);
            try
            {
                connection.Open();
                command = new SqlCommand("[dbo].[sp_GetAuditListMobile]", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@Search", Search));

                dataReader = command.ExecuteReader();

                ArrayList obj = new ArrayList();
                while (dataReader.Read())
                {
                    obj.Add(new
                    {
                        AuditID = dataReader.GetValue(0).ToString(),
                        Name = dataReader.GetValue(1).ToString(),
                        TotalAssetQty = dataReader.GetValue(2).ToString()
                    });
                }
                var json = new JavaScriptSerializer().Serialize(obj);
                //json = "{ \"audit\" : " + json + "}";
                sResult = json;
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

        private string FAuditDetail(int AuditID)
        {
            string sResult = "";

            sMessage = "";

            connection = new SqlConnection(connetionString);
            try
            {
                connection.Open();
                command = new SqlCommand("[dbo].[sp_GetAuditDetailMobile]", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@AuditID", AuditID));

                dataReader = command.ExecuteReader();

                ArrayList obj = new ArrayList();
                while (dataReader.Read())
                {
                    obj.Add(new
                    {
                        AssetCode = dataReader.GetValue(0).ToString(),
                        Description = dataReader.GetValue(1).ToString()
                    });
                }
                var json = new JavaScriptSerializer().Serialize(obj);
                //json = "{ \"audit\" : " + json + "}";
                sResult = json;
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

        private string FAuditInfo(int AuditID)
        {
            string sResult = "";

            sMessage = "";

            connection = new SqlConnection(connetionString);
            try
            {
                connection.Open();
                command = new SqlCommand("[dbo].[sp_GetAuditInfoMobile]", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@AuditID", AuditID));

                dataReader = command.ExecuteReader();

                ArrayList obj = new ArrayList();
                while (dataReader.Read())
                {
                    obj.Add(new
                    {
                        AuditID = dataReader.GetValue(0).ToString(),
                        Name = dataReader.GetValue(1).ToString(),
                        AssetTotal = dataReader.GetValue(2).ToString(),
                        ScannTotal = dataReader.GetValue(3).ToString(),
                        AssetPending = dataReader.GetValue(4).ToString()
                    });
                }
                var json = new JavaScriptSerializer().Serialize(obj);
                //json = "{ \"audit\" : " + json + "}";
                sResult = json;
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
    }
}