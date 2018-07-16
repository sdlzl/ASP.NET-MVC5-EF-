using CaptchaGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.AdminWeb.App_Start;
using ZSZ.AdminWeb.Models;
using ZSZ.CommonMVC;
using ZSZ.IService;

namespace ZSZ.AdminWeb.Controllers
{
    public class MainController : Controller
    {
        public IAdminUserService AdminUserService { get; set; }

        public IRoleService RoleService { get; set; }

        public ICityService CityService { get; set; }
        // GET: Main
       
        public ActionResult Index() //主页
        {
            long? userId = AdminHelper.GetUserId(HttpContext);
            if (userId==null)
            {
                return Redirect("~/Main/Login");
            }
            var user = AdminUserService.GetById(userId.Value);
            return View(user);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            //先验证对象是否通过验证
            if (!ModelState.IsValid)//验证不通过
            {
                return Json(new AjaxResult() { Status = "error", ErrorMsg = CommonMVC.MVCHelper.GetValidMsg(ModelState) });
            }
            //先检查验证码
            string code = (string)Session["VerifyCode"];
            if (model.VerifyCode == (string)TempData["VerifyCode"])//好处:存取一次就清空。防止懂程序的人在前台把验证码错误之后的重新点击事件屏蔽掉！！！
                                                                   //就算屏蔽掉，输入之前的验证码 也不能验证通过
            //if (model.VerifyCode == (string)Session["VerifyCode"])
            {
                bool result = AdminUserService.CheckLogin(model.PhoneNumber, model.Password);
                if (result)
                {
                    //登录成功保存adminid
                    Session["LoginUserId"] = AdminUserService.GetByPhoneNum(model.PhoneNumber).Id;
                    return Json(new AjaxResult() { Status = "ok" });
                }
                else
                {
                    return Json(new AjaxResult() { Status = "error",ErrorMsg="密码或者手机号出错"});
                }
            }
            else
            {
                return Json(new AjaxResult() { Status = "error",ErrorMsg="验证码出错"});
            }


        }   

        public ActionResult CreateVerityCode()
        {
            string code = Common.CommonHelper.CreateVerifyCode(4);
            TempData["VerifyCode"] = code;
            //Session["VerifyCode"] = code;
            //以流的形式返回到浏览器
            MemoryStream ms = ImageFactory.GenerateImage(code, 60, 100, 20, 6);
            return File(ms,"image/jpeg");
        }


        public ActionResult LoginOut()
        {
            //   Session["LoginUserId"] = null;
            Session.Abandon();//销毁session
            return Redirect("~/Main/Login");
        }

     
        
    }
}