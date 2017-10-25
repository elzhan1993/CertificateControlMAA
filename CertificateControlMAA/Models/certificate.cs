using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CertificateControlMAA.Models
{
    public class certificate
    {
        public int id { get; set; }
        public int category { get; set; }
        public string name { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public DateTime getDate { get; set; }
        public int ownerID { get; set; }
        public int vendorID {get; set;}
        public string file1_name { get; set; }
        public string file2_name { get; set; }
        public string file3_name { get; set; }
        public string file4_name { get; set; }
    }
}