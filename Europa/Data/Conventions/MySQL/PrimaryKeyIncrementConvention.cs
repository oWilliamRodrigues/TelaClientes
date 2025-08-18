using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Europa.Data.Conventions.MySQL
{
    public class PrimaryKeyConvention : IIdConvention
    {
        public void Apply(IIdentityInstance instance)
        {
            instance.GeneratedBy.Increment();
        }
    }
}
