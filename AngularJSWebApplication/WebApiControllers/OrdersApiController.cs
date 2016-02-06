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
    [RoutePrefix("api/orders")]
    public class OrdersApiController : ApiController
    {
     
        IOrdersDataService ordersDataService;
        ICustomersDataService customersDataService;
    
        /// <summary>
        /// Constructor with Dependency Injection using Ninject
        /// </summary>
        /// <param name="dataService"></param>
        public OrdersApiController()
        {
            ordersDataService = new OrdersDataService();
            customersDataService = new CustomersDataService();
          
        }

        /// <summary>
        /// Initialize Order
        /// </summary>
        /// <param name="request"></param>
        /// <param name="orderDTO"></param>
        /// <returns></returns>
        [Route("InitializeOrder")]
        [WebApiAuthenication]
        [ValidateModelState]
        [HttpPost]
        public HttpResponseMessage InitializeOrder(HttpRequestMessage request, [FromBody] OrderDTO orderDTO)
        {
            OrdersApiModel ordersWebApiModel = new OrdersApiModel();
            TransactionalInformation transaction = new TransactionalInformation();
            OrdersBusinessService ordersBusinessService;
            CustomersBusinessService customersBusinessService;
            ordersWebApiModel.IsAuthenicated = true;

            customersBusinessService = new CustomersBusinessService(customersDataService);
            Customer customer = customersBusinessService.GetCustomer(orderDTO.CustomerID, out transaction);
            if (transaction.ReturnStatus == false)
            {
                ordersWebApiModel.ReturnMessage = transaction.ReturnMessage;
                ordersWebApiModel.ReturnStatus = transaction.ReturnStatus;
                ordersWebApiModel.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse<OrdersApiModel>(HttpStatusCode.BadRequest, ordersWebApiModel);
                return badResponse;
            }

            ordersBusinessService = new OrdersBusinessService(ordersDataService);
            List<Shipper> shippers = ordersBusinessService.GetShippers(out transaction);
            if (transaction.ReturnStatus == false)
            {
                ordersWebApiModel.ReturnMessage = transaction.ReturnMessage;
                ordersWebApiModel.ReturnStatus = transaction.ReturnStatus;
                ordersWebApiModel.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse<OrdersApiModel>(HttpStatusCode.BadRequest, ordersWebApiModel);
                return badResponse;
            }
            
            ordersWebApiModel.ReturnStatus = transaction.ReturnStatus;
            ordersWebApiModel.ReturnMessage = transaction.ReturnMessage;
            ordersWebApiModel.Customer = customer;
            ordersWebApiModel.Shippers = shippers;

            var response = Request.CreateResponse<OrdersApiModel>(HttpStatusCode.OK, ordersWebApiModel);
            return response;


        }

        /// <summary>
        /// Get Order
        /// </summary>
        /// <param name="request"></param>
        /// <param name="orderDTO"></param>
        /// <returns></returns>
        [Route("GetOrder")]
        [WebApiAuthenication]
        [ValidateModelState]
        [HttpPost]
        public HttpResponseMessage GetOrder(HttpRequestMessage request, [FromBody] GetOrderDTO orderDTO)
        {
            OrdersApiModel ordersWebApiModel = new OrdersApiModel();
            TransactionalInformation transaction = new TransactionalInformation();
            OrdersBusinessService ordersBusinessService;
            CustomersBusinessService customersBusinessService;
            ordersWebApiModel.IsAuthenicated = true;

            ordersBusinessService = new OrdersBusinessService(ordersDataService);
            List<Shipper> shippers = ordersBusinessService.GetShippers(out transaction);
            if (transaction.ReturnStatus == false)
            {
                ordersWebApiModel.ReturnMessage = transaction.ReturnMessage;
                ordersWebApiModel.ReturnStatus = transaction.ReturnStatus;
                ordersWebApiModel.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse<OrdersApiModel>(HttpStatusCode.BadRequest, ordersWebApiModel);
                return badResponse;
            }

            Order order = ordersBusinessService.GetOrder(orderDTO.OrderID, out transaction);
            if (transaction.ReturnStatus == false)
            {
                ordersWebApiModel.ReturnMessage = transaction.ReturnMessage;
                ordersWebApiModel.ReturnStatus = transaction.ReturnStatus;
                ordersWebApiModel.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse<OrdersApiModel>(HttpStatusCode.BadRequest, ordersWebApiModel);
                return badResponse;
            }

            customersBusinessService = new CustomersBusinessService(customersDataService);
            Customer customer = customersBusinessService.GetCustomer(order.CustomerID, out transaction);
            if (transaction.ReturnStatus == false)
            {
                ordersWebApiModel.ReturnMessage = transaction.ReturnMessage;
                ordersWebApiModel.ReturnStatus = transaction.ReturnStatus;
                ordersWebApiModel.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse<OrdersApiModel>(HttpStatusCode.BadRequest, ordersWebApiModel);
                return badResponse;
            }

            List<OrderDetails> orderDetails = ordersBusinessService.GetOrderDetails(orderDTO.OrderID, out transaction);
            if (transaction.ReturnStatus == false)
            {
                ordersWebApiModel.ReturnMessage = transaction.ReturnMessage;
                ordersWebApiModel.ReturnStatus = transaction.ReturnStatus;
                ordersWebApiModel.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse<OrdersApiModel>(HttpStatusCode.BadRequest, ordersWebApiModel);
                return badResponse;
            }
            
            ordersWebApiModel.ReturnStatus = transaction.ReturnStatus;
            ordersWebApiModel.ReturnMessage = transaction.ReturnMessage;
            ordersWebApiModel.Customer = customer;
            ordersWebApiModel.Order = order;
            ordersWebApiModel.Shippers = shippers;
            ordersWebApiModel.OrderDetails = orderDetails;

            var response = Request.CreateResponse<OrdersApiModel>(HttpStatusCode.OK, ordersWebApiModel);
            return response;


        }

        /// <summary>
        /// Order Inquiry
        /// </summary>
        /// <param name="request"></param>
        /// <param name="orderInquiryDTO"></param>
        /// <returns></returns>
        [Route("GetOrders")]
        [HttpPost]
        [WebApiAuthenication]
        [ValidateModelState]
        public HttpResponseMessage OrderInquiry([FromBody] OrderInquiryDTO orderInquiryDTO)
        {

            if (orderInquiryDTO.CustomerCode == null) orderInquiryDTO.CustomerCode = string.Empty;
            if (orderInquiryDTO.CompanyName == null) orderInquiryDTO.CompanyName = string.Empty;
            if (orderInquiryDTO.SortDirection == null) orderInquiryDTO.SortDirection = string.Empty;
            if (orderInquiryDTO.SortExpression == null) orderInquiryDTO.SortExpression = string.Empty;

            OrdersApiModel ordersWebApiModel = new OrdersApiModel();
            TransactionalInformation transaction = new TransactionalInformation();
            OrdersBusinessService ordersBusinessService;

            ordersWebApiModel.IsAuthenicated = true;

            DataGridPagingInformation paging = new DataGridPagingInformation();
            paging.CurrentPageNumber = orderInquiryDTO.CurrentPageNumber;
            paging.PageSize = orderInquiryDTO.PageSize;
            paging.SortExpression = orderInquiryDTO.SortExpression;
            paging.SortDirection = orderInquiryDTO.SortDirection;

            if (paging.SortDirection == "") paging.SortDirection = "DESC";
            if (paging.SortExpression == "") paging.SortExpression = "OrderDate";

            ordersBusinessService = new OrdersBusinessService(ordersDataService);

            List<OrderInquiry> orders = ordersBusinessService.OrderInquiry(orderInquiryDTO.CustomerCode, orderInquiryDTO.CompanyName, paging, out transaction);

            ordersWebApiModel.Orders = orders;           
            ordersWebApiModel.ReturnStatus = transaction.ReturnStatus;
            ordersWebApiModel.ReturnMessage = transaction.ReturnMessage;
            ordersWebApiModel.TotalPages = transaction.TotalPages;
            ordersWebApiModel.TotalRows = transaction.TotalRows;
            ordersWebApiModel.PageSize = paging.PageSize;

            if (transaction.ReturnStatus == true)
            {
                var response = Request.CreateResponse<OrdersApiModel>(HttpStatusCode.OK, ordersWebApiModel);
                return response;
            }

            var badResponse = Request.CreateResponse<OrdersApiModel>(HttpStatusCode.BadRequest, ordersWebApiModel);
            return badResponse;


        }

        /// <summary>
        /// Create Order
        /// </summary>
        /// <param name="request"></param>
        /// <param name="orderDTO"></param>
        /// <returns></returns>
        [Route("CreateOrder")]
        [WebApiAuthenication]
        [ValidateModelState]
        [HttpPost]
        public HttpResponseMessage CreateOrder(HttpRequestMessage request, [FromBody] OrderDTO orderDTO)
        {
            OrdersApiModel ordersWebApiModel = new OrdersApiModel();
            TransactionalInformation transaction = new TransactionalInformation();
            OrdersBusinessService ordersBusinessService;
            ordersWebApiModel.IsAuthenicated = true;
           
            if (orderDTO.ShipAddress == null) orderDTO.ShipAddress = string.Empty;
            if (orderDTO.ShipCity == null) orderDTO.ShipCity = string.Empty;
            if (orderDTO.ShipRegion == null) orderDTO.ShipRegion = string.Empty;
            if (orderDTO.ShipPostalCode == null) orderDTO.ShipPostalCode = string.Empty;
            if (orderDTO.ShipCountry == null) orderDTO.ShipCountry = string.Empty;
           
            ordersBusinessService = new OrdersBusinessService(ordersDataService);

            Order order = ordersBusinessService.CreateOrder(
                orderDTO.CustomerID, 
                orderDTO.RequiredDate,
                orderDTO.ShipName,
                orderDTO.ShipAddress,
                orderDTO.ShipCity, 
                orderDTO.ShipRegion, 
                orderDTO.ShipPostalCode,
                orderDTO.ShipCountry,           
                orderDTO.ShipperID,
                out transaction);

            if (transaction.ReturnStatus == false)
            {
                ordersWebApiModel.ReturnMessage = transaction.ReturnMessage;
                ordersWebApiModel.ReturnStatus = transaction.ReturnStatus;
                ordersWebApiModel.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse<OrdersApiModel>(HttpStatusCode.BadRequest, ordersWebApiModel);
                return badResponse;
            }
           
            ordersWebApiModel.ReturnStatus = transaction.ReturnStatus;
            ordersWebApiModel.ReturnMessage = transaction.ReturnMessage;
            ordersWebApiModel.Order = order;

            var response = Request.CreateResponse<OrdersApiModel>(HttpStatusCode.OK, ordersWebApiModel);
            return response;

        }


        /// <summary>
        /// Update Order
        /// </summary>
        /// <param name="request"></param>
        /// <param name="orderDTO"></param>
        /// <returns></returns>
        [Route("UpdateOrder")]
        [WebApiAuthenication]
        [ValidateModelState]
        [HttpPost]
        public HttpResponseMessage UpdateOrder(HttpRequestMessage request, [FromBody] OrderDTO orderDTO)
        {
            OrdersApiModel ordersWebApiModel = new OrdersApiModel();
            TransactionalInformation transaction = new TransactionalInformation();
            OrdersBusinessService ordersBusinessService;
            ordersWebApiModel.IsAuthenicated = true;

            if (orderDTO.ShipAddress == null) orderDTO.ShipAddress = string.Empty;
            if (orderDTO.ShipCity == null) orderDTO.ShipCity = string.Empty;
            if (orderDTO.ShipRegion == null) orderDTO.ShipRegion = string.Empty;
            if (orderDTO.ShipPostalCode == null) orderDTO.ShipPostalCode = string.Empty;
            if (orderDTO.ShipCountry == null) orderDTO.ShipCountry = string.Empty;

            ordersBusinessService = new OrdersBusinessService(ordersDataService);

            Order order = ordersBusinessService.UpdateOrder(
                orderDTO.OrderID,
                orderDTO.RequiredDate,
                orderDTO.ShipName,
                orderDTO.ShipAddress,
                orderDTO.ShipCity,
                orderDTO.ShipRegion,
                orderDTO.ShipPostalCode,
                orderDTO.ShipCountry,
                orderDTO.ShipperID,
                out transaction);

            if (transaction.ReturnStatus == false)
            {
                ordersWebApiModel.ReturnMessage = transaction.ReturnMessage;
                ordersWebApiModel.ReturnStatus = transaction.ReturnStatus;
                ordersWebApiModel.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse<OrdersApiModel>(HttpStatusCode.BadRequest, ordersWebApiModel);
                return badResponse;
            }
            
            ordersWebApiModel.ReturnStatus = transaction.ReturnStatus;
            ordersWebApiModel.ReturnMessage = transaction.ReturnMessage;
            ordersWebApiModel.Order = order;

            var response = Request.CreateResponse<OrdersApiModel>(HttpStatusCode.OK, ordersWebApiModel);
            return response;

        }

        /// <summary>
        /// Create Order
        /// </summary>
        /// <param name="request"></param>
        /// <param name="orderDTO"></param>
        /// <returns></returns>
        [Route("CreateOrderLineItem")]
        [WebApiAuthenication]
        [ValidateModelState]
        [HttpPost]
        public HttpResponseMessage CreateOrderLineItem(HttpRequestMessage request, [FromBody] OrderDetailDTO orderDTO)
        {
            OrdersApiModel ordersWebApiModel = new OrdersApiModel();
            TransactionalInformation transaction = new TransactionalInformation();
            OrdersBusinessService ordersBusinessService;
            ordersWebApiModel.IsAuthenicated = true;

            ordersBusinessService = new OrdersBusinessService(ordersDataService);

            OrderDetail orderDetail = ordersBusinessService.CreateOrderDetailLineItem(
                orderDTO.OrderID,
                orderDTO.ProductID,
                orderDTO.Quantity,              
                out transaction);

            if (transaction.ReturnStatus == false)
            {
                ordersWebApiModel.ReturnMessage = transaction.ReturnMessage;
                ordersWebApiModel.ReturnStatus = transaction.ReturnStatus;
                ordersWebApiModel.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse<OrdersApiModel>(HttpStatusCode.BadRequest, ordersWebApiModel);
                return badResponse;
            }


            List<OrderDetails> orderDetails = ordersBusinessService.GetOrderDetails(orderDTO.OrderID, out transaction);
            if (transaction.ReturnStatus == false)
            {
                ordersWebApiModel.ReturnMessage = transaction.ReturnMessage;
                ordersWebApiModel.ReturnStatus = transaction.ReturnStatus;
                ordersWebApiModel.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse<OrdersApiModel>(HttpStatusCode.BadRequest, ordersWebApiModel);
                return badResponse;
            }


            Order order = ordersBusinessService.GetOrder(orderDTO.OrderID, out transaction);
            if (transaction.ReturnStatus == false)
            {
                ordersWebApiModel.ReturnMessage = transaction.ReturnMessage;
                ordersWebApiModel.ReturnStatus = transaction.ReturnStatus;
                ordersWebApiModel.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse<OrdersApiModel>(HttpStatusCode.BadRequest, ordersWebApiModel);
                return badResponse;
            }

            transaction.ReturnMessage.Add("Detail line item succcessfully added.");
            
            ordersWebApiModel.ReturnStatus = transaction.ReturnStatus;
            ordersWebApiModel.ReturnMessage = transaction.ReturnMessage;
            ordersWebApiModel.OrderDetails = orderDetails;
            ordersWebApiModel.Order = order;

            var response = Request.CreateResponse<OrdersApiModel>(HttpStatusCode.OK, ordersWebApiModel);
            return response;

        }


       
        /// <summary>
        /// Update Order Line Item
        /// </summary>
        /// <param name="request"></param>
        /// <param name="orderDTO"></param>
        /// <returns></returns>
        [Route("UpdateOrderLineItem")]
        [WebApiAuthenication]
        [ValidateModelState]
        [HttpPost]
        public HttpResponseMessage UpdateOrderLineItem(HttpRequestMessage request, [FromBody] OrderDetailDTO orderDTO)
        {
            OrdersApiModel ordersWebApiModel = new OrdersApiModel();
            TransactionalInformation transaction = new TransactionalInformation();
            OrdersBusinessService ordersBusinessService;
            ordersWebApiModel.IsAuthenicated = true;

            ordersBusinessService = new OrdersBusinessService(ordersDataService);

            OrderDetail orderDetail = ordersBusinessService.UpdateOrderDetailLineItem(                
                orderDTO.OrderID,
                orderDTO.OrderDetailID,
                orderDTO.Quantity,
                out transaction);

            if (transaction.ReturnStatus == false)
            {
                ordersWebApiModel.ReturnMessage = transaction.ReturnMessage;
                ordersWebApiModel.ReturnStatus = transaction.ReturnStatus;
                ordersWebApiModel.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse<OrdersApiModel>(HttpStatusCode.BadRequest, ordersWebApiModel);
                return badResponse;
            }

            Order order = ordersBusinessService.GetOrder(orderDTO.OrderID, out transaction);
            if (transaction.ReturnStatus == false)
            {
                ordersWebApiModel.ReturnMessage = transaction.ReturnMessage;
                ordersWebApiModel.ReturnStatus = transaction.ReturnStatus;
                ordersWebApiModel.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse<OrdersApiModel>(HttpStatusCode.BadRequest, ordersWebApiModel);
                return badResponse;
            }

            transaction.ReturnMessage.Add("Detail line item successfully updated.");
         
            ordersWebApiModel.ReturnStatus = transaction.ReturnStatus;
            ordersWebApiModel.ReturnMessage = transaction.ReturnMessage;        
            ordersWebApiModel.Order = order;

            var response = Request.CreateResponse<OrdersApiModel>(HttpStatusCode.OK, ordersWebApiModel);
            return response;

        }



        /// <summary>
        /// Delete Order Line Item
        /// </summary>
        /// <param name="request"></param>
        /// <param name="orderDTO"></param>
        /// <returns></returns>
        [Route("DeleteOrderLineItem")]
        [WebApiAuthenication]
        [ValidateModelState]
        [HttpPost]
        public HttpResponseMessage DeleteOrderLineItem(HttpRequestMessage request, [FromBody] OrderDetailDTO orderDTO)
        {
            OrdersApiModel ordersWebApiModel = new OrdersApiModel();
            TransactionalInformation transaction = new TransactionalInformation();
            OrdersBusinessService ordersBusinessService;
            ordersWebApiModel.IsAuthenicated = true;

            ordersBusinessService = new OrdersBusinessService(ordersDataService);

            ordersBusinessService.DeleteOrderDetailLineItem(              
                orderDTO.OrderDetailID,               
                out transaction);

            if (transaction.ReturnStatus == false)
            {
                ordersWebApiModel.ReturnMessage = transaction.ReturnMessage;
                ordersWebApiModel.ReturnStatus = transaction.ReturnStatus;
                ordersWebApiModel.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse<OrdersApiModel>(HttpStatusCode.BadRequest, ordersWebApiModel);
                return badResponse;
            }

            Order order = ordersBusinessService.GetOrder(orderDTO.OrderID, out transaction);
            if (transaction.ReturnStatus == false)
            {
                ordersWebApiModel.ReturnMessage = transaction.ReturnMessage;
                ordersWebApiModel.ReturnStatus = transaction.ReturnStatus;
                ordersWebApiModel.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse<OrdersApiModel>(HttpStatusCode.BadRequest, ordersWebApiModel);
                return badResponse;
            }

            List<OrderDetails> orderDetails = ordersBusinessService.GetOrderDetails(orderDTO.OrderID, out transaction);
            if (transaction.ReturnStatus == false)
            {
                ordersWebApiModel.ReturnMessage = transaction.ReturnMessage;
                ordersWebApiModel.ReturnStatus = transaction.ReturnStatus;
                ordersWebApiModel.ValidationErrors = transaction.ValidationErrors;
                var badResponse = Request.CreateResponse<OrdersApiModel>(HttpStatusCode.BadRequest, ordersWebApiModel);
                return badResponse;
            }

            transaction.ReturnMessage.Add("Detail line item successfully deleted.");
            
            ordersWebApiModel.ReturnStatus = transaction.ReturnStatus;
            ordersWebApiModel.ReturnMessage = transaction.ReturnMessage;
            ordersWebApiModel.Order = order;
            ordersWebApiModel.OrderDetails = orderDetails;

            var response = Request.CreateResponse<OrdersApiModel>(HttpStatusCode.OK, ordersWebApiModel);
            return response;

        }


    }
}