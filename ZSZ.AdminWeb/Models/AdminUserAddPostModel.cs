using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZSZ.AdminWeb.Models
{
    public class AdminUserAddPostModel
    {
        //可以通过代码来验证这些属性是否通过
        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string Password2 { get; set; }

        [Required]
        public string PhoneNum { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        
        public long[] RoleIds { get; set; }
        
        public long? CityId { get; set; }


    }
}