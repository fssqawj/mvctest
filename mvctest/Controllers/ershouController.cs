using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.IO;
using Newtonsoft.Json.Linq;
using System.Drawing.Imaging;
using System.Drawing;
using System.Text;

using System.Data.SqlClient;
using System.Configuration;
using Newtonsoft.Json.Linq;
using mvctest.dbop;
namespace mvctest.Controllers
{
    public class ershouController : Controller
    {
        //
        // GET: /ershou/
        public string get_ershou_all_info(int ershouid,int classx)
        {
           return sqlop.get_ershou_all_info(ershouid,classx);
        }
        public string get_ershou_info(int ershouid)
        {
            return sqlop.get_ershou_info(ershouid);
        }
        public string insert_ershou_info(int classx, string Description, int OriginPrice, int Price, string DealTime, string Phone, String img,string ershouname)
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
            bit.Save("C:\\website\\Images\\" + picname + ".jpg");
            picname += ".jpg";
            if (sqlop.insert_ershou_info((string)Session["username"], classx, Description, OriginPrice, Price, DealTime, Phone,picname,ershouname)) return "true";
            return "false";
        }
        public string delete_ershou_info(int erid)
        {
            if (Session["username"] == null)
            {
                return "false";
            }            
            if (sqlop.delete_ershou_info((string)Session["username"],erid)) return "true";
            return "false";
        }
        public string get_ershou_name_info(string name)
        {
           
           return sqlop.get_ershou_name_info(name);
            
        }
    }
}
