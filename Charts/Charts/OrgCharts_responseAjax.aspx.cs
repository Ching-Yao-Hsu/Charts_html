using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace Charts
{
    public partial class OrgCharts_responseAjax : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //string str_conn = WebConfigurationManager.ConnectionStrings["ECOSMARTConnectionString"].ConnectionString;
            //string str_cmd = "SELECT ms.ECO_Account,ms.CtrlNr,ms.MeterID,ms.LineNum,ms.InstallPosition FROM ECOSMART.dbo.MeterSetup as ms WHERE ms.LineNum != '' AND ms.ECO_Account LIKE 'twenergy%' ORDER BY ms.LineNum";
            string str_conn = WebConfigurationManager.ConnectionStrings["TESTConnectionString"].ConnectionString;
            string str_cmd = "SELECT mi.meterId,mi.lineNum from dbo.MeterInfo as mi where mi.lineNum !='' order by mi.lineNum";

            //StringBuilder json = new StringBuilder();
            string json = "";
            int count = 0;
            using (SqlConnection conn = new SqlConnection(str_conn))
            {
                using (SqlCommand cmd = new SqlCommand(str_cmd, conn))
                {
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adp.Fill(dt);
                        count = dt.Rows.Count;
                        Response.Clear();
                        Response.ContentType = "application/json; charset=utf-8";
                        json = "[{\"id\":\"" + dt.Rows[0]["lineNum"] + "\",\"remark\":\"" + dt.Rows[0]["meterId"] + "\"}";
                        for (int i = 1; i < count - 1; i++)
                        {
                            json += ",{\"id\":\"" + dt.Rows[i]["lineNum"] + "\",\"remark\":\"" + dt.Rows[i]["meterId"] + "\"}";
                        }
                        json += ",{\"id\":\"" + dt.Rows[count-1]["lineNum"] + "\",\"remark\":\"" + dt.Rows[count - 1]["meterId"] + "\"}]";
                        Response.Write(json);
                        Response.End();
                    }
                }
            }
            //string json = "{\"name\":\"JOE\"}";
            //Response.Clear();
            //Response.ContentType = "application/json; charset=utf-8";
            //Response.Write(json);
            //Response.End();
        }       
    }
}