using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CertificateControlMAA.Models
{
    public class startInitializator: DropCreateDatabaseAlways<CertContext>
    {
        protected override void Seed(CertContext context)
        {
            base.Seed(context);
        }
    }
}