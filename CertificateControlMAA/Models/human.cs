using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CertificateControlMAA.Models
{
    public abstract class  human
    {
        public int id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string secondName { get; set; }
        public DateTime birthDate { get; set; }
     }
}