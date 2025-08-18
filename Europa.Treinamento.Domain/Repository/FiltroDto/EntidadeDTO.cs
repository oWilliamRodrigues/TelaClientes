using Europa.Domain.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Europa.Treinamento.Domain.Repository.FiltroDto
{
    public class EntidadeDTO
    {
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public DateTime? DataNascimentoDe { get; set; }
        public DateTime? DataNascimentoAte { get; set; }
        public List<Situacao> Situacao { get; set; }
        public int? Idade { get; set; }
        public long? IdUnidade { get; set; }


    }
}
