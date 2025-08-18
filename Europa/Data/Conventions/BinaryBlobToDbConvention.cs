using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace Europa.Data.Conventions
{
    public class BinaryBlobToDbConvention : IPropertyConvention, IPropertyConventionAcceptance
    {
        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
        {
            criteria.Expect(x => x.Type == typeof(byte[]));
        }

        public void Apply(IPropertyInstance instance)
        {
            instance.CustomSqlType("BLOB");
        }
    }
}
