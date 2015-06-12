using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mvctest.Models;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Text;
namespace mvctest.dbop
{
    public class sqlop
    {
        public static void writeLog(string name,string logInfo)
        {
            using (StreamWriter sw = new StreamWriter(@"C:\WebSite\WebSite_Log.txt", true))
            {
                sw.WriteLine(DateTime.Now.ToString() + " " + name + " " + logInfo);
            }
        }
        #region  吐槽
        public static string search_all_tucao_info(string loginid,int ID)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = " with A as(select * from TucaoZan where LoginID = '" + loginid + "')select  top 10* from Tucao left outer join Member on Tucao.LoginID = Member.LoginID left outer join A on A.TucaoID = Tucao.TucaoID where tucao.TucaoID < " + ID.ToString() + "  order by Tucao.TucaoID Desc";
            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();
                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["Tucaocnt"] != DBNull.Value) emp.Add("Tucaocnt", (int)sdr["Tucaocnt"]);
                    if (sdr["TucaoID"] != DBNull.Value) emp.Add("tucao_id", (int)sdr["TucaoID"]);
                    if (sdr["nickname"] != DBNull.Value) emp.Add("nickname", (string)sdr["nickname"]);
                    if (sdr["introduce"] != DBNull.Value) emp.Add("introduce", (string)sdr["introduce"]);
                    if (sdr["TucaoTime"] != DBNull.Value) emp.Add("tucao_time", ((DateTime)sdr["TucaoTime"]).ToString());
                    if (sdr["TucaoContent"] != DBNull.Value) emp.Add("content", (string)sdr["TucaoContent"]);
                    if (sdr["CountZan"] != DBNull.Value) emp.Add("zan_cnt", (int)sdr["CountZan"]);
                    if (sdr["State"] != DBNull.Value) emp.Add("zanstate", (int)sdr["State"]);
                    else emp.Add("zanstate", 0);
                    obj.Add(emp);
                }
                
                emp1.Add("tucao_info", obj);
              
                return emp1.ToString().ToString();
            }


            catch (System.Exception ee)
            {
                writeLog("search_all_tucao_info", ee.Message.ToString());
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
               // conn1.Close();
            }
            //return "error";
        }
        public static string get_tucao_bycnt(int tucaoid,int cnt)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select top 10 * from tucao left join member On tucao.LoginID=member.LoginID where CountZan < '" + cnt + "' and TucaoID > '" + tucaoid + "' order by CountZan desc,tucaoid";
            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();
                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["Tucaocnt"] != DBNull.Value) emp.Add("Tucaocnt", (int)sdr["Tucaocnt"]);
                    if (sdr["TucaoID"] != DBNull.Value) emp.Add("tucao_id", (int)sdr["TucaoID"]);
                    if (sdr["nickname"] != DBNull.Value) emp.Add("nickname", (string)sdr["nickname"]);
                    if (sdr["introduce"] != DBNull.Value) emp.Add("introduce", (string)sdr["introduce"]);
                    if (sdr["TucaoTime"] != DBNull.Value) emp.Add("tucao_time", ((DateTime)sdr["TucaoTime"]).ToString());
                    if (sdr["TucaoContent"] != DBNull.Value) emp.Add("content", (string)sdr["TucaoContent"]);
                    if (sdr["CountZan"] != DBNull.Value) emp.Add("zan_cnt", (int)sdr["CountZan"]);
                    obj.Add(emp);
                }

                emp1.Add("tucao_info", obj);

                return emp1.ToString().ToString();
            }


            catch (System.Exception ee)
            {
                writeLog("get_tucao_bycnt", ee.Message.ToString());
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
                // conn1.Close();
            }
            //return "error";
        }
        public static string get_tucao_all_comment(int tucaoid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from Comment left join member On comment.LoginID=member.LoginID where Comment.TucaoID = " + tucaoid.ToString(); 

            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();
                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["nickname"] != DBNull.Value) emp.Add("nickname", (string)sdr["nickname"]);
                    if (sdr["introduce"] != DBNull.Value) emp.Add("introduce", (string)sdr["introduce"]);
                    if (sdr["CommentTime"] != DBNull.Value) emp.Add("comment_time", ((DateTime)sdr["CommentTime"]).ToString());
                    if (sdr["ComentContent"] != DBNull.Value) emp.Add("content", (string)sdr["ComentContent"]);
                    obj.Add(emp);
                }
                emp1.Add("comment_info", obj);
                
                return emp1.ToString().ToString();
            }

            catch (System.Exception ee)
            {
                writeLog("get_tucao_all_comment", ee.Message.ToString());
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        public static bool insert_tucao(string loginid, string content)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "insert into tucao(loginid,tucaocontent,tucaotime,countzan,Tucaocnt) values ('" + loginid + "','" + content + "','" + DateTime.Now.ToString() + "',0,0)";

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }

            
            catch (System.Exception ee)
            {
                writeLog("insert_tucao", ee.Message.ToString());
                //return (ee.Message.ToString());
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public static bool delete_tucao(string loginid, int tucaoid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "delete from tucao where tucaoid='" + tucaoid + "' and  loginid='" + loginid + "'";

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }


            catch (System.Exception ee)
            {
                writeLog("delete_tucao", ee.Message.ToString());
                //return (ee.Message.ToString());
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public static bool insert_comment(string loginid, int tucaoid, string content)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "insert into comment(loginid,tucaoid,comentcontent,commenttime) values ('" + loginid + "'," + tucaoid + ",'" + content + "','" + DateTime.Now.ToString() + "')";
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }


            catch (System.Exception ee)
            {
                writeLog("insert_comment", ee.Message.ToString());
                //return (ee.Message.ToString());
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public static bool add_zan(string loginid,int tucaoid)
        {       
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from tucaozan where TucaoID = " + tucaoid + " and loginid = '" + loginid + "'";
            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();            
                if (sdr.Read()==false)
                {
                    conn.Close();
                    conn.Open();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "insert into tucaozan(loginid,tucaoid,state) values ('" + loginid + "'," + tucaoid + ",1)";           
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    conn.Close();
                    conn.Open();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "update tucaozan set state = 1 where loginid='" + loginid + "' and TucaoID=" + tucaoid + "";
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (System.Exception ee)
            {
                writeLog("add_zan", ee.Message.ToString());
                return false;
            }
            finally
            {
                conn.Close();
            }
             
        }
        public static bool delete_zan(string loginid, int tucaoid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "update tucaozan set state = 0 where loginid='" + loginid + "' and TucaoID=" + tucaoid + "";

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }


            catch (System.Exception ee)
            {
                writeLog("delete_zan", ee.Message.ToString());
                //return (ee.Message.ToString());
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion
        #region 兼职
        public static string get_jianzhi_all_info(int jzhiid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select top 10 * from jianzhi where jianzhiid < '" + ""+jzhiid+ "' order by jianzhiid desc";

            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();


                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["JianzhiName"] != DBNull.Value) emp.Add("JianzhiName", (string)sdr["JianzhiName"]);
                    if (sdr["JinazhiTime"] != DBNull.Value) emp.Add("JianzhiTime", (string)sdr["JinazhiTime"]);
                    if (sdr["JianzhiAddress"] != DBNull.Value) emp.Add("JianzhiAddress", (string)sdr["JianzhiAddress"]);
                    if (sdr["jianzhiid"] != DBNull.Value) emp.Add("jianzhiid", (int)sdr["jianzhiid"]);
                    if (sdr["Salary"] != DBNull.Value) emp.Add("Salary", (string)sdr["Salary"]);
                    if (sdr["PublishDepart"] != DBNull.Value) emp.Add("PublishDepart", (string)sdr["PublishDepart"]);
                    if (sdr["PublishTime"] != DBNull.Value) emp.Add("PublishTime", ((DateTime)sdr["PublishTime"]).ToString());
                    obj.Add(emp);
                     
                }
                emp1.Add("jianzhi_all_info", obj);
                
                return emp1.ToString();
            }


            catch (System.Exception ee)
            {
                writeLog("get_jianzhi_all_info", ee.Message.ToString());
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            //return "error";
        }
        public static string get_jianzhi_class_info(int classx,int jzid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select top 10 * from jianzhi where Class=" + classx.ToString() + "  and jianzhiid < " + jzid.ToString() + "order by jianzhiid desc";

            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();


                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["JianzhiName"] != DBNull.Value) emp.Add("JianzhiName", (string)sdr["JianzhiName"]);                  
                    if (sdr["JinazhiTime"] != DBNull.Value) emp.Add("JianzhiTime", (string)sdr["JinazhiTime"]);
                    if (sdr["JianzhiAddress"] != DBNull.Value) emp.Add("JianzhiAddress", (string)sdr["JianzhiAddress"]);
                    if (sdr["jianzhiid"] != DBNull.Value) emp.Add("jianzhiid", (int)sdr["jianzhiid"]);
                    if (sdr["Salary"] != DBNull.Value) emp.Add("Salary", (string)sdr["Salary"]);
                    if (sdr["PublishDepart"] != DBNull.Value) emp.Add("PublishDepart", (string)sdr["PublishDepart"]);
                    if (sdr["PublishTime"] != DBNull.Value) emp.Add("PublishTime", ((DateTime)sdr["PublishTime"]).ToString());
                    obj.Add(emp);
                    
                }
               
                emp1.Add("jianzhi_class_info", obj);
                emp1.Add("jianzhi_cla", classx);
               
                return emp1.ToString();
            }


            catch (System.Exception ee)
            {
                writeLog("get_jianzhi_class_info", ee.Message.ToString());
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            //return "error";
        }
        public static string get_jianzhix_info(int jianzhiid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from jianzhi where jianzhiid = " + jianzhiid.ToString();

            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();


                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["JianzhiContent"] != DBNull.Value) emp.Add("JianzhiContent", (string)sdr["JianzhiContent"]);
                    if (sdr["Requirement"] != DBNull.Value) emp.Add("Requirement", (string)sdr["Requirement"]);
                    if (sdr["Phone"] != DBNull.Value) emp.Add("Phone", (string)sdr["Phone"]);
                    if (sdr["CountNeed"] != DBNull.Value) emp.Add("CountNeed", (int)sdr["CountNeed"]);
                    obj.Add(emp);
                   
                }
                emp1.Add("jianzhix_info", obj);
                
                return emp1.ToString();
            }


            catch (System.Exception ee)
            {
                writeLog("get_jianzhix_info", ee.Message.ToString());
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            //return "error";
        }
        //public static bool insert_jianzhi(string loginid, string JianzhiName, string JianzhiContent, string JianzhiTime, string JianzhiAddressstring, string Requirement, string Phone, int CountNeed, string Salary)
        //{
        //    string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        //    SqlConnection conn = new SqlConnection(strConn);
        //    SqlCommand cmd = conn.CreateCommand();
        //    cmd.CommandText = "insert into jianzhi(loginid,JianzhiName,JianzhiContent,JianzhiTime,JianzhiAddress,Requirement,Phone,CountNeed,jianzhiid,Salary) values ('" + loginid + "','" + JianzhiName + "','" + JianzhiContent + "','" + JianzhiTime + "','" + JianzhiAddressstring + "','" + Requirement + "','" + Phone + "','" + "," + CountNeed.ToString() + "','" + Salary + "')";

        //    try
        //    {
        //        conn.Open();
        //        cmd.ExecuteNonQuery();
        //        return true;
        //    }


        //    catch (System.Exception ee)
        //    {
        //        //return (ee.Message.ToString());
        //        return false;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }
        //}
        #endregion
        #region 二手
   
        public static string get_ershou_all_info(int ershouid,int classx)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select top 10 * from ershou where ershouid < " + ershouid.ToString() + " and class = " + classx.ToString() + " order by ershouid desc";

            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();
                
                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["ErshouID"] != DBNull.Value) emp.Add("ErshouID", (int)sdr["ErshouID"]);
                    if (sdr["LoginID"] != DBNull.Value) emp.Add("LoginID", (string)sdr["LoginID"]);
                    if (sdr["Description"] != DBNull.Value) emp.Add("Description", (string)sdr["Description"]);
                    if (sdr["OriginPrice"] != DBNull.Value) emp.Add("OriginPrice", (int)sdr["OriginPrice"]);
                    if (sdr["Price"] != DBNull.Value) emp.Add("Price", (int)sdr["Price"]);
                    if (sdr["PublishTime"] != DBNull.Value) emp.Add("PublishTime", ((DateTime)sdr["PublishTime"]).ToString());
                    if (sdr["Photo"] != DBNull.Value) emp.Add("Photo", (string)sdr["Photo"]);
                    if (sdr["Phone"] != DBNull.Value) emp.Add("Phone", (string)sdr["Phone"]);
                    if (sdr["ErshouName"] != DBNull.Value) emp.Add("ErshouName", (string)sdr["ErshouName"]);
                    obj.Add(emp);
                   
                }
                emp1.Add("ershou_all_info", obj);
                
                return emp1.ToString();
            }


            catch (System.Exception ee)
            {
                writeLog("get_ershou_all_info", ee.Message.ToString());
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        public static string get_ershou_info(int ershouid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select top 10 * from ershou where ershouid < " + ershouid.ToString()+" order by ershouid desc";

            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();

                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["ErshouID"] != DBNull.Value) emp.Add("ErshouID", (int)sdr["ErshouID"]);
                    if (sdr["LoginID"] != DBNull.Value) emp.Add("LoginID", (string)sdr["LoginID"]);
                    if (sdr["Description"] != DBNull.Value) emp.Add("Description", (string)sdr["Description"]);
                    if (sdr["OriginPrice"] != DBNull.Value) emp.Add("OriginPrice", (int)sdr["OriginPrice"]);
                    if (sdr["Price"] != DBNull.Value) emp.Add("Price", (int)sdr["Price"]);
                    if (sdr["PublishTime"] != DBNull.Value) emp.Add("PublishTime", ((DateTime)sdr["PublishTime"]).ToString());
                    if (sdr["Photo"] != DBNull.Value) emp.Add("Photo", (string)sdr["Photo"]);
                    if (sdr["Phone"] != DBNull.Value) emp.Add("Phone", (string)sdr["Phone"]);
                    if (sdr["ErshouName"] != DBNull.Value) emp.Add("ErshouName", (string)sdr["ErshouName"]);
                    obj.Add(emp);

                }
                emp1.Add("ershou_info", obj);

                return emp1.ToString();
            }


            catch (System.Exception ee)
            {
                writeLog("get_ershou_info", ee.Message.ToString());
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        public static string get_ershou_name_info(string name)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from ershou where ershouname like '%" + name + "%'";

            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();

                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["ErshouID"] != DBNull.Value) emp.Add("ErshouID", (int)sdr["ErshouID"]);
                    if (sdr["LoginID"] != DBNull.Value) emp.Add("LoginID", (string)sdr["LoginID"]);
                    if (sdr["Description"] != DBNull.Value) emp.Add("Description", ((string)sdr["Description"]));
                    if (sdr["OriginPrice"] != DBNull.Value) emp.Add("OriginPrice", (int)sdr["OriginPrice"]);
                    if (sdr["Price"] != DBNull.Value) emp.Add("Price", (int)sdr["Price"]);
                    if (sdr["PublishTime"] != DBNull.Value) emp.Add("PublishTime", ((DateTime)sdr["PublishTime"]).ToString());
                    if (sdr["Photo"] != DBNull.Value) emp.Add("Photo", (string)sdr["Photo"]);
                    if (sdr["Phone"] != DBNull.Value) emp.Add("Phone", (string)sdr["Phone"]);
                    if (sdr["ErshouName"] != DBNull.Value) emp.Add("ErshouName", (string)sdr["ErshouName"]);
                    obj.Add(emp);

                }
                emp1.Add("ershou_all_info", obj);

                return emp1.ToString();
            }


            catch (System.Exception ee)
            {
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        public static bool insert_ershou_info(string loginid, int classx, string Description, int OriginPrice, int Price, string DealTime, string Phone, string picname,string ershouname)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            
            cmd.CommandText = "insert into ershou(loginid,class,  Description, OriginPrice,Price, DealTime, Phone,photo,ershouname,publishtime) values ('" + loginid + "'," + classx.ToString()  + ",'" + Description + "'," + OriginPrice.ToString() + "," + Price.ToString() + ",'" + DealTime + "','" + Phone + "','" + picname + "','" + ershouname + "','" + DateTime.Now.ToString() + "')";
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }


            catch (System.Exception ee)
            {
                //return (ee.Message.ToString());
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public static bool delete_ershou_info(string loginid,int ershouid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "delete from Ershou where ErshouID='" + ershouid + "' and LoginID ='" + loginid + "'";

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }


            catch (System.Exception ee)
            {
                //return (ee.Message.ToString());
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion
        #region 活动
        public static string get_activity_info(string loginid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "with A AS (select * from Member_Activity where loginID=" + loginid + ")select * from Member_Organazation inner join activity on Member_Organazation.OrganazationID = Activity.OrganazationID inner join Organazation on Member_Organazation.OrganazationID = Organazation.OrganizationID left outer join A on A.ActivityID = Activity.ActivityID  left outer join school on Organazation.schoolid = school.schoolid where Member_Organazation.LoginID = '" + loginid + "' and activity.activitytime > GETDATE() and member_organazation.gzstate=1";
           
            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();

                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["ActivityID"] != DBNull.Value) emp.Add("ActivityID", (int)sdr["ActivityID"]);
                    if (sdr["OrganazationID"] != DBNull.Value) emp.Add("OrganazationID", (int)sdr["OrganazationID"]);
                    if (sdr["ActivityName"] != DBNull.Value) emp.Add("ActivityName", (string)sdr["ActivityName"]);
                    if (sdr["GroupID"] != DBNull.Value) emp.Add("GroupID", (string)sdr["GroupID"]);
                    if (sdr["OrganizationName"] != DBNull.Value) emp.Add("OrganizationName", (string)sdr["OrganizationName"]);
                    if (sdr["Address"] != DBNull.Value) emp.Add("Address", (string)sdr["Address"]);
                    if (sdr["PhotoDir"] != DBNull.Value) emp.Add("PhotoDir", (string)sdr["PhotoDir"]);
                    if (sdr["OriginalDir"] != DBNull.Value) emp.Add("OriginalDir", (string)sdr["OriginalDir"]);
                    if (sdr["girllim"] != DBNull.Value) emp.Add("girllim", (int)sdr["girllim"]);
                    if (sdr["boylim"] != DBNull.Value) emp.Add("boylim", (int)sdr["boylim"]);
                    if (sdr["boynum"] != DBNull.Value) emp.Add("boynum", (int)sdr["boynum"]);
                    if (sdr["girlnum"] != DBNull.Value) emp.Add("girlnum", (int)sdr["girlnum"]);
                    if (sdr["ActivityTime"] != DBNull.Value) emp.Add("ActivityTime", ((DateTime)sdr["ActivityTime"]).ToString());
                    if (sdr["cjstate"] != DBNull.Value) emp.Add("cjstate", (int)sdr["cjstate"]);
                    else emp.Add("cjstate", 0);
                    if (sdr["SchoolName"] != DBNull.Value) emp.Add("SchoolName", (string)sdr["SchoolName"]);
                    obj.Add(emp);
                    
                }
                
                emp1.Add("activity_all_info", obj);
                
                return emp1.ToString();
            }


            catch (System.Exception ee)
            {
                writeLog("get_activity_info", ee.Message.ToString());
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        public static string get_activity_allmember(string actid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from Member_Activity,Member where Member_Activity.LoginID = Member.LoginID and Member_Activity.cjstate = 1 and Member_Activity.ActivityID = " + actid;

            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();

                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["LoginID"] != DBNull.Value) emp.Add("LoginID", (string)sdr["LoginID"]);
                    if (sdr["nickname"] != DBNull.Value) emp.Add("nickname", (string)sdr["nickname"]);
                    if (sdr["introduce"] != DBNull.Value) emp.Add("introduce", (string)sdr["introduce"]);
                    if (sdr["Sex"] != DBNull.Value) emp.Add("sex", (int)sdr["Sex"]);
                    if (sdr["photo"] != DBNull.Value) emp.Add("photo", (string)sdr["photo"]);
                    else emp.Add("photo", "tidai.jpg");
                    obj.Add(emp);

                }

                emp1.Add("activity_allmember", obj);

                return emp1.ToString();
            }


            catch (System.Exception ee)
            {
                writeLog("get_activity_allmember", ee.Message.ToString());
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        public static string get_actx_comment(int actid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from Activity_Comment left join member on activity_comment.loginid=member.loginid where ActivityID = " + actid.ToString();

            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();


                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["nickname"] != DBNull.Value) emp.Add("nickname", (string)sdr["nickname"]);
                    if (sdr["introduce"] != DBNull.Value) emp.Add("introduce", (string)sdr["introduce"]);
                    if (sdr["CommentContent"] != DBNull.Value) emp.Add("CommentContent", (string)sdr["CommentContent"]);
                    if (sdr["CommentTime"] != DBNull.Value) emp.Add("CommentTime", ((DateTime)sdr["CommentTime"]).ToString());
                    obj.Add(emp);

                }

                emp1.Add("actx_comment", obj);

                return emp1.ToString();
            }


            catch (System.Exception ee)
            {
                writeLog("get_actx_comment", ee.Message.ToString());
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            //return "error";
        }
        
        public static string get_member_info(string loginid,string mid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from Member where LoginID = " + mid;

            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();


                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["nickname"] != DBNull.Value) emp.Add("nickname", (string)sdr["nickname"]);
                    //if (sdr["introduce"] != DBNull.Value) emp.Add("introduce", (string)sdr["introduce"]);
                    if (sdr["Sex"] != DBNull.Value) emp.Add("sex", (int)sdr["sex"]);
                    if (sdr["introduce"] != DBNull.Value) emp.Add("introduce", (string)sdr["introduce"]);
                    else emp.Add("introduce", "赶快去添加个性签名吧");
                    
                    if (sdr["photo"] != DBNull.Value) emp.Add("photo", (string)sdr["photo"]);
                    else emp.Add("photo", "tidai.jpg");
                    obj.Add(emp);

                }
                emp1.Add("mem_info", obj);
                conn.Close();
                cmd.CommandText = "select * from Member_Activity ,activity,organazation, school where member_activity.ActivityID = Activity.ActivityID and activity.OrganazationID = organazation.OrganizationID and organazation.SchoolID = school.SchoolID and Member_Activity.cjstate = 1 and Member_Activity.LoginID=" + mid;
                conn.Open();
                sdr = cmd.ExecuteReader();
                JArray objx = new JArray();
                while(sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["ActivityID"] != DBNull.Value) emp.Add("ActivityID", (int)sdr["ActivityID"]);
                    if (sdr["ActivityName"] != DBNull.Value) emp.Add("ActivityName", (string)sdr["ActivityName"]);
                    if (sdr["GroupID"] != DBNull.Value) emp.Add("GroupID", (string)sdr["GroupID"]);
                    if (sdr["OrganazationID"] != DBNull.Value) emp.Add("OrganazationID", (int)sdr["OrganazationID"]);
                    if (sdr["OrganizationName"] != DBNull.Value) emp.Add("OrganizationName", (string)sdr["OrganizationName"]);
                    if (sdr["ActivirtyContent"] != DBNull.Value) emp.Add("ActivirtyContent", (string)sdr["ActivirtyContent"]);
                    if (sdr["PhotoDir"] != DBNull.Value) emp.Add("PhotoDir", (string)sdr["PhotoDir"]);
                    if (sdr["OriginalDir"] != DBNull.Value) emp.Add("OriginalDir", (string)sdr["OriginalDir"]);
                    if (sdr["logo"] != DBNull.Value) emp.Add("logo", (string)sdr["logo"]);
                    if (sdr["originaldirx"] != DBNull.Value) emp.Add("originaldirx", (string)sdr["originaldirx"]);
                    if (sdr["Taga"] != DBNull.Value) emp.Add("Taga", (string)sdr["Taga"]);
                    if (sdr["Tagb"] != DBNull.Value) emp.Add("Tagb", (string)sdr["Tagb"]);
                    if (sdr["Tagc"] != DBNull.Value) emp.Add("Tagc", (string)sdr["Tagc"]);
                    if (sdr["ActivityTime"] != DBNull.Value) emp.Add("ActivityTime", ((DateTime)sdr["ActivityTime"]).ToString());
                    if (sdr["Address"] != DBNull.Value) emp.Add("Address", (string)sdr["Address"]);
                    if (sdr["Rate"] != DBNull.Value) emp.Add("Rate", (double)sdr["Rate"]);
                    if (sdr["SchoolID"] != DBNull.Value) emp.Add("SchoolID", (int)sdr["SchoolID"]);
                    if (sdr["SchoolName"] != DBNull.Value) emp.Add("SchoolName", (string)sdr["SchoolName"]);
                    
                    if (sdr["girllim"] != DBNull.Value) emp.Add("girllim", (int)sdr["girllim"]);
                    if (sdr["boylim"] != DBNull.Value) emp.Add("boylim", (int)sdr["boylim"]);
                    if (sdr["boynum"] != DBNull.Value) emp.Add("boynum", (int)sdr["boynum"]);
                    if (sdr["girlnum"] != DBNull.Value) emp.Add("girlnum", (int)sdr["girlnum"]); 
                    objx.Add(emp);
                }
                emp1.Add("act_info", objx);
                conn.Close();
                cmd.CommandText = "select * from guanzhu where user1='" + loginid + "' and user2 = '" + mid + "'";
                conn.Open();
                sdr = cmd.ExecuteReader();
                JArray objxm = new JArray();
                JObject empx = new JObject();
                if (sdr.Read())
                {

                    empx.Add("gzstate", (int)sdr["state"]);

                }
                else empx.Add("gzstate", 2);
                objxm.Add(empx);
                emp1.Add("gzinfo", objxm);
                return emp1.ToString();
            }


            catch (System.Exception ee)
            {
                writeLog("get_member_info", ee.Message.ToString());
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            //return "error";
        }        
        
        public static string get_connect_info(string mid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from Member ,guanzhu where (Member.LoginID = guanzhu.user2) and guanzhu.state = 1 and guanzhu.user1 = " + mid;

            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();


                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["nickname"] != DBNull.Value) emp.Add("nickname", (string)sdr["nickname"]);
                    if (sdr["introduce"] != DBNull.Value) emp.Add("introduce", (string)sdr["introduce"]);
                    if (sdr["photo"] != DBNull.Value) emp.Add("photo", (string)sdr["photo"]);
                    else emp.Add("photo", "tidai.jpg");
                    emp.Add("loginid",  (string)sdr["user2"]);
                    obj.Add(emp);

                }
                sdr.Close();
                conn.Close();
                cmd.CommandText = "select * from Member ,guanzhu where (Member.LoginID = guanzhu.user1) and guanzhu.state = 1 and guanzhu.user2 = " + mid;
                conn.Open();
                SqlDataReader sdrx = cmd.ExecuteReader();

                while (sdrx.Read())
                {
                    JObject emp = new JObject();
                    if (sdrx["nickname"] != DBNull.Value) emp.Add("nickname", (string)sdrx["nickname"]);
                    if (sdrx["introduce"] != DBNull.Value) emp.Add("introduce", (string)sdrx["introduce"]);
                    if (sdrx["photo"] != DBNull.Value) emp.Add("photo", (string)sdrx["photo"]);
                    else emp.Add("photo", "tidai.jpg");
                    emp.Add("loginid", (string)sdrx["user1"]);
                    obj.Add(emp);

                }
             
                emp1.Add("connect_info", obj);

                return emp1.ToString();
            }


            catch (System.Exception ee)
            {
                writeLog("get_connect_info", ee.Message.ToString());
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            //return "error";
        }        
        public static string get_activity_bycnt(int cnt, int actid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select top 10 * from Activity  inner join organazation   on Activity.OrganazationID=organazation.OrganizationID where Activity.CountMember< " + cnt + " or (Activity.CountMember =  " + cnt + " and Activity.activityid > " + actid + ")order by Activity.CountMember DESC,Activity.ActivityID";
            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();


                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["ActivityID"] != DBNull.Value) emp.Add("ActivityID", (int)sdr["ActivityID"]);
                    if (sdr["OrganazationID"] != DBNull.Value) emp.Add("OrganazationID", (int)sdr["OrganazationID"]);
                    if (sdr["ActivityName"] != DBNull.Value) emp.Add("ActivityName", (string)sdr["ActivityName"]);
                    if (sdr["GroupID"] != DBNull.Value) emp.Add("GroupID", (string)sdr["GroupID"]);
                    if (sdr["Taga"] != DBNull.Value) emp.Add("Taga", (string)sdr["Taga"]);
                    if (sdr["Tagb"] != DBNull.Value) emp.Add("Tagb", (string)sdr["Tagb"]);
                    if (sdr["Tagc"] != DBNull.Value) emp.Add("Tagc", (string)sdr["Tagc"]);
                    if (sdr["OrganizationName"] != DBNull.Value) emp.Add("OrganizationName", (string)sdr["OrganizationName"]);
                    if (sdr["Address"] != DBNull.Value) emp.Add("Address", (string)sdr["Address"]);
                    if (sdr["PhotoDir"] != DBNull.Value) emp.Add("PhotoDir", (string)sdr["PhotoDir"]);
                    if (sdr["OriginalDir"] != DBNull.Value) emp.Add("OriginalDir", (string)sdr["OriginalDir"]);
                    if (sdr["CountMember"] != DBNull.Value) emp.Add("CountMember", (int)sdr["CountMember"]);
                    if (sdr["ActivityTime"] != DBNull.Value) emp.Add("ActivityTime", ((DateTime)sdr["ActivityTime"]).ToString());
                    obj.Add(emp);

                }

                emp1.Add("act_all", obj);

                return emp1.ToString();
            }


            catch (System.Exception ee)
            {
                writeLog("get_activity_bycnt", ee.Message.ToString());
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            //return "error";
        }
        public static string get_activityx_info(string loginid,int actid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from activity ,ORGANAZATION,school where ActivityID = " + actid.ToString() + " and Activity.OrganazationID = ORGANAZATION.OrganizationID and ORGANAZATION.SchoolID = School.SchoolID";
            int pv = 0,cv = 0;
            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();
                JObject emp = new JObject();
                
                while (sdr.Read())
                {
                    if (sdr["GroupID"] != DBNull.Value) emp.Add("GroupID", (string)sdr["GroupID"]);
                    else emp.Add("GroupID", "");
                    if (sdr["ActivityName"] != DBNull.Value) emp.Add("ActivityName", (string)sdr["ActivityName"]);
                    if (sdr["OrganazationID"] != DBNull.Value) emp.Add("OrganazationID", (int)sdr["OrganazationID"]);
                    if (sdr["OrganizationName"] != DBNull.Value) emp.Add("OrganizationName", (string)sdr["OrganizationName"]);
                    if (sdr["ActivirtyContent"] != DBNull.Value) emp.Add("ActivirtyContent", (string)sdr["ActivirtyContent"]);
                    if (sdr["PhotoDir"] != DBNull.Value) emp.Add("PhotoDir", (string)sdr["PhotoDir"]);
                    if (sdr["OriginalDir"] != DBNull.Value) emp.Add("OriginalDir", (string)sdr["OriginalDir"]);
                    if (sdr["logo"] != DBNull.Value) emp.Add("logo", (string)sdr["logo"]);
                    if (sdr["originaldirx"] != DBNull.Value) emp.Add("originaldirx", (string)sdr["originaldirx"]);
                    if (sdr["Taga"] != DBNull.Value) emp.Add("Taga", (string)sdr["Taga"]);
                    if (sdr["Tagb"] != DBNull.Value) emp.Add("Tagb", (string)sdr["Tagb"]);
                    if (sdr["Tagc"] != DBNull.Value) emp.Add("Tagc", (string)sdr["Tagc"]);
                    if (sdr["ActivityTime"] != DBNull.Value) emp.Add("ActivityTime", ((DateTime)sdr["ActivityTime"]).ToString());
                    if (sdr["Address"] != DBNull.Value) emp.Add("Address", (string)sdr["Address"]);
                    if (sdr["Rate"] != DBNull.Value) emp.Add("Rate", (double)sdr["Rate"]);
                    if (sdr["SchoolID"] != DBNull.Value) emp.Add("SchoolID", (int)sdr["SchoolID"]);
                    if (sdr["SchoolName"] != DBNull.Value) emp.Add("SchoolName", (string)sdr["SchoolName"]);
                    if (sdr["NeedEnroll"] != DBNull.Value) emp.Add("NeedEnroll", (int)sdr["NeedEnroll"]);
                    if (sdr["girllim"] != DBNull.Value) emp.Add("girllim", (int)sdr["girllim"]);
                    if (sdr["boylim"] != DBNull.Value) emp.Add("boylim", (int)sdr["boylim"]);
                    if (sdr["boynum"] != DBNull.Value) emp.Add("boynum", (int)sdr["boynum"]);
                    if (sdr["Pv"] != DBNull.Value) emp.Add("Pv", (int)sdr["Pv"]);
                    if (sdr["Cv"] != DBNull.Value) emp.Add("Cv", (int)sdr["Cv"]);
                    pv = (int)sdr["Pv"];cv = (int)sdr["Cv"];
                    if (sdr["girlnum"] != DBNull.Value) emp.Add("girlnum", (int)sdr["girlnum"]); 
                }
                conn.Close();
                cmd.CommandText = "select * from member_activity where loginid='"+loginid+"' and ActivityID = " + actid.ToString();
                conn.Open();
                sdr = cmd.ExecuteReader();
                if (sdr.Read())
                {
                    
                    
                    if (sdr["cjState"] != DBNull.Value) emp.Add("cjState", (int)sdr["cjState"]);

                }
                else
                {
                    
                    
                    emp.Add("cjState", 0);
                   
                }
                pv = pv + 1;
                obj.Add(emp);
                emp1.Add("activityx_info", obj);
                conn.Close();
                cmd.CommandText = "update activity set pv = " + pv + " where activityid = " + actid;
                conn.Open();
                cmd.ExecuteNonQuery();
                return emp1.ToString();
            }


            catch (System.Exception ee)
            {
                writeLog("get_activityx_info", ee.Message.ToString());
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        public static string get_activityx_info_nologin(int actid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from activity ,ORGANAZATION,school where ActivityID = " + actid.ToString() + " and Activity.OrganazationID = ORGANAZATION.OrganizationID and ORGANAZATION.SchoolID = School.SchoolID";
            int pv = 0, cv = 0;
            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();
                JObject emp = new JObject();

                while (sdr.Read())
                {
                    if (sdr["GroupID"] != DBNull.Value) emp.Add("GroupID", (string)sdr["GroupID"]);
                    else emp.Add("GroupID", "");
                    if (sdr["ActivityName"] != DBNull.Value) emp.Add("ActivityName", (string)sdr["ActivityName"]);
                    if (sdr["OrganazationID"] != DBNull.Value) emp.Add("OrganazationID", (int)sdr["OrganazationID"]);
                    if (sdr["OrganizationName"] != DBNull.Value) emp.Add("OrganizationName", (string)sdr["OrganizationName"]);
                    if (sdr["ActivirtyContent"] != DBNull.Value) emp.Add("ActivirtyContent", (string)sdr["ActivirtyContent"]);
                    if (sdr["PhotoDir"] != DBNull.Value) emp.Add("PhotoDir", (string)sdr["PhotoDir"]);
                    if (sdr["OriginalDir"] != DBNull.Value) emp.Add("OriginalDir", (string)sdr["OriginalDir"]);
                    if (sdr["logo"] != DBNull.Value) emp.Add("logo", (string)sdr["logo"]);
                    if (sdr["originaldirx"] != DBNull.Value) emp.Add("originaldirx", (string)sdr["originaldirx"]);
                    if (sdr["Taga"] != DBNull.Value) emp.Add("Taga", (string)sdr["Taga"]);
                    if (sdr["Tagb"] != DBNull.Value) emp.Add("Tagb", (string)sdr["Tagb"]);
                    if (sdr["Tagc"] != DBNull.Value) emp.Add("Tagc", (string)sdr["Tagc"]);
                    if (sdr["ActivityTime"] != DBNull.Value) emp.Add("ActivityTime", ((DateTime)sdr["ActivityTime"]).ToString());
                    if (sdr["Address"] != DBNull.Value) emp.Add("Address", (string)sdr["Address"]);
                    if (sdr["Rate"] != DBNull.Value) emp.Add("Rate", (double)sdr["Rate"]);
                    if (sdr["SchoolID"] != DBNull.Value) emp.Add("SchoolID", (int)sdr["SchoolID"]);
                    if (sdr["SchoolName"] != DBNull.Value) emp.Add("SchoolName", (string)sdr["SchoolName"]);
                    if (sdr["NeedEnroll"] != DBNull.Value) emp.Add("NeedEnroll", (int)sdr["NeedEnroll"]);
                    if (sdr["girllim"] != DBNull.Value) emp.Add("girllim", (int)sdr["girllim"]);
                    if (sdr["boylim"] != DBNull.Value) emp.Add("boylim", (int)sdr["boylim"]);
                    if (sdr["boynum"] != DBNull.Value) emp.Add("boynum", (int)sdr["boynum"]);
                    if (sdr["Pv"] != DBNull.Value) emp.Add("Pv", (int)sdr["Pv"]);
                    if (sdr["Cv"] != DBNull.Value) emp.Add("Cv", (int)sdr["Cv"]);
                    pv = (int)sdr["Pv"]; cv = (int)sdr["Cv"];
                    if (sdr["girlnum"] != DBNull.Value) emp.Add("girlnum", (int)sdr["girlnum"]);
                }
                conn.Close();
                //cmd.CommandText = "select * from member_activity where loginid='" + loginid + "' and ActivityID = " + actid.ToString();
                //conn.Open();
                //sdr = cmd.ExecuteReader();
                //if (sdr.Read())
                //{


                //    if (sdr["cjState"] != DBNull.Value) emp.Add("cjState", (int)sdr["cjState"]);

                //}
                //else
                //{


                //    emp.Add("cjState", 0);

                //}
                emp.Add("cjState", 0);
                pv = pv + 1;
                obj.Add(emp);
                emp1.Add("activityx_info", obj);
                conn.Close();
                cmd.CommandText = "update activity set pv = " + pv + " where activityid = " + actid;
                conn.Open();
                cmd.ExecuteNonQuery();
                return emp1.ToString();
            }


            catch (System.Exception ee)
            {
                writeLog("get_activityx_info_nologin", ee.Message.ToString());
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        public static void insert_DYClick(string src)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "insert into DYClick(src) values ('" + src + "')";
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                //return ;

            }


            catch (System.Exception ee)
            {
                writeLog("insert_DYClick", ee.Message.ToString());
                //return (ee.Message.ToString());
                
            }
            finally
            {
                conn.Close();
            }
        }

        public static string get_exquisite(string btime, string etime, string src)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from activity ,ORGANAZATION,school,ExquisitePush where activity.activitytime > '" + btime + "' and activity.activitytime < '" + etime + "' and activity.isexquisite = 1 and Activity.OrganazationID = ORGANAZATION.OrganizationID and ORGANAZATION.SchoolID = School.SchoolID and activity.activityid = ExquisitePush.activityid";
            int pv = 0, cv = 0;
            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();
                

                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    //if (sdr["GroupID"] != DBNull.Value) emp.Add("GroupID", (string)sdr["GroupID"]);
                    //else emp.Add("GroupID", "");
                    emp.Add("ActivityID", (int)sdr["ActivityID"]);
                    if (sdr["ActivityName"] != DBNull.Value) emp.Add("ActivityName", (string)sdr["ActivityName"]);
                    //if (sdr["OrganazationID"] != DBNull.Value) emp.Add("OrganazationID", (int)sdr["OrganazationID"]);
                    //if (sdr["OrganizationName"] != DBNull.Value) emp.Add("OrganizationName", (string)sdr["OrganizationName"]);
                    //if (sdr["ActivirtyContent"] != DBNull.Value) emp.Add("ActivirtyContent", (string)sdr["ActivirtyContent"]);
                    
                    //if (sdr["OriginalDir"] != DBNull.Value) emp.Add("OriginalDir", (string)sdr["OriginalDir"]);
                    emp.Add("ActivirtyRecommendWords", (string)sdr["ActIntro"]);
                    //if (sdr["originaldirx"] != DBNull.Value) emp.Add("originaldirx", (string)sdr["originaldirx"]);
                    //if (sdr["Taga"] != DBNull.Value) emp.Add("Taga", (string)sdr["Taga"]);
                    //if (sdr["Tagb"] != DBNull.Value) emp.Add("Tagb", (string)sdr["Tagb"]);
                    //if (sdr["Tagc"] != DBNull.Value) emp.Add("Tagc", (string)sdr["Tagc"]);
                    
                    if (sdr["Address"] != DBNull.Value) emp.Add("Address", (string)sdr["Address"]);
                    if (sdr["PhotoDir"] != DBNull.Value) emp.Add("ActivityImage", (string)sdr["PhotoDir"]);
                    //if (sdr["Rate"] != DBNull.Value) emp.Add("Rate", (double)sdr["Rate"]);
                    if (sdr["SchoolID"] != DBNull.Value) emp.Add("SchoolID", (int)sdr["SchoolID"]);
                    if (sdr["SchoolName"] != DBNull.Value) emp.Add("SchoolName", (string)sdr["SchoolName"]);
                    if (sdr["ActivityTime"] != DBNull.Value) emp.Add("ActivityTime", ((DateTime)sdr["ActivityTime"]).ToString());
                    //if (sdr["NeedEnroll"] != DBNull.Value) emp.Add("NeedEnroll", (int)sdr["NeedEnroll"]);
                    //if (sdr["girllim"] != DBNull.Value) emp.Add("girllim", (int)sdr["girllim"]);
                    //if (sdr["boylim"] != DBNull.Value) emp.Add("boylim", (int)sdr["boylim"]);
                    //if (sdr["boynum"] != DBNull.Value) emp.Add("boynum", (int)sdr["boynum"]);
                    //if (sdr["Pv"] != DBNull.Value) emp.Add("Pv", (int)sdr["Pv"]);
                    //if (sdr["Cv"] != DBNull.Value) emp.Add("Cv", (int)sdr["Cv"]);
                    obj.Add(emp);
                    //if (sdr["girlnum"] != DBNull.Value) emp.Add("girlnum", (int)sdr["girlnum"]);
                }
                
                
                
                emp1.Add("exquisite_info", obj);
                conn.Close();
                cmd.CommandText = "insert into ExquisiteClick(src) values('" + src + "')";
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                return emp1.ToString();
            }


            catch (System.Exception ee)
            {
                //writeLog("exquisite_info", ee.Message.ToString());
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        public static string get_member_actx(string loginid, int actid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from activity ,ORGANAZATION,school where ActivityID = " + actid.ToString() + " and Activity.OrganazationID = ORGANAZATION.OrganizationID and ORGANAZATION.SchoolID = School.SchoolID";
            int pv = 0, cv = 0;
            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();
                JObject emp = new JObject();

                while (sdr.Read())
                {
                    if (sdr["GroupID"] != DBNull.Value) emp.Add("GroupID", (string)sdr["GroupID"]);
                    else emp.Add("GroupID", "");
                    if (sdr["ActivityName"] != DBNull.Value) emp.Add("ActivityName", (string)sdr["ActivityName"]);
                    if (sdr["OrganazationID"] != DBNull.Value) emp.Add("OrganazationID", (int)sdr["OrganazationID"]);
                    if (sdr["OrganizationName"] != DBNull.Value) emp.Add("OrganizationName", (string)sdr["OrganizationName"]);
                    if (sdr["ActivirtyContent"] != DBNull.Value) emp.Add("ActivirtyContent", (string)sdr["ActivirtyContent"]);
                    if (sdr["PhotoDir"] != DBNull.Value) emp.Add("PhotoDir", (string)sdr["PhotoDir"]);
                    if (sdr["OriginalDir"] != DBNull.Value) emp.Add("OriginalDir", (string)sdr["OriginalDir"]);
                    if (sdr["logo"] != DBNull.Value) emp.Add("logo", (string)sdr["logo"]);
                    if (sdr["originaldirx"] != DBNull.Value) emp.Add("originaldirx", (string)sdr["originaldirx"]);
                    if (sdr["Taga"] != DBNull.Value) emp.Add("Taga", (string)sdr["Taga"]);
                    if (sdr["Tagb"] != DBNull.Value) emp.Add("Tagb", (string)sdr["Tagb"]);
                    if (sdr["Tagc"] != DBNull.Value) emp.Add("Tagc", (string)sdr["Tagc"]);
                    if (sdr["ActivityTime"] != DBNull.Value) emp.Add("ActivityTime", ((DateTime)sdr["ActivityTime"]).ToString());
                    if (sdr["Address"] != DBNull.Value) emp.Add("Address", (string)sdr["Address"]);
                    if (sdr["Rate"] != DBNull.Value) emp.Add("Rate", (double)sdr["Rate"]);
                    if (sdr["SchoolID"] != DBNull.Value) emp.Add("SchoolID", (int)sdr["SchoolID"]);
                    if (sdr["SchoolName"] != DBNull.Value) emp.Add("SchoolName", (string)sdr["SchoolName"]);

                    if (sdr["girllim"] != DBNull.Value) emp.Add("girllim", (int)sdr["girllim"]);
                    if (sdr["boylim"] != DBNull.Value) emp.Add("boylim", (int)sdr["boylim"]);
                    if (sdr["boynum"] != DBNull.Value) emp.Add("boynum", (int)sdr["boynum"]);
                    if (sdr["Pv"] != DBNull.Value) emp.Add("Pv", (int)sdr["Pv"]);
                    if (sdr["Cv"] != DBNull.Value) emp.Add("Cv", (int)sdr["Cv"]);
                    pv = (int)sdr["Pv"]; cv = (int)sdr["Cv"];
                    if (sdr["girlnum"] != DBNull.Value) emp.Add("girlnum", (int)sdr["girlnum"]);
                }
                conn.Close();
                cmd.CommandText = "select * from member_activity where loginid='" + loginid + "' and ActivityID = " + actid.ToString();
                conn.Open();
                sdr = cmd.ExecuteReader();
                if (sdr.Read())
                {

                    if (sdr["Rate"] != DBNull.Value) emp.Add("mRate", (double)sdr["Rate"]);
                    if (sdr["cjState"] != DBNull.Value) emp.Add("cjState", (int)sdr["cjState"]);

                }
                else
                {

                    emp.Add("mRate", 0);
                    emp.Add("cjState", 0);

                }
                pv = pv + 1;
                obj.Add(emp);
                emp1.Add("member_actx_info", obj);
                conn.Close();
                cmd.CommandText = "update activity set pv = " + pv + " where activityid = " + actid;
                conn.Open();
                cmd.ExecuteNonQuery();
                return emp1.ToString();
            }


            catch (System.Exception ee)
            {
                writeLog("get_activityx_info", ee.Message.ToString());
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        public static bool add_activity_cj(string loginid,int actid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from Member_Activity where activityid = " + actid + " and LoginID = '" + loginid + "'";
            string sstime = DateTime.Now.ToString();
            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.Read() == false)
                {
                    conn.Close();
                    conn.Open();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "insert into Member_Activity(ActivityID,LoginID,cjState,cjtime) values (" + actid + ",'" + loginid + "',1,'"+sstime+"')";
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    conn.Close();
                    conn.Open();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "update Member_Activity set cjstate = 1, cjtime='"+sstime+"' where LoginID='" + loginid + "' and activityid = " + actid + "";
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (System.Exception ee)
            {
                writeLog("add_activity_cj", ee.Message.ToString());
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        public static string hasEnroll(string loginid, int actid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from EnrollInfo where loginID = '" + loginid + "' and ActID = " + actid.ToString();
            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                
                JObject emp = new JObject();
                if (sdr.Read())
                {
                    emp.Add("hasEnroll", "true");
                    if (sdr["Name"] != DBNull.Value) emp.Add("Name", (string)sdr["Name"]);
                    if (sdr["Phone"] != DBNull.Value) emp.Add("Phone", (string)sdr["Phone"]);
                    if (sdr["Major"] != DBNull.Value) emp.Add("Major", (string)sdr["Major"]);
                    if (sdr["Grade"] != DBNull.Value) emp.Add("Grade", (string)sdr["Grade"]);
                    if (sdr["Sex"] != DBNull.Value) emp.Add("Sex", (int)sdr["Sex"]);

                }
                else emp.Add("hasEnroll", "false");
                return emp.ToString();
            }


            catch (System.Exception ee)
            {
                writeLog("hasEnroll", ee.Message.ToString());
                //return (ee.Message.ToString());
                return "false";
            }
            finally
            {
                conn.Close();
            }
        }

        public static bool addEnrollInfo(string loginid, int actid, string name, string phone,string major,string grade,int sex)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "insert into EnrollInfo(ActID,loginID,name,phone,major,grade,sex) values(" + actid + ",'" + loginid + "','" + name + "','" + phone + "','" + major + "','" + grade + "'," + sex + ")";
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
               
            }


            catch (System.Exception ee)
            {
                writeLog("addEnrollInfo", ee.Message.ToString());
                //return (ee.Message.ToString());
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public static bool updateEnrollInfo(string loginid, int actid, string name, string phone, string major, string grade, int sex)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "update EnrollInfo set name = '" + name + "',phone = '" + phone + "',major = '" + major + "',grade = '" + grade + "',sex = " + sex; 
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }


            catch (System.Exception ee)
            {
                writeLog("updateEnrollInfo", ee.Message.ToString());
                //return (ee.Message.ToString());
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public static bool deleteEnollInfo(string loginid, int actid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "delete from EnrollInfo where ActID = " + actid + " and loginID = '" + loginid + "'";
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }


            catch (System.Exception ee)
            {
                writeLog("deleteEnollInfo", ee.Message.ToString());
                //return (ee.Message.ToString());
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public static bool delete_activity_cj(string loginid, int actid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "update Member_Activity set cjstate = 0 where LoginID='" + loginid + "' and activityid = " + actid + "";
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }


            catch (System.Exception ee)
            {
                writeLog("delete_activity_cj", ee.Message.ToString());
                //return (ee.Message.ToString());
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        public static bool add_activity_comment(string loginid, int actid, string content)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            string sstime = DateTime.Now.ToString();
            cmd.CommandText = "insert into Activity_Comment (ActivityID,LoginID,CommentContent,CommentTime) values(" + actid + ",'" + loginid + "','" + content + "','" + sstime + "')";       
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (System.Exception ee)
            {
                writeLog("add_activity_comment", ee.Message.ToString());
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        //public static bool judge_can_add_comment(string loginid, int actid, string content)
        //{
        //    string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        //    SqlConnection conn = new SqlConnection(strConn);
        //    SqlCommand cmd = conn.CreateCommand();
        //    cmd.CommandText = "UPDATE Member_Activity SET Comment = '" + content + "' where activityid = " + actid + " and LoginID = '" + loginid + "' and (gzstate=1 or cjstate=1)";
        //    try
        //    {
        //        conn.Open();
        //        cmd.ExecuteNonQuery();
        //        return true;
        //    }
        //    catch (System.Exception ee)
        //    {

        //        return false;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }
        //}
        public static bool add_activity_rate(string loginid, int actid, float rate)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Member_Activity SET rate = " + rate + " where activityid = " + actid + " and LoginID = '" + loginid + "' ";
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }


            catch (System.Exception ee)
            {
                writeLog("add_activity_rate", ee.Message.ToString());
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public static string get_relatedschool(int schoolid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from RelateSchool where SchoolID = " + schoolid;

            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();

                int cnt = 0;
                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["relateschoolid"] != DBNull.Value) emp.Add(cnt.ToString(), (int)sdr["relateschoolid"]);
                    cnt++;
                    obj.Add(emp);

                }
                JObject empx = new JObject();
                empx.Add("cnt", cnt);
                obj.Add(empx);
                emp1.Add("relateschool", obj);

                return emp1.ToString();
            }


            catch (System.Exception ee)
            {
                writeLog("get_relatedschool", ee.Message.ToString());
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        public static string get_act(int schoolid, int actid, string acttime)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select top 10 * from activity  inner join Organazation on organazation.OrganizationID = Activity.OrganazationID where Organazation.schoolID = " + schoolid + " and ((Activity.ActivityTime = '" + acttime + "' and Activity.ActivityID > " + actid.ToString() + ") or Activity.ActivityTime < '" + acttime + "') order by activity.ActivityTime desc,activity.ActivityID";

            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();

                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["ActivityID"] != DBNull.Value) emp.Add("ActivityID", (int)sdr["ActivityID"]);
                    if (sdr["OrganazationID"] != DBNull.Value) emp.Add("OrganazationID", (int)sdr["OrganazationID"]);
                    if (sdr["ActivityName"] != DBNull.Value) emp.Add("ActivityName", (string)sdr["ActivityName"]);
                    if (sdr["GroupID"] != DBNull.Value) emp.Add("GroupID", (string)sdr["GroupID"]);
                    if (sdr["Taga"] != DBNull.Value) emp.Add("Taga", (string)sdr["Taga"]);
                    if (sdr["Tagb"] != DBNull.Value) emp.Add("Tagb", (string)sdr["Tagb"]);
                    if (sdr["Tagc"] != DBNull.Value) emp.Add("Tagc", (string)sdr["Tagc"]);
                    if (sdr["OrganizationName"] != DBNull.Value) emp.Add("OrganizationName", (string)sdr["OrganizationName"]);
                    if (sdr["Address"] != DBNull.Value) emp.Add("Address", (string)sdr["Address"]);
                    if (sdr["PhotoDir"] != DBNull.Value) emp.Add("PhotoDir", (string)sdr["PhotoDir"]);
                    if (sdr["OriginalDir"] != DBNull.Value) emp.Add("OriginalDir", (string)sdr["OriginalDir"]);
                    if (sdr["girllim"] != DBNull.Value) emp.Add("girllim", (int)sdr["girllim"]);
                    if (sdr["boylim"] != DBNull.Value) emp.Add("boylim", (int)sdr["boylim"]);
                    if (sdr["boynum"] != DBNull.Value) emp.Add("boynum", (int)sdr["boynum"]);
                    if (sdr["girlnum"] != DBNull.Value) emp.Add("girlnum", (int)sdr["girlnum"]);
                    if (sdr["ActivityTime"] != DBNull.Value) emp.Add("ActivityTime", ((DateTime)sdr["ActivityTime"]).ToString());
                    obj.Add(emp);
                    //return emp.ToString();
                }

                emp1.Add("activity_all_info", obj);
                //obj.ToString();
                return emp1.ToString();
            }

            catch (System.Exception ee)
            {
                writeLog("get_act", ee.Message.ToString());
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        public static string get_actx(string loginid, int actid, string acttime)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "with A as (select * from member_activity where loginid = '" + loginid + "')select top 10 * from activity   inner join Organazation on organazation.OrganizationID = Activity.OrganazationID inner join school on organazation.SchoolID = School.SchoolID left outer join A on Activity.ActivityID = A.ActivityID where Organazation.schoolID in (select SchoolID from Member_School where LoginID = '" + loginid + "' and gzstate = 1) and ((Activity.ActivityTime = '" + acttime + "' and Activity.ActivityID > " + actid.ToString() + ") or Activity.ActivityTime > '" + acttime + "') order by activity.ActivityTime,activity.ActivityID";
            int[] ac = new int[11];
            int top = 0;
            int[] cv = new int[11];
            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();

                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["ActivityID"] != DBNull.Value) emp.Add("ActivityID", (int)sdr["ActivityID"]);
                    ac[top] = (int)sdr["ActivityID"];
                    if (sdr["OrganazationID"] != DBNull.Value) emp.Add("OrganazationID", (int)sdr["OrganazationID"]);
                    if (sdr["ActivityName"] != DBNull.Value) emp.Add("ActivityName", (string)sdr["ActivityName"]);
                    if (sdr["GroupID"] != DBNull.Value) emp.Add("GroupID", (string)sdr["GroupID"]);
                    if (sdr["Taga"] != DBNull.Value) emp.Add("Taga", (string)sdr["Taga"]);
                    if (sdr["Tagb"] != DBNull.Value) emp.Add("Tagb", (string)sdr["Tagb"]);
                    if (sdr["Tagc"] != DBNull.Value) emp.Add("Tagc", (string)sdr["Tagc"]);
                    if (sdr["ActivirtyContent"] != DBNull.Value) emp.Add("ActivirtyContent", (string)sdr["ActivirtyContent"]);
                    if (sdr["OrganizationName"] != DBNull.Value) emp.Add("OrganizationName", (string)sdr["OrganizationName"]);
                    if (sdr["Address"] != DBNull.Value) emp.Add("Address", (string)sdr["Address"]);
                    if (sdr["Rate"] != DBNull.Value) emp.Add("Rate", (double)sdr["Rate"]);
                    if (sdr["PhotoDir"] != DBNull.Value) emp.Add("PhotoDir", (string)sdr["PhotoDir"]);
                    if (sdr["OriginalDir"] != DBNull.Value) emp.Add("OriginalDir", (string)sdr["OriginalDir"]);
                    if (sdr["logo"] != DBNull.Value) emp.Add("logo", (string)sdr["logo"]);
                    if (sdr["originaldirx"] != DBNull.Value) emp.Add("originaldirx", (string)sdr["originaldirx"]);
                    if (sdr["SchoolID"] != DBNull.Value) emp.Add("SchoolID", (int)sdr["SchoolID"]);
                    if (sdr["SchoolName"] != DBNull.Value) emp.Add("SchoolName", (string)sdr["SchoolName"]);

                    if (sdr["girllim"] != DBNull.Value) emp.Add("girllim", (int)sdr["girllim"]);
                    if (sdr["boylim"] != DBNull.Value) emp.Add("boylim", (int)sdr["boylim"]);
                    if (sdr["boynum"] != DBNull.Value) emp.Add("boynum", (int)sdr["boynum"]);
                    if (sdr["girlnum"] != DBNull.Value) emp.Add("girlnum", (int)sdr["girlnum"]);
                    if (sdr["Pv"] != DBNull.Value) emp.Add("Pv", (int)sdr["Pv"]);
                    if (sdr["Cv"] != DBNull.Value) emp.Add("Cv", (int)sdr["Cv"]);
                    cv[top++] = (int)sdr["Cv"] + 1;
                    if (sdr["ActivityTime"] != DBNull.Value) emp.Add("ActivityTime", ((DateTime)sdr["ActivityTime"]).ToString());
                    if (sdr["cjstate"] != DBNull.Value) emp.Add("cjstate", (int)sdr["cjstate"]);
                    else emp.Add("cjstate", 0);
                    obj.Add(emp);
                    //return emp.ToString();
                }

                emp1.Add("activity_all_info", obj);
                //obj.ToString();
                for (int i = 0; i < 10; i++)
                {
                    conn.Close();
                    cmd.CommandText = "update activity set cv = " + cv[i] + " where activityid = " + ac[i];
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                return emp1.ToString();
            }

            catch (System.Exception ee)
            {
                writeLog("get_actbytime", ee.Message.ToString());
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        public static string get_actx_all(string loginid, int actid, string acttime)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "with A as (select * from member_activity where loginid = '" + loginid + "')select * from activity   inner join Organazation on organazation.OrganizationID = Activity.OrganazationID inner join school on organazation.SchoolID = School.SchoolID left outer join A on Activity.ActivityID = A.ActivityID where Organazation.schoolID in (select SchoolID from Member_School where LoginID = '" + loginid + "' and gzstate = 1) and ((Activity.ActivityTime = '" + acttime + "' and Activity.ActivityID > " + actid.ToString() + ") or Activity.ActivityTime > '" + acttime + "') order by activity.ActivityTime,activity.ActivityID";
            int[] ac = new int[1100];
            int top = 0;
            int[] cv = new int[1100];
            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();

                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["ActivityID"] != DBNull.Value) emp.Add("ActivityID", (int)sdr["ActivityID"]);
                    ac[top] = (int)sdr["ActivityID"];
                    if (sdr["OrganazationID"] != DBNull.Value) emp.Add("OrganazationID", (int)sdr["OrganazationID"]);
                    if (sdr["ActivityName"] != DBNull.Value) emp.Add("ActivityName", (string)sdr["ActivityName"]);
                    if (sdr["GroupID"] != DBNull.Value) emp.Add("GroupID", (string)sdr["GroupID"]);
                    if (sdr["Taga"] != DBNull.Value) emp.Add("Taga", (string)sdr["Taga"]);
                    if (sdr["Tagb"] != DBNull.Value) emp.Add("Tagb", (string)sdr["Tagb"]);
                    if (sdr["Tagc"] != DBNull.Value) emp.Add("Tagc", (string)sdr["Tagc"]);
                    if (sdr["ActivirtyContent"] != DBNull.Value) emp.Add("ActivirtyContent", (string)sdr["ActivirtyContent"]);
                    if (sdr["OrganizationName"] != DBNull.Value) emp.Add("OrganizationName", (string)sdr["OrganizationName"]);
                    if (sdr["Address"] != DBNull.Value) emp.Add("Address", (string)sdr["Address"]);
                    if (sdr["Rate"] != DBNull.Value) emp.Add("Rate", (double)sdr["Rate"]);
                    if (sdr["PhotoDir"] != DBNull.Value) emp.Add("PhotoDir", (string)sdr["PhotoDir"]);
                    if (sdr["OriginalDir"] != DBNull.Value) emp.Add("OriginalDir", (string)sdr["OriginalDir"]);
                    if (sdr["logo"] != DBNull.Value) emp.Add("logo", (string)sdr["logo"]);
                    if (sdr["originaldirx"] != DBNull.Value) emp.Add("originaldirx", (string)sdr["originaldirx"]);
                    if (sdr["SchoolID"] != DBNull.Value) emp.Add("SchoolID", (int)sdr["SchoolID"]);
                    if (sdr["SchoolName"] != DBNull.Value) emp.Add("SchoolName", (string)sdr["SchoolName"]);

                    if (sdr["girllim"] != DBNull.Value) emp.Add("girllim", (int)sdr["girllim"]);
                    if (sdr["boylim"] != DBNull.Value) emp.Add("boylim", (int)sdr["boylim"]);
                    if (sdr["boynum"] != DBNull.Value) emp.Add("boynum", (int)sdr["boynum"]);
                    if (sdr["girlnum"] != DBNull.Value) emp.Add("girlnum", (int)sdr["girlnum"]);
                    if (sdr["Pv"] != DBNull.Value) emp.Add("Pv", (int)sdr["Pv"]);
                    if (sdr["Cv"] != DBNull.Value) emp.Add("Cv", (int)sdr["Cv"]);
                    cv[top++] = (int)sdr["Cv"] + 1;
                    if (sdr["ActivityTime"] != DBNull.Value) emp.Add("ActivityTime", ((DateTime)sdr["ActivityTime"]).ToString());
                    if (sdr["cjstate"] != DBNull.Value) emp.Add("cjstate", (int)sdr["cjstate"]);
                    else emp.Add("cjstate", 0);
                    obj.Add(emp);
                    //return emp.ToString();
                }

                emp1.Add("activity_all_info", obj);
                //obj.ToString();
                for (int i = 0; i < 10; i++)
                {
                    conn.Close();
                    cmd.CommandText = "update activity set cv = " + cv[i] + " where activityid = " + ac[i];
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                return emp1.ToString();
            }

            catch (System.Exception ee)
            {
                writeLog("get_actbytime", ee.Message.ToString());
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        public static string get_sch_act(int schoolid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from activity  inner join Organazation on organazation.OrganizationID = Activity.OrganazationID where Organazation.schoolID = " + schoolid + " and activity.ActivityTime > (select dateadd(HOUR,-3,getdate())) order by activity.ActivityTime desc,activity.ActivityID";

            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();

                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["ActivityID"] != DBNull.Value) emp.Add("ActivityID", (int)sdr["ActivityID"]);
                    if (sdr["OrganazationID"] != DBNull.Value) emp.Add("OrganazationID", (int)sdr["OrganazationID"]);
                    if (sdr["ActivityName"] != DBNull.Value) emp.Add("ActivityName", (string)sdr["ActivityName"]);
                    if (sdr["GroupID"] != DBNull.Value) emp.Add("GroupID", (string)sdr["GroupID"]);
                    if (sdr["Taga"] != DBNull.Value) emp.Add("Taga", (string)sdr["Taga"]);
                    if (sdr["Tagb"] != DBNull.Value) emp.Add("Tagb", (string)sdr["Tagb"]);
                    if (sdr["Tagc"] != DBNull.Value) emp.Add("Tagc", (string)sdr["Tagc"]);
                    if (sdr["OrganizationName"] != DBNull.Value) emp.Add("OrganizationName", (string)sdr["OrganizationName"]);
                    if (sdr["Address"] != DBNull.Value) emp.Add("Address", (string)sdr["Address"]);
                    if (sdr["PhotoDir"] != DBNull.Value) emp.Add("PhotoDir", (string)sdr["PhotoDir"]);
                    if (sdr["OriginalDir"] != DBNull.Value) emp.Add("OriginalDir", (string)sdr["OriginalDir"]);
                    if (sdr["girllim"] != DBNull.Value) emp.Add("girllim", (int)sdr["girllim"]);
                    if (sdr["boylim"] != DBNull.Value) emp.Add("boylim", (int)sdr["boylim"]);
                    if (sdr["boynum"] != DBNull.Value) emp.Add("boynum", (int)sdr["boynum"]);
                    if (sdr["girlnum"] != DBNull.Value) emp.Add("girlnum", (int)sdr["girlnum"]);
                    if (sdr["ActivityTime"] != DBNull.Value) emp.Add("ActivityTime", ((DateTime)sdr["ActivityTime"]).ToShortDateString() + " " + ((DateTime)sdr["ActivityTime"]).ToShortTimeString());
                    obj.Add(emp);
                    //return emp.ToString();
                }

                emp1.Add("activity_all_info", obj);
                //obj.ToString();
                return emp1.ToString();
            }

            catch (System.Exception ee)
            {
                writeLog("get_sch_act", ee.Message.ToString());
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        public static string get_actbytime(string loginid,int schoolid, int actid, string acttime)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "with A as (select * from member_activity where loginid = '" + loginid + "')select top 10 * from activity   inner join Organazation on organazation.OrganizationID = Activity.OrganazationID inner join school on organazation.SchoolID = School.SchoolID left outer join A on Activity.ActivityID = A.ActivityID where Organazation.schoolID = " + schoolid + " and ((Activity.ActivityTime = '" + acttime + "' and Activity.ActivityID > " + actid.ToString() + ") or Activity.ActivityTime > '" + acttime + "') order by activity.ActivityTime,activity.ActivityID";
            int[] ac = new int[1100];
            int top = 0;
            int[] cv = new int[1100];
            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();

                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["ActivityID"] != DBNull.Value) emp.Add("ActivityID", (int)sdr["ActivityID"]);
                    ac[top] = (int)sdr["ActivityID"];
                    if (sdr["OrganazationID"] != DBNull.Value) emp.Add("OrganazationID", (int)sdr["OrganazationID"]);
                    if (sdr["ActivityName"] != DBNull.Value) emp.Add("ActivityName", (string)sdr["ActivityName"]);
                    if (sdr["GroupID"] != DBNull.Value) emp.Add("GroupID", (string)sdr["GroupID"]);
                    if (sdr["Taga"] != DBNull.Value) emp.Add("Taga", (string)sdr["Taga"]);
                    if (sdr["Tagb"] != DBNull.Value) emp.Add("Tagb", (string)sdr["Tagb"]);
                    if (sdr["Tagc"] != DBNull.Value) emp.Add("Tagc", (string)sdr["Tagc"]);
                    if (sdr["ActivirtyContent"] != DBNull.Value) emp.Add("ActivirtyContent", (string)sdr["ActivirtyContent"]);
                    if (sdr["OrganizationName"] != DBNull.Value) emp.Add("OrganizationName", (string)sdr["OrganizationName"]);
                    if (sdr["Address"] != DBNull.Value) emp.Add("Address", (string)sdr["Address"]);
                    if (sdr["Rate"] != DBNull.Value) emp.Add("Rate", (double)sdr["Rate"]);
                    if (sdr["PhotoDir"] != DBNull.Value) emp.Add("PhotoDir", (string)sdr["PhotoDir"]);
                    if (sdr["OriginalDir"] != DBNull.Value) emp.Add("OriginalDir", (string)sdr["OriginalDir"]);
                    if (sdr["logo"] != DBNull.Value) emp.Add("logo", (string)sdr["logo"]);
                    if (sdr["originaldirx"] != DBNull.Value) emp.Add("originaldirx", (string)sdr["originaldirx"]);
                    if (sdr["SchoolID"] != DBNull.Value) emp.Add("SchoolID", (int)sdr["SchoolID"]);
                    if (sdr["SchoolName"] != DBNull.Value) emp.Add("SchoolName", (string)sdr["SchoolName"]);
                    
                    if (sdr["girllim"] != DBNull.Value) emp.Add("girllim", (int)sdr["girllim"]);
                    if (sdr["boylim"] != DBNull.Value) emp.Add("boylim", (int)sdr["boylim"]);
                    if (sdr["boynum"] != DBNull.Value) emp.Add("boynum", (int)sdr["boynum"]);
                    if (sdr["girlnum"] != DBNull.Value) emp.Add("girlnum", (int)sdr["girlnum"]);
                    if (sdr["Pv"] != DBNull.Value) emp.Add("Pv", (int)sdr["Pv"]);
                    if (sdr["Cv"] != DBNull.Value) emp.Add("Cv", (int)sdr["Cv"]);
                    cv[top++] = (int)sdr["Cv"] + 1;
                    if (sdr["ActivityTime"] != DBNull.Value) emp.Add("ActivityTime", ((DateTime)sdr["ActivityTime"]).ToString());
                    if (sdr["cjstate"] != DBNull.Value) emp.Add("cjstate", (int)sdr["cjstate"]);
                    else emp.Add("cjstate", 0);
                    obj.Add(emp);
                    //return emp.ToString();
                }

                emp1.Add("activity_all_info", obj);
                //obj.ToString();
                for (int i = 0; i < 10; i++)
                {
                    conn.Close();
                    cmd.CommandText = "update activity set cv = " + cv[i] + " where activityid = " + ac[i];
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                    return emp1.ToString();
            }

            catch (System.Exception ee)
            {
                writeLog("get_actbytime", ee.Message.ToString());
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        public static string get_relatedact(int relatedid, int actid, string acttime)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = String.Format(
@"select top 10 a.*,o.* from Organazation as o  inner join Activity as a on o.OrganizationID = a.OrganazationID 
where o.schoolID = (select SchoolID from Organazation as o inner join Activity as a on o.OrganizationID = a.OrganazationID where a.ActivityID = {0}) 
and ((a.ActivityTime = '{2}' and a.ActivityID > {1}) or a.ActivityTime < '{2}')  
and a.ActivityID != {0} and ((a.Address = (select Address from Activity where ActivityID = {0})) 
OR a.OrganazationID = (select OrganazationID from Activity where ActivityID = {0})) 
AND ABS(DATEDIFF(DAY,a.ActivityTime,(Select ActivityTime from Activity where ActivityID = {0}))) <= 7 
order by a.ActivityTime desc,a.ActivityID",relatedid,actid,acttime);

            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();

                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["ActivityID"] != DBNull.Value) emp.Add("ActivityID", (int)sdr["ActivityID"]);
                    if (sdr["OrganazationID"] != DBNull.Value) emp.Add("OrganazationID", (int)sdr["OrganazationID"]);
                    if (sdr["ActivityName"] != DBNull.Value) emp.Add("ActivityName", (string)sdr["ActivityName"]);
                    if (sdr["GroupID"] != DBNull.Value) emp.Add("GroupID", (string)sdr["GroupID"]);
                    if (sdr["Taga"] != DBNull.Value) emp.Add("Taga", (string)sdr["Taga"]);
                    if (sdr["Tagb"] != DBNull.Value) emp.Add("Tagb", (string)sdr["Tagb"]);
                    if (sdr["Tagc"] != DBNull.Value) emp.Add("Tagc", (string)sdr["Tagc"]);
                    if (sdr["OrganizationName"] != DBNull.Value) emp.Add("OrganizationName", (string)sdr["OrganizationName"]);
                    if (sdr["Address"] != DBNull.Value) emp.Add("Address", (string)sdr["Address"]);
                    if (sdr["PhotoDir"] != DBNull.Value) emp.Add("PhotoDir", (string)sdr["PhotoDir"]);
                    if (sdr["OriginalDir"] != DBNull.Value) emp.Add("OriginalDir", (string)sdr["OriginalDir"]);
                    if (sdr["CountMember"] != DBNull.Value) emp.Add("CountMember", (int)sdr["CountMember"]);
                    if (sdr["ActivityTime"] != DBNull.Value) emp.Add("ActivityTime", ((DateTime)sdr["ActivityTime"]).ToString());
                    obj.Add(emp);
                    //return emp.ToString();
                }

                emp1.Add("activity_all_info", obj);
                //obj.ToString();
                return emp1.ToString();
            }


            catch (System.Exception ee)
            {
                writeLog("get_relatedact", ee.Message.ToString());
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        public static bool post_confirm(string mid, int b)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            string sstime = DateTime.Now.ToString();
            cmd.CommandText = "UPDATE Message_Member SET status = " + (b == 1 ? 1 : 2) + " , confirmtime = getdate()  where  MsgMemID = '" + mid + "' ";
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (System.Exception ee)
            {
                writeLog("post_confirm", ee.Message.ToString());
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion
        #region 社团
        public static string get_org_byclass(string loginID ,int classid,int schoolid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "select * from Member_School where LoginID = '" + loginID + "'";
            conn.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            int count = 0;
            int[] schid=new int[100000];
            while (sdr.Read())
            {
                schid[count++] = (int)sdr["SchoolID"];
            }
            conn.Close();
         
            try
            {
                JObject emp1 = new JObject();
                JArray obj = new JArray();
                for (int i = 0; i < count; i++)
                {
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "with A AS (select * from member_organazation where loginID='" + loginID + "')select  * from organazation left outer join A on A.organazationID = organazation.organizationID where organazation.schoolid=" + schid[i] + " and organazation.classid = " + classid + "";
                    conn.Open();
                    SqlDataReader sdrx = cmd.ExecuteReader();
                    while (sdrx.Read())
                    {
                        JObject emp = new JObject();
                        if (sdrx["OrganizationID"] != DBNull.Value) emp.Add("OrganizationID", (int)sdrx["OrganizationID"]);
                        if (sdrx["OrganizationName"] != DBNull.Value) emp.Add("OrganizationName", (string)sdrx["OrganizationName"]);
                        if (sdrx["GroupID"] != DBNull.Value) emp.Add("GroupID", (string)sdrx["GroupID"]);
                        if (sdrx["Introduction"] != DBNull.Value) emp.Add("Introduction", (string)sdrx["Introduction"]);
                        if (sdrx["Logo"] != DBNull.Value) emp.Add("Logo", (string)sdrx["Logo"]);
                        if (sdrx["originaldirx"] != DBNull.Value) emp.Add("originaldirx", (string)sdrx["originaldirx"]);
                        if (sdrx["Rate"] != DBNull.Value) emp.Add("Rate", (double)sdrx["Rate"]);
                        if (sdrx["gzState"] != DBNull.Value) emp.Add("gzstate", (int)sdrx["gzState"]);
                        else emp.Add("gzstate", 0);
                        int countnum = (int)sdrx["girlnum"] + (int)sdrx["boynum"];
                        emp.Add("Countnumber", countnum);

                        obj.Add(emp);

                    }
                    conn.Close();
                }
                   
                
                emp1.Add("org_byclass", obj);
                 
                return emp1.ToString();
            }


            catch (System.Exception ee)
            {
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            //return "error";
        }

        public static string get_org_all(string loginID, int schoolid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "select * from Member_School where LoginID = '" + loginID + "'";
            conn.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            int count = 0;
            int[] schid = new int[100000];
            while (sdr.Read())
            {
                schid[count++] = (int)sdr["SchoolID"];
            }
            conn.Close();

            try
            {
                JObject emp1 = new JObject();
                JArray obj = new JArray();
                for (int i = 0; i < count; i++)
                {
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "with A AS (select * from member_organazation where loginID='" + loginID + "')select  * from organazation left outer join A on A.organazationID = organazation.organizationID where organazation.schoolid=" + schid[i];
                    conn.Open();
                    SqlDataReader sdrx = cmd.ExecuteReader();
                    while (sdrx.Read())
                    {
                        JObject emp = new JObject();
                        if (sdrx["OrganizationID"] != DBNull.Value) emp.Add("OrganizationID", (int)sdrx["OrganizationID"]);
                        if (sdrx["OrganizationName"] != DBNull.Value) emp.Add("OrganizationName", (string)sdrx["OrganizationName"]);
                        if (sdrx["GroupID"] != DBNull.Value) emp.Add("GroupID", (string)sdrx["GroupID"]);
                        if (sdrx["Introduction"] != DBNull.Value) emp.Add("Introduction", (string)sdrx["Introduction"]);
                        if (sdrx["Logo"] != DBNull.Value) emp.Add("Logo", (string)sdrx["Logo"]);
                        if (sdrx["originaldirx"] != DBNull.Value) emp.Add("originaldirx", (string)sdrx["originaldirx"]);
                        if (sdrx["Rate"] != DBNull.Value) emp.Add("Rate", (double)sdrx["Rate"]);
                        if (sdrx["gzState"] != DBNull.Value) emp.Add("gzstate", (int)sdrx["gzState"]);
                        else emp.Add("gzstate", 0);
                        int countnum = (int)sdrx["girlnum"] + (int)sdrx["boynum"];
                        emp.Add("Countnumber", countnum);

                        obj.Add(emp);

                    }
                    conn.Close();
                }


                emp1.Add("org_all", obj);

                return emp1.ToString();
            }


            catch (System.Exception ee)
            {
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            //return "error";
        }
        public static string get_org_byrate(string loginid,double rate,int orgid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "with A as (select * from member_organazation where loginID='" + loginid + "') select top 10 * from organazation left outer join A on A.organazationID = organazation.organizationID where rate < " + rate + " or (rate = " + rate + " and organizationid > " + orgid + ")order by rate DESC,OrganizationID";

            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();


                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["OrganizationID"] != DBNull.Value) emp.Add("OrganizationID", (int)sdr["OrganizationID"]);
                    if (sdr["OrganizationName"] != DBNull.Value) emp.Add("OrganizationName", (string)sdr["OrganizationName"]);
                    if (sdr["GroupID"] != DBNull.Value) emp.Add("GroupID", (string)sdr["GroupID"]);
                    if (sdr["Introduction"] != DBNull.Value) emp.Add("Introduction", (string)sdr["Introduction"]);
                    if (sdr["Logo"] != DBNull.Value) emp.Add("Logo", (string)sdr["Logo"]);
                    if (sdr["originaldirx"] != DBNull.Value) emp.Add("originaldirx", (string)sdr["originaldirx"]);
                    if (sdr["Rate"] != DBNull.Value) emp.Add("Rate", (double)sdr["Rate"]);
                    if (sdr["gzState"] != DBNull.Value) emp.Add("gzstate", (int)sdr["gzState"]);
                    else emp.Add("gzstate", 0);
                    if (sdr["ClassID"] != DBNull.Value) emp.Add("ClassID", (int)sdr["ClassID"]);
                    
                    //if (sdr["ClassID"] != DBNull.Value) emp.Add("ClassID", (int)sdr["ClassID"]);
                    int countnum = (int)sdr["girlnum"] + (int)sdr["boynum"];
                    emp.Add("Countnumber", countnum);
                    obj.Add(emp);

                }

                emp1.Add("org_all", obj);

                return emp1.ToString();
            }


            catch (System.Exception ee)
            {
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            //return "error";
        }
        public static string get_org_web(int classid, int schoolid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            if (classid == 0) cmd.CommandText = "select * from Organazation left outer join class on organazation.ClassID = class.ClassID left outer join school on organazation.schoolid = school.schoolid where School.SchoolID = " + schoolid;
            else cmd.CommandText = "select * from Organazation left outer join class on organazation.ClassID = class.ClassID left outer join school on organazation.schoolid = school.schoolid where organazation.classid = " + classid + " and School.SchoolID = " + schoolid;
            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();


                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["OrganizationID"] != DBNull.Value) emp.Add("OrganizationID", (int)sdr["OrganizationID"]);
                    if (sdr["OrganizationName"] != DBNull.Value) emp.Add("OrganizationName", (string)sdr["OrganizationName"]);
                    if (sdr["GroupID"] != DBNull.Value) emp.Add("GroupID", (string)sdr["GroupID"]);
                    if (sdr["Introduction"] != DBNull.Value) emp.Add("Introduction", (string)sdr["Introduction"]);
                    if (sdr["Logo"] != DBNull.Value) emp.Add("Logo", (string)sdr["Logo"]);
                    if (sdr["originaldirx"] != DBNull.Value) emp.Add("originaldirx", (string)sdr["originaldirx"]);
                    if (sdr["Rate"] != DBNull.Value) emp.Add("Rate", (double)sdr["Rate"]);
                    if (sdr["SchoolID"] != DBNull.Value) emp.Add("SchoolID", (int)sdr["SchoolID"]);
                    if (sdr["SchoolName"] != DBNull.Value) emp.Add("SchoolName", (string)sdr["SchoolName"]);
                    if (sdr["ClassName"] != DBNull.Value) emp.Add("ClassName", (string)sdr["ClassName"]);
                    
                    //if (sdr["ClassID"] != DBNull.Value) emp.Add("ClassID", (int)sdr["ClassID"]);
                    int countnum = (int)sdr["girlnum"] + (int)sdr["boynum"];
                    emp.Add("Countnumber", countnum);
                    obj.Add(emp);

                }

                emp1.Add("org_all", obj);

                return emp1.ToString();
            }


            catch (System.Exception ee)
            {
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            //return "error";
        }
        public static string get_org_bycnt(int cnt,int orgid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select top 10 * from organazation where CountMember + Fansnum < " + cnt + " or (CountMember + Fansnum = " + cnt + " and organizationid > " + orgid + ")order by (CountMember + Fansnum) DESC,OrganizationID";

            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();


                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["OrganizationID"] != DBNull.Value) emp.Add("OrganizationID", (int)sdr["OrganizationID"]);
                    if (sdr["OrganizationName"] != DBNull.Value) emp.Add("OrganizationName", (string)sdr["OrganizationName"]);
                    if (sdr["GroupID"] != DBNull.Value) emp.Add("GroupID", (string)sdr["GroupID"]);
                    if (sdr["Logo"] != DBNull.Value) emp.Add("Logo", (string)sdr["Logo"]);
                    if (sdr["originaldirx"] != DBNull.Value) emp.Add("originaldirx", (string)sdr["originaldirx"]);
                    if (sdr["Rate"] != DBNull.Value) emp.Add("Rate", (double)sdr["Rate"]);
                    if (sdr["ClassID"] != DBNull.Value) emp.Add("ClassID", (int)sdr["ClassID"]);
                    int countnum = (int)sdr["girlnum"] + (int)sdr["boynum"];
                    emp.Add("Countnumber", countnum);
                    obj.Add(emp);

                }

                emp1.Add("org_all", obj);

                return emp1.ToString();
            }


            catch (System.Exception ee)
            {
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            //return "error";
        }
        public static string get_org_byname(string name)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from organazation where organizationname like '%" + name + "%' order by rate DESC";

            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();


                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["OrganizationID"] != DBNull.Value) emp.Add("OrganizationID", (int)sdr["OrganizationID"]);
                    if (sdr["OrganizationName"] != DBNull.Value) emp.Add("OrganizationName", (string)sdr["OrganizationName"]);
                    if (sdr["GroupID"] != DBNull.Value) emp.Add("GroupID", (string)sdr["GroupID"]);
                    if (sdr["Logo"] != DBNull.Value) emp.Add("Logo", (string)sdr["Logo"]);
                    if (sdr["originaldirx"] != DBNull.Value) emp.Add("originaldirx", (string)sdr["originaldirx"]);
                    if (sdr["Rate"] != DBNull.Value) emp.Add("Rate", (double)sdr["Rate"]);
                    if (sdr["ClassID"] != DBNull.Value) emp.Add("ClassID", (int)sdr["ClassID"]);
                    int countnum = (int)sdr["girlnum"] + (int)sdr["boynum"];
                    emp.Add("Countnumber", countnum);
                    obj.Add(emp);

                }

                emp1.Add("org_byclass", obj);

                return emp1.ToString();
            }


            catch (System.Exception ee)
            {
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            //return "error";
        }
        public static string get_org_allmember(string orgid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from Member_Organazation ,Member where Member_Organazation.LoginID = Member.LoginID and Member_Organazation.gzstate = 1 and Member_Organazation.OrganazationID = " + orgid;

            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();


                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    //if (sdr["OrganizationID"] != DBNull.Value) emp.Add("OrganizationID", (int)sdr["OrganizationID"]);
                    if (sdr["LoginID"] != DBNull.Value) emp.Add("LoginID", (string)sdr["LoginID"]);
                    if (sdr["nickname"] != DBNull.Value) emp.Add("nickname", (string)sdr["nickname"]);
                    if (sdr["introduce"] != DBNull.Value) emp.Add("introduce", (string)sdr["introduce"]);
                    if (sdr["Sex"] != DBNull.Value) emp.Add("sex", (int)sdr["Sex"]);
                    if (sdr["photo"] != DBNull.Value) emp.Add("photo", (string)sdr["photo"]);
                    else emp.Add("photo", "tidai.jpg");
                    obj.Add(emp);

                }

                emp1.Add("org_allmember", obj);

                return emp1.ToString();
            }


            catch (System.Exception ee)
            {
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            //return "error";
        }
        public static string get_orgx_info(int orgid,string loginid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from  Organazation ,school, Class where organazation.OrganizationID = " + orgid + " and ORGANAZATION.SchoolID = School.SchoolID and ORGANAZATION.ClassID = Class.ClassID";

            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();

                JObject emp = new JObject();
                while (sdr.Read())
                {
                    
                    if (sdr["Introduction"] != DBNull.Value) emp.Add("Introduction", (string)sdr["Introduction"]);
                    else emp.Add("Introduction", "");
                    if (sdr["OrganizationName"] != DBNull.Value) emp.Add("OrganizationName", (string)sdr["OrganizationName"]);
                    else emp.Add("OrganizationName", "");
                    if (sdr["GroupID"] != DBNull.Value) emp.Add("GroupID", (string)sdr["GroupID"]);
                    if (sdr["Logo"] != DBNull.Value) emp.Add("Logo", (string)sdr["Logo"]);
                    if (sdr["originaldirx"] != DBNull.Value) emp.Add("originaldirx", (string)sdr["originaldirx"]);
                    if (sdr["ClassID"] != DBNull.Value) emp.Add("ClassID", (int)sdr["ClassID"]);
                    if (sdr["ClassName"] != DBNull.Value) emp.Add("ClassName", (string)sdr["ClassName"]);
                    if (sdr["SchoolID"] != DBNull.Value) emp.Add("SchoolID", (int)sdr["SchoolID"]);
                    if (sdr["SchoolName"] != DBNull.Value) emp.Add("SchoolName", (string)sdr["SchoolName"]);
                    
                    if (sdr["Rate"] != DBNull.Value) emp.Add("Rate", (double)sdr["Rate"]);
                    if (sdr["boynum"] != DBNull.Value) emp.Add("boynum", (int)sdr["boynum"]);
                    if (sdr["girlnum"] != DBNull.Value) emp.Add("girlnum", (int)sdr["girlnum"]);
                   
                     
                }
                conn.Close();
                cmd.CommandText = "select * from  member_Organazation where OrganazationID = " + orgid.ToString() + " and loginid = '" + loginid + "'";
                conn.Open();
                SqlDataReader sdrx = cmd.ExecuteReader();
                if (sdrx.Read() == false)
                {
                    emp.Add("gzstate", 0);
                }
                else
                {
                    if (sdrx["gzstate"] != DBNull.Value) emp.Add("gzstate", (int)sdrx["gzstate"]);

                }
                obj.Add(emp);
                emp1.Add("orgx_info", obj);
                 
                return emp1.ToString();
            }

            catch (System.Exception ee)
            {
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            //return "error";
        }
        public static string get_orgx_activity(int orgid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
             
            cmd.CommandText = "with actcnt as(select ActivityID,COUNT(distinct(loginid)) as cnt from member_activity group by(ActivityID))select * from Activity,actcnt where Activity.OrganazationID = " + orgid.ToString() + " and Activity.ActivityID = actcnt.ActivityID and Activity.Activitytime< getdate()";

            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();

                double sum = 0;
                int cnt = 0;
                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["ActivityID"] != DBNull.Value) emp.Add("ActivityID", (int)sdr["ActivityID"]);
                    if (sdr["ActivityName"] != DBNull.Value) emp.Add("ActivityName", (string)sdr["ActivityName"]);
                    if (sdr["GroupID"] != DBNull.Value) emp.Add("GroupID", (string)sdr["GroupID"]);
                    if (sdr["ActivityTime"] != DBNull.Value) emp.Add("ActivityTime", ((DateTime)sdr["ActivityTime"]).ToString());
                    if (sdr["PhotoDir"] != DBNull.Value) emp.Add("PhotoDir", (string)sdr["PhotoDir"]);
                    if (sdr["OriginalDir"] != DBNull.Value) emp.Add("OriginalDir", (string)sdr["OriginalDir"]);
                    if (sdr["Address"] != DBNull.Value) emp.Add("Address", (string)sdr["Address"]);
                    if (sdr["boynum"] != DBNull.Value) emp.Add("boynum", (int)sdr["boynum"]);
                    if (sdr["girlnum"] != DBNull.Value) emp.Add("girlnum", (int)sdr["girlnum"]);
                    if (sdr["rate"] != DBNull.Value) emp.Add("rate", (double)sdr["rate"]);
                    sum += (double)sdr["rate"];
                    cnt++;
                    if (sdr["cnt"] != DBNull.Value) emp.Add("cnt", (int)sdr["cnt"]);
                    obj.Add(emp);
                
                }
                
                emp1.Add("orgx_activity", obj);
                emp1.Add("act_avgrate", sum / cnt);
                
                return emp1.ToString();
            }


            catch (System.Exception ee)
            {
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            //return "error";
        }
        public static string get_orgx_nowact(string loginid,int orgid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select top 1 * from activity ,organazation ,school where organazationID = " + orgid + " and activitytime > getdate() and activity.OrganazationID = ORGANAZATION.OrganizationID and organazation.SchoolID = School.SchoolID order by activitytime desc";

            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();
                JObject emp = new JObject();
                int actid;
                if (sdr.Read())
                {
                    actid = (int)sdr["ActivityID"];
                    if (sdr["ActivityID"] != DBNull.Value) emp.Add("ActivityID", (int)sdr["ActivityID"]);
                    if (sdr["ActivityName"] != DBNull.Value) emp.Add("ActivityName", (string)sdr["ActivityName"]);
                    if (sdr["GroupID"] != DBNull.Value) emp.Add("GroupID", (string)sdr["GroupID"]);
                    if (sdr["OrganizationName"] != DBNull.Value) emp.Add("OrganizationName", (string)sdr["OrganizationName"]);
                    if (sdr["ActivirtyContent"] != DBNull.Value) emp.Add("ActivirtyContent", (string)sdr["ActivirtyContent"]);
                    if (sdr["PhotoDir"] != DBNull.Value) emp.Add("PhotoDir", (string)sdr["PhotoDir"]);
                    if (sdr["OriginalDir"] != DBNull.Value) emp.Add("OriginalDir", (string)sdr["OriginalDir"]);
                    if (sdr["logo"] != DBNull.Value) emp.Add("logo", (string)sdr["logo"]);
                    if (sdr["originaldirx"] != DBNull.Value) emp.Add("originaldirx", (string)sdr["originaldirx"]);
                    if (sdr["ActivityTime"] != DBNull.Value) emp.Add("ActivityTime", ((DateTime)sdr["ActivityTime"]).ToString());
                    if (sdr["Address"] != DBNull.Value) emp.Add("Address", (string)sdr["Address"]);
                    if (sdr["Rate"] != DBNull.Value) emp.Add("Rate", (double)sdr["Rate"]);
                    if (sdr["SchoolID"] != DBNull.Value) emp.Add("SchoolID", (int)sdr["SchoolID"]);
                    if (sdr["SchoolName"] != DBNull.Value) emp.Add("SchoolName", (string)sdr["SchoolName"]);
                    
                    if (sdr["girllim"] != DBNull.Value) emp.Add("girllim", (int)sdr["girllim"]);
                    if (sdr["boylim"] != DBNull.Value) emp.Add("boylim", (int)sdr["boylim"]);
                    if (sdr["boynum"] != DBNull.Value) emp.Add("boynum", (int)sdr["boynum"]);
                    if (sdr["girlnum"] != DBNull.Value) emp.Add("girlnum", (int)sdr["girlnum"]); 

                    conn.Close();
                    cmd.CommandText = "select * from member_activity where loginid='" + loginid + "' and ActivityID = " + actid.ToString();
                    conn.Open();
                    sdr = cmd.ExecuteReader();
                    if (sdr.Read())
                    {
                        if (sdr["cjState"] != DBNull.Value) emp.Add("cjState", (int)sdr["cjState"]);
                        else emp.Add("cjState", 0);
                    }
                    else
                    {


                        emp.Add("cjState", 0);

                    }
                    obj.Add(emp);
                    emp1.Add("nowact_info", obj);
                }
                return emp1.ToString();
            }


            catch (System.Exception ee)
            {
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        
        
        public static bool add_org_gz(string loginid, int orgid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from Member_Organazation where Organazationid = " + orgid + " and LoginID = '" + loginid + "'";
            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.Read() == false)
                {
                    conn.Close();
                    conn.Open();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "insert into Member_Organazation(Organazationid,LoginID,gzState,gzTime) values (" + orgid + ",'" + loginid + "',1,getdate())";
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    conn.Close();
                    conn.Open();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "update Member_Organazation set gzstate = 1 where LoginID='" + loginid + "' and Organazationid = " + orgid + "";
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (System.Exception ee)
            {
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public static bool add_sch_gz(string loginid, int schid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from Member_School where schoolid = " + schid + " and LoginID = '" + loginid + "'";
            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.Read() == false)
                {
                    conn.Close();
                    conn.Open();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "insert into Member_School(schoolid,LoginID,gzState,gztime) values (" + schid + ",'" + loginid + "',1,getdate())";
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    conn.Close();
                    conn.Open();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "update Member_School set gzstate = 1 where LoginID='" + loginid + "' and schoolid = " + schid + "";
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (System.Exception ee)
            {
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public static bool add_per_gz(string from, string to)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from guanzhu where (user1 = '" + from + "' and user2 = '" + to + "') or (user2 = '" + from + "' and user1 = '" + to + "')";
            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.Read() == false)
                {
                    conn.Close();
                    conn.Open();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "insert into guanzhu(user1,user2,state,ctime) values (" + from + "," + to + ",0,getdate())";
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    conn.Close();
                    conn.Open();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "update guanzhu set state = 0,ctime = getdate() where (user1='" + from + "' and user2 = '" + to + "') or (user2='" + from + "' and user1 = '" + to + "')";
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (System.Exception ee)
            {
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public static bool add_org_cj(string loginid, int orgid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from Member_Organazation where Organazationid = " + orgid + " and LoginID = '" + loginid + "'";
            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.Read() == false)
                {
                    conn.Close();
                    conn.Open();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "insert into Member_Organazation(Organazationid,LoginID,cjState) values (" + orgid + ",'" + loginid + "',1)";
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    conn.Close();
                    conn.Open();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "update Member_Organazation set cjstate = 1 where LoginID='" + loginid + "' and Organazationid = " + orgid + "";
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (System.Exception ee)
            {
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public static bool delete_org_gz(string loginid, int orgid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "update Member_Organazation set gzstate = 0 where LoginID='" + loginid + "' and Organazationid = " + orgid;
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }


            catch (System.Exception ee)
            {
                //return (ee.Message.ToString());
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public static bool delete_sch_gz(string loginid, int schid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "update Member_School set gzstate = 0 where LoginID='" + loginid + "' and schoolid = " + schid;
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }


            catch (System.Exception ee)
            {
                //return (ee.Message.ToString());
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public static bool delete_per_gz(string loginid, string mid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "update guanzhu set state = 2 where (user1 ='" + loginid + "' and user2 = '" + mid + "')" + "or (user2 = '" + loginid + "' and user1 = '" + mid + "')";
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }


            catch (System.Exception ee)
            {
                //return (ee.Message.ToString());
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public static bool confirm_per_gz(string from, string to,int b)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "update guanzhu set state = " + (b == 1?1:2) + " where user1='" + from + "' and user2 = " + to;
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }


            catch (System.Exception ee)
            {
                //return (ee.Message.ToString());
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public static bool delete_org_cj(string loginid, int orgid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "update Member_Organazation set cjstate = 0 where LoginID='" + loginid + "' and Organazationid = " + orgid + "";
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }


            catch (System.Exception ee)
            {
                //return (ee.Message.ToString());
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion
        #region 账户管理
        public static string get_my_org(string loginid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from Member_Organazation as m inner join Organazation as o On m.OrganazationID = o.OrganizationID inner join school on o.SchoolID = school.SchoolID where m.loginid = '"+ loginid +"' and m.gzState = 1";
            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();


                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["OrganizationID"] != DBNull.Value) emp.Add("OrganizationID", (int)sdr["OrganizationID"]);
                    if (sdr["GroupID"] != DBNull.Value) emp.Add("GroupID", (string)sdr["GroupID"]);
                    if (sdr["OrganizationName"] != DBNull.Value) emp.Add("OrganizationName", (string)sdr["OrganizationName"]);
                    if (sdr["Introduction"] != DBNull.Value) emp.Add("Introduction", (string)sdr["Introduction"]);
                    if (sdr["SchoolID"] != DBNull.Value) emp.Add("SchoolID", (int)sdr["SchoolID"]);
                    if (sdr["SchoolName"] != DBNull.Value) emp.Add("SchoolName", (string)sdr["SchoolName"]);

                    if (sdr["ClassID"] != DBNull.Value) emp.Add("ClassID", (int)sdr["ClassID"]);
                    if (sdr["Logo"] != DBNull.Value) emp.Add("Logo", (string)sdr["Logo"]);
                    if (sdr["originaldirx"] != DBNull.Value) emp.Add("originaldirx", (string)sdr["originaldirx"]);
                    if (sdr["Rate"] != DBNull.Value) emp.Add("Rate", (double)sdr["Rate"]);
                    int countnum = (int)sdr["girlnum"] + (int)sdr["boynum"];
                    emp.Add("Countnumber", countnum);
                    
                    obj.Add(emp);
                    //return emp.ToString();
                }
               
                emp1.Add("my_org", obj);
                //obj.ToString();
                return emp1.ToString();
            }


            catch (System.Exception ee)
            {
                writeLog("get_my_org", ee.Message.ToString());
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            //return "error";
        }
        public static string get_my_group(string loginid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from Member_Organazation as m inner join Organazation as o On m.OrganazationID = o.OrganizationID inner join school on o.SchoolID = school.SchoolID where m.loginid = '" + loginid + "' and m.gzState = 1";
            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();


                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["OrganizationID"] != DBNull.Value) emp.Add("OrganizationID", (int)sdr["OrganizationID"]);
                    if (sdr["GroupID"] != DBNull.Value) emp.Add("GroupID", (string)sdr["GroupID"]);
                    if (sdr["OrganizationName"] != DBNull.Value) emp.Add("OrganizationName", (string)sdr["OrganizationName"]);
                    if (sdr["Introduction"] != DBNull.Value) emp.Add("Introduction", (string)sdr["Introduction"]);
                    if (sdr["SchoolID"] != DBNull.Value) emp.Add("SchoolID", (int)sdr["SchoolID"]);
                    if (sdr["SchoolName"] != DBNull.Value) emp.Add("SchoolName", (string)sdr["SchoolName"]);
                    
                    if (sdr["ClassID"] != DBNull.Value) emp.Add("ClassID", (int)sdr["ClassID"]);
                    if (sdr["Logo"] != DBNull.Value) emp.Add("Logo", (string)sdr["Logo"]);
                    if (sdr["originaldirx"] != DBNull.Value) emp.Add("originaldirx", (string)sdr["originaldirx"]);
                    if (sdr["Rate"] != DBNull.Value) emp.Add("Rate", (double)sdr["Rate"]);
                    int countnum = (int)sdr["girlnum"] + (int)sdr["boynum"];
                    emp.Add("Countnumber", countnum);

                    obj.Add(emp);
                    //return emp.ToString();
                }
                conn.Close();
                cmd.CommandText = "select * from Member_activity as m inner join activity as o On m.activityID = o.activityID inner join Organazation on o.OrganazationID = Organazation.OrganizationID inner join school on organazation.SchoolID = School.SchoolID where m.loginid ='" + loginid + "' and m.cjstate=1 order by ActivityTime DESC";
                conn.Open();
                sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["ActivityID"] != DBNull.Value) emp.Add("ActivityID", (int)sdr["ActivityID"]);
                    if (sdr["OrganazationID"] != DBNull.Value) emp.Add("OrganazationID", (int)sdr["OrganazationID"]);
                    if (sdr["ActivityName"] != DBNull.Value) emp.Add("ActivityName", (string)sdr["ActivityName"]);
                    if (sdr["Taga"] != DBNull.Value) emp.Add("Taga", (string)sdr["Taga"]);
                    if (sdr["Tagb"] != DBNull.Value) emp.Add("Tagb", (string)sdr["Tagb"]);
                    if (sdr["Tagc"] != DBNull.Value) emp.Add("Tagc", (string)sdr["Tagc"]);
                    if (sdr["GroupID"] != DBNull.Value) emp.Add("GroupID", (string)sdr["GroupID"]);
                    if (sdr["ActivirtyContent"] != DBNull.Value) emp.Add("ActivityContent", (string)sdr["ActivirtyContent"]);
                    if (sdr["OrganizationName"] != DBNull.Value) emp.Add("OrganizationName", (string)sdr["OrganizationName"]);
                    if (sdr["Address"] != DBNull.Value) emp.Add("Address", (string)sdr["Address"]);
                    if (sdr["PhotoDir"] != DBNull.Value) emp.Add("PhotoDir", (string)sdr["PhotoDir"]);
                    if (sdr["OriginalDir"] != DBNull.Value) emp.Add("OriginalDir", (string)sdr["OriginalDir"]);
                    if (sdr["logo"] != DBNull.Value) emp.Add("logo", (string)sdr["logo"]);
                    if (sdr["originaldirx"] != DBNull.Value) emp.Add("originaldirx", (string)sdr["originaldirx"]);
                    if (sdr["Rate"] != DBNull.Value) emp.Add("Rate", (double)sdr["Rate"]);
                    if (sdr["SchoolID"] != DBNull.Value) emp.Add("SchoolID", (int)sdr["SchoolID"]);
                    if (sdr["SchoolName"] != DBNull.Value) emp.Add("SchoolName", (string)sdr["SchoolName"]);
                    
                    if (sdr["girllim"] != DBNull.Value) emp.Add("girllim", (int)sdr["girllim"]);
                    if (sdr["boylim"] != DBNull.Value) emp.Add("boylim", (int)sdr["boylim"]);
                    if (sdr["boynum"] != DBNull.Value) emp.Add("boynum", (int)sdr["boynum"]);
                    if (sdr["girlnum"] != DBNull.Value) emp.Add("girlnum", (int)sdr["girlnum"]);
                    if (sdr["ActivityTime"] != DBNull.Value) emp.Add("ActivityTime", ((DateTime)sdr["ActivityTime"]).ToString());

                    if (sdr["cjstate"] != DBNull.Value) emp.Add("cjstate", (int)sdr["cjstate"]);

                    obj.Add(emp);

                }
                emp1.Add("my_group", obj);
                //obj.ToString();
                return emp1.ToString();
            }


            catch (System.Exception ee)
            {
                writeLog("get_my_group", ee.Message.ToString());
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            //return "error";
        }
        public static string get_my_activity(string logid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from Member_activity as m inner join activity as o On m.activityID = o.activityID inner join Organazation on o.OrganazationID = Organazation.OrganizationID inner join school on organazation.SchoolID = School.SchoolID where m.loginid ='" + logid + "' and m.cjstate=1 order by ActivityTime DESC";

            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();


                while (sdr.Read())
                {
                    JObject emp = new JObject();                  
                    if (sdr["ActivityID"] != DBNull.Value) emp.Add("ActivityID", (int)sdr["ActivityID"]);
                    if (sdr["OrganazationID"] != DBNull.Value) emp.Add("OrganazationID", (int)sdr["OrganazationID"]);
                    if (sdr["ActivityName"] != DBNull.Value) emp.Add("ActivityName", (string)sdr["ActivityName"]);
                    if (sdr["GroupID"] != DBNull.Value) emp.Add("GroupID", (string)sdr["GroupID"]);
                    if (sdr["Taga"] != DBNull.Value) emp.Add("Taga", (string)sdr["Taga"]);
                    if (sdr["Tagb"] != DBNull.Value) emp.Add("Tagb", (string)sdr["Tagb"]);
                    if (sdr["Tagc"] != DBNull.Value) emp.Add("Tagc", (string)sdr["Tagc"]);
                    if (sdr["ActivirtyContent"] != DBNull.Value) emp.Add("ActivityContent", (string)sdr["ActivirtyContent"]);
                    if (sdr["OrganizationName"] != DBNull.Value) emp.Add("OrganizationName", (string)sdr["OrganizationName"]);
                    if (sdr["Address"] != DBNull.Value) emp.Add("Address", (string)sdr["Address"]);
                    if (sdr["PhotoDir"] != DBNull.Value) emp.Add("PhotoDir", (string)sdr["PhotoDir"]);
                    if (sdr["OriginalDir"] != DBNull.Value) emp.Add("OriginalDir", (string)sdr["OriginalDir"]);
                    if (sdr["logo"] != DBNull.Value) emp.Add("logo", (string)sdr["logo"]);
                    if (sdr["originaldirx"] != DBNull.Value) emp.Add("originaldirx", (string)sdr["originaldirx"]);
                    if (sdr["Rate"] != DBNull.Value) emp.Add("Rate", (double)sdr["Rate"]);
                    if (sdr["SchoolID"] != DBNull.Value) emp.Add("SchoolID", (int)sdr["SchoolID"]);
                    if (sdr["SchoolName"] != DBNull.Value) emp.Add("SchoolName", (string)sdr["SchoolName"]);
                    

                    if (sdr["girllim"] != DBNull.Value) emp.Add("girllim", (int)sdr["girllim"]);
                    if (sdr["boylim"] != DBNull.Value) emp.Add("boylim", (int)sdr["boylim"]);
                    if (sdr["boynum"] != DBNull.Value) emp.Add("boynum", (int)sdr["boynum"]);
                    if (sdr["girlnum"] != DBNull.Value) emp.Add("girlnum", (int)sdr["girlnum"]);
                    if (sdr["ActivityTime"] != DBNull.Value) emp.Add("ActivityTime", ((DateTime)sdr["ActivityTime"]).ToString());
                    
                    if (sdr["cjstate"] != DBNull.Value) emp.Add("cjstate", (int)sdr["cjstate"]);
                   
                    obj.Add(emp);
                    
                }
               
                emp1.Add("my_activity", obj);
                //obj.ToString();
                return emp1.ToString();
            }


            catch (System.Exception ee)
            {
                writeLog("get_my_activity", ee.Message.ToString());
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            //return "error";
        }
        public static string get_my_tucao(string loginid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select  * from tucao left join member on tucao.loginid=member.loginid where  tucao.loginid = '" + loginid + "' order by tucao.tucaotime desc";

            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();
                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    emp.Add("tucao_id", (int)sdr["TucaoID"]);
                    emp.Add("nickname", (string)sdr["nickname"]);
                    if (sdr["introduce"] != DBNull.Value) emp.Add("introduce", (string)sdr["introduce"]);
                    emp.Add("tucao_time", ((DateTime)sdr["TucaoTime"]).ToString());
                    emp.Add("content", (string)sdr["TucaoContent"]);
                    emp.Add("zan_cnt", (int)sdr["CountZan"]);
                    emp.Add("Tucaocnt", (int)sdr["Tucaocnt"]);                  
                    obj.Add(emp);
                }
                
                emp1.Add("my_tucao", obj);
                //obj.ToString();
                return emp1.ToString().ToString();
            }


            catch (System.Exception ee)
            {
                writeLog("get_my_tucao", ee.Message.ToString());
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            //return "error";
        }
        public static string get_my_ershou(string loginid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from ershou where loginid = '" + loginid + "'";

            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();


                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["ErshouID"] != DBNull.Value) emp.Add("ErshouID", (int)sdr["ErshouID"]);
                    if (sdr["LoginID"] != DBNull.Value) emp.Add("LoginID", (string)sdr["LoginID"]);
                    if (sdr["Description"] != DBNull.Value) emp.Add("Description", ((string)sdr["Description"]));
                    if (sdr["OriginPrice"] != DBNull.Value) emp.Add("OriginPrice", (int)sdr["OriginPrice"]);
                    if (sdr["Price"] != DBNull.Value) emp.Add("Price", (int)sdr["Price"]);
                    if (sdr["PublishTime"] != DBNull.Value) emp.Add("PublishTime", ((DateTime)sdr["PublishTime"]).ToString());
                    if (sdr["Photo"] != DBNull.Value) emp.Add("Photo", (string)sdr["Photo"]);
                    if (sdr["Phone"] != DBNull.Value) emp.Add("Phone", (string)sdr["Phone"]);
                    if (sdr["ErshouName"] != DBNull.Value) emp.Add("ErshouName", (string)sdr["ErshouName"]);
                    obj.Add(emp);
                    //return emp.ToString();
                }

                emp1.Add("my_org", obj);
                //obj.ToString();
                return emp1.ToString();
            }


            catch (System.Exception ee)
            {
                writeLog("get_my_ershou", ee.Message.ToString());
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            //return "error";
        }
        public static string get_my_guanzhuinfo(string loginid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from guanzhu ,member where guanzhu.user2 = " + loginid + " and guanzhu.state = 0 and guanzhu.user1 = member.loginid";

            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();


                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["loginid"] != DBNull.Value) emp.Add("loginid", (string)sdr["loginid"]);
                    if (sdr["Sex"] != DBNull.Value) emp.Add("Sex", (int)sdr["Sex"]);
                    if (sdr["nickname"] != DBNull.Value) emp.Add("nickname", (string)sdr["nickname"]);
                    if (sdr["introduce"] != DBNull.Value) emp.Add("introduce", (string)sdr["introduce"]);
                    if (sdr["photo"] != DBNull.Value) emp.Add("photo", (string)sdr["photo"]);
                    else emp.Add("photo", "tidai.jpg");
                    obj.Add(emp);
                    //return emp.ToString();
                }

                emp1.Add("guanzhuinfo", obj);
                //obj.ToString();
                return emp1.ToString();
            }


            catch (System.Exception ee)
            {
                writeLog("get_my_guanzhuinfo", ee.Message.ToString());
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            //return "error";
        }
        public static bool  change_passwd(string loginid, string passwd)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Member SET loginpwd = '" + passwd + "' where loginid = '" + loginid + "'";



            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }


            catch (System.Exception ee)
            {
                writeLog("change_passwd", ee.Message.ToString());
                //return (ee.Message.ToString());
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        public static string deletetest()
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "delete from guanzhu where user1='18818272795' or user2 = '18818272795';delete from Member_Organazation where loginid = '18818272795';delete from Member_Activity where loginid = '18818272795';delete from Member_School where loginid = '18818272795';delete from MemberInfo where LoginID = '18818272795';delete from member where loginid = '18818272795'";

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return "true";
            }


            catch (System.Exception ee)
            {
                writeLog("change_passwd", ee.Message.ToString());
                //return (ee.Message.ToString());
                return ee.Message.ToString();
            }
            finally
            {
                conn.Close();
            }
        }


        public static bool change_introduce(string loginid, string introduce)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Member SET introduce = '" + introduce + "' where loginid = '" + loginid + "'";



            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }


            catch (System.Exception ee)
            {
                writeLog("change_introduce", ee.Message.ToString());
                //return (ee.Message.ToString());
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public static bool change_nickname(string loginid, string nickname)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Member SET nickname = '" + nickname + "' where loginid = '" + loginid + "'";



            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }


            catch (System.Exception ee)
            {
                writeLog("change_nickname", ee.Message.ToString());
                //return (ee.Message.ToString());
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public static bool upload_photo(string loginid,string picname)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Member SET photo = '" + picname + "' where loginid = '" + loginid + "'";



            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }


            catch (System.Exception ee)
            {
                writeLog("upload_photo", ee.Message.ToString());
                //return (ee.Message.ToString());
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public static string getacount(string loginid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from member where loginid = '" + loginid + "'";
            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();
                while (sdr.Read())
                {
                   
                    JObject emp = new JObject();
                    emp.Add("LoginID", (string)sdr["LoginID"]);
                    emp.Add("nickname", (string)sdr["nickname"]);
                    if (sdr["introduce"] != DBNull.Value) emp.Add("introduce", (string)sdr["introduce"]);
                    emp.Add("LoginPWD", ((string)sdr["LoginPWD"]).ToString());
                    emp.Add("SchoolID", (int)sdr["SchoolID"]);
                    emp.Add("Sex", (int)sdr["Sex"]);
                   
                    obj.Add(emp);
                }

                emp1.Add("my_acount", obj);
                //obj.ToString();
                return emp1.ToString().ToString();
            }


            catch (System.Exception ee)
            {
                writeLog("getacount", ee.Message.ToString());
                return (ee.Message.ToString());
              
            }
            finally
            {
                conn.Close();
            }
        }  
        public static bool  change_school(string loginid, int schid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Member SET schoolid = '" + schid + "' where loginid = '" + loginid + "'";
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }


            catch (System.Exception ee)
            {
                writeLog("change_school", ee.Message.ToString());
                //return (ee.Message.ToString());
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public static string  login(string loginid, string passwd,string ipadr)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from member where loginid = '" + loginid + "' and loginpwd = '" + passwd + "'";
            
            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();
                if (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["SchoolID"] != DBNull.Value) emp.Add("SchoolID", (int)sdr["SchoolID"]);
                    else emp.Add("SchoolID", 7);
                    if (sdr["Sex"] != DBNull.Value) emp.Add("Sex", (int)sdr["Sex"]);
                    if (sdr["nickname"] != DBNull.Value) emp.Add("nickname", (string)sdr["nickname"]);
                    if (sdr["introduce"] != DBNull.Value) emp.Add("introduce", (string)sdr["introduce"]);
                    else emp.Add("introduce", "赶快去添加个性签名吧");
                    if (sdr["photo"] != DBNull.Value) emp.Add("photo", (string)sdr["photo"]);
                    else emp.Add("photo", "tidai.jpg");
                    if (sdr["regtime"] != DBNull.Value)
                    {
                        emp.Add("regtime", ((DateTime)sdr["regtime"]).ToShortDateString());
                        if (DateTime.Compare((DateTime)sdr["regtime"], DateTime.Parse("2015-04-15")) < 0) emp.Add("needsch", 1);
                        else emp.Add("needsch", 0);
                    }
                    else
                    {
                        emp.Add("regtime", "2014-7-1");
                        emp.Add("needsch", 1);
                    }
                    conn.Close();
                    cmd.CommandText = "insert into memberinfo (loginid,logintime,loginip) values ('" + loginid + "','" + DateTime.Now.ToString() + "','" + ipadr + "' )";
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    obj.Add(emp);

                    emp1.Add("memberinfo", obj);
                    //obj.ToString();
                    conn.Close();
                    cmd.CommandText = "select COUNT(1) as cnt from Member_Activity where LoginID = '" + loginid + "' and cjState = 1";
                    conn.Open();
                    sdr = cmd.ExecuteReader();
                    if (sdr.Read()) emp1.Add("actnum", (int)sdr["cnt"]);
                    else emp1.Add("actnum", 0);
                    conn.Close();
                    cmd.CommandText = "select COUNT(1) as cnt from Member_Organazation where LoginID = '" + loginid + "' and gzState = 1";
                    conn.Open();
                    sdr = cmd.ExecuteReader();
                    if (sdr.Read()) emp1.Add("orgnum", (int)sdr["cnt"]);
                    else emp1.Add("orgnum", 0);
                    conn.Close();
                    cmd.CommandText = "select COUNT(1) as cnt from guanzhu where (user1= '" + loginid + "' or user2 = '" + loginid + "') and state = 1";
                    conn.Open();
                    sdr = cmd.ExecuteReader();
                    if (sdr.Read()) emp1.Add("memnum", (int)sdr["cnt"]);
                    else emp1.Add("memnum", 0);
                    conn.Close();
                    cmd.CommandText = "select * from member_school where loginid = '" + loginid + "' and gzstate = 1";
                    conn.Open();
                     sdr = cmd.ExecuteReader();
                    if (sdr.Read()) 
                        emp1.Add("schstate", 1);
                    else emp1.Add("schstate", 0);
                    return emp1.ToString();
                }
                else
                    return "false";
            }


            catch (System.Exception ee)
            {
                writeLog("login", ee.Message.ToString());
                return (ee.Message.ToString());
               
            }
            finally
            {
                conn.Close();
            }
        }

        public static string login_md5(string loginid, string passwd, string ipadr)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from member where loginid = '" + loginid + "' and md5pwd = '" + passwd + "'";

            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();
                if (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["SchoolID"] != DBNull.Value) emp.Add("SchoolID", (int)sdr["SchoolID"]);
                    else emp.Add("SchoolID", 7);
                    if (sdr["Sex"] != DBNull.Value) emp.Add("Sex", (int)sdr["Sex"]);
                    if (sdr["nickname"] != DBNull.Value) emp.Add("nickname", (string)sdr["nickname"]);
                    if (sdr["introduce"] != DBNull.Value) emp.Add("introduce", (string)sdr["introduce"]);
                    else emp.Add("introduce", "赶快去添加个性签名吧");
                    if (sdr["photo"] != DBNull.Value) emp.Add("photo", (string)sdr["photo"]);
                    else emp.Add("photo", "tidai.jpg");
                    if (sdr["regtime"] != DBNull.Value)
                    {
                        emp.Add("regtime", ((DateTime)sdr["regtime"]).ToShortDateString());
                        if (DateTime.Compare((DateTime)sdr["regtime"], DateTime.Parse("2015-04-15")) < 0) emp.Add("needsch", 1);
                        else emp.Add("needsch", 0);
                    }
                    else
                    {
                        emp.Add("regtime", "2014-7-1");
                        emp.Add("needsch", 1);
                    }
                    conn.Close();
                    cmd.CommandText = "insert into memberinfo (loginid,logintime,loginip) values ('" + loginid + "','" + DateTime.Now.ToString() + "','" + ipadr + "' )";
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    obj.Add(emp);

                    emp1.Add("memberinfo", obj);
                    //obj.ToString();
                    conn.Close();
                    cmd.CommandText = "select COUNT(1) as cnt from Member_Activity where LoginID = '" + loginid + "' and cjState = 1";
                    conn.Open();
                    sdr = cmd.ExecuteReader();
                    if (sdr.Read()) emp1.Add("actnum", (int)sdr["cnt"]);
                    else emp1.Add("actnum", 0);
                    conn.Close();
                    cmd.CommandText = "select COUNT(1) as cnt from Member_Organazation where LoginID = '" + loginid + "' and gzState = 1";
                    conn.Open();
                    sdr = cmd.ExecuteReader();
                    if (sdr.Read()) emp1.Add("orgnum", (int)sdr["cnt"]);
                    else emp1.Add("orgnum", 0);
                    conn.Close();
                    cmd.CommandText = "select COUNT(1) as cnt from guanzhu where (user1= '" + loginid + "' or user2 = '" + loginid + "') and state = 1";
                    conn.Open();
                    sdr = cmd.ExecuteReader();
                    if (sdr.Read()) emp1.Add("memnum", (int)sdr["cnt"]);
                    else emp1.Add("memnum", 0);
                    conn.Close();
                    cmd.CommandText = "select * from member_school where loginid = '" + loginid + "' and gzstate = 1";
                    conn.Open();
                    sdr = cmd.ExecuteReader();
                    if (sdr.Read())
                        emp1.Add("schstate", 1);
                    else emp1.Add("schstate", 0);
                    return emp1.ToString();
                }
                else
                    return "false";
            }


            catch (System.Exception ee)
            {
                writeLog("login", ee.Message.ToString());
                return (ee.Message.ToString());

            }
            finally
            {
                conn.Close();
            }
        }

        public static string login_md5_leanid(string loginid, string passwd, string ipadr,string leanid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from member where loginid = '" + loginid + "' and md5pwd = '" + passwd + "'";

            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();
                if (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["SchoolID"] != DBNull.Value) emp.Add("SchoolID", (int)sdr["SchoolID"]);
                    else emp.Add("SchoolID", 7);
                    if (sdr["Sex"] != DBNull.Value) emp.Add("Sex", (int)sdr["Sex"]);
                    if (sdr["nickname"] != DBNull.Value) emp.Add("nickname", (string)sdr["nickname"]);
                    if (sdr["introduce"] != DBNull.Value) emp.Add("introduce", (string)sdr["introduce"]);
                    else emp.Add("introduce", "赶快去添加个性签名吧");
                    if (sdr["photo"] != DBNull.Value) emp.Add("photo", (string)sdr["photo"]);
                    else emp.Add("photo", "tidai.jpg");
                    if (sdr["regtime"] != DBNull.Value)
                    {
                        emp.Add("regtime", ((DateTime)sdr["regtime"]).ToShortDateString());
                        if (DateTime.Compare((DateTime)sdr["regtime"], DateTime.Parse("2015-04-15")) < 0) emp.Add("needsch", 1);
                        else emp.Add("needsch", 0);
                    }
                    else
                    {
                        emp.Add("regtime", "2014-7-1");
                        emp.Add("needsch", 1);
                    }
                    conn.Close();
                    cmd.CommandText = "insert into memberinfo (loginid,logintime,loginip) values ('" + loginid + "','" + DateTime.Now.ToString() + "','" + ipadr + "' )";
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    conn.Close();
                    cmd.CommandText = "update member set leancloudid = '" + leanid + "' where loginid = '" + loginid + "'";
                    conn.Open();
                    cmd.ExecuteNonQuery();


                    obj.Add(emp);

                    emp1.Add("memberinfo", obj);
                    //obj.ToString();
                    conn.Close();
                    cmd.CommandText = "select COUNT(1) as cnt from Member_Activity where LoginID = '" + loginid + "' and cjState = 1";
                    conn.Open();
                    sdr = cmd.ExecuteReader();
                    if (sdr.Read()) emp1.Add("actnum", (int)sdr["cnt"]);
                    else emp1.Add("actnum", 0);
                    conn.Close();
                    cmd.CommandText = "select COUNT(1) as cnt from Member_Organazation where LoginID = '" + loginid + "' and gzState = 1";
                    conn.Open();
                    sdr = cmd.ExecuteReader();
                    if (sdr.Read()) emp1.Add("orgnum", (int)sdr["cnt"]);
                    else emp1.Add("orgnum", 0);
                    conn.Close();
                    cmd.CommandText = "select COUNT(1) as cnt from guanzhu where (user1= '" + loginid + "' or user2 = '" + loginid + "') and state = 1";
                    conn.Open();
                    sdr = cmd.ExecuteReader();
                    if (sdr.Read()) emp1.Add("memnum", (int)sdr["cnt"]);
                    else emp1.Add("memnum", 0);
                    conn.Close();
                    cmd.CommandText = "select * from member_school where loginid = '" + loginid + "' and gzstate = 1";
                    conn.Open();
                    sdr = cmd.ExecuteReader();
                    if (sdr.Read())
                        emp1.Add("schstate", 1);
                    else emp1.Add("schstate", 0);
                    return emp1.ToString();
                }
                else
                    return "false";
            }


            catch (System.Exception ee)
            {
                writeLog("login", ee.Message.ToString());
                return (ee.Message.ToString());

            }
            finally
            {
                conn.Close();
            }
        }
        public static string tg_count(string tgid, string ck, string psys, string agent)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from Tuiguang_count where Tg_Cookie = '" + ck + "'and Tg_ID = "+tgid;
            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                if (!sdr.Read())
                {
                    conn.Close();
                    cmd.CommandText = "insert into Tuiguang_count(Tg_ID,Tg_Cookie,Tg_Sys,Tg_Agent,Tg_Time) values('" + tgid + "','" + ck + "','" + psys + "','" + agent + "','" + DateTime.Now.ToString() + "')";
                    conn.Open();
                    sdr = cmd.ExecuteReader();
                    return "true";
                }
                return "already";
            }
            catch (System.Exception ee)
            {
                writeLog("tg_count", ee.Message.ToString());
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        public static string tg_calc(string ck, int schoolid, int qd)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from Tuiguang_Calc where Cookie = '" + ck + "'and SchoolID = " + schoolid + " and Qudao = " + qd;
            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                if (!sdr.Read())
                {
                    conn.Close();
                    cmd.CommandText = "insert into Tuiguang_Calc(Cookie,SchoolID,Qudao,Time) values('" + ck + "'," + schoolid + "," + qd + ",'" + DateTime.Now.ToString() + "')";
                    conn.Open();
                    sdr = cmd.ExecuteReader();
                    return "true";
                }
                return "already";
            }
            catch (System.Exception ee)
            {
                writeLog("tg_calc", ee.Message.ToString());
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        public static string tg_calcdl(string ck, int schoolid, int qd, string client)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from Tuiguang_Calc where Cookie = '" + ck + "'and SchoolID = " + schoolid + " and Qudao = " + qd;
            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.Read())
                {
                    conn.Close();
                    cmd.CommandText = "UPDATE Tuiguang_Calc SET Download = 1,CSys = '" + client + "', Dl_Time='" + DateTime.Now.ToString() + "' Where Cookie = '" + ck + "'and SchoolID = " + schoolid + " and Qudao = " + qd;
                    conn.Open();
                    sdr = cmd.ExecuteReader();
                    return "true";
                }
                return "none";
            }
            catch (System.Exception ee)
            {
                writeLog("tg_calcdl", ee.Message.ToString());
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        public static string wxlogin(string openid, string ipadr)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from member where loginid in (select loginid from wxuser where openid = '" + openid + "')";

            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();
                string loginid = "";
                if (sdr.Read())
                {
                    JObject emp = new JObject();
                    loginid = (string)sdr["loginid"];
                    if (sdr["SchoolID"] != DBNull.Value) emp.Add("SchoolID", (int)sdr["SchoolID"]);
                    if (sdr["Sex"] != DBNull.Value) emp.Add("Sex", (int)sdr["Sex"]);
                    if (sdr["nickname"] != DBNull.Value) emp.Add("nickname", (string)sdr["nickname"]);
                    if (sdr["introduce"] != DBNull.Value) emp.Add("introduce", (string)sdr["introduce"]);
                    else emp.Add("introduce", "赶快去添加个性签名吧");
                    if (sdr["photo"] != DBNull.Value) emp.Add("photo", (string)sdr["photo"]);
                    else emp.Add("photo", "tidai.jpg");
                    conn.Close();
                    cmd.CommandText = "insert into memberinfo (loginid,logintime,loginip) values ('" + loginid + "','" + DateTime.Now.ToString() + "','" + ipadr + "' )";
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    obj.Add(emp);

                    emp1.Add("memberinfo", obj);
                    //obj.ToString();
                    conn.Close();
                    return emp1.ToString();
                }
                else
                    return "false";
            }


            catch (System.Exception ee)
            {
                writeLog("login", ee.Message.ToString());
                return (ee.Message.ToString());

            }
            finally
            {
                conn.Close();
            }
        }       

        public static bool  reg(string name, string passwd,string phone,int schid,int sex)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            Random random = new Random();
            int n = random.Next(1, 5);
            cmd.CommandText = "insert into member(loginid,loginpwd,nickname,photo,schoolid,sex,regtime) values('" + name + "','" + passwd + "','" + phone + "','tidai"+ n.ToString() + ".jpg'," + schid.ToString() + "," + sex + ",'" + DateTime.Now.ToString() + "')";


            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                add_sch_gz(name, schid);
                return true;
            }


            catch (System.Exception ee)
            {
                writeLog("reg", ee.Message.ToString());
                //return (ee.Message.ToString());
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        public static bool reg_md5(string name, string passwd, string phone, int schid, int sex)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            Random random = new Random();
            int n = random.Next(1, 5);
            cmd.CommandText = "insert into member(loginid,loginpwd,md5pwd,nickname,photo,schoolid,sex,regtime) values('" + name + "','" + passwd + "','" + passwd + "','" + phone + "','tidai" + n.ToString() + ".jpg'," + schid.ToString() + "," + sex + ",'" + DateTime.Now.ToString() + "')";


            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                add_sch_gz(name, schid);
                return true;
            }


            catch (System.Exception ee)
            {
                writeLog("reg", ee.Message.ToString());
                //return (ee.Message.ToString());
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public static bool reg_md5_src(string name, string passwd, string phone, int schid, int sex,string src)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            Random random = new Random();
            int n = random.Next(1, 5);
            cmd.CommandText = "insert into member(loginid,loginpwd,md5pwd,nickname,photo,schoolid,sex,regtime,src) values('" + name + "','" + passwd + "','" + passwd + "','" + phone + "','tidai" + n.ToString() + ".jpg'," + schid.ToString() + "," + sex + ",'" + DateTime.Now.ToString() + "','" + src + "')";


            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                add_sch_gz(name, schid);
                return true;
            }


            catch (System.Exception ee)
            {
                writeLog("reg", ee.Message.ToString());
                //return (ee.Message.ToString());
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public static bool reg_md5_device(string name, string passwd, string phone, int schid, int sex, string deviceversion, string operationsystem, string deviceNumber)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            Random random = new Random();
            int n = random.Next(1, 5);
            cmd.CommandText = "insert into member(loginid,loginpwd,md5pwd,nickname,photo,schoolid,sex,regtime,deviceversion,operationsystem,deviceNumber) values('" + name + "','" + passwd + "','" + passwd + "','" + phone + "','tidai" + n.ToString() + ".jpg'," + schid.ToString() + "," + sex + ",'" + DateTime.Now.ToString() + "','" + deviceversion + "','" + operationsystem + "','" + deviceNumber + "')";


            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                add_sch_gz(name, schid);
                return true;
            }


            catch (System.Exception ee)
            {
                writeLog("reg", ee.Message.ToString());
                //return (ee.Message.ToString());
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public static bool bind_wx(string openid,string loginid,string passwd)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from member where loginid = '" + loginid + "' and passwd = '" + passwd + "'";
 
            //cmd.CommandText = "insert into WxUser(OpenID,LoginID) values('" + openid + "','" + loginid + "')";


            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.Read())
                {
                    conn.Close();
                    cmd.CommandText = "insert into WxUser(OpenID,LoginID) values('" + openid + "','" + loginid + "')";
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
                return false;
            }


            catch (System.Exception ee)
            {
                writeLog("reg", ee.Message.ToString());
                //return (ee.Message.ToString());
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        public static bool  reg_already(string name)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from member where loginid = '" + name + "'";


            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.Read()) return true;
                return false;
            }


            catch (System.Exception ee)
            {
                writeLog("reg_already", ee.Message.ToString());
                //return (ee.Message.ToString());
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public static string get_schoollist()
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select Schoolid,SchoolName from School where schoolid < 67";

            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();


                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["Schoolid"] != DBNull.Value) emp.Add("Schoolid", (int)sdr["Schoolid"]);
                    if (sdr["SchoolName"] != DBNull.Value) emp.Add("SchoolName", (string)sdr["SchoolName"]);
                    
                    
                    obj.Add(emp);
                    //return emp.ToString();
                }

                emp1.Add("schoollist", obj);
                //obj.ToString();
                return emp1.ToString();
            }


            catch (System.Exception ee)
            {
                writeLog("get_schoollist", ee.Message.ToString());
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            //return "error";
        }
        public static string get_newschoollist(string loginid,string city)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();

            //city = Encoding.UTF8.GetString(Encoding.ASCII.GetBytes(city.ToCharArray()));
            cmd.CommandText = "with A as (select * from Member_School where loginid='" + loginid + "') select * from School left outer join A on school.SchoolID = A.SchoolID where school.City like '%" + city + "%'";

            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();


                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["Schoolid"] != DBNull.Value) emp.Add("Schoolid", (int)sdr["Schoolid"]);
                    if (sdr["SchoolName"] != DBNull.Value) emp.Add("SchoolName", (string)sdr["SchoolName"]);
                    if (sdr["gzstate"] != DBNull.Value) emp.Add("gzstate", (int)sdr["gzstate"]);
                    else emp.Add("gzstate", 0);

                    obj.Add(emp);
                    //return emp.ToString();
                }

                emp1.Add("schoollist", obj);
                //obj.ToString();
                return emp1.ToString();
            }


            catch (System.Exception ee)
            {
                writeLog("get_newschoollist", ee.Message.ToString());
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            //return "error";
        }
        public static string get_schoollistx(string loginid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "with A AS (select * from member_school where loginID=" + loginid + ")select  * from school left outer join A on A.schoolID = School.SchoolID";

            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();


                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["Schoolid"] != DBNull.Value) emp.Add("Schoolid", (int)sdr["Schoolid"]);
                    if (sdr["SchoolName"] != DBNull.Value) emp.Add("SchoolName", (string)sdr["SchoolName"]);
                    

                    if (sdr["gzstate"] != DBNull.Value) emp.Add("gzstate", (int)sdr["gzstate"]);
                    else emp.Add("gzstate", 0);
                    obj.Add(emp);
                    //return emp.ToString();
                }

                emp1.Add("schoollist", obj);
                //obj.ToString();
                return emp1.ToString();
            }


            catch (System.Exception ee)
            {
                writeLog("get_schoollistx", ee.Message.ToString());
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            //return "error";
        }
        public static string get_school(string loginid)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from member_school where loginid = '" + loginid + "' and gzstate = 1";

            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                string result = "";


                while (sdr.Read())
                {
                    result += (int)sdr["schoolid"] + ",";
                }

                
                return result;
            }


            catch (System.Exception ee)
            {
                writeLog("get_schoollistx", ee.Message.ToString());
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            //return "error";
        }
        public static string get_version()
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select top 1* from versionupdate order by version desc";

            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();
                while (sdr.Read())
                {
                  
                    JObject emp = new JObject();
                    if (sdr["Version"] != DBNull.Value) emp.Add("Version", (string)sdr["Version"]);
                    if (sdr["Address"] != DBNull.Value) emp.Add("Address", (string)sdr["Address"]);
                    if (sdr["Time"] != DBNull.Value) emp.Add("Time", ((DateTime)sdr["Time"]).ToString());
                    if (sdr["Description"] != DBNull.Value) emp.Add("Description", (string)sdr["Description"]);
                    
                    obj.Add(emp);
                    
                }

                emp1.Add("version", obj);   
            
                return emp1.ToString();
            }


            catch (System.Exception ee)
            {
                writeLog("get_version", ee.Message.ToString());
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            //return "error";
        }

        public static bool add_fankui(string loginid, string content)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "insert into fankui (loginid,contents,fktime) values ('" + loginid + "' ,  '" + content + "', '" + DateTime.Now.ToString() + "')";



            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }


            catch (System.Exception ee)
            {
                writeLog("add_fankui", ee.Message.ToString());
                //return (ee.Message.ToString());
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public static string get_hcptop()
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select top 5 count(loginid) as cnt,num,month,day from hcp group by num,month,day order by cnt desc";
            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();
                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["num"] != DBNull.Value) emp.Add("num", (string)sdr["num"]);
                    if (sdr["month"] != DBNull.Value) emp.Add("month", (int)sdr["month"]);
                    if (sdr["day"] != DBNull.Value) emp.Add("day", (int)sdr["day"]);
                    if (sdr["cnt"] != DBNull.Value) emp.Add("count", (int)sdr["cnt"]);
                    obj.Add(emp);
                }
                emp1.Add("hcptop", obj);
                return emp1.ToString();
            }
            catch (System.Exception ee)
            {
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        public static bool has_hcp(string LoginID, int sex, string num, int month, int day)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from hcp where loginid = '" + LoginID + "'";
            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.Read()) return true;
                return false;
            }
            catch (System.Exception ee)
            {
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public static string insert_hcp(string LoginID, int sex, string num, int month, int day)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "insert into hcp(LoginID, sex,  num, month, day) values ('" + LoginID + "'," + sex + ",'" + num + "'," + month + "," + day + ")";
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                cmd.CommandText = "select * from hcp left outer join Member on Hcp.LoginID=Member.LoginID where num='" + num + "' and month='" + month + "' and  day='" + day + "'";
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();
                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["LoginID"] != DBNull.Value) emp.Add("LoginID", (string)sdr["LoginID"]);
                    if (sdr["sex"] != DBNull.Value) emp.Add("sex", (int)sdr["sex"]);
                    if (sdr["nickname"] != DBNull.Value) emp.Add("nickname", (string)sdr["nickname"]);
                    else
                    {
                        string a = (string)sdr["LoginID"];
                        emp.Add("nickname", a.Replace(a.Substring(3, 4), "****"));
                    }
                    obj.Add(emp);
                }
                emp1.Add("hcp", obj);
                return emp1.ToString();
            }
            catch (System.Exception ee)
            {
                return ee.Message.ToString();
            }
            finally
            {
                conn.Close();
            }
        }
        public static string update_hcp(string LoginID, int sex, string num, int month, int day)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "update hcp set num = '" + num + "',month = " + month + ",day = " + day + "  where loginid = '" + LoginID + "'";
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                cmd.CommandText = "select * from hcp left outer join Member on Hcp.LoginID=Member.LoginID where num='" + num + "' and month='" + month + "' and  day='" + day + "'";
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();
                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["LoginID"] != DBNull.Value) emp.Add("LoginID", (string)sdr["LoginID"]);
                    if (sdr["sex"] != DBNull.Value) emp.Add("sex", (int)sdr["sex"]);
                    if (sdr["nickname"] != DBNull.Value) emp.Add("nickname", (string)sdr["nickname"]);
                    else
                    {
                        string a = (string)sdr["LoginID"];
                        emp.Add("nickname", a.Replace(a.Substring(3, 4), "****"));
                    }
                    obj.Add(emp);
                }
                emp1.Add("hcp", obj);
                return emp1.ToString();
            }
            catch (System.Exception ee)
            {
                return ee.Message.ToString();
            }
            finally
            {
                conn.Close();
            }
        }
        public static string get_hcp(string LoginID)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from Hcp left outer join Member on Hcp.LoginID=Member.LoginID where num in (select num from Hcp where LoginID ='" + LoginID + "') and month in (select month from Hcp where LoginID ='" + LoginID + "') and day in (select day from Hcp where LoginID ='" + LoginID + "')";
            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();
                while (sdr.Read())
                {
                    JObject emp = new JObject();
                    if (sdr["LoginID"] != DBNull.Value) emp.Add("LoginID", (string)sdr["LoginID"]);
                    if (sdr["sex"] != DBNull.Value) emp.Add("sex", (int)sdr["sex"]);
                    if (sdr["nickname"] != DBNull.Value) emp.Add("nickname", (string)sdr["nickname"]);
                    else
                    {
                        string a = (string)sdr["LoginID"];
                        emp.Add("nickname", a.Replace(a.Substring(3, 4), "****"));
                    }
                    obj.Add(emp);
                }
                emp1.Add("hcptop", obj);
                return emp1.ToString();
            }
            catch (System.Exception ee)
            {
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        public static string getAroundSchool(double x,double y)
        {
            string strConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from SchoolMail";
            Struct[] myStruct = new Struct[61];
            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                JArray obj = new JArray();
                JObject emp1 = new JObject();
                int cnt = 0;
                while (sdr.Read())
                {
                    myStruct[cnt] = new Struct();
                    myStruct[cnt].A = (int)sdr["SchoolID"];
                    myStruct[cnt].C = (string)sdr["SchoolName"];
                    //myStruct[cnt].B = 0;
                    myStruct[cnt].B = Math.Pow((float)sdr["Lng"] - x, 2) + Math.Pow((float)sdr["Lat"] - y, 2);
                    //myStruct[cnt].y = (double)sdr["Lat"];
                    cnt++;
                }
                Array.Sort(myStruct, new MyComparer());
                cnt = 0;
                Dictionary<int, int> imap = new Dictionary<int, int>();
                for (int i = 0; cnt < 10; i++)
                {
                    JObject emp = new JObject();
                    if (imap.ContainsKey(myStruct[i].A)) continue;
                    imap.Add(myStruct[i].A, 0);
                    emp.Add("schoolid", myStruct[i].A);
                    emp.Add("schoolname", myStruct[i].C);
                    obj.Add(emp);
                    cnt++;
                }
                emp1.Add("AroundSchool", obj);
                return emp1.ToString();
            }
            catch (System.Exception ee)
            {
                return (ee.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        public struct Struct
        {
            public int A;
            public string C;
            public double B;
        }
        public class MyComparer : IComparer<Struct>
        {
            #region IComparer<Struct> 成员

            int IComparer<Struct>.Compare(Struct x, Struct y)
            {
                if (x.B < y.B) return -1;
                else if (x.B == y.B) return 0;
                else return 1;
            }

            #endregion
        }
#endregion
    }
}
