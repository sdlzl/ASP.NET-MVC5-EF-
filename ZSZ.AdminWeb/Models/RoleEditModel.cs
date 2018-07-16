using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZSZ.DTO;

namespace ZSZ.AdminWeb.Models
{
    public class RoleEditGetModel
    {
        public RoleDTO Role { get; set; }

        public PermissionDTO[] AllPermission { get; set; }

        public PermissionDTO[] RolePermission { get; set; }
    }
}