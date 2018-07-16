using CaptchaGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.Common;
using ZSZ.CommonMVC;
using ZSZ.FrontWeb.Models;
using ZSZ.IService;

namespace ZSZ.FrontWeb.Controllers
{
    public class MainController : Controller
    {
        public ICityService ICity{ get; set; }
        public ISettingService ISettingServie { get; set; }
        public IUserService UserService { get; set; }
        // GET: Main
        public ActionResult Index()
        {
            long CityId = FrontUtils.GetCityId(HttpContext);
            string CityName = ICity.GetById(CityId).Name;
            ViewBag.CityName = CityName;
            var cities = ICity.GetAll();
            return View(cities);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        public ActionResult CreateVerifyCode()
        {
            string code = Common.CommonHelper.CreateVerifyCode(4);

            TempData["VerityCode"] = code;
            //图形验证码
            MemoryStream ms = ImageFactory.GenerateImage(code, 60, 100, 20, 6);
            //将内存流返回给浏览器，表明是一个图片
            return File(ms, "image/jpeg");
          

        }

        public ActionResult SendSMS(string phoneNum,string verifyCode)
        {
            //先验证验证码
            string code = TempData["VerityCode"].ToString();
            if (code!=verifyCode)
            {
                return Json(new AjaxResult() { Status="error",ErrorMsg="验证码错误!"});
            }

            //发送短信
            RuPengSMSSender sendMsg = new RuPengSMSSender();
            int  Vcode = new Random().Next(1111, 9999);
            TempData["Msgcode"] = Vcode.ToString();
            sendMsg.AppKey = ISettingServie.GetValue("如鹏网短信AppKey");
            sendMsg.UserName = ISettingServie.GetValue("如鹏网短信用户名");
            string tempId = ISettingServie.GetValue("如鹏网短信模版");
            RuPengSMSResult result= sendMsg.SendSMS(tempId, Vcode.ToString(), phoneNum);
            if (result.code!=0)
            {
                return Json(new AjaxResult() { Status = "error", ErrorMsg = result.msg });
            }
            TempData["RegPhoneNum"] = phoneNum;
            return Json(new AjaxResult() { Status = "ok" });


        }

        [HttpPost]
        public ActionResult Register(RegisterPostModel model)
        {
            //验证传来的model是否合乎验证 
            if (!ModelState.IsValid)
            {
                return Json(new AjaxResult { Status="error",ErrorMsg=CommonMVC.MVCHelper.GetValidMsg(ModelState)});
            }

            //验证发送验证码的手机与当前手机号是否相同！！！
            string regPhoneNum =(string)TempData["RegPhoneNum"];
            if (regPhoneNum!=model.PhoneNum)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "手机号与发送验证号手机号不同" });
            }

            //验证验证码
            string MsgCode = (string)TempData["Msgcode"];
            if (MsgCode != model.SMSCode)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "短信验证码错误" });
            }

            //验证手机号是否存在
            var exist = UserService.GetByPhoneNum(model.PhoneNum);
            if (exist!=null)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "手机号存在" });
            }

            //新增
            UserService.AddNew(model.PhoneNum, model.Password);

            return Json(new AjaxResult { Status = "ok" });
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserLoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new AjaxResult { Status = "error",ErrorMsg=CommonMVC.MVCHelper.GetValidMsg(ModelState) });
            }
            //判读帐号是否存在
            var user = UserService.GetByPhoneNum(model.PhoneNum);
         
            bool flag=  UserService.CheckLogin(model.PhoneNum, model.Password);
            if (flag)
            {
                //先判断帐号是否被锁定

                if (UserService.IsLockUser(user.Id))//锁定了
                {
                    TimeSpan? span = TimeSpan.FromMinutes(30) - (DateTime.Now - user.LastLoginErrorDateTime);
                    return Json(new AjaxResult { Status = "error", ErrorMsg = "帐号已被锁定，请在"+(int)span.Value.TotalMinutes+"分钟后再登录"});

                }
                //重置登录错误次数
                UserService.ResetLoginErrorTimes(user.Id);

                //封装方法获取session更容易 防止出错
                Session["UserId"] = FrontUtils.GetUserId(HttpContext);
                Session["CityId"] = FrontUtils.GetCityId(HttpContext);
                return Json(new AjaxResult { Status = "ok" });
            }
            else
            {
                if (user != null)
                {
                    //增加错误次数
                    UserService.IncLoginErrorTimes(user.Id);
                }   
                return Json(new AjaxResult { Status = "error",ErrorMsg="帐号或密码错误"});
            }
        }

        public ActionResult SwitchCity(long CityId)
        {
            long? userId = (long?)Session["UserId"];

            //先判断sessionuserid有无值
            long? cityId = (long?)Session["CityId"];
            if (userId == null)//无人登录，设置session为当前CityId
            {
                Session["CityId"] = cityId;
            }
            else //有人登录，设置用户的城市Id为当前Id
            {
                UserService.SetUserCityId(userId.Value, CityId);
                Session["CityId"] = CityId;
            }

            return Json(new AjaxResult { Status = "ok"});

        }
    }
}