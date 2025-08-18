using Europa.Domain.Core.Enums;
using Europa.Domain.Treinamento.Models.Views;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Europa.Treinamento.Domain.Maps
{
    public class ViewClienteMap : ClassMap<ViewCliente>
    {
        public ViewClienteMap()
        {
            Table("VW_CLIENTES");
            Id(x => x.Id).Column("ID_CLIENTE");
            Map(x => x.Nome).Column("NM_CLIENTE");
            Map(x => x.CpfCliente).Column("NR_CPF");
            Map(x => x.DataNascimento).Column("DT_NASCIMENTO");
            Map(x => x.CepEndereco).Column("NR_CEP");
            Map(x => x.NumeroEndereco).Column("NR_ENDERECO");
            Map(x => x.Complemento).Column("DS_COMPLEMENTO");
            Map(x => x.Logradouro).Column("DS_LOGRADOURO");
            Map(x => x.Cidade).Column("NM_CIDADE");
            Map(x => x.Bairro).Column("NM_BAIRRO");
            Map(x => x.Uf).Column("NM_UF");
            Map(x => x.Email).Column("DS_EMAIL");
            Map(x => x.Telefone).Column("DS_TELEFONE");
            Map(x => x.Celular).Column("DS_CELULAR");
            Map(x => x.Situacao).Column("TP_SITUACAO").CustomType<Situacao>();
        }
    }
}
