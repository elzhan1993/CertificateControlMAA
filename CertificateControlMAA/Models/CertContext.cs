using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace CertificateControlMAA.Models
{
    public class CertContext:DbContext
    {
        public DbSet<certificate> certificates { get; set; }
        public DbSet<owner> owners { get; set; }
        public DbSet<vendor> vendors { get; set; }
        public DbSet<department> departments { get; set; }
        public DbSet<cert_category> categories { get; set; }

    }
}