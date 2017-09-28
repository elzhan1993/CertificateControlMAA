using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CertificateControlMAA.Models
{
    public class certificate
    {
        public int id { get; set; }
        public string category { get; set; }
        public string name { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public DateTime getDate { get; set; }
        public int ownerID { get; set; }
        public int vendorID {get; set;}


    }
}