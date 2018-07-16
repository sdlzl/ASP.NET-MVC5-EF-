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
    public class AdminUserController : Controller
    {
        public IAdminUserService AdminUserService { get; set; }

        public IRoleService RoleService { get; set; }

        public ICityService CityService { get; set; }

        [CheckPermission("AdminUser.List")]
        // GET: AdminUser
        public ActionResult List()
        {
            var result = AdminUserService.GetAll();
            List<AdminUserListModel> list = new List<AdminUserListModel>();
            foreach (var admin in result)
            {
                AdminUserListModel usermodel = new AdminUserListModel();
                var adminroles = RoleService.GetByAdminUserId(admin.Id);
                foreach (var r in adminroles)
                {
                    usermodel.listRole.Add(r);
                }
                usermodel.adminUser = admin;

                list.Add(usermodel);
            }
            return View(list.ToArray());
        }     

        [CheckPermission("AdminUser.Add")]
        [HttpGet]
        public ActionResult Add()
        {
            //获取所有的角色
            var Role = RoleService.GetAll();

            //获取所有城市
            var city = CityService.GetAll().ToList();
            city.Insert(0, new ZSZ.DTO.CityDTO { Name = "总部", Id = 0 });

            AdminUserAddGetModel model = new AdminUserAddGetModel();
            model.Roles = Role;
            model.Cities = city.ToArray();
            return View(model);
        }


        [HttpPost]
        public ActionResult Add(AdminUserAddPostModel model)
        {
            //先校验Model的属性校验是否通过
            if (!ModelState.IsValid)
            {
                string msg = MVCHelper.GetValidMsg(ModelState);
                return Json(new AjaxResult { Status="error",ErrorMsg=msg});
            }

            //前端判断了一次手机号 后台还要再帮他判断一次手机号 前台判断只起到判断提醒的作用
            bool exist = AdminUserService.GetByPhoneNum(model.PhoneNum) != null;
            if (exist)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "手机号已经存在" });
            }
            long? cityId = null;
            if (model.CityId!=0)
            {
                cityId = model.CityId;
            }

           //事务
            long id=  AdminUserService.AddAdminUser(model.Name, model.PhoneNum, model.Password, model.Email, cityId);
            RoleService.AddRoleIds(id, model.RoleIds);
            return Json(new AjaxResult() { Status="ok"});


        }

        [CheckPermission("AdminUser.Delete")]
        public ActionResult BatchDel(long[] adminUserIds)
        {
            foreach (var s in adminUserIds)
            {
                AdminUserService.MarkDeleted(s);
            }
            return Json(new AjaxResult { Status = "ok" });

        }

        [CheckPermission("AdminUser.Delete")]
        public ActionResult Delete(long id)
        {
            AdminUserService.MarkDeleted(id);
            return Json(new AjaxResult { Status = "ok" });

        }

        [CheckPermission("CheckPhoneNum")]
        public ActionResult CheckPhoneNum(string phoneNum)
        {
          var exist=  AdminUserService.GetAll().Any(e => e.PhoneNum == phoneNum);
            if (exist)
            {
                return Json(new AjaxResult() { Status = "exist" });
            }
            else
            {
                return Json(new AjaxResult() { Status = "ok" });
            }
        }

        [CheckPermission("AdminUser.Edit")]
        [HttpGet]
        public ActionResult Edit(long id)
        {
            //先判断用户是否存在--！！前台后台都要进行验证
            var exist = AdminUserService.GetById(id);
            //找到错误视图来显示错误信息!! 它和重定向到一个Control的区别是:重定向到Control会改变页面地址 
            //而ReturnView 不会刷新地址，只会把视图的内容显示到当前页面
            if (exist==null)
            {
                return View("Error", (object)"id指定的操作员不存在");
            }

            //获取所有角色
            var allRoles = RoleService.GetAll();
            //获取管理员所拥有的角色
            var adminUserRoles = RoleService.GetByAdminUserId(id);

            //获取所有城市
            var allCities = CityService.GetAll().ToList();
            allCities.Insert(0, new DTO.CityDTO() { Id = 0, Name = "总部" });           

            //获取管理员
            var adminUser = AdminUserService.GetById(id);

            AdminUserEditGetModel model = new AdminUserEditGetModel();

            model.AdminUser = adminUser;
            if (adminUser.CityId==null)
            {
                model.AdminUser.CityId = 0;//0为总部员工
            }
            model.AdminUserRoles = adminUserRoles;
            model.AllCities = allCities.ToArray();
            model.AllRoles = allRoles.ToArray();
            
            

            
            return View(model);

        }

        [HttpPost]
        public ActionResult Edit(AdminUserEditPostModel model)
        {
            var adminUser = AdminUserService.GetById(model.Id);
            //先判断密码是否为空，为空就不更新密码
         
            
            AdminUserService.UpdateAdminUser(model.Id, model.Name, model.PhoneNum, model.Password, model.Email, model.CityIds);
            //更新角色
            RoleService.UpdateRoleIds(model.Id,model.RoleIds);
            return Json(new AjaxResult() { Status="ok"});

            


        }
    }
}