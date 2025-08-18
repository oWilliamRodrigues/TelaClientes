using Microsoft.Owin;
using Owin;
using Europa.Treinamento.Web;
using Europa.Treinamento.Web.App_Start;

[assembly: OwinStartup(typeof(Startup))]
namespace Europa.Treinamento.Web.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}