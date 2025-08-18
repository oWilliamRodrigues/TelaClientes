using Europa.Commons;
using Europa.Domain.Core.Enums;
using Europa.Domain.Treinamento.Models;
using Europa.Extensions;
using Europa.Resources;
using Europa.Treinamento.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Europa.Treinamento.Domain.Services
{
    public class EntidadeService : BaseService
    {
        public _clienteRepository EntidadeRepository { get; set; }

        public Entidade SalvarEntidade(Entidade entidade)
        {
            /* Aqui instanciamos o BusinessRuleException, uma exceção
             * do framework para regras de negócio
             */
            var bre = new BusinessRuleException();

            /* Iremos verificar se as propriedades da entidade estão vazias
             * ou com valores inválidos de acordo com as regreas de negócio
             * da entidade.
             * Para cada regra de negócio quebrada iremos adicionar uma mensagem de erro e o Id HTML
             * dos campos inválidos
             */ 
            if(entidade.DataNascimento.IsEmpty())
            {
                bre.Errors.Add(string.Format(GlobalMessages.CampoObrigatorioVazio, GlobalMessages.DataNascimento));
                bre.ErrorsFields.Add("DataNascimento");
            }
            if(entidade.Idade.IsEmpty())
            {
                bre.Errors.Add(string.Format(GlobalMessages.CampoObrigatorioVazio, GlobalMessages.Idade));
                bre.ErrorsFields.Add("Idade");
            }
            if(entidade.Nome.IsEmpty())
            {
                bre.Errors.Add(string.Format(GlobalMessages.CampoObrigatorioVazio, GlobalMessages.Nome));
                bre.ErrorsFields.Add("Nome");
            }
            if(entidade.Sobrenome.IsEmpty())
            {
                bre.Errors.Add(string.Format(GlobalMessages.CampoObrigatorioVazio, GlobalMessages.Sobrenome));
                bre.ErrorsFields.Add("Sobrenome");
            }
            if (entidade.Unidade.IsEmpty())
            {
                bre.Errors.Add(string.Format(GlobalMessages.CampoObrigatorioVazio, GlobalMessages.Unidade));
                bre.ErrorsFields.Add("Unidade.Id");
            }

            // Lançamos a exceção caso exista alguma mensagem de erro na lista de erros
            bre.ThrowIfHasError();

            // Se a exceção não for lançada, continuamos com o processo de salvar a entidade
            entidade = PreencherBaseEntity(entidade);
            EntidadeRepository.Save(entidade);

            // Retornamos a entidade depois de salvá-la para algum procedimento adicional
            return entidade;
        }

        private Entidade PreencherBaseEntity(Entidade entidade)
        {
            var obj = entidade;
            
            if(obj.Id.IsEmpty())
            {
                obj.CriadoEm = DateTime.Now;
                obj.CriadoPor = 1;
            }

            obj.AtualizadoEm = DateTime.Now;
            obj.AtualizadoPor = 1;

            return obj;
        }
    }
}
