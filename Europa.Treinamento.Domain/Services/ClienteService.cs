using Europa.Commons;
using Europa.Domain.Core.Enums;
using Europa.Domain.Treinamento.Models;
using Europa.Extensions;
using Europa.Resources;
using Europa.Treinamento.Domain.Repository;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.WebPages;

namespace Europa.Treinamento.Domain.Services
{
    public class ClienteService : BaseService
    {
        public ClienteRepository _clienteRepository { get; set; }
        public EnderecoRepository _enderecoRepository { get; set; }
        public ContatoRepository _contatoRepository { get; set; }

        public Cliente SalvarCliente(Cliente cliente, Endereco endereco)
        {
            var bre = new BusinessRuleException();
            endereco.Id = cliente.Id;

            if (cliente.Nome.IsEmpty())
            {
                bre.Errors.Add(string.Format(GlobalMessages.CampoObrigatorioVazio, "Nome"));
                bre.ErrorsFields.Add("Nome");
            }
            if (cliente.Cpf.IsEmpty())
            {
                bre.Errors.Add(string.Format(GlobalMessages.CampoObrigatorioVazio, "Cpf"));
                bre.ErrorsFields.Add("CpfCliente");
            }
            if (cliente.Cpf.Length != 11)
            {
                bre.Errors.Add(string.Format(GlobalMessages.CampoObrigatorioVazio, "Cpf"));
                bre.ErrorsFields.Add("CpfCliente");
            }
            if (cliente.DataNascimento.IsEmpty())
            {
                bre.Errors.Add(string.Format(GlobalMessages.CampoObrigatorioVazio, "DataNascimentoFiltro"));
                bre.ErrorsFields.Add("DataNascimento");
            }
            if (endereco.Cep.IsEmpty())
            {
                bre.Errors.Add(string.Format(GlobalMessages.CampoObrigatorioVazio, "Cep"));
                bre.ErrorsFields.Add("CepEndereco");
            }
            if (endereco.Cep.Length != 8)
            {
                bre.Errors.Add(string.Format(GlobalMessages.CampoObrigatorioVazio, "Cep"));
                bre.ErrorsFields.Add("CepEndereco");
            }
            if (endereco.Logradouro.IsEmpty())
            {
                bre.Errors.Add(string.Format(GlobalMessages.CampoObrigatorioVazio, "Logradouro"));
                bre.ErrorsFields.Add("Logradouro");
            }
            if (endereco.Numero.IsEmpty())
            {
                bre.Errors.Add(string.Format(GlobalMessages.CampoObrigatorioVazio, "Numero"));
                bre.ErrorsFields.Add("NumeroEndereco");
            }
            if (endereco.Complemento.IsEmpty())
            {
                bre.Errors.Add(string.Format(GlobalMessages.CampoObrigatorioVazio, "Complemento"));
                bre.ErrorsFields.Add("Complemento");
            }
            if (endereco.Bairro.IsEmpty())
            {
                bre.Errors.Add(string.Format(GlobalMessages.CampoObrigatorioVazio, "Bairro"));
                bre.ErrorsFields.Add("Bairro");
            }
            if (endereco.Cidade.IsEmpty())
            {
                bre.Errors.Add(string.Format(GlobalMessages.CampoObrigatorioVazio, "Cidade"));
                bre.ErrorsFields.Add("Cidade");
            }
            if (endereco.Uf.IsEmpty())
            {
                bre.Errors.Add(string.Format(GlobalMessages.CampoObrigatorioVazio, "Uf"));
                bre.ErrorsFields.Add("Uf");
            }

            bre.ThrowIfHasError();

            cliente = PreencherBaseCliente(cliente);
            _clienteRepository.Save(cliente);

            endereco.Cliente = cliente;
            endereco = PreencherBaseEndereco(endereco);
            _enderecoRepository.Save(endereco);

            return cliente;
        }

        public Contato SalvarContato(Contato contato, long clienteId)
        {
            var bre = new BusinessRuleException();
            var cliente = _clienteRepository.FindById(clienteId);

            if (contato.Descricao.IsEmpty())
            {
                bre.Errors.Add(string.Format(GlobalMessages.CampoObrigatorioVazio, "Descricao"));
                bre.ErrorsFields.Add("DescricaoContato");
            }

            if (contato.Principal)
            {
                bool jaExistePrincipal = _contatoRepository.Queryable()
                    .Any(x => x.Cliente.Id == clienteId
                           && x.Tipo == contato.Tipo
                           && x.Principal == true
                           && x.Id != contato.Id);

                if (jaExistePrincipal)
                {
                    bre.Errors.Add("Já existe um contato principal para este tipo.");
                    bre.ErrorsFields.Add("PrincipalContato");
                }
            }
            if (contato.Tipo == TipoContato.Email && contato.Descricao.Length > 254)
            {
                bre.Errors.Add("Email não pode exceder 254 caracteres.");
                bre.ErrorsFields.Add("DescricaoContato");
            }
            else if ((contato.Tipo == TipoContato.Celular || contato.Tipo == TipoContato.Telefone)
                && contato.Descricao.Length != 8)
            {
                bre.Errors.Add("Celular ou Telefone não pode exceder 8 caracteres.");
                bre.ErrorsFields.Add("DescricaoContato");
            }

            bre.ThrowIfHasError();

            contato.Cliente = cliente;
            contato = PreencherBaseContato(contato);
            _contatoRepository.Save(contato);

            return contato;
        }

        private Cliente PreencherBaseCliente(Cliente cliente)
        {
            var obj = cliente;

            if (obj.Id.IsEmpty())
            {
                obj.CriadoEm = DateTime.Now;
                obj.CriadoPor = 1;
            }

            obj.AtualizadoEm = DateTime.Now;
            obj.AtualizadoPor = 1;

            return obj;
        }

        private Endereco PreencherBaseEndereco(Endereco endereco)
        {
            var obj = endereco;

            if (obj.Id.IsEmpty())
            {
                obj.CriadoEm = DateTime.Now;
                obj.CriadoPor = 1;
            }

            obj.AtualizadoEm = DateTime.Now;
            obj.AtualizadoPor = 1;

            return obj;
        }

        private Contato PreencherBaseContato(Contato contato)
        {
            if(contato.Id.IsEmpty())
            {
                contato.CriadoEm = DateTime.Now;
                contato.CriadoPor = 1;
            }

            contato.AtualizadoEm = DateTime.Now;
            contato.AtualizadoPor = 1;

            return contato;
        }
    }
}
