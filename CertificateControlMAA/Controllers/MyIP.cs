using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CertificateControlMAA.Controllers
{
    public class MyIPController:IController
    {
        public void Execute(RequestContext requestcontext)
        {
            string ip = requestcontext.HttpContext.Request.UserHostAddress;
            var response = requestcontext.HttpContext.Response;
            response.Write("<h2>"+ip+"</h2>");
        }
    }
}