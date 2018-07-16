using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZSZ.DTO;

namespace ZSZ.AdminWeb.Models
{
    public class RoleEditPostModel
    {

        public long Id { get; set; }

        public string Name { get; set; }

        public long[] PermissionIds { get; set; }
    }
}