using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZSZ.FrontWeb.Models
{
    public class RegisterPostModel
    {

        [Required]
        public string PhoneNum { get; set; }

        [StringLength(4,MinimumLength =4)]
        [Required]
        public string SMSCode { get; set; }

        [Required]
        public string Password { get; set; }
        
        [Compare(nameof(Password))]
        public string Password2 { get; set; }

        public long? CityId { get; set; }
    }
}