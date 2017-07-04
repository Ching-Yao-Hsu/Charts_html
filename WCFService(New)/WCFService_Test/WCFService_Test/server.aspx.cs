using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Text;

namespace WCFService_Test
{
    public partial class server : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Return_Data();
            }
        }        

        private void Return_Data()
        {
            string str = "";            
            string str_conn = WebConfigurationManager.ConnectionStrings["ECOSMARTConnectionString"].ConnectionString;
            string str_cmd = "SELECT M.InstallPosition,(M.ECO_Account + '-' + CONVERT(nvarchar(50), MeterID)) AS ECO_AccountAndMeterId FROM AdminSetup AS A";
            str_cmd += " INNER JOIN ControllerSetup AS C ON A.Account  = C.Account";
            str_cmd += " INNER JOIN MeterSetup AS M ON M.ECO_Account = C.ECO_Account";
            str_cmd += " WHERE LineNum = @line AND A.Account = @eco ";
            str_cmd += " ORDER BY LineNum";

            
            StringBuilder json = new StringBuilder();
            str = (Request.Form["nodeId"] != null) ? Request.Form["nodeId"].ToString() : null;

            Response.Clear();
            Response.ContentType = "application/json; charset=utf-8";
            using (SqlConnection conn = new SqlConnection(str_conn))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(str_cmd, conn))
                {
                    cmd.Parameters.AddWithValue("@eco","eco");
                    cmd.Parameters.AddWithValue("@line", str);
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows) {
                        dr.Read();                        
                        json.Append("[{\"ECO_AccountAndMeterId\":\"" + dr["ECO_AccountAndMeterId"].ToString() + "\",\"InstallPosition\":\"" + dr["InstallPosition"].ToString() + "\"}]");
                    }
                }
            }
            Response.Write(json);
            Response.End();
        }
    }
}