using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Reflection;
using System.Net.Http;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Net;
using AngularJSDataModels;
using AngularJSWebApplication.Helpers;

namespace AngularJSWebApplication.Filters
{    

    public class ValidateModelStateAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {

             HttpContext ctx = default(HttpContext);
             ctx = HttpContext.Current;
          
            var request = actionContext.Request;
            if (!actionContext.ModelState.IsValid)
            {
                TransactionalInformation transactionInformation = new TransactionalInformation();

                transactionInformation.ReturnMessage = ModelStateHelper.ReturnErrorMessages(actionContext.ModelState.Values);
                transactionInformation.ValidationErrors = ModelStateHelper.ReturnValidationErrors(actionContext.ModelState.Values);
                transactionInformation.ReturnStatus = false;
                transactionInformation.IsAuthenicated = ctx.User.Identity.IsAuthenticated;
                actionContext.Response = request.CreateResponse<TransactionalInformation>(HttpStatusCode.BadRequest, transactionInformation);

            }
      
        }

    }
}