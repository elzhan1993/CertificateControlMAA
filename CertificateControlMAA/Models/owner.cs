using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CertificateControlMAA.Models
{
    public class owner:human
    {
        public string position { get; set; }
        public int departmentID { get; set; }
        public string phone_number { get; set; }
        public string email { get; set; }
    }
}