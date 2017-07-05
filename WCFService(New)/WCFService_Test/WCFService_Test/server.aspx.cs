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
            if (Request.Form["ECO_Group_account"] == null)
            {
                Return_Data();
            }
            else if (Request.Form["ECO_Group_account"] != null)
            {

                Return_Data(Request.Form["ECO_Group_account"].ToString());

            }

        }

        private void Return_Data()
        {
            string str_conn = WebConfigurationManager.ConnectionStrings["ECOSMARTConnectionString"].ConnectionString;
            string str_cmd_dropdownlist = "";
            StringBuilder json = new StringBuilder();

            str_cmd_dropdownlist = "SELECT [ECO_Group], [Account] FROM [AdminSetup]";
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
                        json.Append("[{\"ECO_Group\":\"" + dt.Rows[0]["ECO_Group"].ToString() + "\",\"Account\":\"" + dt.Rows[0]["Account"].ToString() + "\"}");
                        for (int i = 1; i < dt.Rows.Count; i++)
                        {
                            json.Append(",{\"ECO_Group\":\"" + dt.Rows[i]["ECO_Group"].ToString() + "\",\"Account\":\"" + dt.Rows[i]["Account"].ToString() + "\"}");
                        }
                        json.Append("]");
                    }
                }
            }
            Response.Write(json);
            Response.End();
        }

        private void Return_Data(string _ECO_Group_account)
        {
            string str_conn = WebConfigurationManager.ConnectionStrings["ECOSMARTConnectionString"].ConnectionString;
            string str_cmd_TreeView = "";
            StringBuilder json = new StringBuilder();

            str_cmd_TreeView = "SELECT M.LineNum,M.InstallPosition FROM AdminSetup AS A";
            str_cmd_TreeView += " INNER JOIN ControllerSetup AS C ON A.Account = C.Account";
            str_cmd_TreeView += " INNER JOIN MeterSetup AS M ON M.ECO_Account = C.ECO_Account";
            str_cmd_TreeView += " WHERE M.LineNum != '' AND A.Account = @Account ORDER BY M.LineNum";

            Response.Clear();
            Response.ContentType = "application/json; charset=utf-8";
            using (SqlConnection conn = new SqlConnection(str_conn))
            {
                using (SqlCommand cmd = new SqlCommand(str_cmd_TreeView, conn))
                {
                    cmd.Parameters.AddWithValue("@Account", _ECO_Group_account);
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adp.Fill(dt);
                        json.Append("[{\"id\":\"" + dt.Rows[0]["lineNum"] + "\",\"remark\":\"" + dt.Rows[0]["InstallPosition"] + "\"}");
                        for (int i = 1; i < dt.Rows.Count; i++)
                        {
                            json.Append(",{\"id\":\"" + dt.Rows[i]["lineNum"] + "\",\"remark\":\"" + dt.Rows[i]["InstallPosition"] + "\"}");
                        }
                        json.Append("]");
                    }
                }
            }
            Response.Write(json);
            Response.End();
        }


        //private string Return_Data(string aaa)
        //{
        //    string nodeId = "";
        //    string str_conn = WebConfigurationManager.ConnectionStrings["ECOSMARTConnectionString"].ConnectionString;
        //    string str_cmd_MeterIdAccount = "";
        //    string str_cmd_dropdownlist = "";
        //    string str_cmd_TreeView = "";
        //    StringBuilder json = new StringBuilder();

        //    Response.Clear();
        //    Response.ContentType = "application/json; charset=utf-8";
        //    json.Append("[{\"Modal\":[");
        //    using (SqlConnection conn = new SqlConnection(str_conn))
        //    {
        //        conn.Open();

        //        str_cmd_MeterIdAccount = "SELECT M.InstallPosition,(M.ECO_Account + '-' + CONVERT(nvarchar(50), MeterID)) AS ECO_AccountAndMeterId FROM AdminSetup AS A";
        //        str_cmd_MeterIdAccount += " INNER JOIN ControllerSetup AS C ON A.Account  = C.Account";
        //        str_cmd_MeterIdAccount += " INNER JOIN MeterSetup AS M ON M.ECO_Account = C.ECO_Account";
        //        str_cmd_MeterIdAccount += " WHERE LineNum = @nodeId AND A.Account = @eco ";
        //        str_cmd_MeterIdAccount += " ORDER BY LineNum";
        //        using (SqlCommand cmd = new SqlCommand(str_cmd_MeterIdAccount, conn))
        //        {
        //            nodeId = (Request.Form["nodeId"] != null) ? Request.Form["nodeId"].ToString() : null;
        //            cmd.Parameters.AddWithValue("@eco", "eco");
        //            cmd.Parameters.AddWithValue("@nodeId", nodeId);
        //            SqlDataReader dr = cmd.ExecuteReader();
        //            if (dr.HasRows)
        //            {
        //                dr.Read();
        //                json.Append("{\"ECO_AccountAndMeterId\":\"" + dr["ECO_AccountAndMeterId"].ToString() + "\",\"InstallPosition\":\"" + dr["InstallPosition"].ToString() + "\"}");
        //            }
        //            dr.Close();
        //        }
        //        json.Append("]},{\"DropDownList\":[");


        //        str_cmd_dropdownlist = "SELECT [ECO_Group], [Account] FROM [AdminSetup]";
        //        using (SqlCommand cmd = new SqlCommand(str_cmd_dropdownlist, conn))
        //        {
        //            using (SqlDataAdapter adr = new SqlDataAdapter(cmd))
        //            {
        //                DataTable dt = new DataTable();
        //                adr.Fill(dt);
        //                json.Append("{\"ECO_Group\":\"" + dt.Rows[0]["ECO_Group"].ToString() + "\",\"Account\":\"" + dt.Rows[0]["Account"].ToString() + "\"}");
        //                for (int i = 1; i < dt.Rows.Count; i++)
        //                {
        //                    json.Append(",{\"ECO_Group\":\"" + dt.Rows[i]["ECO_Group"].ToString() + "\",\"Account\":\"" + dt.Rows[i]["Account"].ToString() + "\"}");
        //                }
        //            }
        //        }

        //        str_cmd_TreeView = "SELECT ms.ECO_Account,ms.CtrlNr,ms.MeterID,ms.LineNum,ms.InstallPosition FROM ECOSMART.dbo.MeterSetup as ms WHERE ms.LineNum != '' AND ms.ECO_Account LIKE 'twenergy%' ORDER BY ms.LineNum";
        //        int count = 0;
        //        json.Append("]},{\"TreeView\":[");
        //        using (SqlCommand cmd = new SqlCommand(str_cmd_TreeView, conn))
        //        {
        //            using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
        //            {
        //                DataTable dt = new DataTable();
        //                adp.Fill(dt);
        //                count = dt.Rows.Count;
        //                json.Append("{\"id\":\"" + dt.Rows[0]["lineNum"] + "\",\"remark\":\"" + dt.Rows[0]["InstallPosition"] + "\"}");
        //                for (int i = 1; i < count; i++)
        //                {
        //                    json.Append(",{\"id\":\"" + dt.Rows[i]["lineNum"] + "\",\"remark\":\"" + dt.Rows[i]["InstallPosition"] + "\"}");
        //                }
        //            }
        //        }

        //        json.Append("]}]");
        //        conn.Close();
        //    }
        //    Response.Write(json);
        //    Response.End();
        //    return aaa;
        //}
    }
}