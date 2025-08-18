using Europa.Data;
using Europa.Domain.Treinamento.Models.Views;
using Europa.Extensions;
using Europa.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Europa.Treinamento.Domain.Repository
{
    public class ViewEntidadeRepository : NHibernateRepository<ViewEntidade>
    {
        public DataSourceResponse<ViewEntidade> Listar(DataSourceRequest request)
        {
            var query = Queryable();

            return query.ToDataRequest(request);
        }

        public byte[] Exportar(DataSourceRequest request)
        {
            ExcelUtil excel = ExcelUtil.NewInstance(23)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(GetHeader());

            var list = Listar(request);

            const string formatDate = "dd/MM/yyyy";
            const string formatTime = "HH:mm:ss";
            const string empty = "";

            foreach (var model in list.records.ToList())
            {
                excel
                    .CreateCellValue(model.NomeEntidade).Width(25)
                    .CreateCellValue(model.SobrenomeEntidade).Width(25)
                    .CreateCellValue(model.Idade).Width(25)
                    .CreateCellValue(model.DataNascimento).Format(formatDate).Width(25)
                    .CreateCellValue(model.NomeUnidade).Width(25)
                    .CreateCellValue(model.Situacao.AsString()).Width(25);


            };
            excel.Close();
            return excel.DownloadFile();
        }

        private string[] GetHeader()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.Nome,
                GlobalMessages.Sobrenome,
                GlobalMessages.Idade,
                GlobalMessages.DataNascimento,
                GlobalMessages.Unidade,
                GlobalMessages.Situacao
            };
            return header.ToArray();
        }
    }
}
