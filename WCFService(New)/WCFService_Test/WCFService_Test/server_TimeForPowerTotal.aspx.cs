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
    public partial class server_TimeForPowerTotal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Return_Data();
        }

        private void Return_Data()
        {
            string str_conn = WebConfigurationManager.ConnectionStrings["ECO_shichiConnectionString"].ConnectionString;
            string str_cmd_shichi = "SELECT top 1 [W], [KWh] , ([KWh]*3) as powertotal FROM [PowerRecord]";

            StringBuilder json = new StringBuilder();
            Response.Clear();
            Response.ContentType = "application/json; charset=utf-8";
            using (SqlConnection conn = new SqlConnection(str_conn))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(str_cmd_shichi, conn))
                {                    
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        json.Append("[{\"W\":\"" + dr["W"].ToString() + "\",\"KWh\":\"" + dr["KWh"].ToString() + "\",\"powertotal\":\"" + dr["powertotal"].ToString() + "\"}]");
                    }
                    dr.Close();
                }
            }
            Response.Write(json);
            Response.End();
        }
    }
}