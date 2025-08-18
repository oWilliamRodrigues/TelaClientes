using Europa.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Europa.Domain.Treinamento.Models
{
    public class Endereco : BaseEntity
    {
        public virtual string Cep { get; set; }
        public virtual string Logradouro { get; set; }
        public virtual long Numero { get; set; }
        public virtual string Complemento { get; set; }
        public virtual string Bairro { get; set; }
        public virtual string Uf { get; set; }
        public virtual string Cidade { get; set; }
        public virtual Cliente Cliente { get; set; }
        public override string ChaveCandidata()
        {
            return Cep;
        } 
    }
}
