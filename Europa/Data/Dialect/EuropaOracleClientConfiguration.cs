using FluentNHibernate.Cfg.Db;
using NHibernate.Dialect;

namespace Europa.Data.Dialect{
    public class EuropaOracleClientConfiguration : OracleClientConfiguration{
        public static OracleClientConfiguration Oracle12c
        {
            get
            {
                return new EuropaOracleClientConfiguration().Dialect<Oracle12cDialect>();
            }
        }
    }
}