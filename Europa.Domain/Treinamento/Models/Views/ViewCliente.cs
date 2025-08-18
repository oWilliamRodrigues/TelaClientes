using Europa.Data.Model;
using Europa.Domain.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Europa.Domain.Treinamento.Models.Views
{
    public class ViewCliente : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual string CpfCliente { get; set; }
        public virtual DateTime DataNascimento { get; set; }
        public virtual string Endereco { get; set; }
        public virtual string CepEndereco { get; set; }
        public virtual string Logradouro { get; set; }
        public virtual long NumeroEndereco { get; set; }
        public virtual string Complemento { get; set; }
        public virtual string Bairro { get; set; }
        public virtual string Cidade { get; set; }
        public virtual string Uf { get; set; }
        public virtual string Email { get; set; }
        public virtual string Celular { get; set; }
        public virtual string Telefone { get; set; }
        public virtual Situacao Situacao { get; set; }
        public virtual TipoContato Tipo { get; set; }
        public virtual Cliente Cliente { get; set; }

        public override string ChaveCandidata()
        {
            return Nome;
        }
    }
}
