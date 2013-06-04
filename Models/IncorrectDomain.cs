using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace APITest.Models
{
    [MetadataType(typeof(DomainMetaData))]
    public partial class IncorrectDomain
    {
        class DomainMetaData
        {
            [Required(ErrorMessage = "Titles are required")]
            [System.ComponentModel.DataAnnotations.MinLength(30, ErrorMessage= "please work")]
            public string Title { get; set; }

            [Required(ErrorMessage = "The Price is required.")]
            [Range(5, 100, ErrorMessage = "Movies cost between $5 and $100.")]
            public decimal Price { get; set; }
        }
    }
}