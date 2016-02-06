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

namespace AngularJSWebApplication.WebApiModels.WebApiControllers
{
   [RoutePrefix("api/customers")]
    public class CustomersApiController : ApiController
    {
     
        ICustomersDataService customersDataService;
    
        /// <summary>
        /// Constructor with Dependency Injection using Ninject
        /// </summary>
        /// <param name="dataService"></param>
        public CustomersApiController()
        {
            customersDataService = new CustomersDataService();
          
        }

        [Route("GetCustomer")]
        [HttpGet]
        [WebApiAuthenication]
        [ValidateModelState]
        public HttpResponseMessage GetCustomer([FromUri] Guid customerID)
        {

            CustomersApiModel customersWebApiModel = new CustomersApiModel();
            TransactionalInformation transaction = new TransactionalInformation();
            CustomersBusinessService customersBusinessService;         

            customersWebApiModel = new CustomersApiModel();

            customersBusinessService = new CustomersBusinessService(customersDataService);

            Customer customer = customersBusinessService.GetCustomer(customerID, out transaction);

            customersWebApiModel.Customer = customer;
            customersWebApiModel.IsAuthenicated = true;
            customersWebApiModel.ReturnStatus = transaction.ReturnStatus;
            customersWebApiModel.ReturnMessage = transaction.ReturnMessage;          

            if (transaction.ReturnStatus == true)
            {
                var response = Request.CreateResponse<CustomersApiModel>(HttpStatusCode.OK, customersWebApiModel);
                return response;
            }

            var badResponse = Request.CreateResponse<CustomersApiModel>(HttpStatusCode.BadRequest, customersWebApiModel);
            return badResponse;


        }

        /// <summary>
        /// Customer Inquiry
        /// </summary>
        /// <param name="request"></param>
        /// <param name="customerInquiryDTO"></param>
        /// <returns></returns>
        [Route("GetCustomers")]
        [HttpGet]
        [WebApiAuthenication]
        [ValidateModelState]
        public HttpResponseMessage CustomerInquiry([FromUri] CustomerInquiryDTO customerInquiryDTO)
        {

            if (customerInquiryDTO.CustomerCode == null) customerInquiryDTO.CustomerCode = string.Empty;
            if (customerInquiryDTO.CompanyName == null) customerInquiryDTO.CompanyName = string.Empty;
            if (customerInquiryDTO.SortDirection == null) customerInquiryDTO.SortDirection = string.Empty;
            if (customerInquiryDTO.SortExpression == null) customerInquiryDTO.SortExpression = string.Empty;

            CustomersApiModel customersWebApiModel = new CustomersApiModel();
            TransactionalInformation transaction = new TransactionalInformation();
            CustomersBusinessService customersBusinessService;

            customersWebApiModel = new CustomersApiModel();

            DataGridPagingInformation paging = new DataGridPagingInformation();
            paging.CurrentPageNumber = customerInquiryDTO.CurrentPageNumber;
            paging.PageSize = customerInquiryDTO.PageSize;
            paging.SortExpression = customerInquiryDTO.SortExpression;
            paging.SortDirection = customerInquiryDTO.SortDirection;

            if (paging.SortDirection == "") paging.SortDirection = "ASC";
            if (paging.SortExpression == "") paging.SortExpression = "CompanyName";

            customersBusinessService = new CustomersBusinessService(customersDataService);

            List<Customer> customers = customersBusinessService.CustomerInquiry(customerInquiryDTO.CustomerCode, customerInquiryDTO.CompanyName, paging, out transaction);

            customersWebApiModel.Customers = customers;
            customersWebApiModel.IsAuthenicated = true;
            customersWebApiModel.ReturnStatus = transaction.ReturnStatus;
            customersWebApiModel.ReturnMessage = transaction.ReturnMessage;
            customersWebApiModel.TotalPages = transaction.TotalPages;
            customersWebApiModel.TotalRows = transaction.TotalRows;
            customersWebApiModel.PageSize = paging.PageSize;
            customersWebApiModel.IsAuthenicated = true;

            if (transaction.ReturnStatus == true)
            {
                var response = Request.CreateResponse<CustomersApiModel>(HttpStatusCode.OK, customersWebApiModel);
                return response;
            }

            var badResponse = Request.CreateResponse<CustomersApiModel>(HttpStatusCode.BadRequest, customersWebApiModel);
            return badResponse;


        }


