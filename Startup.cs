using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Bai4_lab1.Startup))]
namespace Bai4_lab1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
