using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace APITest.Models
{
    public class LoginWithTenantDetailModel
    {
        [Required]
        public LoginModel Login { get; set; }
        public TenantDetailModel TenantDetail { get; set; }
    }
}