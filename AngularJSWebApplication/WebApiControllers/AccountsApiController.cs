using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;

using AngularJSWebApplication.Filters;
using AngularJSWebApplication.WebApiModels;
using AngularJSWebApplication.Helpers;
using AngularJSDataModels;
using AngularJSApplicationService;
using AngularJSDataServiceInterface;
using AngularJSDataService;

namespace AngularJSWebApplication.WebApiControllers
{

    [RoutePrefix("api/accounts")]
    public class AccountsApiController : ApiController
    {

        IAccountsDataService accountsDataService;
        IApplicationDataService applicationDataService;

        /// <summary>
        /// Constructor with Dependency Injection using Ninject
        /// </summary>
        /// <param name="dataService"></param>
        public AccountsApiController()
        {
            accountsDataService = new AccountsDataService();
            applicationDataService = new ApplicationDataService();
        }

        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="request"></param>
        /// <param name="registerUserDTO"></param>
        /// <returns></returns>
        [Route("RegisterUser")]
        [ValidateModelState]
        [HttpPost]
        public HttpResponseMessage RegisterUser(HttpRequestMessage request, [FromBody] UserDTO registerUserDTO)
        {

            AccountsApiModel accountsWebApiModel = new AccountsApiModel();
            TransactionalInformation transaction = new TransactionalInformation();
            AccountsBusinessService accountsBusinessService;
          
            if (registerUserDTO.FirstName == null) registerUserDTO.FirstName = "";
            if (registerUserDTO.LastName == null) registerUserDTO.LastName = "";
            if (registerUserDTO.EmailAddress == null) registerUserDTO.EmailAddress = "";
            if (registerUserDTO.UserName == null) registerUserDTO.UserName = "";
            if (registerUserDTO.Password == null) registerUserDTO.Password = "";
            if (registerUserDTO.PasswordConfirmation == null) registerUserDTO.PasswordConfirmation = "";
      
            accountsBusinessService = new AccountsBusinessService(accountsDataService);
            User user = accountsBusinessService.RegisterUser(
                registerUserDTO.FirstName, 
                registerUserDTO.LastName,
                registerUserDTO.UserName,
                registerUserDTO.EmailAddress,                
                registerUserDTO.Password, 
                registerUserDTO.PasswordConfirmation, 
                out transaction);

            if (transaction.ReturnStatus == false)
            {
                accountsWebApiModel.ReturnMessage = transaction.ReturnMessage;
                accountsWebApiModel.ReturnStatus = transaction.ReturnStatus;
                accountsWebApiModel.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse<AccountsApiModel>(HttpStatusCode.BadRequest, accountsWebApiModel);
                return badResponse;
            }

            ApplicationInitializationBusinessService initializationBusinessService;
            initializationBusinessService = new ApplicationInitializationBusinessService(applicationDataService);
            List<ApplicationMenu> menuItems = initializationBusinessService.GetMenuItems(true, out transaction);

            if (transaction.ReturnStatus == false)
            {
                accountsWebApiModel.ReturnMessage = transaction.ReturnMessage;
                accountsWebApiModel.ReturnStatus = transaction.ReturnStatus;
                var badResponse = Request.CreateResponse<AccountsApiModel>(HttpStatusCode.BadRequest, accountsWebApiModel);
                return badResponse;
            }

            accountsWebApiModel.IsAuthenicated = true;
            accountsWebApiModel.ReturnStatus = transaction.ReturnStatus;         
            accountsWebApiModel.ReturnMessage.Add("Register User successful.");
            accountsWebApiModel.MenuItems = menuItems;
            accountsWebApiModel.User = user;

            FormsAuthentication.SetAuthCookie(user.UserId.ToString(), createPersistentCookie: false);

            var response = Request.CreateResponse<AccountsApiModel>(HttpStatusCode.OK, accountsWebApiModel);
            return response;


        }


