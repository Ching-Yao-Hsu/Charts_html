using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace Test
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string str_conn = "Data Source=.;Initial Catalog=ChartTest;Persist Security Info=True;User ID=sa;Password=1qaz@wsx";
            string str_cmd = "SELECT * FROM ChartTest.dbo.MeterInfo WHERE lineNum != '' ORDER BY lineNum";
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
                        json = "[{\"id\":\"" + dt.Rows[0]["lineNum"] + "\"}";
                        for (int i = 1; i < count - 1; i++)
                        {
                            json += ",{\"id\":\"" + dt.Rows[i]["lineNum"] + "\"}";
                        }
                        json += ",{\"id\":\"" + dt.Rows[count - 1]["lineNum"] + "\"}]";                   
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