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
                if (Request.Form["node"] != null)
                {
                    Return_Data(Request.Form["node"].ToString(), Request.Form["ECO_Group_account"].ToString());
                }
                else
                {
                    Return_Data(Request.Form["ECO_Group_account"].ToString());
                }
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

        private void Return_Data(string _node, string _account)
        {
            string str_conn = WebConfigurationManager.ConnectionStrings["ECOSMARTConnectionString"].ConnectionString;
            string str_conn_GetOtherDBdata = WebConfigurationManager.ConnectionStrings["ECO_" + _account + "ConnectionString"].ConnectionString;
            string str_cmd_GetOtherDBdata = "";
            string str_cmd_MeterIdAccount = "";
            string _merterId = "";
            string _ctrlnr = "";
            StringBuilder json = new StringBuilder();

            str_cmd_MeterIdAccount = "SELECT M.InstallPosition,(M.ECO_Account + '-' + CONVERT(nvarchar(50), M.MeterID)) AS ECO_AccountAndMeterId ,M.MeterID,M.CtrlNr FROM AdminSetup AS A";
            str_cmd_MeterIdAccount += " INNER JOIN ControllerSetup AS C ON A.Account  = C.Account";
            str_cmd_MeterIdAccount += " INNER JOIN MeterSetup AS M ON M.ECO_Account = C.ECO_Account";
            str_cmd_MeterIdAccount += " WHERE LineNum = @nodeId AND A.Account = @account ";
            str_cmd_MeterIdAccount += " ORDER BY LineNum";

            Response.Clear();
            Response.ContentType = "application/json; charset=utf-8";
            using (SqlConnection conn = new SqlConnection(str_conn))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(str_cmd_MeterIdAccount, conn))
                {
                    cmd.Parameters.AddWithValue("@account", _account);
                    cmd.Parameters.AddWithValue("@nodeId", _node);
                    SqlDataReader dr = cmd.ExecuteReader();
                    json.Append("[{\"ECOSMART\":");
                    if (dr.HasRows)
                    {
                        dr.Read();
                        _merterId = dr["MeterID"].ToString();
                        _ctrlnr = dr["CtrlNr"].ToString();
                        json.Append("[{\"ECO_AccountAndMeterId\":\"" + dr["ECO_AccountAndMeterId"].ToString() + "\",\"InstallPosition\":\"" + dr["InstallPosition"].ToString() + "\"}]}");
                    }
                    dr.Close();
                }
            }

            str_cmd_GetOtherDBdata = "SELECT TOP 1 [W], [KWh], [Vavg], [Iavg] FROM [PowerRecord] WHERE (([CtrlNr] =@ctrlnr) AND ([MeterID] =@merterId))";
            str_cmd_GetOtherDBdata += " ORDER BY RecDate desc, RecTime desc";
            using (SqlConnection conn = new SqlConnection(str_conn_GetOtherDBdata))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(str_cmd_GetOtherDBdata, conn))
                {
                    cmd.Parameters.AddWithValue("@merterId", _merterId);
                    cmd.Parameters.AddWithValue("@ctrlnr", _ctrlnr);
                    SqlDataReader dr = cmd.ExecuteReader();
                    json.Append(",{\"OtherDB\":");
                    if (dr.HasRows)
                    {
                        dr.Read();
                        json.Append("[{\"W\":\"" + dr["W"].ToString() + "\",\"KWh\":\"" + dr["KWh"].ToString() + "\",\"Vavg\":\"" + dr["Vavg"].ToString() + "\",\"Iavg\":\"" + dr["Iavg"].ToString() + "\"}]}]");
                    }
                    dr.Close();
                }
            }
            Response.Write(json);
            Response.End();
        }        
    }
}