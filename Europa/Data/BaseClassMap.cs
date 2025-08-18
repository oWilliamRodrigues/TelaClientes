using Europa.Data.Model;
using FluentNHibernate.Mapping;

namespace Europa.Data
{
    public class BaseClassMap<T> : ClassMap<T> where T : BaseEntity
    {
        public BaseClassMap()
        {
            Map(x => x.CriadoPor).Column("ID_CRIADO_POR").Not.Nullable().Not.Update();
            Map(x => x.CriadoEm).Column("DT_CRIADO_EM").Not.Nullable().Not.Update();
            Map(x => x.AtualizadoPor).Column("ID_ATUALIZADO_POR").Not.Nullable();
            Map(x => x.AtualizadoEm).Column("DT_ATUALIZADO_EM").Not.Nullable();
        }
    }
}
