using Europa.Data.Model;
using Europa.Domain.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Europa.Domain.Treinamento.Models
{
    public class Cliente : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual string Cpf { get; set; }
        public virtual DateTime DataNascimento { get; set; }
        public virtual Situacao Situacao { get; set; }

        public override string ChaveCandidata()
        {
            return Nome;
        }
    }
}
