using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CertificateControlMAA.Startup))]
namespace CertificateControlMAA
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
