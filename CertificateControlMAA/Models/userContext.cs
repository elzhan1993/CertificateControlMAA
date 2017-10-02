using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CertificateControlMAA.Models
{
    public class userContext:DbContext
    {
        public userContext() : base("DefaultConnection")
        { }
        public DbSet<user> users { get; set; }
    }
}