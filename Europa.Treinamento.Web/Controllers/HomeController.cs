using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Europa.Domain.Core.Enums;
using Europa.Treinamento.Domain.Services;
using Europa.Treinamento.Web.Controllers;

namespace Europa.AppCliente.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}