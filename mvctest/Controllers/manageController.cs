using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using Newtonsoft.Json.Linq;
using mvctest.dbop;
namespace mvctest.Controllers
{
    public class manageController : Controller
    {
        //
        // GET: /manage/

        public string get_my_org()
        {
            if (Session["username"] == null)
            {
                return "false";
            }
            return sqlop.get_my_org((string)Session["username"]);
        }
        public string get_my_group()
        {
            if (Session["username"] == null)
            {
                return "false";
            }
            return sqlop.get_my_group((string)Session["username"]);
        }
        public string get_my_acount()
        {
            if (Session["username"] == null)
            {
                return "false";
            }
            return sqlop.getacount((string)Session["username"]);
        }
        public string get_my_activity()
        {
            if (Session["username"] == null)
            {
                return "false";
            }
            return sqlop.get_my_activity((string)Session["username"]);
        }
        public string get_my_tucao()
        {
            if (Session["username"] == null)
            {
                return "false";
            }
            return sqlop.get_my_tucao((string)Session["username"]);
        }
        public string get_my_ershou()
        {
            if (Session["username"] == null)
            {
                return "false";
            }
            return sqlop.get_my_ershou("wdy");
        }
        public string get_my_guanzhuinfo()
        {
            if (Session["username"] == null)
            {
                return "false";
            }
            return sqlop.get_my_guanzhuinfo((string)Session["username"]);
        }
        public string change_password(string passwd)
        {
            if (Session["username"] == null)
            {
                return "false";
            }
            if (sqlop.change_passwd((string)Session["username"], passwd)) return "true";
            return "false";
        }
        public string change_nickname(string nickname)
        {
            if (Session["username"] == null)
            {
                return "false";
            }
            if (sqlop.change_nickname((string)Session["username"], nickname)) return "true";
            return "false";
        }
        public string change_introduce(string introduce)
        {
            if (Session["username"] == null)
            {
                return "false";
            }
            if (sqlop.change_introduce((string)Session["username"], introduce)) return "true";
            return "false";
        }
        public string change_school(int schid)
        {
            if (Session["username"] == null)
            {
                return "false";
            }
            if (sqlop.change_school((string)Session["username"], schid)) return "true";
            return "false";
        }
        public string get_schoollist()
        {
            return sqlop.get_schoollist();
        }
        public string get_newschoollist(string city)
        {
            if (Session["username"] == null)
            {
                return sqlop.get_newschoollist("111111", city);
            }
            return sqlop.get_newschoollist((string)Session["username"],city);
        }
        public string get_schoollistx()
        {
            if (Session["username"] == null)
            {
                return sqlop.get_schoollistx("18888888888");
            }
            return sqlop.get_schoollistx((string)Session["username"]);
        }
        public string get_member_info(string mid)
        {
            if (Session["username"] == null)
            {
                return "false";
            }
            return sqlop.get_member_info((string)Session["username"],mid);
        }
        public string get_connect_info(string mid)
        {
            return sqlop.get_connect_info(mid);
        }
        public string add_sch_gz(int schid)
        {
            if (Session["username"] == null)
            {
                return "false";
            }
            if (sqlop.add_sch_gz((string)Session["username"], schid)) return "true";
            return "false";
        }
        public string add_sch_gzs(string schid)
        {
            if (Session["username"] == null)
            {
                return "false";
            }
            string[] schidw = sqlop.get_school((string)Session["username"]).Split(new char[] { ',' });
            string[] schidx = schid.Split(new char[] { ',' });
            for (int i = 0; i < schidx.Length; i++)
            {
                if (!schidw.Contains(schidx[i]))
                {
                    if (sqlop.add_sch_gz((string)Session["username"], Int32.Parse(schidx[i]))) continue;
                    else return "false";
                }
            }
            for (int i = 0; i < schidw.Length; i++)
            {
                if (!schidx.Contains(schidw[i]))
                {
                    if (sqlop.delete_sch_gz((string)Session["username"], Int32.Parse(schidw[i]))) continue;
                    return "false";
                }
            }
                return "true";
        }
        public string add_per_gz(string loginid)
        {
            if (Session["username"] == null)
            {
                return "false";
            }
            if (sqlop.add_per_gz((string)Session["username"], loginid)) return "true";
            return "false";
        }
        public string confirm_per_gz(string loginid,int b)
        {
            if (Session["username"] == null)
            {
                return "false";
            }
            if (sqlop.confirm_per_gz(loginid,(string)Session["username"],b)) return "true";
            return "false";
        }
        public string delete_sch_gz(int schid)
        {
            if (Session["username"] == null)
            {
                return "false";
            }
            if (sqlop.delete_sch_gz((string)Session["username"], schid)) return "true";
            return "false";
        }
        public string delete_per_gz(string loginid)
        {
            if (Session["username"] == null)
            {
                return "false";
            }
            if (sqlop.delete_per_gz((string)Session["username"], loginid)) return "true";
            return "false";
        }
        public string bind_wx(string openid, string loginid, string passwd)
        {
            if (sqlop.bind_wx(openid, loginid, passwd))
            {
                Session["username"] = loginid;
                return "true";
            }
            return "false";
        }
        public string deletetest()
        {
            return sqlop.deletetest();
        }

        public string getAroundSchool(double x,double y)
        {
            return sqlop.getAroundSchool(x, y);
        }
        public string upload_photo(String img)
        {
            if (Session["username"] == null)
            {
                return "false";
            }
            byte[] bytes1 = Convert.FromBase64String(img);
            MemoryStream ms = new MemoryStream(bytes1);
            Bitmap bit = new Bitmap(ms);
            string picname = string.Format("{0:yyyyMMddHHmmssffff}", DateTime.Now);
            //string picname = "ss";
            bit.Save("C:\\test\\admin\\images\\" + picname + ".jpg");
            picname += ".jpg";
            if(sqlop.upload_photo((string)(Session["username"]),picname)) return picname;
            return "false";
        }

    }
}
