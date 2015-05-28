using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using System.Xml;

using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

using System.Web.Mvc;
using System.Web.Services.Protocols;
using System.Web.Services.Description;
using System.Diagnostics;
namespace mvctest
{
    /// <summary>
    /// hello 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://192.168.1.113")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]

    //[SoapDocumentService(RoutingStyle = SoapServiceRoutingStyle.RequestElement)]

    public class hello : System.Web.Services.WebService
    {
        //[SoapRpcMethod(Use = SoapBindingUse.Literal, Action = "http://192.168.1.113/HelloWorld", RequestNamespace = "http://192.168.1.113", ResponseNamespace = "http://192.168.1.113")]
        [WebMethod]
        public List<string> HelloWorld(string tname,string rname)
        {
            List<string> links = new List<string>();
            links.Add("Hello World" + tname + rname);
            return links;
        }
        //[SoapRpcMethod(Use = SoapBindingUse.Literal, Action = "http://192.168.1.113/another", RequestNamespace = "http://192.168.1.113", ResponseNamespace = "http://192.168.1.113")]
        [WebMethod]
        public List<string> another()
        {
            List<string> links = new List<string>();
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            //方法一：拼接sql字符串，易带来sql注入漏洞,比如，用户名或密码处输入 hi' or 1=1 -- 
            cmd.CommandText = "select name from users";
            //方法二：参数化查询，ADO.NET会对一些特殊字符进行转义，这样可以在一定程序上防止SQL注入，提高程序执行效率
            //cmd.CommandText = "select * from admin where U_name=@username and U_pwd=@password";
            //cmd.Parameters.Add(new SqlParameter("username",txtUsername.Text.Trim()));
            //cmd.Parameters.Add(new SqlParameter("password",txtPwd.Text.Trim()));
            //方法三：调用存储过程
            //cmd.CommandText = "sp_Login"; //存储过程的名称
            //cmd.CommandType = CommandType.StoredProcedure;  //设置类型为存储过程
            //cmd.Parameters.Add("@Username",SqlDbType.VarChar,20).Value=txtUsername.Text.Trim();
            //cmd.Parameters.Add("@Password",SqlDbType.VarChar,20).Value=txtPwd.Text.Trim();
            string res = "";
            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    res = (string)sdr["name"];
                    links.Add(res);
                }

            }


            catch (System.Exception ee)
            {
                //Response.Write(ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            //return res;
            return links;
        }
    }
}
