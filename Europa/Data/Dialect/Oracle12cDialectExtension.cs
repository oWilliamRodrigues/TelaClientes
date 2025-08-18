using NHibernate;
using NHibernate.Dialect;
using NHibernate.Dialect.Function;

namespace Europa.Data.Dialect
{
    public class Oracle12cDialectExtension : Oracle12cDialect
    {
        public Oracle12cDialectExtension()
        {
            RegisterFunction("FN_DECOMPOSE", new StandardSQLFunction("FN_DECOMPOSE", NHibernateUtil.Int32));
        }
    }
}
