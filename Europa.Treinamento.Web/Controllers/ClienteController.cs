using Europa.Commons;
using Europa.Data;
using Europa.Domain.Core.Enums;
using Europa.Domain.Treinamento.Models;
using Europa.Domain.Treinamento.Models.Views;
using Europa.Extensions;
using Europa.Resources;
using Europa.Treinamento.Domain.Repository;
using Europa.Treinamento.Domain.Repository.FiltroDto;
using Europa.Treinamento.Domain.Services;
using Europa.Web;
using NHibernate.Exceptions;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Europa.Treinamento.Web.Controllers
{
    public class ClienteController : BaseController
    {
        public ClienteRepository _clienteRepository { get; set; }
        public ContatoRepository _contatoRepository { get; set; }
        public EnderecoRepository _enderecoRepository { get; set; }
        public ViewClienteRepository _viewClienteRepository { get; set; }
        public ClienteService _clienteService { get; set; }

        public JsonResult ListarContatos(DataSourceRequest request, int? filtro)
        {
            var lista = _contatoRepository.Listar(request, filtro);
            var resposta = new JsonResponse();

            resposta.Sucesso = true;
            resposta.Objeto = lista;

            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SalvarCliente(Cliente cliente, Endereco endereco)
        {
            var resposta = new JsonResponse();

            try
            {
                var objCliente = _clienteService.SalvarCliente(cliente, endereco);

                resposta.Sucesso = true;
                resposta.Objeto = objCliente;
                resposta.Mensagens.Add(string.Format(GlobalMessages.RegistroSucesso, cliente.ChaveCandidata(), GlobalMessages.Incluido));
            }
            catch (BusinessRuleException bre)
            {
                resposta.Mensagens.AddRange(bre.Errors);
                resposta.Campos.AddRange(bre.ErrorsFields);
            }

            return Json(resposta, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SalvarContato(Contato contato, long clienteId)
        {
            var resposta = new JsonResponse();

            try
            {
                _clienteService.SalvarContato(contato, clienteId);

                resposta.Sucesso = true;
                resposta.Objeto = contato;
                resposta.Mensagens.Add(string.Format(GlobalMessages.RegistroSucesso, contato.ChaveCandidata(), GlobalMessages.Incluido));
            }
            catch (BusinessRuleException bre)
            {
                resposta.Mensagens.AddRange(bre.Errors);
                resposta.Campos.AddRange(bre.ErrorsFields);
            }

            return Json(resposta, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ExcluirCliente(long id)
        {
            var resposta = new JsonResponse();

            var obj = _clienteRepository.FindById(id);

            try
            {
                _clienteRepository.Delete(obj);

                resposta.Mensagens.Add(GlobalMessages.RegistroIncluidoOuAlteradoSucesso.FormatString(obj.Nome, GlobalMessages.Excluido.ToLower()));
                resposta.Sucesso = true;
            }
            catch (GenericADOException ex)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(ex))
                {
                    resposta.Mensagens.Add(String.Format(GlobalMessages.ErroViolacaoConstraint, obj.Nome));
                }
                else
                {
                    resposta.Mensagens.Add(String.Format(GlobalMessages.ErroNaoTratado, ex.Message));
                }

                _clienteRepository._session.Transaction.Rollback();
            }

            return Json(resposta, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ExcluirContato(long id)
        {
            var resposta = new JsonResponse();

            var obj = _contatoRepository.FindById(id);

            try
            {
                _contatoRepository.Delete(obj);

                resposta.Mensagens.Add(GlobalMessages.RegistroIncluidoOuAlteradoSucesso.FormatString(obj.Descricao, GlobalMessages.Excluido.ToLower()));
                resposta.Sucesso = true;
            }
            catch (GenericADOException ex)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(ex))
                {
                    resposta.Mensagens.Add(String.Format(GlobalMessages.ErroViolacaoConstraint, obj.Descricao));
                }
                else
                {
                    resposta.Mensagens.Add(String.Format(GlobalMessages.ErroNaoTratado, ex.Message));
                }

                _contatoRepository._session.Transaction.Rollback();
            }

            return Json(resposta, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Detalhes(long id)
        {
            var cliente = _viewClienteRepository.FindById(id);
            return View("Index", cliente);
        }

        public ActionResult Criar()
        {
            return View("Index");
        }
    }
}