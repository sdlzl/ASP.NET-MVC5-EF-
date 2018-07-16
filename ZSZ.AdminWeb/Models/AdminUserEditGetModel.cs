using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.DTO;

namespace ZSZ.AdminWeb.Models
{
    public class AdminUserEditGetModel
    {
        public AdminUserDTO AdminUser { get; set; }

        public CityDTO[] AllCities { get; set; }

        public RoleDTO[] AllRoles { get; set; }

        public RoleDTO[] AdminUserRoles { get; set; }

        
    }
}