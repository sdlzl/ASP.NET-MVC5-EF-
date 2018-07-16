using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestService;

namespace WebApplication1.Controllers
{
    public class DefaultController : Controller
    {
        //自动给属性赋值,PropertiesAutowired() 这句话可以给属性自动赋值
        public IUserService UserService { get; set; }

        // GET: Default
        public ActionResult Index()
        {
            bool b = UserService.CheckLogin("aaa","123");
            return Content(b.ToString());
        }

        [HttpGet]
        public ActionResult TestJson()
        {
            return View();

        }



        [HttpPost]
        public ActionResult TestJson(FormCollection fc)
        {
             Dog dog = new Dog() { BirthDate = DateTime.Now, Id = 5, Name = "帅哥狗" };
            //return Json(dog);
            return new JsonNetResult() { Data = dog };
        }


        public ActionResult Page1()
        {
            return View();

        }
    }
}