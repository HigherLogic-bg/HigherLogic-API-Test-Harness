using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APITest.Models
{
    public class AuthenticationModel
    {
        public string TenantKey { get; set; }
        public string AuthToken { get; set; }
    }
}