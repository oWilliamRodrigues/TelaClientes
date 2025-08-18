using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Europa.Data.Dialect;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;

namespace Europa.Data.FluentConfigurationHelpers{
    public static class FluentConfigExtension{
        public static FluentConfiguration MapWithAssemblies(this FluentConfiguration fluentConfiguration,Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                fluentConfiguration.Mappings(m => m.FluentMappings.AddFromAssembly(assembly));
            }

            return fluentConfiguration;
        }

        public static FluentConfiguration MapWithClassArray(this FluentConfiguration fluentConfiguration, IEnumerable<Type> classes)
        {
            foreach (var classe in classes)
            {
                fluentConfiguration.Mappings(m => m.FluentMappings.Add(classe));
            }

            return fluentConfiguration;
        }

        
        public static FluentConfiguration OracleConfig(this FluentConfiguration fluentConfiguration, string connectionString, bool useAppSettings=false)
        {
            fluentConfiguration.Database(OracleDriver(connectionString,
                                                      useAppSettings
                                                          ? ConnectionStringConfigurationTag.AppSettings
                                                          : ConnectionStringConfigurationTag.ConnectionString));
                                             
            return fluentConfiguration;
        }
        
        public static FluentConfiguration PostgreConfig(this FluentConfiguration fluentConfiguration, string connectionString, bool useAppSettings=false)
        {
            fluentConfiguration.Database(PostgreSQLConfiguration.Standard.Dialect<PostgreSQLDialect>()
                     .ConnectionString(c =>
                                       {
                                           if (useAppSettings)
                                           {
                                               c.FromAppSetting(connectionString);
                                           }
                                           else
                                           {
                                               c.FromConnectionStringWithKey(connectionString);
                                           }
                                       })
                     .Driver<NpgsqlDriver>());
            return fluentConfiguration;
        }
        
        
        [Obsolete("Ainda não foi testado")]
        public static FluentConfiguration SqlServerConfig(this FluentConfiguration fluentConfiguration, string connectionString, bool useAppSettings=false)
        {
            fluentConfiguration.Database(SqlServerDriver(connectionString,
                                                      useAppSettings
                                                          ? ConnectionStringConfigurationTag.AppSettings
                                                          : ConnectionStringConfigurationTag.ConnectionString));
                                             
            return fluentConfiguration;
        }
        
        private static OracleClientConfiguration OracleDriver(string      connectionString,
                                                              ConnectionStringConfigurationTag tagConnectionStringWillUse = ConnectionStringConfigurationTag.ConnectionString)
        {
            return EuropaOracleClientConfiguration
                   .Oracle12c.ConnectionString(ConnectionStringExpression(connectionString,tagConnectionStringWillUse))
                   .Driver<OracleManagedDataClientDriver>();
        }

        private static MsSqlConfiguration SqlServerDriver(string      connectionString,
                                                          ConnectionStringConfigurationTag tagConnectionStringWillUse = ConnectionStringConfigurationTag.ConnectionString)
        {
            return 
                MsSqlConfiguration
                    .MsSql2008
                    .ConnectionString(ConnectionStringExpression(connectionString, tagConnectionStringWillUse))
                    .Driver<Sql2008ClientDriver>();
        }

        private static Action<ConnectionStringBuilder> ConnectionStringExpression(
            string connectionString, ConnectionStringConfigurationTag tagConnectionStringWillUse)
        {
            return c =>
                   {
                       if (tagConnectionStringWillUse == ConnectionStringConfigurationTag.ConnectionString)
                           c.FromConnectionStringWithKey(connectionString);
                       else c.FromAppSetting(connectionString);
                   };
        }

        public static FluentConfiguration  DefineProperties(this FluentConfiguration fluentconfig, bool showSql, bool formatSql, bool generateStatistics,
                                                              bool exposeDDL, bool executeExposed=false)
        {
            fluentconfig.ExposeConfiguration(DefineAnalysisProperties(showSql,formatSql, generateStatistics,exposeDDL, executeExposed));
            return fluentconfig;
        }

