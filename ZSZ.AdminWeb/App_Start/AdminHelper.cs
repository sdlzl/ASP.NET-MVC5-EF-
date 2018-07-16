using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZSZ.AdminWeb.App_Start
{
    public class AdminHelper
    {
        public static long? GetUserId(HttpContextBase ctx)
        {
            return (long?)(ctx.Session["LoginUserId"]);
        }
    }
}