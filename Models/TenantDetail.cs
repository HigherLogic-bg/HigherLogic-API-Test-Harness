using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace APITest.Models
{
    public class TenantDetailModel
    {
        [Required]
        [Display(Name = "Display Name")]
        public string TenantKey { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string HomePage { get; set; }
    }
}