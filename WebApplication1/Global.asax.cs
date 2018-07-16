using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //下面只给TestServiceImpl和Controllers注册了注入,其他新增的类均不能注入,要注入就需要
            //通过ICityService cityService = 
            //   DependencyResolver.Current.GetService<ICityService>() 这句话拿到对象

            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly)//给所有的Controllers注册事件
                .PropertiesAutowired();//最后一句话是属性注入，声明一个属性也可以获得实现该接口的实现类

            Assembly asm = Assembly.Load("CityService");//加载那些所有的实现类，加载实现类的类库程序集
                        builder.RegisterAssemblyTypes(asm)
            .Where(type => !type.IsAbstract)
            .AsImplementedInterfaces().PropertiesAutowired();//As 的意思是找到实现了该接口的那些类，Propertyes属性注入，
                                                             //声明了一个接口属性，Autofac自动帮我们找到实现该接口的类，并把他赋值,加载实现类，加上PropertiesAutowired()，
                                                             //就可以声明其他的实现类，容器自动给他赋值



            //给这个MVC项目所有的类都注册，通过ICityService cityService = 
                    //   DependencyResolver.Current.GetService<ICityService>() 这句话拿到对象
            //builder.RegisterAssemblyTypes(typeof(MvcApplication).Assembly).PropertiesAutowired();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
