using Europa.Data;
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
    public class ContatoRepository : NHibernateRepository<Contato>
    {
        public DataSourceResponse<Contato> Listar(DataSourceRequest request, int? filtro)
        {
            var consulta = Queryable();

            if (filtro != null)
            {
                consulta = consulta.Where(x => x.Cliente.Id == filtro);
            }

            return consulta.ToDataRequest(request);
        }
    }
}
