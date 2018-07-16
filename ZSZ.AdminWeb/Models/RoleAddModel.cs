using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ZSZ.DTO;

namespace ZSZ.AdminWeb.Models
{
    public class RoleAddModel
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public long[] PermissionIds { get; set; }
    }
}