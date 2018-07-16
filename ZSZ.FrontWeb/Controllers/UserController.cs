using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.Common;
using ZSZ.CommonMVC;
using ZSZ.IService;

namespace ZSZ.FrontWeb.Controllers
{
    public class UserController : Controller
    {
        public IUserService UserService { get; set; }
        public ISettingService SettingService { get; set; }

        [HttpGet]
        public ActionResult ForGotPassword()
        {
            return View();
        }
        // GET: User
        [HttpPost]
        public ActionResult ForGotPassword(string phoneNum,string regCode)
        {
            //判断手机号是否存在
            var user = UserService.GetByPhoneNum(phoneNum);
            if (user==null)
            {
                return Json(new AjaxResult() { Status = "error", ErrorMsg = "手机号不存在!" });
            }
            //验证图形验证码是否正确
            string code= (string)TempData["VerityCode"];
            if (regCode!=code)
            {
                return Json(new AjaxResult() { Status = "error", ErrorMsg = "验证码错误!" });
            }


            //发送短信
            RuPengSMSSender sendMsg = new RuPengSMSSender();
            int Vcode = new Random().Next(1111, 9999);

            //sendMsg.AppKey = SettingService.GetValue("如鹏网短信AppKey");
            //sendMsg.UserName = SettingService.GetValue("如鹏网短信用户名");
            //string tempId = SettingService.GetValue("如鹏网密码找回模版");
            sendMsg.AppKey = "904466bf849dce5db13e17";
            sendMsg.UserName = "lzl456";
            string tempId = "1024";
            RuPengSMSResult result = sendMsg.SendSMS(tempId, Vcode.ToString(), phoneNum);
            if (result.code != 0)
            {
                
                return Json(new AjaxResult() { Status = "error", ErrorMsg = result.msg });
            }
            TempData["MsgRegCode"] = Vcode.ToString();
            TempData["ForGetPhoneNum"] = phoneNum;
            return Json(new AjaxResult() { Status = "ok" });

        
        }

        [HttpGet]
        public ActionResult ForGotPassword2()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForGotPassword2(string RegCode)
        {
            //判断输入验证码与短信发送的验证码是否一直
            string regcode = (string)TempData["MsgRegCode"];
            if (regcode!= RegCode)
            {
                return Json(new AjaxResult() { Status = "error",ErrorMsg="验证码不正确" });
            }

            Session["IsPassword2"] = true;
            return Json(new AjaxResult() { Status = "ok"});

        }

        [HttpGet]
        public ActionResult ForGotPassword3()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForGotPassword3(string password)
        {
            //判断是否经过了步骤2 短信验证
            bool? IsPassword2 = (bool?)Session["IsPassword2"];
            if (IsPassword2 != true)
            {
                return Json(new AjaxResult() { Status = "error", ErrorMsg = "你没有通过短信验证" });
            }
            else
            {
                //拿到登录者的手机号,找到用户信息
                string phoneNum = (string)TempData["ForGetPhoneNum"];
                var user = UserService.GetByPhoneNum(phoneNum);
                UserService.UpdatePwd(user.Id, password);

                return Json(new AjaxResult() { Status = "ok" });
            }
           
        }

        public ActionResult ForGotPassword4()
        {
            return View();
        }
    }
}