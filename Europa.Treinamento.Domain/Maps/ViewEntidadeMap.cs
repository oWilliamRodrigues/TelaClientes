using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Europa.Data;
using Europa.Domain.Core.Enums;
using Europa.Domain.Treinamento.Models.Views;
using NHibernate.Type;

namespace Europa.Treinamento.Domain.Maps
{
    public class ViewEntidadeMap : BaseViewClassMap<ViewEntidade>
    {
        // Aqui herdamos de BaseViewClassMap para obter tabelas com permissão apenas de leitura
        public ViewEntidadeMap()
        {
            Table("VW_REL_ENTIDADES");

            Id(reg => reg.Id).Column("ID_ENTIDADE");
            Map(reg => reg.NomeEntidade).Column("NM_ENTIDADE");
            Map(reg => reg.SobrenomeEntidade).Column("DS_SOBRENOME");
            Map(reg => reg.Idade).Column("NR_IDADE");
            Map(reg => reg.DataNascimento).Column("DT_NASCIMENTO");
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<Situacao>>();
            Map(reg => reg.NomeUnidade).Column("NM_UNIDADE");
            Map(reg => reg.IdUnidade).Column("ID_UNIDADE");
        }
    }
}
