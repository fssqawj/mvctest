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
    public class tucaoController : Controller
    {
        //
        // GET: /tucao/

        public string get_all_info(int ID)
        {
            string res = sqlop.search_all_tucao_info((string)Session["username"], ID);
            return res;
        }
        public string get_tucao_all_bycnt(int tucaoid,int zancnt)
        {
            
            return sqlop.get_tucao_bycnt(tucaoid,zancnt);
        }
        public string get_tucao_all_cnmment(int tucaoid)
        {
            return sqlop.get_tucao_all_comment(tucaoid);
        }
        public string insert_comment(int tucao_id, string comment)
        {

            if (Session["username"] == null)
            {
                return "false";
            }
            if (sqlop.insert_comment((string)Session["username"], tucao_id, comment)) return "true";
            return "false";
        }
        public string insert_tocao(string content)
        {

            if (Session["username"] == null)
            {
                return "rrfalse";
            }
            if (sqlop.insert_tucao((string)Session["username"], content)) return "true";
            return "false";
        }
        public string delete_tocao(int tucaoid)
        {

            if (Session["username"] == null)
            {
                return "rrfalse";
            }
            if (sqlop.delete_tucao((string)Session["username"], tucaoid)) return "true";
            return "false";
        }
        public string tucao_add_zan(int tucaoid)
        {
            if (Session["username"] == null)
            {
                return "false";
            }
            if (sqlop.add_zan((string)Session["username"], tucaoid)) return "true";
            return "false";
        }
        public string tucao_delete_zan(int tucaoid)
        {
            if (Session["username"] == null)
            {
                return "false";
            }
            if (sqlop.delete_zan((string)Session["username"], tucaoid)) return "true";
            return "false";
        }
    }
}
