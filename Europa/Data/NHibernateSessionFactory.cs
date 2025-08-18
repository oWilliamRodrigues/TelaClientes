using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Europa.Data.FluentConfigurationHelpers;
using FluentNHibernate.Cfg;
using NHibernate;

namespace Europa.Data
{
    public static class NHibernateSessionFactory
    {
        private static readonly IDictionary<string, ISessionFactory> SessionFactorys = new Dictionary<string, ISessionFactory>();

        public static IStatelessSession OpenStatelessSession(string connectionString, IInterceptor localSessionInterceptor, params Assembly[] assemblies)
        {
            var session = CurrentSessionFactory(connectionString, assemblies).OpenStatelessSession();
            return session;
        }

        public static void DestroyFactories()
        {
            if (!SessionFactorys.Any())
            {
                return;
            }

            foreach (var factory in SessionFactorys.Values)
            {
                factory.Close();
                factory.Dispose();
            }
            SessionFactorys.Clear();
        }
        
        public static ISession OpenSession(string connectionString, IInterceptor localSessionInterceptor, params Assembly[] assemblies)
        {
            var session = localSessionInterceptor != null
                              ? CurrentSessionFactory(connectionString, assemblies).OpenSession(localSessionInterceptor)
                              : CurrentSessionFactory(connectionString, assemblies).OpenSession();            
            return session;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private static ISessionFactory CurrentSessionFactory(string connectionString, params Assembly[] assemblies)
        {
            if (!SessionFactorys.TryGetValue(connectionString, out var factory))
            {
                factory = CreateSessionFactory(connectionString, assemblies);
            }
            return factory;
        }
        
        [MethodImpl(MethodImplOptions.Synchronized)]
        private static ISessionFactory CreateSessionFactory(string connectionString, params Assembly[] assemblies)
        {
            var fluentConfiguration =
                Fluently.Configure()
                        .PostgreConfig(connectionString)
                        .DefineProperties(showSql: false, formatSql: false, generateStatistics: false,exposeDDL: false,executeExposed: false)
                        .DefineCacheConfigurations(useSecondLevel: false, useQueryCache: false)
                        .MapWithAssemblies(assemblies);
            
            var factory = fluentConfiguration.BuildSessionFactory();
            SessionFactorys.Add(connectionString, factory);
            return factory;
        }
        
        public static void CloseAllSessionFactory()
        {
            foreach (var factory in SessionFactorys.Values)
            {
                if (factory.IsClosed == false)
                {
                    factory.Close();
                }
            }
        }

        #region Oracle (Obsolete)

        [Obsolete("Mock para a API do SUAT. Remover assim que a API SUAT estiver funcional")]
        public static ISession OpenOracleSession(string connectionString, IInterceptor localSessionInterceptor, IEnumerable<Type> classes)
        {
            var session = localSessionInterceptor != null
                              ? CurrentOracleSessionFactory(connectionString, classes).OpenSession(localSessionInterceptor)
                              : CurrentOracleSessionFactory(connectionString, classes).OpenSession();            
            return session;
        }
        
        [MethodImpl(MethodImplOptions.Synchronized)]
        [Obsolete("Mock para a API do SUAT. Remover assim que a API SUAT estiver funcional")]
        private static ISessionFactory CreateOracleSessionFactory(string connectionString, IEnumerable<Type> classes)
        {
            var fluentConfiguration =
                Fluently.Configure()
                        .OracleConfig(connectionString)
                        .DefineProperties(showSql: false, formatSql: false, generateStatistics: false,exposeDDL: false)
                        .DefineCacheConfigurations(useSecondLevel: false, useQueryCache: false)
                        .MapWithClassArray(classes);
            
            var factory = fluentConfiguration.BuildSessionFactory();
            SessionFactorys.Add(connectionString, factory);
            return factory;
        }

        [Obsolete("Mock para a API do SUAT. Remover assim que a API SUAT estiver funcional")]
        [MethodImpl(MethodImplOptions.Synchronized)]
        private static ISessionFactory CurrentOracleSessionFactory(string connectionString,  IEnumerable<Type> classes)
        {
            if (!SessionFactorys.TryGetValue(connectionString, out var factory))
            {
                factory = CreateOracleSessionFactory(connectionString, classes);
            }
            return factory;
        }
        

        #endregion
    }
    
}