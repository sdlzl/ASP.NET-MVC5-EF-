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
    public class RoleController : Controller
    {
        public IRoleService RoleService { get; set; }
        public IPermissionService PermService { get; set; }

        [CheckPermission("Role.List")]
        // GET: Role
        public ActionResult List()
        {
            var roles = RoleService.GetAll();
            return View(roles);
        }

        [CheckPermission("Role.Delete")]
        public ActionResult Delete(long id)
        {
            RoleService.MarkDeleted(id);
            return Json(new AjaxResult() { Status="ok"});
        }

        [CheckPermission("Role.Add")]
        [HttpGet]
        public ActionResult Add()
        {
            //获取所有权限项
            var perms = PermService.GetAll();
            return View(perms);
        }

        [HttpPost]
        public ActionResult Add(RoleAddModel model)
        {
            //表单数据验证 最后一关
            if (!ModelState.IsValid)
            {
                return Json(new AjaxResult() { Status="error",ErrorMsg=CommonMVC.MVCHelper.GetValidMsg(ModelState)});
            }


            //Wait:事务
          long id=  RoleService.AddNew(model.Name);
            PermService.AddPermIds(id, model.PermissionIds);
            return Json(new AjaxResult() { Status = "ok" });
            

        }


        [CheckPermission("Role.Edit")]
        [HttpGet]
        public ActionResult Edit(long id)
        {
            //获取所有权限
            var Allperms = PermService.GetAll();
            //获取角色已有的权限
            var roleperms = PermService.GetByRoleId(id);
            //获取角色
            var role = RoleService.GetById(id);

            RoleEditGetModel model = new RoleEditGetModel();
            model.Role = role;
            model.AllPermission = Allperms;
            model.RolePermission = roleperms;
            return View(model);

        }


        [HttpPost]
        public ActionResult Edit(RoleEditPostModel model)
        {

            PermService.UpdatePermIds(model.Id, model.PermissionIds);
            RoleService.Update(model.Id, model.Name);
            return Json(new AjaxResult() { Status = "ok" });

        }

        [CheckPermission("Role.Delete")]
        public ActionResult BatchDel(long[] selectedIds)
        {
            foreach (var s in selectedIds)
            {
                RoleService.MarkDeleted(s);
            }
            return Json(new AjaxResult() { Status = "ok" });
        }
    }
}