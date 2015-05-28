using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Drawing.Imaging;
using System.Drawing;
using System.Text;
using System.Net;
using mvctest.dbop;
using System.Drawing.Drawing2D;
using restCreateTokenDemo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Thinhunan.Cnblogs.Com.RSAUtility;
namespace mvctest.Controllers
{

    public class userinfo
    {
        public string name, passwd, nickname,phone,infcode;
        public int schid, sex;
    }
    enum EBodyType : uint
    {
        EType_XML = 0,
        EType_JSON
    };
    public class HomeController : Controller
    {

        
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }
        public string logintest(string name, string passwd)
        {
            string privateKey = GetPemContent(@"C:\rsa_private_key.pem");
            string publicKey = GetPemContent(@"C:\rsa_public_key.pem");

            //DebugRsaKey(privateKey,true);
            return TestSignAndEncrypt(privateKey, publicKey,passwd);
        }
        static string GetPemContent(string filePath)
        {
            string content = System.IO.File.ReadAllText(filePath, Encoding.ASCII);
            return content;
        }
        public static string TestSignAndEncrypt(string privateKey, string publicKey,string passwd)
        {
            //sign
            RSAParameters para = PemConverter.ConvertFromPemPrivateKey(privateKey);
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(para);
            //byte[] testData = Encoding.UTF8.GetBytes("222");
            //MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            //byte[] signData = rsa.SignData(testData, md5);

            //verify
            RSAParameters paraPub = PemConverter.ConvertFromPemPublicKey(publicKey);
            RSACryptoServiceProvider rsaPub = new RSACryptoServiceProvider();
            rsaPub.ImportParameters(paraPub);
            //if (rsaPub.VerifyData(testData, md5, signData))
            //{
            //    Console.WriteLine("verify sign successful");
            //}
            //else
            //{
            //    Console.WriteLine("verify sign failed");
            //}

            //encrypt and decrypt data
            //string str = @"gKg13tdSKDxneUfcrqV9lpwuqLKOX0UoBZ11ujo7snFDMIZo3hr9NsIFMBgQmjOuTHiX3z7YhHkF+rYpnv/WpqsS4EPp51dA4ktMVu3lVDJwjR+8YfbXj9jhCAcrWj0OCGMlyNUeAUAXCj6OeQFyaMdfYRc9mVu6TcMor1gGNps=";
            byte[] testData = Convert.FromBase64String(passwd);
            byte[] encryptedData = rsaPub.Encrypt(testData, false);
            //byte[] bytes1 = Encoding.UTF8.GetBytes(str);
            //byte[] bytes1 = Convert.FromBase64String(str);
            //byte[] encryptedData = System.Text.Encoding.UTF8.GetBytes(str); 

            byte[] decryptedData = rsa.Decrypt(encryptedData, false);

            return Encoding.UTF8.GetString(decryptedData);
        }

        public string getMD5(string pwd)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5CSP = new System.Security.Cryptography.MD5CryptoServiceProvider();

            //获取要加密的字段，并转化为Byte[]数组
            byte[] testEncrypt = System.Text.Encoding.UTF8.GetBytes(pwd);

            //加密Byte[]数组
            byte[] resultEncrypt = md5CSP.ComputeHash(testEncrypt);

            //将加密后的数组转化为字段(普通加密)
            string res = "";

