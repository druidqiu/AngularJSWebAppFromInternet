using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;

using AngularJSWebApplication.Filters;
using AngularJSWebApplication.WebApiModels;
using AngularJSDataModels;
using AngularJSApplicationService;
using AngularJSDataServiceInterface;
using AngularJSDataService;


namespace AngularJSWebApplication.WebApiControllers
{
    [RoutePrefix("api/main")]
    public class MainApiController : ApiController
    {

        IApplicationDataService applicationDataService;       

        /// <summary>
        /// Constructor with Dependency Injection using Ninject
        /// </summary>
        /// <param name="dataService"></param>
        public MainApiController()
        {           
            applicationDataService = new ApplicationDataService();
        }


          /// <summary>
        /// Initialize Application
        /// </summary>
        /// <returns></returns>
        [Route("AuthenicateUser")]
        [HttpGet]
        public HttpResponseMessage AuthenicateUser([FromUri] string route)
        {
            TransactionalInformation transaction = new TransactionalInformation();
            transaction.IsAuthenicated = User.Identity.IsAuthenticated;
            var response = Request.CreateResponse<TransactionalInformation>(HttpStatusCode.OK, transaction);
            return response;

        }

        [Route("Logout")]
        [HttpGet]
        public HttpResponseMessage Logout()
        {
            TransactionalInformation transaction = new TransactionalInformation();
            FormsAuthentication.SignOut();
            transaction.IsAuthenicated = false;
            var response = Request.CreateResponse<TransactionalInformation>(HttpStatusCode.OK, transaction);
            return response;

        }

        /// <summary>
        /// Initialize Application
        /// </summary>
        /// <returns></returns>
        [Route("InitializeApplication")]
        [HttpGet]   
        public HttpResponseMessage InitializeApplication()
        {

            ApplicationApiModel applicationWebApiModel = new ApplicationApiModel();
            TransactionalInformation transaction = new TransactionalInformation();
            ApplicationInitializationBusinessService initializationBusinessService;

            initializationBusinessService = new ApplicationInitializationBusinessService(applicationDataService);
            initializationBusinessService.InitializeApplication(out transaction);

            if (transaction.ReturnStatus == false)
            {
                applicationWebApiModel.ReturnMessage = transaction.ReturnMessage;
                applicationWebApiModel.ReturnStatus = transaction.ReturnStatus;
                var badResponse = Request.CreateResponse<ApplicationApiModel>(HttpStatusCode.BadRequest, applicationWebApiModel);
                return badResponse;
            }

            initializationBusinessService = new ApplicationInitializationBusinessService(applicationDataService);
            List<ApplicationMenu> menuItems = initializationBusinessService.GetMenuItems(User.Identity.IsAuthenticated, out transaction);

            if (transaction.ReturnStatus == false)
            {
                applicationWebApiModel.ReturnMessage = transaction.ReturnMessage;
                applicationWebApiModel.ReturnStatus = transaction.ReturnStatus;
                var badResponse = Request.CreateResponse<ApplicationApiModel>(HttpStatusCode.BadRequest, applicationWebApiModel);
                return badResponse;
            }

            applicationWebApiModel.ReturnMessage.Add("Application has been initialized.");
            applicationWebApiModel.ReturnStatus = transaction.ReturnStatus;
            applicationWebApiModel.MenuItems = menuItems;
            applicationWebApiModel.IsAuthenicated = User.Identity.IsAuthenticated;

            var response = Request.CreateResponse<ApplicationApiModel>(HttpStatusCode.OK, applicationWebApiModel);
            return response;
            
                
        }

    }


}
