using Europa.Commons;
using Europa.Data;
using Europa.Domain.Core.Enums;
using Europa.Domain.Treinamento.Models;
using Europa.Extensions;
using Europa.Resources;
using Europa.Treinamento.Domain.Repository;
using Europa.Treinamento.Domain.Repository.FiltroDto;
using Europa.Treinamento.Domain.Services;
using Europa.Web;
using NHibernate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Europa.Treinamento.Web.Controllers
{
    public class EntidadeController : BaseController
    {
        public _clienteRepository EntidadeRepository { get; set; }
        public EntidadeService EntidadeService { get; set; }
        public UnidadeRepository UnidadeRepository { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ListarEntidades(DataSourceRequest request, EntidadeDTO filtro)
        {
            var lista = EntidadeRepository.Listar(request, filtro);
            var resposta = new JsonResponse();

            resposta.Sucesso = true;
            resposta.Objeto = lista;

            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListarUnidades(DataSourceRequest request)
        {
            var lista = UnidadeRepository.Listar(request);
            var resposta = new JsonResponse();

            resposta.Sucesso = true;
            resposta.Objeto = lista;

            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SalvarEntidade(Entidade entidade)
        {
            var resposta = new JsonResponse();

            /*
             * Vamos tentar adicionar uma nova entidade,
             * ela passará por validações antes de ser persistida no banco de dados
             * Se ela houver algum erro, o EntidadeService lançará um BusinessRuleException com todos
             * os erros de negócio
             */
            try
            {
                // Aqui tentamos salvar
                EntidadeService.SalvarEntidade(entidade);

                // Se o EntidadeService salvar a entidade sem erros, o fluxo do código nos levará até esta linha
                // Senão o 'catch' pegará a exceção e tratará de acordo com as instruções dentro do bloco
                resposta.Sucesso = true;
                resposta.Objeto = entidade;
                // Adiciona uma mensagem de sucesso
                resposta.Mensagens.Add(string.Format(GlobalMessages.RegistroSucesso, entidade.ChaveCandidata(), GlobalMessages.Incluido));
            }
            catch (BusinessRuleException bre)
            {
                // Se cair aqui, houve erro, portanto iremos adicionar ao JsonResponse todos os erros e campos com erro 
                // vindo do BusinessRuleException
                resposta.Mensagens.AddRange(bre.Errors);
                resposta.Campos.AddRange(bre.ErrorsFields);
            }

            // Agora é só retornar a resposta em JSON
            return Json(resposta, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ExcluirEntidade(long id)
        {
            // Prepara a resposta
            var resposta = new JsonResponse();
            // Busca a entidade por Id
            var obj = EntidadeRepository.FindById(id);

            try
            {
                // Tenta deletar a entidade
                // Se ela for utilizada em algum lugar, o método lançará um GenericADOException
                EntidadeRepository.Delete(obj);
                // Se passar pelo método de deletar sem erros, inserimos uma mensagem de sucesso na resposta
                resposta.Mensagens.Add(GlobalMessages.RegistroIncluidoOuAlteradoSucesso.FormatString(obj.Nome, GlobalMessages.Excluido.ToLower()));
                resposta.Sucesso = true;
            }

            catch (GenericADOException ex)
            {
                // Verificamos se a exceção lançada se refere à um registro em uso por outra entidade
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(ex))
                {
                    // Se for, adicionamos a mensagem de erro de entidade em uso por outra entidade
                    resposta.Mensagens.Add(String.Format(GlobalMessages.ErroViolacaoConstraint, obj.Nome));
                }
                else
                {
                    // Senão, adicionamos uma mensagem de erro desconhecido
                    resposta.Mensagens.Add(String.Format(GlobalMessages.ErroNaoTratado, ex.Message));
                }

                // Desfazemos a transação em caso de erro
                EntidadeRepository._session.Transaction.Rollback();
            }

            return Json(resposta, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ReativarEntidadesEmLote(List<long> ids)
        {
            return AlterarSituacaoEmLote(ids, Situacao.Ativo);
        }

        [HttpPost]
        public JsonResult SuspenderEntidadesEmLote(List<long> ids)
        {
            return AlterarSituacaoEmLote(ids, Situacao.Suspenso);
        }

        [HttpPost]
        public JsonResult CancelarEntidadesEmLote(List<long> ids)
        {
            return AlterarSituacaoEmLote(ids, Situacao.Cancelado);
        }

        /*
         * Recebe uma lista de ids de Entidade e um valor do tipo enumerado Situacao e 
         * altera a situação das entidades em contexto
         */ 
        private JsonResult AlterarSituacaoEmLote(List<long> ids, Situacao situacao)
        {
            // Instância uma resposta
            var resposta = new JsonResponse();
            string stringSituacao = null;

            // Verifica qual é o valor do Tipo Situacao e preenche uma string com a ação a ser executada
            switch (situacao)
            {
                case Situacao.Ativo:
                    stringSituacao = GlobalMessages.Ativados;
                    break;
                case Situacao.Cancelado:
                    stringSituacao = GlobalMessages.Cancelados;
                    break;
                case Situacao.Suspenso:
                    stringSituacao = GlobalMessages.Suspensos;
                    break;
            }

            // Obtém o número de alterações realizadas
            var numAlteracoes = EntidadeRepository.AlterarSituacaoEmLote(ids, situacao);
            resposta.Sucesso = true;
            // Preenche a mensagem de retorno
            resposta.Mensagens.Add(string.Format(GlobalMessages.NRegistrosAtualizados, stringSituacao, numAlteracoes, ids.Count));

            return Json(resposta, JsonRequestBehavior.AllowGet);
        }
    }
}