            for (int i = 0; i < 16; i++)
            {
                //Console.WriteLine(resultEncrypt[i]);
                //Console.WriteLine(resultEncrypt[i] >> 4);
                //Console.WriteLine(resultEncrypt[i] & 0xf);
                res += Convert.ToString((resultEncrypt[i] >> 4), 16);
                res += Convert.ToString((resultEncrypt[i] & 0xf), 16);
            }
            return res;
        }
        
        public string login(string name, string passwd)
        {
            string s = Request.UserHostAddress;
            
            Session.Timeout = 30;
            string result = sqlop.login(name, passwd, s);
            if (result != "false")
            {

                Session["username"] = name;
                Session["passwd"] = passwd;
                
                return result;
            }
            return "false";
        }

        public string login_md5(string name, string passwd)
        {
            string s = Request.UserHostAddress;

            Session.Timeout = 30;
            string result = sqlop.login_md5(name, passwd, s);
            if (result != "false")
            {

                Session["username"] = name;
                Session["passwd"] = passwd;

                return result;
            }
            return "false";
        }
        public string wxlogin(string name)
        {
            string s = Request.UserHostAddress;

            Session.Timeout = 30;
            string result = sqlop.wxlogin(name, s);
            if (result != "false")
            {

                Session["username"] = name;

                return result;
            }
            return "false";
        }
       
        public string logout()
        {
            if (Session["username"] == null) return "login first";
            Session["username"] = null;
            Session["passwd"] = null;
            return "logout sucess";
        }
        public string CreateValidateImage()
        {
            const int ImageHeigth = 22;             //验证码图片的高度

            const double ImageLengthBase = 12.5;    //验证码图片中每个字符的宽度

            const int ImageLineNumber = 25;         //噪音线的数量

            const int ImagePointNumber = 100;       //噪点的数量

            string code = "";
            string fname = string.Format("{0:yyyyMMddHHmmssffff}", DateTime.Now);
            Random random = new Random();

            //StringBuilder builder = new StringBuilder();

            //产生随机的验证码并拼接起来

            for (int i = 0; i < 4; i++)
            {

                code += random.Next(0, 9);

            }
            Session["infcode"] = code;
            //string VALIDATECODEKEY = "VALIDATECODEKEY";
            

            //this.Session[VALIDATECODEKEY] = code;

            //初始化位图Bitmap对象，指定图片对象的大小(宽,高)

            Bitmap image = new Bitmap((int)Math.Ceiling((double)(code.Length * ImageLengthBase)), ImageHeigth);

            //初始化一块画布

            Graphics graphics = Graphics.FromImage(image);

            //Random random = new Random();

            try
            {

                int num5;

                graphics.Clear(Color.White);

                //绘制噪音线

                for (num5 = 0; num5 < ImageLineNumber; num5++)
                {

                    int num = random.Next(image.Width);

                    int num3 = random.Next(image.Height);

                    int num2 = random.Next(image.Width);

                    int num4 = random.Next(image.Height);

                    graphics.DrawLine(new Pen(Color.Silver), num, num3, num2, num4);

                }

                //验证码字体样式

                Font font = new Font("Tahoma", 12, FontStyle.Italic | FontStyle.Bold);

                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);

                //绘制验证码到画布

                graphics.DrawString(code, font, brush, (float)2, (float)2);

                //绘制噪点

                for (num5 = 0; num5 < ImagePointNumber; num5++)
                {

                    int x = random.Next(image.Width);

                    int y = random.Next(image.Height);

                    image.SetPixel(x, y, Color.FromArgb(random.Next()));

                }

                graphics.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

                //MemoryStream stream = new MemoryStream();

                //保存图片

                image.Save("C:\\website\\Images\\" + fname + ".png", ImageFormat.Png);
                //image.Save()
                    
            }

            finally
            {

                graphics.Dispose();

                image.Dispose();

            }
            return fname + ".png";
            

        }
        public string webreg(string loginid, string passwd, string nickname, int schid, int sex, string code)
        {
            string infcode = (string)Session["infcode"];
            if (infcode == code)
            {
                if (sqlop.reg(loginid, passwd, nickname, schid, sex)) return "true";
                return "false";
                 
            }
            return "wrong code";
        }
        public string webreg_md5(string loginid, string passwd, string nickname, int schid, int sex, string code)
        {
            string infcode = (string)Session["infcode"];
            if (infcode == code)
            {
                if (sqlop.reg_md5(loginid, passwd, nickname, schid, sex)) return "true";
                return "false";

            }
            return "wrong code";
        }
        public string reg(string loginid, string passwd, string nickname, int schid, int sex)
        {
            if (sqlop.reg(loginid, passwd, nickname, schid, sex)) return "true";
            return "false";
        }
        public string addfankui(string content)
        {
            if (Session["username"] == null)
            {
                return "false";
            }
            if (sqlop.add_fankui((string)Session["username"], content)) return "true";
            return "false";
        }
        public string regcode(  string name )
        {
            if (sqlop.reg_already(name))
            {
                return "reg already";
            }


            else
            {
                string infcode = "";
                Random ran = new Random();

                for (int i = 0; i < 4; i++)
                {
                    infcode += ran.Next(0, 9);
                }
                userinfo user = new userinfo();
                user.name = name;  
                user.infcode = infcode;
                

                string serverIp = "api.ucpaas.com";
                string serverPort = "443";
                string account = "9b3d73d740064903a0406705bc9c7d1a";
                string token = "2129636f8717e189eec566e427c64e77";
                string appId = "f578a49c0f5740868b1d1be448ffcc36";
               
                string toPhone = name;
                string templatedId = "1082";


               
                UCSRestRequest.UCSRestRequest api = new UCSRestRequest.UCSRestRequest();

                api.init(serverIp, serverPort);
                api.setAccount(account, token);
                //api.enabeLog(true);
                api.setAppId(appId);
                //api.enabeLog(true);

                string result = api.SendSMS(toPhone, templatedId, infcode);
                
                Session["infcode"] = user;
                return "true";
            }
        }
        public string regcode_find(string name)
        {
            if (!sqlop.reg_already(name))
            {
                return "Not Reg";
            }

            else
            {
                string infcode = "";
                Random ran = new Random();

                for (int i = 0; i < 4; i++)
                {
                    infcode += ran.Next(0, 9);
                }
                userinfo user = new userinfo();
                user.name = name;
                user.infcode = infcode;

                string serverIp = "api.ucpaas.com";
                string serverPort = "443";
                string account = "9b3d73d740064903a0406705bc9c7d1a";
                string token = "2129636f8717e189eec566e427c64e77";
                string appId = "f578a49c0f5740868b1d1be448ffcc36";

                string toPhone = name;
                string templatedId = "1082";



                UCSRestRequest.UCSRestRequest api = new UCSRestRequest.UCSRestRequest();

                api.init(serverIp, serverPort);
                api.setAccount(account, token);
                //api.enabeLog(true);
                api.setAppId(appId);
                //api.enabeLog(true);

                string result = api.SendSMS(toPhone, templatedId, infcode);
                Session["infcode"] = user;
                //sqlop.change_passwd(name, infcode);
                return "true";
            }
        }
        public string find_password(string rinfcode,string passwd)
        {
            string code = (string)((userinfo)(Session["infcode"])).infcode;
            string name = (string)((userinfo)(Session["infcode"])).name;
            if (rinfcode == code)
            {
                if (sqlop.change_passwd(name, passwd)) return "true";
                return "false";
            }
            else
                return "wrong code";
        }
        public string regx(string rinfcode,string passwd,string nickname,int schid,int sex )
        {

           string code = (string)((userinfo)(Session["infcode"])).infcode;
            
            if (rinfcode == code)
            {
                if (sqlop.reg(((userinfo)(Session["infcode"])).name,  passwd, nickname,  schid,  sex)) return "true";
                return "false";
            }
            else 
                return "wrong code";
        }

        public string regx_md5(string rinfcode, string passwd, string nickname, int schid, int sex)
        {

            string code = (string)((userinfo)(Session["infcode"])).infcode;

            if (rinfcode == code)
            {
                if (sqlop.reg_md5(((userinfo)(Session["infcode"])).name, passwd, nickname, schid, sex)) return "true";
                return "false";
            }
            else
                return "wrong code";
        }
        public string regalready(string name)
        {
            if (sqlop.reg_already(name))
            {
                return "reg already";
            }
            return "false";
        }        
        public string judge()
        {
            if (Session["username"] != null) return "true";
            return "false";
        }
        public string get_version()
        {
            return sqlop.get_version();
        }
        public string tg_count(string tgid,string ck,string psys,string agent)
        {
            return sqlop.tg_count(tgid,ck,psys,agent);
        }
        public string tg_calc(string ck, int schoolid, int qd)
        {
            return sqlop.tg_calc(ck, schoolid, qd);
        }
        public string tg_calcdl(string ck, int schoolid, int qd, string client)
        {
            return sqlop.tg_calcdl(ck, schoolid, qd, client);
        }
    }
}
