using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Europa.Data.Conventions
{
    public class OraclePrimaryKeySequenceConvention : IIdConvention
    {
        public void Apply(IIdentityInstance instance)
        {
            instance.GeneratedBy.Sequence($"Sequence_{instance.EntityType.Name}".ToUpper());
        }
    }
}
