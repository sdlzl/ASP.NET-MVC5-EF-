using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.AdminWeb.App_Start;
using ZSZ.AdminWeb.Models;
using ZSZ.CommonMVC;
using ZSZ.IService;


namespace ZSZ.AdminWeb.Controllers
{
    public class PermissionController : Controller
    {

        public IPermissionService PermSvc { get; set; }


        [CheckPermission("Permission.List")]
        // GET: Permission
        public ActionResult List()
        {
            var perms=  PermSvc.GetAll();
            return View(perms);
        }

        [CheckPermission("Permission.Delete")]
        //如何Ajax请求不要用重定向 该方法为get 用该方法有风险 有些网页预加载(提前加载某些链接)可能把你的所有数据全部删除
        public ActionResult Delete(long id)
        {
            PermSvc.MarkDeleted(id);
            return RedirectToAction(nameof(List));
        }

        [CheckPermission("Permission.Delete")]
        //Ajax请求的正确用法 不刷新页面 删除了把对应的<tr>移除
        public ActionResult AjaxDelete(long id)
        {
            PermSvc.MarkDeleted(id);
            AjaxResult a = new AjaxResult { Status = "ok" };
            // return new JsonNetResult() {Data=a };
            return Json(a);
        }

        [CheckPermission("Permission.Add")]
        [HttpGet]
        public ActionResult Add()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Add(PermissionAddModel model)
        {
            PermSvc.AddPermission(model.Name, model.Descript);
            return Json(new AjaxResult() { Status = "ok" });
        }

        [CheckPermission("Permission.Edit")]
        [HttpGet]
        public ActionResult Edit(long id)
        {
          var permdto=  PermSvc.GetById(id);
            return View(permdto);
        }


        [HttpPost]
        public ActionResult Edit(PermissionEditModel model)
        {
            PermSvc.UpdatePermission(model.Id, model.Name, model.Descript);
            return Json(new AjaxResult() { Status = "ok" });
            
        }

        [CheckPermission("Permission.Delete")]
        public ActionResult BatchDel(long[] permissionIds)
        {
            foreach (var s in permissionIds)
            {
                PermSvc.MarkDeleted(s);
            }
            return Json(new AjaxResult() { Status = "ok" });
        }



    }
}