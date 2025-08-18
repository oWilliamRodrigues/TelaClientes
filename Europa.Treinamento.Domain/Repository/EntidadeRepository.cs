using Europa.Data;
using Europa.Domain.Core.Enums;
using Europa.Domain.Treinamento.Models;
using Europa.Extensions;
using Europa.Treinamento.Domain.Repository.FiltroDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Europa.Treinamento.Domain.Repository
{
    /*
     * Representa o nosso repositório de dados.
     * Neste caso traz será o meio de transporte de dados entre o banco e a aplicação.
     * Herda de NHibernateRepository e especifica o Tipo de Classe a ser implantado.
     * Realiza as 4 operações do CRUD através do método Save, Delete e Queryable().
     */
    public class _clienteRepository : NHibernateRepository<Entidade>
    {
        /*
         * Método que lista os dados da Tabela Entidade.
         * Recebe um DataSourceRequest vindo do componente Datatable,
         * que contém dados acerca de paginação, ordenação de colunas e etc.
         * Retorna um DataSourceResponse do tipo do modelo
         * Que representa a lista de modelos retornada junto com alguns outros metadados.
         */
        public DataSourceResponse<Entidade> Listar(DataSourceRequest request, EntidadeDTO filtro)
        {
            // Recebe a lista de elementos do banco em forma de IQueryable
            var query = Queryable();

            // Filtramos a consulta de acordo com os parâmetros de filtro, sempre verificando se o valor do filtro é válido antes de filtrar
            if (!filtro.Nome.IsEmpty())
            {
                query = query.Where(x => x.Nome.StartsWith(filtro.Nome));
            }
            if (!filtro.Sobrenome.IsEmpty())
            {
                query = query.Where(x => x.Sobrenome.StartsWith(filtro.Sobrenome));
            }
            if (filtro.DataNascimentoDe.HasValue)
            {
                query = query.Where(x => x.DataNascimento.Date >= filtro.DataNascimentoDe.Value.Date);
            }
            if (filtro.DataNascimentoAte.HasValue)
            {
                query = query.Where(x => x.DataNascimento.Date <= filtro.DataNascimentoAte.Value.Date);
            }
            if (!filtro.Situacao.IsEmpty())
            {
                query = query.Where(x => filtro.Situacao.Contains(x.Situacao));
            }
            if (filtro.Idade.HasValue)
            {
                query = query.Where(x => x.Idade == filtro.Idade.Value);
            }
            if (filtro.IdUnidade.HasValue)
            {
                query = query.Where(x => x.Unidade.Id == filtro.IdUnidade.Value);
            }

            // Retorna a lista de elementos em forma de DataSourceResponse do Tipo Entidade
            return query.ToDataRequest(request);
        }

        public DataSourceResponse<Entidade> Listar(DataSourceRequest request)
        {
            var consulta = Queryable();

            return consulta.ToDataRequest(request);
        }

        public int AlterarSituacaoEmLote(List<long> ids, Situacao situacao)
        {
            // Criamos um string builder para construir a string da consulta em várias linhas
            var stringConsulta = new StringBuilder();

            /*
             * Aqui vamos construir a consulta em HQL, uma linguagem SQL do NHibernate
             * Basicamente, no lugar de tabelas e colunas, usaremos as classes e suas propriedades em C#
             */
            stringConsulta.Append(" UPDATE Entidade ent ");
            // Não podemos esquecer de atualizar, além da situação, a data de atualização dos registros
            stringConsulta.Append(" SET ent.Situacao = :sit,  ent.AtualizadoEm = :atualizadoEm");
            stringConsulta.Append(" WHERE ent.Id IN (:idsEntidade) ");
            // A situação não pode ser igual a que vai ser alterada
            stringConsulta.Append(" AND ent.Situacao != :sit ");
            // Também não podemos alterar a situação de registros já cancelados
            stringConsulta.Append(" AND ent.Situacao != :situacaoCancelado ");

            // Aqui criamos a consulta
            var consulta = Session.CreateQuery(stringConsulta.ToString());
            consulta.SetParameter("sit", situacao);
            consulta.SetParameter("atualizadoEm", DateTime.Now);
            consulta.SetParameterList("idsEntidade", ids);
            consulta.SetParameter("situacaoCancelado", Situacao.Cancelado);

            // Executa a consulta e obtém o número de alterações realizadas
            var numAlteracoes = consulta.ExecuteUpdate();

            return numAlteracoes;
        }
    }
}
