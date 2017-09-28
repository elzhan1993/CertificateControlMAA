using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CertificateControlMAA.Models
{
    public class vendor
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime? foundationDate { get; set; }
        public string address { get; set; }
    }
}