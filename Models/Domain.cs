using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace APITest.Models
{
    public class DomainModel
    {
        [Required(ErrorMessage = "Domain Required")]
        [Display(Name = "Domain Name")]
        public string Domain { get; set; }
    }
}