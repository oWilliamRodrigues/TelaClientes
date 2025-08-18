using Europa.Data;
using Europa.Data.Model;
using Europa.Domain.Core.Enums;
using Europa.Domain.Treinamento.Models;
using NHibernate.Type;

namespace Europa.Treinamento.Domain.Maps
{
    public class UnidadeMap : BaseClassMap<Unidade>
    {
        public UnidadeMap()
        {
            Table("TBL_UNIDADES");

            Id(reg => reg.Id).Column("ID_UNIDADE").GeneratedBy.Sequence("SEQ_UNIDADES");
            Map(reg => reg.Nome).Column("NM_UNIDADE").Length(DatabaseStandardDefinitions.STD_CARACTER_LENGTH_EXTENDED).Not.Nullable();
        }
    }
}
