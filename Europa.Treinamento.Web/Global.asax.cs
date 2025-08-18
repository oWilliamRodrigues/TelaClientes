using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Europa.Web;
using Europa.Treinamento.Web.App_Start;

namespace Europa.Treinamento.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AutofacConfig.ConfigureContainer();

            /*QuartzConfig.Config();*/

            MessagesPublisher.Publish();

            ModelBinders.Binders.Add(typeof(string), new TrimModelBinder());
            ModelBinders.Binders[typeof(double)] = new DoubleModelBinder();
            ModelBinders.Binders[typeof(double?)] = new NullableDoubleModelBinder();

        }
    }
}
