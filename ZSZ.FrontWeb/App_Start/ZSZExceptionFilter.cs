using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZSZ.FrontWeb.App_Start
{
    public class ZSZExceptionFilter : IExceptionFilter
    {
        //把未处理异常记录到日记中去
        private static ILog log = LogManager.GetLogger(typeof(ZSZExceptionFilter));
        public void OnException(ExceptionContext filterContext)
        {
            //获取未处理异常
            log.Error("出现未处理异常" + filterContext.Exception);
        }
    }
} 