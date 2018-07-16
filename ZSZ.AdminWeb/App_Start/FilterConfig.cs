using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.CommonMVC;
using ZSZ.FrontWeb.App_Start;

namespace ZSZ.AdminWeb.App_Start
{
    public class FilterConfig
    {
        //过滤器统一放到这里
        public static void RegisterFilter(GlobalFilterCollection filters)
        {
            filters.Add(new ZSZExceptionFilter());
            filters.Add(new JsonNetActionFilter());
            filters.Add(new ZSZAuthorizationFilter());

        }
    }
}