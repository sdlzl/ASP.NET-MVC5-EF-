using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZSZ.DTO;

namespace ZSZ.AdminWeb.Models
{
    public class AdminUserListModel
    {
        public AdminUserDTO adminUser { get; set; }

        public List<RoleDTO> listRole { get; set; } = new List<RoleDTO>();
        
    }
}