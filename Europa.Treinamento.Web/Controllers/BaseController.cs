using Europa.Commons;
using Europa.Extensions;
using Europa.Web;
using Europa.Web.Mvc;
using NHibernate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Europa.Treinamento.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        public ISession _session { get; set; }
        private ITransaction _transaction;
        private List<string> _errorMessages;
        private List<string> _successMessages;

        protected BaseController()
        {
        }

        protected BaseController(ISession session)
        {
            _session = session;
        }

        protected ISession CurrentSession()
        {
            return _session;
        }

        protected ITransaction CurrentTransaction()
        {
            return _transaction;
        }

        protected void AddSuccessMessage(string message)
        {
            _successMessages.Add(message);
        }

        protected void ClearMessages()
        {
            _successMessages = new List<String>();
            _errorMessages = new List<String>();
        }

        protected void AddErrorMessage(string message)
        {
            _errorMessages.Add(message);
        }

        protected bool HasErrorMessage()
        {
            return _errorMessages.IsEmpty() == false;
        }

        public ActionResult JsonResponseWrapper(object result, bool allowGet = true)
        {
            var response = new JsonResponse
            {
                Sucesso = true,
                Objeto = result
            };
            return Json(response, allowGet ? JsonRequestBehavior.DenyGet : JsonRequestBehavior.AllowGet);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            if (!_transaction.IsNull() && _transaction.IsActive)
            {
                _transaction.Rollback();
            }

            filterContext.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                //FIXME: Use ViewModel to respond. 
                //FIXME: Use HTTP error codes instead Success
                filterContext.Result = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new
                    {
                        Success = false,
                        Message = filterContext.Exception.Message,
                        StackTrace = filterContext.Exception.StackTrace
                    }
                };
                filterContext.ExceptionHandled = true;
            }
            else
            {
                //FIXME: Verificar como usar o System.Web.Mvc.HandleErrorInfo para tratar isso.
                var view = new ViewResult { ViewName = "Error" };
                view.ViewBag.Message = filterContext.Exception.Message;
                view.ViewBag.StackTrace = filterContext.Exception.StackTrace;
                filterContext.Result = view;
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ClearMessages();
            _transaction = !filterContext.ActionDescriptor.GetCustomAttributes(typeof(NoTransactionActionFilterAttribute), false)
                .Any() ? CurrentSession().BeginTransaction() : null;
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception.IsNull() && !_transaction.IsNull() && _transaction.IsActive)
            {
                _transaction.Commit();
            }
            else if (!filterContext.Exception.IsNull() && _transaction.IsActive)
            {
                _transaction.Rollback();
                _transaction.Dispose();
            }

            ViewBag.ErrorMessages = _errorMessages;
            ViewBag.SuccessMessages = _successMessages;

            DesabilitarCacheBrowser(filterContext);
        }

        private void DesabilitarCacheBrowser(ActionExecutedContext filterContext)
        {
            filterContext.HttpContext.Response.Buffer = true;
            filterContext.HttpContext.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            filterContext.HttpContext.Response.Expires = 0;
            filterContext.HttpContext.Response.CacheControl = "no-cache";
        }

        public string RenderRazorViewToString(string viewName, object model, bool detailMode)
        {
            ViewData.Model = model;

            ViewBag.ErrorMessages = _errorMessages;
            ViewBag.SuccessMessages = _successMessages;


            if (!model.IsNull() && detailMode)
            {
                ViewData.Model = model;
                ViewData.Add("detailMode", true);
            }

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                var aux = sw.GetStringBuilder().ToString();
                return aux;
            }
        }

        protected List<string> ModelStateErrors(ModelStateDictionary modelState)
        {
            var query = from state in modelState.Values
                        from error in state.Errors
                        select error.ErrorMessage;

            return query.ToList();
        }

        protected void HandleBusinessRuleException(BusinessRuleException exc)
        {
            _errorMessages.AddRange(exc.Errors);
        }

    }
}