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
    public partial class WebForm3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        //    string str_conn = WebConfigurationManager.ConnectionStrings["ECOSMARTConnectionString"].ConnectionString;
        //    string str_cmd_dropdownlist = "";
        //    StringBuilder json = new StringBuilder();

        //    str_cmd_dropdownlist = "SELECT [ECO_Group], [Account] FROM [AdminSetup]";
        //    Response.Clear();
        //    Response.ContentType = "application/json; charset=utf-8";
        //    using (SqlConnection conn = new SqlConnection(str_conn))
        //    {
        //        using (SqlCommand cmd = new SqlCommand(str_cmd_dropdownlist, conn))
        //        {
        //            using (SqlDataAdapter adr = new SqlDataAdapter(cmd))
        //            {
        //                DataTable dt = new DataTable();
        //                adr.Fill(dt);
        //                json.Append("[{\"ECO_Group\":\"" + dt.Rows[0]["ECO_Group"].ToString() + "\",\"Account\":\"" + dt.Rows[0]["Account"].ToString() + "\"}");
        //                for (int i = 1; i < dt.Rows.Count; i++)
        //                {
        //                    json.Append(",{\"ECO_Group\":\"" + dt.Rows[i]["ECO_Group"].ToString() + "\",\"Account\":\"" + dt.Rows[i]["Account"].ToString() + "\"}");
        //                }
        //                json.Append("]");
        //            }
        //        }
        //    }
        //    Response.Write(json);
        //    Response.End();
        }
    }
}