using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data.SqlClient;
using System.Configuration;
using Newtonsoft.Json.Linq;
using mvctest.dbop;
using System.IO;
using System.Text;
namespace mvctest.Controllers
{
    public class activityController : Controller
    {
        //
        // GET: /activity/
        public string get_activity_info()
        {
            if (Session["username"] == null)
            {
                return "false";
            }
            return sqlop.get_activity_info((string)Session["username"]);
        }
        public string get_exquisite(string btime, string etime, string src)
        {
            return sqlop.get_exquisite(btime, etime, src);
        }
        public string get_activity_allmember(string actid)
        {
            return sqlop.get_activity_allmember(actid);
        }
        public string get_activity_bycnt(int cnt, int actid)
        {
           
            return sqlop.get_activity_bycnt(cnt, actid);
        }
        public string get_activityx_info(int actid,string src)
        {
            sqlop.insert_DYClick(src);
            if (Session["username"] == null)
            {
                return sqlop.get_activityx_info_nologin(actid);
            }
            return sqlop.get_activityx_info((string)Session["username"],actid);
        }
        public string add_comment(int activity_id, string content)
        {
            if (Session["username"] == null)
            {
                return "false";
            }
            if(sqlop.add_activity_comment((string)Session["username"], activity_id, content))return "true";
            return "false";
        }
        public string add_rate(int activity_id, float rate)
        {
            if (Session["username"] == null)
            {
                return "false";
            }
            if (sqlop.add_activity_rate((string)Session["username"], activity_id, rate)) return "true";
            return "false";
        }
       
        public string add_cj_activity(int activity_id)
        {
            if (Session["username"] == null)
            {
                return "false";
            }
            if (sqlop.add_activity_cj((string)Session["username"], activity_id)) return "true";
            return "false";
        }
      
        public string delete_cj_activity(int activity_id)
        {
            if (Session["username"] == null)
            {
                return "false";
            }
            if (sqlop.delete_activity_cj((string)Session["username"], activity_id)) return "true";
            return "false";
        }
        public string get_relatedschool( int schoolid)
        {
            return sqlop.get_relatedschool(schoolid);
        }
        public string get_actx_comment(int actid)
        {
            
            return sqlop.get_actx_comment(actid);
        }
        public string get_member_actx(string loginid, int actid)
        {
            return sqlop.get_member_actx(loginid, actid);
        }
        public string get_act(int schoolid, int actid, string acttime)
        {
            
           return sqlop.get_act(schoolid, actid, acttime);
        }

        public string get_actx(int actid, string acttime)
        {
            if (Session["username"] == null)
            {
                return "false";
            }
            return sqlop.get_actx((string)Session["username"], actid, acttime);
        }

        public string get_actx_all(int actid, string acttime)
        {
            if (Session["username"] == null)
            {
                return "false";
            }
            return sqlop.get_actx_all((string)Session["username"], actid, acttime);
        }


        public string get_sch_act(int schoolid)
        {

            return sqlop.get_sch_act(schoolid);
        }
        public string get_actbytime(int schoolid, int actid, string acttime)
        {
            if (Session["username"] == null)
            {
                return sqlop.get_actbytime("17777777777",schoolid, actid, acttime);
            }
            return sqlop.get_actbytime((string)Session["username"],schoolid, actid, acttime);
        }
        public string get_relatedact(int relatedid, int actid, string acttime)
        {
           
            return sqlop.get_relatedact(relatedid, actid, acttime);
        }
        public string post_confirm(string mid, int b)
        {

            if (sqlop.post_confirm(mid, b)) return "success";
            return "false";
        }
        public string post_hcp(string LoginID, int sex, string num, int month, int day)
        {
            num = num.ToUpper();
            num = num.Replace("　", "");
            num = num.Replace(" ", "");

            if(sqlop.has_hcp(LoginID, sex, num, month, day)){
                return sqlop.update_hcp(LoginID, sex, num, month, day);
            }
            return sqlop.insert_hcp(LoginID, sex, num, month, day);
        }
        public string get_hcp(string LoginID)
        {
            return sqlop.get_hcp(LoginID);
        }
        public string get_hcptop()
        {
            int hcpnum;
            StreamReader sr = new StreamReader(@"c:\hcp.txt", Encoding.GetEncoding("utf-8"));
            string line = sr.ReadLine();
            sr.Close();
            hcpnum = int.Parse(line)+1;
            StreamWriter sw = new StreamWriter(@"c:\hcp.txt");
            sw.WriteLine(hcpnum);
            sw.Close();
            return sqlop.get_hcptop();
        }

        public string hasEnroll(int actid)
        {
            if (Session["username"] == null)
            {
                return "false";
            }
            return sqlop.hasEnroll((string)Session["username"], actid);
        }
        public string addEnrollInfo(int actid, string name, string phone, string major, string grade, int sex)
        {
            if (Session["username"] == null)
            {
                return "false";
            }
            if (sqlop.addEnrollInfo((string)Session["username"], actid, name, phone, major, grade, sex)) return "true";
            return "false";
        }
        public string updateEnrollInfo(int actid, string name, string phone, string major, string grade, int sex)
        {
            if (Session["username"] == null)
            {
                return "false";
            }
            if (sqlop.updateEnrollInfo((string)Session["username"], actid, name, phone, major, grade, sex)) return "true";
            return "false";
        }
        public string deleteEnrollInfo(int actid)
        {
            if (Session["username"] == null)
            {
                return "false";
            }
            if (sqlop.deleteEnollInfo((string)Session["username"], actid)) return "true";
            return "false";
        }
    }
}
