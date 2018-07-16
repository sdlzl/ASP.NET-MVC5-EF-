using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZSZ.AdminWeb.App_Start
{

    //表明该属性只能用于方法，且一个方法可以标注多个属性
    [AttributeUsage(AttributeTargets.Method,AllowMultiple =true)]
    public class CheckPermissionAttribute:Attribute
    {
        public string Permission { get; set; }

        public CheckPermissionAttribute(string permission)
        {
            this.Permission = permission;
        }

    }
}