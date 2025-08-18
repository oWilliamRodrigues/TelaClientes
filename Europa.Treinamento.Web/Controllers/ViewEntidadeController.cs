using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Treinamento.Domain.Repository;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Europa.Treinamento.Web.Controllers
{
    public class ViewEntidadeController : BaseController
    {
        public ViewEntidadeRepository ViewEntidadeRepository { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Listar(DataSourceRequest request)
        {
            var lista = ViewEntidadeRepository.Listar(request);
            var resposta = new JsonResponse();

            resposta.Sucesso = true;
            resposta.Objeto = lista;

            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportarPagina(DataSourceRequest request)
        {
            byte[] file = ViewEntidadeRepository.Exportar(request);
            string nomeArquivo = GlobalMessages.RelatorioEntidades;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

        public ActionResult ExportarTodos(DataSourceRequest request)
        {
            var modifiedRequest = request;
            modifiedRequest.start = 0;
            modifiedRequest.pageSize = 0;

            byte[] file = ViewEntidadeRepository.Exportar(modifiedRequest);
            string nomeArquivo = GlobalMessages.RelatorioEntidades;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }
    }
}