using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Europa.Data
{
    public static class NHibernateSchemaValidator
    {
        public static void Validate(Configuration config)
        {
            new SchemaValidator(config).Validate();
        }
    }
}
