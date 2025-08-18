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
using System.Web.Mvc;


namespace Europa.Treinamento.Web.Controllers
{
    public class ViewClienteController : BaseController
    {

        public ViewClienteRepository _viewClienteRepository { get; set; }
        public EnderecoRepository _enderecoRepository { get; set; }
        public ContatoRepository _contatoRepository { get; set; }
        public ClienteRepository _clienteRepository { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ListarClientes(DataSourceRequest request, ViewClienteDTO filtro)
        {
            var lista = _viewClienteRepository.Listar(request, filtro);
            var resposta = new JsonResponse();

            resposta.Sucesso = true;
            resposta.Objeto = lista;

            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListarEnderecos(DataSourceRequest request)
        {
            var lista = _enderecoRepository.Listar(request);
            var resposta = new JsonResponse();

            resposta.Sucesso = true;
            resposta.Objeto = lista;

            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ReativarClientesEmLote(List<long> ids)
        {
            return AlterarSituacaoEmLote(ids, Situacao.Ativo);
        }

        [HttpPost]
        public JsonResult SuspenderClientesEmLote(List<long> ids)
        {
            return AlterarSituacaoEmLote(ids, Situacao.Suspenso);
        }

        [HttpPost]
        public JsonResult CancelarClientesEmLote(List<long> ids)
        {
            return AlterarSituacaoEmLote(ids, Situacao.Cancelado);
        }

        private JsonResult AlterarSituacaoEmLote(List<long> ids, Situacao situacao)
        {
            var resposta = new JsonResponse();
            string stringSituacao = null;

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

            var numAlteracoes = _clienteRepository.AlterarSituacaoEmLote(ids, situacao);
            resposta.Sucesso = true;

            resposta.Mensagens.Add(string.Format(GlobalMessages.NRegistrosAtualizados, stringSituacao, numAlteracoes, ids.Count));

            return Json(resposta, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportarPagina(DataSourceRequest request, ViewClienteDTO filtro)
        {
            byte[] file = _viewClienteRepository.Exportar(request, filtro);
            string nomeArquivo = GlobalMessages.RelatorioClientes;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

        public ActionResult ExportarTodos(DataSourceRequest request)
        {
            var modifiedRequest = request;
            modifiedRequest.start = 0;
            modifiedRequest.pageSize = 0;

            byte[] file = _viewClienteRepository.Exportar(modifiedRequest, null);
            string nomeArquivo = GlobalMessages.RelatorioClientes;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }
    }
}