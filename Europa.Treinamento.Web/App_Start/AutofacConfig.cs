using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using NHibernate;
using Europa.Domain.Shared;
using Treinamento.Domain.Core.Data;

namespace Europa.Treinamento.Web.App_Start
{
    public static class AutofacConfig
    {

        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();

            builder.RegisterModule(new AutofacWebTypesModule());

            builder.Register(x => new DefaultInterceptor()).InstancePerRequest();

            builder.Register(x => Domain.Data.NHibernateSession
                .Session())
                .As<ISession>().InstancePerHttpRequest();

            builder.Register(x => Domain.Data.NHibernateSession
             .StatelessSession())
             .As<IStatelessSession>().InstancePerHttpRequest();

            //App
            Domain.AppStart.AutofacConfig.Register(builder);


            var container = builder.Build();

            // Set MVC DI resolver to use our Autofac container
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}