using Europa.Data;
using Europa.Domain.Treinamento.Models.Views;
using Europa.Extensions;
using Europa.Resources;
using Europa.Treinamento.Domain.Repository.FiltroDto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Europa.Treinamento.Domain.Repository
{
    public class ViewClienteRepository : NHibernateRepository<ViewCliente>
    {
        public DataSourceResponse<ViewCliente> Listar(DataSourceRequest request, ViewClienteDTO filtro)
        {
            var query = Queryable();

            if (!filtro.Nome.IsEmpty())
            {
                query = query.Where(x => x.Nome.ToLower().Contains(filtro.Nome.ToLower()));
            }

            if (!filtro.Endereco.IsEmpty())
            {
                query = query.Where(x => x.Cidade.ToLower().Contains(filtro.Endereco.ToLower()) || x.Uf.ToLower().Contains(filtro.Endereco.ToLower()));
            }

            if (filtro.Situacao != null && filtro.Situacao.Any())
            {
                query = query.Where(x => filtro.Situacao.Contains(x.Situacao));
            }

            return query.ToDataRequest(request);
        }

        public DataSourceResponse<ViewCliente> ListarTodos(DataSourceRequest request)
        {
            var consulta = Queryable();

            return consulta.ToDataRequest(request);
        }

        public byte[] Exportar(DataSourceRequest request, ViewClienteDTO filtro)
        {
            ExcelUtil excel = ExcelUtil.NewInstance(23)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(GetHeader());

            var list = filtro != null ? Listar(request, filtro) : ListarTodos(request);

            const string formatDate = "dd/MM/yyyy";
            const string empty = "";

            foreach (var model in list.records.ToList())
            {
                excel
                    .CreateCellValue(model.Nome).Width(25)
                    .CreateCellValue(model.CpfCliente).Width(25)
                    .CreateCellValue(model.DataNascimento).Width(25).Format(formatDate)
                    .CreateCellValue(model.Cidade).Width(20)
                    .CreateCellValue(model.Uf).Width(10)
                    .CreateCellValue(model.Telefone).Width(20)
                    .CreateCellValue(model.Celular).Width(20)
                    .CreateCellValue(model.Email).Width(20)
                    .CreateCellValue(model.Situacao.AsString()).Width(15);
            }

            excel.Close();
            return excel.DownloadFile();
        }

        private string[] GetHeader()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.Nome,
                GlobalMessages.Cpf,
                GlobalMessages.DataNascimento,
                GlobalMessages.Cidade,
                GlobalMessages.Estado,
                GlobalMessages.Telefone,
                GlobalMessages.Celular,
                GlobalMessages.Email,
                GlobalMessages.Situacao
            };
            return header.ToArray();
        }
    }
}
