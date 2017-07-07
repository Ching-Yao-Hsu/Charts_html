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
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Return_Data();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

        }

        private void Return_Data()
        {
            string str_conn = WebConfigurationManager.ConnectionStrings["ECO_shichiConnectionString"].ConnectionString;
            string str_cmd_dropdownlist = "";
            StringBuilder json = new StringBuilder();

            str_cmd_dropdownlist = "select top 40000 * from PowerRecord";
            Response.Clear();
            Response.ContentType = "application/json; charset=utf-8";
            using (SqlConnection conn = new SqlConnection(str_conn))
            {
                using (SqlCommand cmd = new SqlCommand(str_cmd_dropdownlist, conn))
                {
                    using (SqlDataAdapter adr = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adr.Fill(dt);
                        json.Append("[{\"MeterID\":\"" + dt.Rows[0]["MeterID"].ToString() + "\",\"CtrlNr\":\"" + dt.Rows[0]["CtrlNr"].ToString() + "\",\"RecDate\":\"" + dt.Rows[0]["RecDate"].ToString() + "\",\"RecTime\":\"" + dt.Rows[0]["RecTime"].ToString() + "\"}");
                        for (int i = 1; i < dt.Rows.Count; i++)
                        {
                            json.Append(",{\"MeterID\":\"" + dt.Rows[i]["MeterID"].ToString() + "\",\"CtrlNr\":\"" + dt.Rows[i]["CtrlNr"].ToString() + "\",\"RecDate\":\"" + dt.Rows[i]["RecDate"].ToString() + "\",\"RecTime\":\"" + dt.Rows[i]["RecTime"].ToString() + "\"}");
                        }
                        json.Append("]");
                    }
                }
            }
            Response.Write(json);
            Response.End();
        }
    }
}