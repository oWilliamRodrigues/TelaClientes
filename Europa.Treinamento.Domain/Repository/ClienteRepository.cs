using Europa.Data;
using Europa.Domain.Core.Enums;
using Europa.Domain.Treinamento.Models;
using Europa.Extensions;
using Europa.Resources;
using Europa.Treinamento.Domain.Repository.FiltroDto;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Europa.Treinamento.Domain.Repository
{
    public class ClienteRepository : NHibernateRepository<Cliente>
    {
        public DataSourceResponse<Cliente> Listar(DataSourceRequest request)
        {
            var query = Queryable();

            return query.ToDataRequest(request);
        }

        public int AlterarSituacaoEmLote(List<long> ids, Situacao situacao)
        {
            var stringConsulta = new StringBuilder();

            stringConsulta.Append(" UPDATE Cliente cli ");
            stringConsulta.Append(" SET cli.Situacao = :sit,  cli.AtualizadoEm = :atualizadoEm");
            stringConsulta.Append(" WHERE cli.Id IN (:idsCliente) ");
            stringConsulta.Append(" AND cli.Situacao != :sit ");
            stringConsulta.Append(" AND cli.Situacao != :situacaoCancelado ");

            var consulta = Session.CreateQuery(stringConsulta.ToString());
            consulta.SetParameter("sit", situacao);
            consulta.SetParameter("atualizadoEm", DateTime.Now);
            consulta.SetParameterList("idsCliente", ids);
            consulta.SetParameter("situacaoCancelado", Situacao.Cancelado);

            var numAlteracoes = consulta.ExecuteUpdate();

            return numAlteracoes;
        }
    }
}