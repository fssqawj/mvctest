using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data.SqlClient;
using System.Configuration;
using Newtonsoft.Json.Linq;
using mvctest.dbop;
namespace mvctest.Controllers
{
    public class organizationController : Controller
    {
        public string get_org_byclass(int classid,int schoolid)
        {
            if (Session["username"] == null)
            {
                return "false";
            }
            return sqlop.get_org_byclass((string)Session["username"],classid,schoolid);
        }

        public string get_org_all(int schoolid)
        {
            if (Session["username"] == null)
            {
                return "false";
            }
            return sqlop.get_org_all((string)Session["username"], schoolid);
        }

        public string get_org_web(int classid, int schoolid)
        {
            return sqlop.get_org_web(classid, schoolid);
        }
        public string get_org_session_web(int classid)
        {
            if (Session["schid"] != null)
            {
                return sqlop.get_org_web(classid, (int)Session["schid"]);
            }
            else
            {
                return "false";
            }
        }
        public string get_org_byrate(double rate, int orgid)
        {
            if (Session["username"] == null)
            {
                return "false";
            }
            return sqlop.get_org_byrate((string)Session["username"],rate, orgid);
        }
        public string get_org_bycnt(int cnt, int orgid)
        {
            return sqlop.get_org_bycnt(cnt, orgid);
        }
        public string get_org_byname(string name)
        {
           
            return sqlop.get_org_byname(name);
        }
        public string get_org_allmember(string orgid)
        {

            return sqlop.get_org_allmember(orgid);
        }
        public string get_orgx_info(int orgid)
        {
            return sqlop.get_orgx_info(orgid,(string)Session["username"]);
        }
        //
        public string get_orgx_activity(int orgid)
        {
             
            return sqlop.get_orgx_activity(orgid);
        }
        public string get_orgx_nowact(int orgid)
        {
            return sqlop.get_orgx_nowact((string)Session["username"],orgid);
        }
        public string add_gz(int org_id){
            if (Session["username"] == null)
            {
                return "false";
            }
            if(sqlop.add_org_gz((string)Session["username"],org_id))return "true";
            return "false";
        }
        //public string join_org(int org_id){
        //    if (Session["username"] == null)
        //    {
        //        return "rrfalse";
        //    }
        //    if(sqlop.add_org_cj((string)Session["username"],org_id))return "true";
        //    return "false";
        //}
        public string delete_gz_org(int org_id)
        {
            if (Session["username"] == null)
            {
                return "false";
            }
            if (sqlop.delete_org_gz((string)Session["username"], org_id)) return "true";
            return "false";
        }
        //public string delete_join_org(int org_id)
        //{
        //    if (Session["username"] == null)
        //    {
        //        return "rrfalse";
        //    }
        //    if (sqlop.delete_org_cj((string)Session["username"], org_id)) return "true";
        //    return "false";
        //}
    }
}
