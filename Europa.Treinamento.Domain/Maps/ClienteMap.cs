using Europa.Data;
using Europa.Data.Model;
using Europa.Domain.Core.Enums;
using Europa.Domain.Treinamento.Models;
using NHibernate.Type;

namespace Europa.Treinamento.Domain.Maps
{
    public class ClienteMap : BaseClassMap<Cliente>
    {
        public ClienteMap() 
        {
            Table("TBL_CLIENTES");
            Id(x => x.Id).Column("ID_CLIENTE").GeneratedBy.Sequence("SEQ_CLIENTES");
            Map(x => x.Nome).Column("NM_CLIENTE").Not.Nullable();
            Map(x => x.Cpf).Column("NR_CPF").Not.Nullable();
            Map(x => x.DataNascimento).Column("DT_NASCIMENTO").Not.Nullable();
            Map(x => x.Situacao).Column("TP_SITUACAO").CustomType<EnumType<Situacao>>();
        }
    }
}
