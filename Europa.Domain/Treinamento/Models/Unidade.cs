using Europa.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Europa.Domain.Treinamento.Models
{
    public class Unidade : BaseEntity
    {
        public virtual string Nome { get; set; }

        public override string ChaveCandidata()
        {
            return Nome;
        }
    }
}