        [Route("CreateCustomer")]
        [WebApiAuthenication]
        [ValidateModelState]
        [HttpPost]
        public HttpResponseMessage CreateCustomer(HttpRequestMessage request, [FromBody] CustomerDTO customerDTO)
        {
            CustomersApiModel customersWebApiModel = new CustomersApiModel();
            TransactionalInformation transaction = new TransactionalInformation();
            CustomersBusinessService customersBusinessService;       

            if (customerDTO.CustomerCode == null) customerDTO.CustomerCode = string.Empty;
            if (customerDTO.CompanyName == null) customerDTO.CompanyName = string.Empty;
            if (customerDTO.Address == null) customerDTO.Address = string.Empty;
            if (customerDTO.City == null) customerDTO.City = string.Empty;
            if (customerDTO.Region == null) customerDTO.Region = string.Empty;
            if (customerDTO.PostalCode == null) customerDTO.PostalCode = string.Empty;
            if (customerDTO.Country == null) customerDTO.Country = string.Empty;
            if (customerDTO.PhoneNumber == null) customerDTO.PhoneNumber = string.Empty;
            if (customerDTO.WebSiteUrl == null) customerDTO.WebSiteUrl = string.Empty;
          
            customersBusinessService = new CustomersBusinessService(customersDataService);
            customersWebApiModel.IsAuthenicated = true;

            Customer customer = customersBusinessService.CreateCustomer(
                customerDTO.CustomerCode,
                customerDTO.CompanyName,
                customerDTO.Address,
                customerDTO.City,
                customerDTO.Region,
                customerDTO.PostalCode,
                customerDTO.Country,
                customerDTO.PhoneNumber,
                customerDTO.WebSiteUrl,
                out transaction);

            if (transaction.ReturnStatus == false)
            {
                customersWebApiModel.ReturnMessage = transaction.ReturnMessage;
                customersWebApiModel.ReturnStatus = transaction.ReturnStatus;
                customersWebApiModel.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse<CustomersApiModel>(HttpStatusCode.BadRequest, customersWebApiModel);
                return badResponse;
            }

    
            customersWebApiModel.ReturnStatus = transaction.ReturnStatus;
            customersWebApiModel.ReturnMessage = transaction.ReturnMessage;
            customersWebApiModel.Customer = customer;
  
            var response = Request.CreateResponse<CustomersApiModel>(HttpStatusCode.OK, customersWebApiModel);
            return response;

        }

        [Route("UpdateCustomer")]
        [WebApiAuthenication]
        [ValidateModelState]
        [HttpPost]
        public HttpResponseMessage UpdateCustomer(HttpRequestMessage request, [FromBody] CustomerDTO customerDTO)
        {
            CustomersApiModel customersWebApiModel = new CustomersApiModel();
            TransactionalInformation transaction = new TransactionalInformation();
            CustomersBusinessService customersBusinessService;
            customersWebApiModel.IsAuthenicated = true;
              
            if (customerDTO.CustomerCode == null) customerDTO.CustomerCode = string.Empty;
            if (customerDTO.CompanyName == null) customerDTO.CompanyName = string.Empty;
            if (customerDTO.Address == null) customerDTO.Address = string.Empty;
            if (customerDTO.City == null) customerDTO.City = string.Empty;
            if (customerDTO.Region == null) customerDTO.Region = string.Empty;
            if (customerDTO.PostalCode == null) customerDTO.PostalCode = string.Empty;
            if (customerDTO.Country == null) customerDTO.Country = string.Empty;
            if (customerDTO.PhoneNumber == null) customerDTO.PhoneNumber = string.Empty;
            if (customerDTO.WebSiteUrl == null) customerDTO.WebSiteUrl = string.Empty;

            customersBusinessService = new CustomersBusinessService(customersDataService);

            Customer customer = customersBusinessService.UpdateCustomer(
                customerDTO.CustomerID,
                customerDTO.CustomerCode,
                customerDTO.CompanyName,
                customerDTO.Address,
                customerDTO.City,
                customerDTO.Region,
                customerDTO.PostalCode,
                customerDTO.Country,
                customerDTO.PhoneNumber,
                customerDTO.WebSiteUrl,
                out transaction);

            if (transaction.ReturnStatus == false)
            {
                customersWebApiModel.ReturnMessage = transaction.ReturnMessage;
                customersWebApiModel.ReturnStatus = transaction.ReturnStatus;
                customersWebApiModel.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse<CustomersApiModel>(HttpStatusCode.BadRequest, customersWebApiModel);
                return badResponse;
            }
           
            customersWebApiModel.ReturnStatus = transaction.ReturnStatus;
            customersWebApiModel.ReturnMessage = transaction.ReturnMessage;
            customersWebApiModel.Customer = customer;

            var response = Request.CreateResponse<CustomersApiModel>(HttpStatusCode.OK, customersWebApiModel);
            return response;

        }


        [Route("ImportCustomers")]
        [WebApiAuthenication]
        [ValidateModelState]
        [HttpGet]
        public HttpResponseMessage ImportCustomers(HttpRequestMessage request)
        {

            CustomersApiModel customersWebApiModel = new CustomersApiModel();
            TransactionalInformation transaction = new TransactionalInformation();
            CustomersBusinessService customersBusinessService;
            customersWebApiModel.IsAuthenicated = true;
          
            customersBusinessService = new CustomersBusinessService(customersDataService);
            customersBusinessService.ImportCustomers(out transaction);         

            if (transaction.ReturnStatus == false)
            {
                customersWebApiModel.ReturnMessage = transaction.ReturnMessage;
                customersWebApiModel.ReturnStatus = transaction.ReturnStatus;
                customersWebApiModel.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse<CustomersApiModel>(HttpStatusCode.BadRequest, customersWebApiModel);
                return badResponse;
            }

            customersWebApiModel.ReturnStatus = transaction.ReturnStatus;
            customersWebApiModel.ReturnMessage = transaction.ReturnMessage;                  
    
            var response = Request.CreateResponse<CustomersApiModel>(HttpStatusCode.OK, customersWebApiModel);
            return response;


        }

       
    }
}