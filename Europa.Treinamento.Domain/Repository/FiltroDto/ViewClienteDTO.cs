using Europa.Domain.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Europa.Treinamento.Domain.Repository.FiltroDto
{
    public class ViewClienteDTO
    {
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public List<Situacao> Situacao { get; set; }
        public long? IdCliente { get; set; }
    }
}
