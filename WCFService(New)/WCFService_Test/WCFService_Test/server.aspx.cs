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
            string str_cmd_MeterIdAccount = "SELECT M.InstallPosition,(M.ECO_Account + '-' + CONVERT(nvarchar(50), MeterID)) AS ECO_AccountAndMeterId FROM AdminSetup AS A";
            str_cmd_MeterIdAccount += " INNER JOIN ControllerSetup AS C ON A.Account  = C.Account";
            str_cmd_MeterIdAccount += " INNER JOIN MeterSetup AS M ON M.ECO_Account = C.ECO_Account";
            str_cmd_MeterIdAccount += " WHERE LineNum = @line AND A.Account = @eco ";
            str_cmd_MeterIdAccount += " ORDER BY LineNum";

            string str_cmd_dropdownlist = "SELECT [ECO_Group], [Account] FROM [AdminSetup]";
            
            StringBuilder json = new StringBuilder();
            str = (Request.Form["nodeId"] != null) ? Request.Form["nodeId"].ToString() : null;

            Response.Clear();
            Response.ContentType = "application/json; charset=utf-8";
            json.Append("[{\"AccountMeterId\":[");
            using (SqlConnection conn = new SqlConnection(str_conn))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(str_cmd_MeterIdAccount, conn))
                {
                    cmd.Parameters.AddWithValue("@eco","eco");
                    cmd.Parameters.AddWithValue("@line", "00");
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows) {
                        dr.Read();                        
                        json.Append("{\"ECO_AccountAndMeterId\":\"" + dr["ECO_AccountAndMeterId"].ToString() + "\",\"InstallPosition\":\"" + dr["InstallPosition"].ToString() + "\"}");
                    }
                    dr.Close();
                }
                json.Append("]},{\"Group\":[");

                using (SqlCommand cmd = new SqlCommand(str_cmd_dropdownlist, conn))
                {
                    using (SqlDataAdapter adr = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adr.Fill(dt);
                        json.Append("{\"ECO_Group\":\"" + dt.Rows[0]["ECO_Group"].ToString() + "\",\"Account\":\"" + dt.Rows[0]["Account"].ToString() + "\"}");
                        for (int i = 1; i < dt.Rows.Count; i++)
                        {
                            json.Append(",{\"ECO_Group\":\"" + dt.Rows[i]["ECO_Group"].ToString() + "\",\"Account\":\"" + dt.Rows[i]["Account"].ToString() + "\"}");
                        }
                    }
                }
                json.Append("]}]");
                conn.Close();
            }
            Response.Write(json);
            Response.End();
        }
    }
}