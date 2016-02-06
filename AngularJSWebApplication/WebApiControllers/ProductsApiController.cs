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
   [RoutePrefix("api/products")]
    public class ProductsApiController : ApiController
    {
     
        IProductsDataService productsDataService;
    
        /// <summary>
        /// Constructor with Dependency Injection using Ninject
        /// </summary>
        /// <param name="dataService"></param>
        public ProductsApiController()
        {
            productsDataService = new ProductsDataService();
          
        }

        [Route("GetProduct")]
        [HttpGet]
        [WebApiAuthenication]
        [ValidateModelState]
        public HttpResponseMessage GetProduct(Guid productID)
        {

            ProductsApiModel productsWebApiModel = new ProductsApiModel();
            TransactionalInformation transaction = new TransactionalInformation();
            ProductsBusinessService productsBusinessService;        

            productsWebApiModel = new ProductsApiModel();
            productsWebApiModel.IsAuthenicated = true;

            productsBusinessService = new ProductsBusinessService(productsDataService);

            Product product = productsBusinessService.GetProduct(productID, out transaction);

            productsWebApiModel.Product = product;
            productsWebApiModel.IsAuthenicated = true;
            productsWebApiModel.ReturnStatus = transaction.ReturnStatus;
            productsWebApiModel.ReturnMessage = transaction.ReturnMessage;          

            if (transaction.ReturnStatus == true)
            {
                var response = Request.CreateResponse<ProductsApiModel>(HttpStatusCode.OK, productsWebApiModel);
                return response;
            }

            var badResponse = Request.CreateResponse<ProductsApiModel>(HttpStatusCode.BadRequest, productsWebApiModel);
            return badResponse;


        }

        /// <summary>
        /// Product Inquiry
        /// </summary>
        /// <param name="request"></param>
        /// <param name="productInquiryDTO"></param>
        /// <returns></returns>
        [Route("GetProducts")]
        [HttpGet]
        [WebApiAuthenication]
        [ValidateModelState]
        public HttpResponseMessage ProductInquiry([FromUri] ProductInquiryDTO productInquiryDTO)
        {

            if (productInquiryDTO.ProductCode == null) productInquiryDTO.ProductCode = string.Empty;
            if (productInquiryDTO.Description == null) productInquiryDTO.Description = string.Empty;
            if (productInquiryDTO.SortDirection == null) productInquiryDTO.SortDirection = string.Empty;
            if (productInquiryDTO.SortExpression == null) productInquiryDTO.SortExpression = string.Empty;

            ProductsApiModel productsWebApiModel = new ProductsApiModel();
            TransactionalInformation transaction = new TransactionalInformation();
            ProductsBusinessService productsBusinessService;

            productsWebApiModel.IsAuthenicated = true;

            DataGridPagingInformation paging = new DataGridPagingInformation();
            paging.CurrentPageNumber = productInquiryDTO.CurrentPageNumber;
            paging.PageSize = productInquiryDTO.PageSize;
            paging.SortExpression = productInquiryDTO.SortExpression;
            paging.SortDirection = productInquiryDTO.SortDirection;

            if (paging.SortDirection == "") paging.SortDirection = "ASC";
            if (paging.SortExpression == "") paging.SortExpression = "Description";

            productsBusinessService = new ProductsBusinessService(productsDataService);

            List<Product> products = productsBusinessService.ProductInquiry(productInquiryDTO.ProductCode, productInquiryDTO.Description, paging, out transaction);

            productsWebApiModel.Products = products;       
            productsWebApiModel.ReturnStatus = transaction.ReturnStatus;
            productsWebApiModel.ReturnMessage = transaction.ReturnMessage;
            productsWebApiModel.TotalPages = transaction.TotalPages;
            productsWebApiModel.TotalRows = transaction.TotalRows;
            productsWebApiModel.PageSize = paging.PageSize;

            if (transaction.ReturnStatus == true)
            {
                var response = Request.CreateResponse<ProductsApiModel>(HttpStatusCode.OK, productsWebApiModel);
                return response;
            }

            var badResponse = Request.CreateResponse<ProductsApiModel>(HttpStatusCode.BadRequest, productsWebApiModel);
            return badResponse;


        }

        [Route("CreateProduct")]
        [WebApiAuthenication]
        [ValidateModelState]
        [HttpPost]
        public HttpResponseMessage CreateProduct(HttpRequestMessage request, [FromBody] ProductDTO productDTO)
        {
            ProductsApiModel productsWebApiModel = new ProductsApiModel();
            TransactionalInformation transaction = new TransactionalInformation();
            ProductsBusinessService productsBusinessService;
            productsWebApiModel.IsAuthenicated = true;                                

            if (productDTO.ProductCode == null) productDTO.ProductCode = string.Empty;
            if (productDTO.Description == null) productDTO.Description = string.Empty;
            if (productDTO.UnitOfMeasure == null) productDTO.UnitOfMeasure = string.Empty;
         
            productsBusinessService = new ProductsBusinessService(productsDataService);
   
            Product product = productsBusinessService.CreateProduct(
                productDTO.ProductCode,
                productDTO.Description,
                productDTO.UnitPrice,
                productDTO.UnitOfMeasure,               
                out transaction);

            if (transaction.ReturnStatus == false)
            {
                productsWebApiModel.ReturnMessage = transaction.ReturnMessage;
                productsWebApiModel.ReturnStatus = transaction.ReturnStatus;
                productsWebApiModel.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse<ProductsApiModel>(HttpStatusCode.BadRequest, productsWebApiModel);
                return badResponse;
            }
            
            productsWebApiModel.ReturnStatus = transaction.ReturnStatus;
            productsWebApiModel.ReturnMessage = transaction.ReturnMessage;
            productsWebApiModel.Product = product;

            var response = Request.CreateResponse<ProductsApiModel>(HttpStatusCode.OK, productsWebApiModel);
            return response;

        }

        [Route("UpdateProduct")]
        [WebApiAuthenication]
        [ValidateModelState]
        [HttpPost]
        public HttpResponseMessage UpdateProduct(HttpRequestMessage request, [FromBody] ProductDTO productDTO)
        {
            ProductsApiModel productsWebApiModel = new ProductsApiModel();
            TransactionalInformation transaction = new TransactionalInformation();
            ProductsBusinessService productsBusinessService;
            productsWebApiModel.IsAuthenicated = true;

            if (productDTO.ProductCode == null) productDTO.ProductCode = string.Empty;
            if (productDTO.Description == null) productDTO.Description = string.Empty;
            if (productDTO.UnitOfMeasure == null) productDTO.UnitOfMeasure = string.Empty;

            productsBusinessService = new ProductsBusinessService(productsDataService);
       
            Product product = productsBusinessService.UpdateProduct(
                productDTO.ProductID,
                productDTO.ProductCode,
                productDTO.Description,
                productDTO.UnitPrice,
                productDTO.UnitOfMeasure,              
                out transaction);

            if (transaction.ReturnStatus == false)
            {
                productsWebApiModel.ReturnMessage = transaction.ReturnMessage;
                productsWebApiModel.ReturnStatus = transaction.ReturnStatus;
                productsWebApiModel.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse<ProductsApiModel>(HttpStatusCode.BadRequest, productsWebApiModel);
                return badResponse;
            }
           
            productsWebApiModel.ReturnStatus = transaction.ReturnStatus;
            productsWebApiModel.ReturnMessage = transaction.ReturnMessage;
            productsWebApiModel.Product = product;

            var response = Request.CreateResponse<ProductsApiModel>(HttpStatusCode.OK, productsWebApiModel);
            return response;

        }

        [Route("ImportProducts")]
        [WebApiAuthenication]
        [ValidateModelState]
        [HttpGet]
        public HttpResponseMessage ImportProducts(HttpRequestMessage request)
        {

            ProductsApiModel productsWebApiModel = new ProductsApiModel();
            TransactionalInformation transaction = new TransactionalInformation();
            ProductsBusinessService productsBusinessService;
            productsWebApiModel.IsAuthenicated = true;
    
            productsBusinessService = new ProductsBusinessService(productsDataService);
            productsBusinessService.ImportProducts(out transaction);         

            if (transaction.ReturnStatus == false)
            {
                productsWebApiModel.ReturnMessage = transaction.ReturnMessage;
                productsWebApiModel.ReturnStatus = transaction.ReturnStatus;
                productsWebApiModel.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse<ProductsApiModel>(HttpStatusCode.BadRequest, productsWebApiModel);
                return badResponse;
            }

            productsWebApiModel.ReturnStatus = transaction.ReturnStatus;          
            productsWebApiModel.ReturnMessage = transaction.ReturnMessage;                  
    
            var response = Request.CreateResponse<ProductsApiModel>(HttpStatusCode.OK, productsWebApiModel);
            return response;

        }

       
    }
}
