using Europa.Data.Model;
using Europa.Domain.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Europa.Domain.Treinamento.Models
{
    public class Contato : BaseEntity
    {
        public virtual TipoContato Tipo { get; set; }
        public virtual string Descricao { get; set; }
        public virtual bool Principal { get; set; }
        public virtual Cliente Cliente { get; set; }

        public override string ChaveCandidata()
        {
            return Descricao;
        }
    }
}