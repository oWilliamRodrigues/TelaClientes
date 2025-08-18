using Europa.Data.Model;
using Europa.Domain.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Europa.Domain.Treinamento.Models.Views
{
    public class ViewEntidade : BaseEntity
    {
        public virtual string NomeEntidade { get; set; }
        public virtual string SobrenomeEntidade { get; set; }
        public virtual long Idade { get; set; }
        public virtual DateTime DataNascimento { get; set; }
        public virtual long IdUnidade { get; set; }
        public virtual string NomeUnidade { get; set; }
        public virtual Situacao Situacao { get; set; }
        
        public override string ChaveCandidata()
        {
            return NomeEntidade;
        }
    }
}
