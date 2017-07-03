using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace WCFService_Test
{
    public partial class PowerDayReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ControllerSet();
            }
            
        }




        private void ControllerSet()
        {
            string str_cmd = "SELECT [ECO_Group], [Account] FROM [AdminSetup]";
            DataTable dr_AdminSetup = DataBaseLink_ECOSMARTConnectionString(str_cmd);
            Group_DropDownList.DataSource = dr_AdminSetup;           
            Group_DropDownList.DataTextField = dr_AdminSetup.Columns[0].ToString();
            Group_DropDownList.DataValueField = dr_AdminSetup.Columns[1].ToString();
            Group_DropDownList.DataBind();
        }

        private DataTable DataBaseLink_ECOSMARTConnectionString(string _str_cmd)
        {
            string str_conn = WebConfigurationManager.ConnectionStrings["ECOSMARTConnectionString"].ConnectionString;
            string str_cmd = _str_cmd;

            using (SqlConnection conn = new SqlConnection(str_conn))
            {
                using (SqlCommand cmd = new SqlCommand(str_cmd, conn))
                {
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adp.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        protected void submit_btn_Click(object sender, EventArgs e)
        {
            string[] para = new string[4];

            para[0] = (CtrlNr_CB.Checked) ? "ECO_Group" : "";
            para[1] = (MeterId_CB.Checked) ? "MeterID" : "";
            para[2] = (Position_CB.Checked) ? "InstallPosition" : "";
            para[3] = (LineNum_CB.Checked) ? "LineNum_CB" : "";

            //string str_cmd = "SELECT [ECO_Group],[MeterID],C.InstallPosition,[LineNum] FROM AdminSetup as A INNER JOIN ControllerSetup as C ON A.Account = C.Account INNER JOIN MeterSetup as M ON M.ECO_Account = C.ECO_Account WHERE LineNum != '' AND A.Account = 'eco' ORDER BY LineNum";

            string str_cmd = "SELECT [ECO_Group],[MeterID],C.InstallPosition,[LineNum] FROM AdminSetup as A";
            str_cmd += " INNER JOIN ControllerSetup as C ON A.Account = C.Account";
            str_cmd += " INNER JOIN MeterSetup as M ON M.ECO_Account = C.ECO_Account";
            str_cmd += " WHERE LineNum != '' AND A.Account = 'eco'";
            str_cmd += " ORDER BY LineNum";
            DataTable dr_Join3Table = DataBaseLink_ECOSMARTConnectionString(str_cmd);

            Response.Write(dr_Join3Table.Rows[0]["InstallPosition"]);
        }

        protected void Meter_TreeView_SelectedNodeChanged(object sender, EventArgs e)
        {
            Response.Write("123");
        }
    }
}