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
    public class jianzhiController : Controller
    {
        public string get_jianzhi_all_info(int jianzhiid)
        {
           
            return sqlop.get_jianzhi_all_info(jianzhiid);
        }
        public string get_jianzhi_class_info(int classx,int jzid)
        {
           
            return sqlop.get_jianzhi_class_info(classx,jzid);
        }
        public string get_jianzhix_info(int jianzhiid)
        {
           
            return sqlop.get_jianzhix_info(jianzhiid);
        }


    }
}
