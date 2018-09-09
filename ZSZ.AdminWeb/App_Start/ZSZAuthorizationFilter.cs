using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.CommonMVC;
using ZSZ.IService;

namespace ZSZ.AdminWeb.App_Start
{
    /// <summary>
    /// 在执行action之前判断有无登陆，没有登陆的话就先登陆。登陆了的话判断当前用户有无该action的访问权限(通过标注属性)
    /// </summary>
    public class ZSZAuthorizationFilter : IAuthorizationFilter
    {
        //由于该过滤类是我们自己定义，AutoFac没有帮我们注册，不能属性注入。想要自动属性注入就要在Global注册这个类。
        //此处我们手动获取对应的Service类
         IAdminUserService AdminUserService =
                    DependencyResolver.Current.GetService<IAdminUserService>();


        //全局权限验证
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            //获取标注在Action上的权限名，以自定义属性类的集合返回
            CheckPermissionAttribute[] attrs = (CheckPermissionAttribute[])filterContext.
                ActionDescriptor.GetCustomAttributes(typeof(CheckPermissionAttribute), false);

            //方法没有标注自定义属性
            if (attrs.Length<=0)
            {
                //直接返回
                return;
            }


            //判断用户是否有登录
            if (filterContext.HttpContext.Session["LoginUserId"] ==null)
            {

                //判断是Ajax请求还是输入地址的Get请求 Ajax请求的时候返回以下内容的时候就不符合Json格式
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    AjaxResult ajaxResult = new AjaxResult()
                    {
                         Data="~/Main/Login",
                         ErrorMsg="请先登录",
                         Status="redirect"
                    };
                    filterContext.Result = new JsonNetResult { Data = ajaxResult };
                }
                else {
                    //filterContext.Result = new ContentResult()
                    //{ Content = "请先登录" };
                    filterContext.Result= new RedirectResult("/Main/Login");
                    return;
                }

              
            }

            //已经登录,判断有无权限名
            long adminUserId = (long)filterContext.HttpContext.Session["LoginUserId"];
            bool flag = false;
            foreach (var perm in attrs)
            {
              flag=  AdminUserService.HasPermission(adminUserId,perm.Permission);
                if (!flag)//如果没有该权限 提示
                {
                    //判断是Ajax请求还是输入地址的Get请求 Ajax请求的时候返回以下内容的时候就不符合Json格式
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        AjaxResult ajaxResult = new AjaxResult()
                        {
                            ErrorMsg = "你没有权限",
                            Status = "error"
                        };
                        filterContext.Result = new JsonNetResult { Data = ajaxResult };
                    }
                    else
                    {

                        filterContext.Result = new ContentResult()
                        { Content = "你没有权限" };
                        return;

                    }
                
                }

            }
           


        }
    }
}