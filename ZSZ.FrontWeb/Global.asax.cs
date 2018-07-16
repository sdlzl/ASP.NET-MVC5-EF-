using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ZSZ.CommonMVC;
using ZSZ.FrontWeb.App_Start;
using ZSZ.IService;

namespace ZSZ.FrontWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {


            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();//把当前
                                                                                               // 程序集中的 Controller 都注册
                                                                                               //不要忘了.PropertiesAutowired()
                                                                                               // 获取所有相关类库的程序集
            Assembly[] asm = new Assembly[] {  Assembly.Load("ZSZ.Service") };//Assembly[] assemblies = new Assembly[]{Assembly.Load("ZSZ.Service") };
           // Assembly asm = Assembly.Load("ZSZ.Service");
            builder.RegisterAssemblyTypes(asm)
            .Where(type => !type.IsAbstract&&typeof(IServiceSupport).IsAssignableFrom(type))
            .AsImplementedInterfaces().PropertiesAutowired();
            var container = builder.Build();
            //注册系统级别的 DependencyResolver，这样当 MVC 框架创建 Controller 等对象的时候都
            //是管 Autofac 要对象。
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));//!!!



            //加上这个捕获未处理异常的过滤器
            GlobalFilters.Filters.Add(new ZSZExceptionFilter());
            GlobalFilters.Filters.Add(new JsonNetActionFilter());

            //绑定数据
            ModelBinders.Binders.Add(typeof(string),new TrimToDBCModelBinder());
            ModelBinders.Binders.Add(typeof(long), new TrimToDBCModelBinder());
            ModelBinders.Binders.Add(typeof(double), new TrimToDBCModelBinder());
            ModelBinders.Binders.Add(typeof(int), new TrimToDBCModelBinder());

            //日志
            log4net.Config.XmlConfigurator.Configure();


            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
