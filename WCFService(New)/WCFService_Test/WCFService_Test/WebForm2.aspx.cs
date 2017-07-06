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
using System.Web.Services;

namespace WCFService_Test
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)  // 第一次執行
            {   // 自己寫的副程式，從第一頁開始。
                Repeater1.DataSource = MIS2000Lab_GetPageData(1);
                Repeater1.DataBind();
            }
        }


        // ***** 分頁。使用SQL指令進行分頁 *****
        public static DataSet MIS2000Lab_GetPageData(int currentPage)
        {
            string str_conn = WebConfigurationManager.ConnectionStrings["ECOSMARTConnectionString"].ConnectionString;
            SqlConnection Conn = new SqlConnection(str_conn);

            String SqlStr = "Select * from AdminCom ORDER BY [EnabledTime] ASC ";
            SqlStr += " OFFSET " + ((currentPage - 1) * 10) + " ROWS FETCH NEXT " + 10 + " ROWS ONLY";
            //==SQL 2012 指令的 Offset...Fetch。參考資料： http://sharedderrick.blogspot.tw/2012/06/t-sql-offset-fetch.html  

            SqlDataAdapter myAdapter = new SqlDataAdapter(SqlStr, Conn);
            DataSet ds = new DataSet();
            myAdapter.Fill(ds, "AdminCom");

            //-- 用來計算分頁的「總頁數」 ---      
            SqlCommand cmd = new SqlCommand("select Count(EnabledTime) from AdminCom", Conn);
            Conn.Open();
            int myTotalCount = (int)cmd.ExecuteScalar();

            DataTable dt = new DataTable("PageCount");
            dt.Columns.Add("PageCount");
            dt.Rows.Add();
            dt.Rows[0][0] = myTotalCount;
            ds.Tables.Add(dt);

            

            if (Conn.State == ConnectionState.Open)
            {
                Conn.Close();
                Conn.Dispose();
            }

            return ds;
        }


        [WebMethod]
        public static string GetCustomers(int pageIndex)
        {
            return MIS2000Lab_GetPageData(pageIndex).GetXml();
        }
    }
}