        [Route("UpdateUser")]
        [WebApiAuthenication]
        [ValidateModelState]
        [HttpPost]
        public HttpResponseMessage UpdateUser(HttpRequestMessage request, [FromBody] UserDTO updateUserDTO)
        {

            Guid userID = new Guid(User.Identity.Name);

            AccountsApiModel accountsWebApiModel = new AccountsApiModel();
            TransactionalInformation transaction = new TransactionalInformation();
            AccountsBusinessService accountsBusinessService;
            accountsWebApiModel.IsAuthenicated = true;

            if (updateUserDTO.FirstName == null) updateUserDTO.FirstName = "";
            if (updateUserDTO.LastName == null) updateUserDTO.LastName = "";
            if (updateUserDTO.EmailAddress == null) updateUserDTO.EmailAddress = "";
            if (updateUserDTO.UserName == null) updateUserDTO.UserName = "";
            if (updateUserDTO.Password == null) updateUserDTO.Password = "";
            if (updateUserDTO.PasswordConfirmation == null) updateUserDTO.PasswordConfirmation = "";

            accountsBusinessService = new AccountsBusinessService(accountsDataService);
            User user = accountsBusinessService.UpdateUser(
                userID,
                updateUserDTO.FirstName,
                updateUserDTO.LastName,
                updateUserDTO.UserName,
                updateUserDTO.EmailAddress,
                updateUserDTO.Password,
                updateUserDTO.PasswordConfirmation,
                out transaction);

            if (transaction.ReturnStatus == false)
            {
                accountsWebApiModel.ReturnMessage = transaction.ReturnMessage;
                accountsWebApiModel.ReturnStatus = transaction.ReturnStatus;
                accountsWebApiModel.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse<AccountsApiModel>(HttpStatusCode.BadRequest, accountsWebApiModel);
                return badResponse;
            }
          
            accountsWebApiModel.ReturnStatus = transaction.ReturnStatus;
            accountsWebApiModel.ReturnMessage.Add("User successful updated.");
                      
            var response = Request.CreateResponse<AccountsApiModel>(HttpStatusCode.OK, accountsWebApiModel);
            return response;

        }


        [Route("Login")]
        [ValidateModelState]
        [HttpPost]
        public HttpResponseMessage Login(HttpRequestMessage request, [FromBody] LoginUserDTO loginUserDTO)
        {

            AccountsApiModel accountsWebApiModel = new AccountsApiModel();
            TransactionalInformation transaction = new TransactionalInformation();
            AccountsBusinessService accountsBusinessService;   

            if (loginUserDTO.UserName == null) loginUserDTO.UserName = "";
            if (loginUserDTO.Password == null) loginUserDTO.Password = "";
     
            accountsBusinessService = new AccountsBusinessService(accountsDataService);
            User user = accountsBusinessService.Login(               
                loginUserDTO.UserName,             
                loginUserDTO.Password,              
                out transaction);

            if (transaction.ReturnStatus == false)
            {
                accountsWebApiModel.ReturnMessage = transaction.ReturnMessage;
                accountsWebApiModel.ReturnStatus = transaction.ReturnStatus;
                accountsWebApiModel.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse<AccountsApiModel>(HttpStatusCode.BadRequest, accountsWebApiModel);
                return badResponse;
            }

            ApplicationInitializationBusinessService initializationBusinessService;
            initializationBusinessService = new ApplicationInitializationBusinessService(applicationDataService);
            List<ApplicationMenu> menuItems = initializationBusinessService.GetMenuItems(true, out transaction);

            if (transaction.ReturnStatus == false)
            {
                accountsWebApiModel.ReturnMessage = transaction.ReturnMessage;
                accountsWebApiModel.ReturnStatus = transaction.ReturnStatus;
                var badResponse = Request.CreateResponse<AccountsApiModel>(HttpStatusCode.BadRequest, accountsWebApiModel);
                return badResponse;
            }

            accountsWebApiModel.ReturnStatus = transaction.ReturnStatus;
            accountsWebApiModel.IsAuthenicated = true;
            accountsWebApiModel.ReturnMessage.Add("Login successful.");
            accountsWebApiModel.MenuItems = menuItems;
            accountsWebApiModel.User = user;

            FormsAuthentication.SetAuthCookie(user.UserId.ToString(), createPersistentCookie: false);

            var response = Request.CreateResponse<AccountsApiModel>(HttpStatusCode.OK, accountsWebApiModel);
            return response;


        }


        [Route("GetUser")]
        [HttpGet]
        [WebApiAuthenication]
        [ValidateModelState]
        public HttpResponseMessage GetUser()   
        {

            Guid userID = new Guid(User.Identity.Name);

            AccountsApiModel accountsWebApiModel = new AccountsApiModel();
            TransactionalInformation transaction = new TransactionalInformation();
            AccountsBusinessService accountsBusinessService;
            accountsWebApiModel.IsAuthenicated = true;

            accountsBusinessService = new AccountsBusinessService(accountsDataService);
            User user = accountsBusinessService.GetUser(userID, out transaction);
           
            transaction.ReturnStatus = true;

            if (transaction.ReturnStatus == false)
            {
                accountsWebApiModel.ReturnMessage = transaction.ReturnMessage;
                accountsWebApiModel.ReturnStatus = transaction.ReturnStatus;
                accountsWebApiModel.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse<AccountsApiModel>(HttpStatusCode.BadRequest, accountsWebApiModel);
                return badResponse;
            }

            accountsWebApiModel.ReturnStatus = transaction.ReturnStatus;               
            accountsWebApiModel.User = user;

            var response = Request.CreateResponse<AccountsApiModel>(HttpStatusCode.OK, accountsWebApiModel);
            return response;


        }


    }
}