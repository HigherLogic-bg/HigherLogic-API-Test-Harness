using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace APITest.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Username Required")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password Required")]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public string preView { get; set; }
    }
}