using Europa.Data;
using Europa.Data.Model;
using Europa.Domain.Core.Enums;
using Europa.Domain.Treinamento.Models;
using NHibernate.Type;

namespace Europa.Treinamento.Domain.Maps
{
    public class ContatoMap : BaseClassMap<Contato>
    {
        public ContatoMap() 
        { 
            Table("TBL_CONTATOS");

            Id(x => x.Id).Column("ID_CONTATO").GeneratedBy.Sequence("SEQ_CONTATOS");
            Map(x => x.Tipo).Column("TP_CONTATO").CustomType<EnumType<TipoContato>>();
            Map(x => x.Descricao).Column("DS_CONTATO")
                .Length(DatabaseStandardDefinitions.STD_CARACTER_LENGTH_EXTENDED)
                .Not.Nullable();
            Map(x => x.Principal).Column("FL_PRINCIPAL").Not.Nullable();
            References(x => x.Cliente).Column("ID_CLIENTE").ForeignKey("FK_CONTATOS_X_CLIENTES_01").Not.Nullable();
        }
    }
}
