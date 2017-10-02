using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CertificateControlMAA.Models
{
    public class user:human
    {
        public string email { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public int access_level { get; set; }
        public int department_id { get; set; }
        public string position { get; set; }
    }
}