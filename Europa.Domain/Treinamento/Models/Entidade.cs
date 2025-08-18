using Europa.Data.Model;
using Europa.Domain.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Europa.Domain.Treinamento.Models
{
    /*
     * Os nossos modelos devem sempre herdar de BaseEntity
     * para que o sistema funcione corretamente
     */
    public class Entidade : BaseEntity
    {
        /*
         * As propriedades precisam ser públicas e virtuais
         * para que o NHibernate consiga escrever e sobrescever os valores
         * das propriedades
         */
        public virtual string Nome { get; set; }
        public virtual string Sobrenome { get; set; }
        public virtual long Idade { get; set; }
        public virtual DateTime DataNascimento { get; set; }
        public virtual Situacao Situacao { get; set; }
        public virtual Unidade Unidade { get; set; }

        /*
         * BaseEntity força a implementação de um método ChaveCandidata()
         * que retorna uma propriedade que representa o modelo
         * neste caso a chave candidata é a propriedade 'Nome'
         */ 
        public override string ChaveCandidata()
        {
            return Nome;
        }
    }
}
