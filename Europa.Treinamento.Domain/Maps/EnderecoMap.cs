using Europa.Data;
using Europa.Data.Model;
using Europa.Domain.Treinamento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Europa.Treinamento.Domain.Maps
{
    public class EnderecoMap : BaseClassMap<Endereco>
    {
        public EnderecoMap()
        {
            Table("TBL_ENDERECOS");
            Id(x => x.Id).Column("ID_ENDERECO").GeneratedBy.Sequence("SEQ_ENDERECOS");
            Map(x => x.Cep).Column("NR_CEP").Not.Nullable();
            Map(x => x.Logradouro).Column("DS_LOGRADOURO").Not.Nullable();
            Map(x => x.Numero).Column("NR_ENDERECO").Not.Nullable();
            Map(x => x.Complemento).Column("DS_COMPLEMENTO").Not.Nullable();
            Map(x => x.Bairro).Column("NM_BAIRRO").Not.Nullable();
            Map(x => x.Uf).Column("NM_UF").Not.Nullable();
            Map(x => x.Cidade).Column("NM_CIDADE").Not.Nullable();
            References(x => x.Cliente).Column("ID_CLIENTE").ForeignKey("FK_ENDERECOS_X_CLIENTES_01").Not.Nullable();
        }
    }
}
