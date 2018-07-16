using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ZSZ.FrontWeb.App_Start;
using Autofac;
using System.Reflection;
using Autofac.Integration.Mvc;
using ZSZ.IService;
using ZSZ.CommonMVC;
using ZSZ.AdminWeb.App_Start;

namespace ZSZ.AdminWeb
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
            Assembly[] assemblies = new Assembly[] { Assembly.Load("ZSZ.Service") };
            builder.RegisterAssemblyTypes(assemblies)
            .Where(type => !type.IsAbstract && typeof(IServiceSupport).IsAssignableFrom(type))
            .AsImplementedInterfaces().PropertiesAutowired();
            var container = builder.Build();
            //Assign:赋值
            //type1.IsAssignableFrom(type2);type类型的变量是否可以指向type2类型的对象
            //换一种说法:type2是否实现了type1接口/type2是否继承自type1
            //避免其他无关的类注册到AutoFac中


            //注册系统级别的 DependencyResolver，这样当 MVC 框架创建 Controller 等对象的时候都
            //是管 Autofac 要对象。
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));//!!!



            //加上这个捕获未处理异常的过滤器
            //filters.Add(new ZSZExceptionFilter());
            //filters.Add(new JsonNetActionFilter());
            //filters.Add(new ZSZAuthorizationFilter());

            //如果不选择空项目+MVC创建项目 微软会自动帮我们如下生成，将filter专门放在一个类中
            FilterConfig.RegisterFilter(GlobalFilters.Filters);


            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            log4net.Config.XmlConfigurator.Configure();


            ModelBinders.Binders.Add(typeof(string), new TrimToDBCModelBinder());
            ModelBinders.Binders.Add(typeof(long), new TrimToDBCModelBinder());
            ModelBinders.Binders.Add(typeof(double), new TrimToDBCModelBinder());
            ModelBinders.Binders.Add(typeof(int), new TrimToDBCModelBinder());


          

        }
    }
}
