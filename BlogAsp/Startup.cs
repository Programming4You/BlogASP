using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BlogAsp.Startup))]
namespace BlogAsp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }


    }
}
