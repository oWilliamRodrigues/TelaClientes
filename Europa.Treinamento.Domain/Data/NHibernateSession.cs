using Autofac;
using Autofac.Integration.Mvc;
using NHibernate;
using System.Collections.Generic;
using System.Reflection;
using Europa.Data;
using Europa.Domain.Shared;
using Treinamento.Domain.Core.Data;

namespace Europa.Treinamento.Domain.Data
{
    public static class NHibernateSession
    {

        public static IStatelessSession StatelessSession()
        {
            return NHibernateSessionFactory.OpenStatelessSession(ProjectProperties.ConnectionStringPostgres, null,CurrentAssemblies());
        }

        public static ISession Session()
        {
            return NHibernateSessionFactory.OpenSession(ProjectProperties.ConnectionStringPostgres, null, CurrentAssemblies());
        }

        public static ISession Session(IInterceptor localSessionInterceptor)
        {
            return NHibernateSessionFactory.OpenSession(ProjectProperties.ConnectionStringPostgres, localSessionInterceptor, CurrentAssemblies());
        }

        public static ISession NestedScopeSession()
        {
            var nestedScope = AutofacDependencyResolver.Current.RequestLifetimeScope.BeginLifetimeScope(builder =>
                builder.Register(x => NHibernateSession.Session()));
            return nestedScope.Resolve<ISession>();
        }
        
        public static Assembly[] CurrentAssemblies()
        {
            var assemblies = new List<Assembly> {
                Assembly.GetExecutingAssembly(),
            };
            return assemblies.ToArray();
        }

        #region SessionHandler

        public static void CloseIfOpen(ISession session)
        {
            if (session != null && session.IsOpen)
            {
                session.Close();
            }
        }

        public static void CloseIfOpen(IStatelessSession session)
        {
            if (session != null && session.IsOpen)
            {
                session.Close();
            }
        }

        #endregion

    }
}