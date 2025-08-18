using Europa.Data;
using Europa.Domain.Treinamento.Models;
using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Europa.Treinamento.Domain.Repository
{
    public class EnderecoRepository : NHibernateRepository<Endereco>
    {
        public DataSourceResponse<Endereco> Listar(DataSourceRequest request)
        {
            var consulta = Queryable();

            return consulta.ToDataRequest(request);
        }
    }
}