        /// <summary>
        /// Recomenda-se FORTEMENTE não utilizar o querycache sem o cache de segundo nível. O query cache salva apenas as chaves identificadoras dos registros.
        /// <para>Se for configurado de maneira errada, pode gerar um overload maior do que sem utilizá-lo.</para>
        /// É também recomendado chamar esse método passando os parâmetros por extenso para ilustrar melhor quais configurações estão sendo definidas.
        /// <example>
        /// Esse exemplo demonstra como utilizá-lo de maneira correta.
        /// <code>
        /// Fluently.Configure().DefineCacheConfigurations(useSecondLevel:true,useQueryCache:true)
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="fluentconfig"></param>
        /// <param name="useSecondLevel"></param>
        /// <param name="useQueryCache"></param>
        /// <returns></returns>
        public static FluentConfiguration  DefineCacheConfigurations(this FluentConfiguration fluentconfig, bool useSecondLevel, bool useQueryCache)
        {
            fluentconfig.ExposeConfiguration(DefineCacheConfigurations(useSecondLevel, useQueryCache));
            return fluentconfig;
        }

        private static Action<Configuration> DefineCacheConfigurations(bool useSecondLevel, bool useQueryCache)
        {
            return cfg =>
                   {
                       if (useQueryCache) cfg.SetProperty(NHibernate.Cfg.Environment.UseQueryCache,        "true");
                       if (useSecondLevel) cfg.SetProperty(NHibernate.Cfg.Environment.UseSecondLevelCache, "true");
                   };
        }
        
        /// <summary>
        /// Define propriedades analíticas para a configuração do nhibernate
        /// </summary>
        /// <param name="showSql"></param>
        /// <param name="formatSql"></param>
        /// <param name="generateStatistics"></param>
        /// <param name="exposeDDL"></param>
        /// <returns></returns>
        private static Action<Configuration> DefineAnalysisProperties(bool showSql, bool formatSql, bool generateStatistics,
                                                              bool exposeDDL,bool executeExposed)
        {
            return cfg =>
                   {
                       if (showSql) cfg.SetProperty(NHibernate.Cfg.Environment.ShowSql,                       "true");
                       if (formatSql) cfg.SetProperty(NHibernate.Cfg.Environment.FormatSql,                   "true");
                       if (generateStatistics) cfg.SetProperty(NHibernate.Cfg.Environment.GenerateStatistics, "true");
                       if (exposeDDL)
                       {
                           var schemaUpdate = new SchemaUpdate(cfg);
                           SkipKnownExceptions(schemaUpdate);
                           if (executeExposed)
                           {
                               schemaUpdate.Execute(WriteDataDefinitionOnFile(), true);
                           }
                       }
                   };
        }

        private static void SkipKnownExceptions(SchemaUpdate schemaUpdate)
        {
            if (schemaUpdate != null && schemaUpdate.Exceptions.Count > 0)
            {
                // Skip das mensagens de sequencia
                string erroSequenciaExiste = "42P07: relation ";
                foreach (var exception in schemaUpdate.Exceptions)
                {
                    Exception innerException = exception;
                    if (innerException.Message.StartsWith(erroSequenciaExiste) == false)
                    {
                        throw exception;
                    }

                    // Descendo na hierarquia de exceção
                    while (innerException.InnerException != null)
                    {
                        innerException = innerException.InnerException;

                        if (innerException.Message.StartsWith(erroSequenciaExiste) == false)
                        {
                            throw exception;
                        }
                    }
                }
            }
        }
        
        private static Action<string> WriteDataDefinitionOnFile()
        {
            var path = $@"\tmp\DES1716d03_ddl.sql";

            void UpdateExport(string x)
            {
                using (var file = new FileStream(path, FileMode.Append, FileAccess.Write))
                    using (var sw = new StreamWriter(file))
                    {
                        sw.WriteLine($"{x};");
                    }
            }

            return UpdateExport;
        }
    }
}