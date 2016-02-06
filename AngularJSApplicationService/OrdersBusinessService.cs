using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;

using AngularJSDataServiceInterface;
using AngularJSDataModels;
using AngularJSUtilities;

namespace AngularJSApplicationService
{
    public class OrdersBusinessService
    {

        IOrdersDataService _ordersDataService;

        private IOrdersDataService ordersDataService
        {
            get { return _ordersDataService; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OrdersBusinessService(IOrdersDataService dataService)
        {
            _ordersDataService = dataService;
        }

        /// <summary>
        /// Create Order
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="requiredDate"></param>
        /// <param name="shipName"></param>
        /// <param name="shipAddress"></param>
        /// <param name="shipCity"></param>
        /// <param name="shipRegion"></param>
        /// <param name="shipPostalCode"></param>
        /// <param name="shipCountry"></param>
        /// <param name="shipperID"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public Order CreateOrder(Guid customerID, DateTime requiredDate, string shipName, 
                                string shipAddress, string shipCity, string shipRegion, 
                                string shipPostalCode, string shipCountry, int shipperID, 
                                out TransactionalInformation transaction)
        {
       
            transaction = new TransactionalInformation();

            OrdersBusinessRules ordersBusinessRules = new OrdersBusinessRules();

            Order order = new Order();

            order.CustomerID = customerID;
            order.OrderDate = DateTime.Now;
            order.RequiredDate = requiredDate;
            order.ShipAddress = shipAddress;
            order.ShipName = shipName;
            order.ShipCity = shipCity;
            order.ShipPostalCode = shipPostalCode;
            order.ShipRegion = shipRegion;
            order.ShipCountry = shipCountry;
            order.ShipVia = shipperID;
            order.OrderTotal = 0.00m;


            try
            {

                ordersDataService.CreateSession();

                ordersBusinessRules.ValidateOrder(order, ordersDataService);

                if (ordersBusinessRules.ValidationStatus == true)
                {
                    ordersDataService.BeginTransaction();
                    ordersDataService.CreateOrder(order);
                    ordersDataService.CommitTransaction(true);
                    transaction.ReturnStatus = true;
                    transaction.ReturnMessage.Add("Order successfully created.");
                }
                else
                {
                    transaction.ReturnStatus = ordersBusinessRules.ValidationStatus;
                    transaction.ReturnMessage = ordersBusinessRules.ValidationMessage;
                    transaction.ValidationErrors = ordersBusinessRules.ValidationErrors;
                }

            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                string errorMessage = ex.Message;
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(errorMessage);
            }
            finally
            {
                ordersDataService.CloseSession();
            }

            return order;


        }

        /// <summary>
        /// Update Order
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="requiredDate"></param>
        /// <param name="shipName"></param>
        /// <param name="shipAddress"></param>
        /// <param name="shipCity"></param>
        /// <param name="shipRegion"></param>
        /// <param name="shipPostalCode"></param>
        /// <param name="shipCountry"></param>
        /// <param name="shipperID"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public Order UpdateOrder(int orderID, DateTime requiredDate, string shipName,
                                string shipAddress, string shipCity, string shipRegion,
                                string shipPostalCode, string shipCountry, int shipperID,
                                out TransactionalInformation transaction)
            
        {

            transaction = new TransactionalInformation();

            OrdersBusinessRules ordersBusinessRules = new OrdersBusinessRules();

            Order order = new Order();

            try
            {

                ordersDataService.CreateSession();

                order = ordersDataService.GetOrder(orderID);
                order.RequiredDate = requiredDate;
                order.ShipAddress = shipAddress;
                order.ShipName = shipName;
                order.ShipCity = shipCity;
                order.ShipPostalCode = shipPostalCode;
                order.ShipRegion = shipRegion;
                order.ShipCountry = shipCountry;
                order.ShipVia = shipperID;
            
                ordersBusinessRules.ValidateOrder(order, ordersDataService);

                if (ordersBusinessRules.ValidationStatus == true)
                {
                    ordersDataService.BeginTransaction();
                    ordersDataService.UpdateOrder(order);
                    ordersDataService.CommitTransaction(true);
                    transaction.ReturnStatus = true;
                    transaction.ReturnMessage.Add("Order successfully updated at " + order.DateUpdated.ToString());
                }
                else
                {
                    transaction.ReturnStatus = ordersBusinessRules.ValidationStatus;
                    transaction.ReturnMessage = ordersBusinessRules.ValidationMessage;
                    transaction.ValidationErrors = ordersBusinessRules.ValidationErrors;
                }

            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                string errorMessage = ex.Message;
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(errorMessage);
            }
            finally
            {
                ordersDataService.CloseSession();
            }

            return order;

        }        
   

        /// <summary>
        /// Get Order
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public Order GetOrder(int orderID, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            Order order = new Order();

            try
            {
                ordersDataService.CreateSession();
                order = ordersDataService.GetOrder(orderID);
                transaction.ReturnStatus = true;
            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                string errorMessage = ex.Message;
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(errorMessage);
            }
            finally
            {
                ordersDataService.CloseSession();
            }

            return order;

        }


           
        /// <summary>
        /// Get Shippers
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public List<Shipper> GetShippers(out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            List<Shipper> shippers = new List<Shipper>();

            try
            {
                ordersDataService.CreateSession();
                shippers = ordersDataService.GetShippers();            
                transaction.ReturnStatus = true;             
            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                string errorMessage = ex.Message;
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(errorMessage);
            }
            finally
            {
                ordersDataService.CloseSession();               
            }

            return shippers;

        }

        /// <summary>
        /// Get Order Details
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public List<OrderDetails> GetOrderDetails(long orderID, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            List<OrderDetails> orderDetail = new List<OrderDetails>();

            try
            {
                ordersDataService.CreateSession();
                orderDetail = ordersDataService.GetOrderDetails(orderID);
            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                string errorMessage = ex.Message;
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(errorMessage);
            }
            finally
            {
                ordersDataService.CloseSession();
            }

            return orderDetail;

        }

        /// <summary>
        /// Order Inquiry
        /// </summary>
        /// <param name="customerCode"></param>
        /// <param name="companyName"></param>
        /// <param name="paging"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public List<OrderInquiry> OrderInquiry(string customerCode, string companyName, DataGridPagingInformation paging, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            List<OrderInquiry> orders = new List<OrderInquiry>();

            try
            {
                ordersDataService.CreateSession();
                orders = ordersDataService.OrderInquiry(customerCode, companyName, paging, out transaction);
            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                string errorMessage = ex.Message;
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(errorMessage);
            }
            finally
            {
                ordersDataService.CloseSession();
            }

            return orders;

        }

      
        /// <summary>
        /// Create Order Line Item
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="productID"></param>
        /// <param name="quantity"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public OrderDetail CreateOrderDetailLineItem(long orderID, Guid productID, int quantity, out TransactionalInformation transaction)
        {

            transaction = new TransactionalInformation();

            OrdersBusinessRules ordersBusinessRules = new OrdersBusinessRules();

            OrderDetail order = new OrderDetail();

            order.OrderID = orderID;
            order.ProductID = productID;
            order.Quantity = quantity;     
     
            try
            {

                ordersDataService.CreateSession();

                ordersBusinessRules.ValidateOrderDetailLineItem(order, ordersDataService);

                if (ordersBusinessRules.ValidationStatus == true)
                {
                    ordersDataService.BeginTransaction();
                    ordersDataService.CreateOrderDetailLineItem(order);
                    ordersDataService.CommitTransaction(true);
                    transaction.ReturnStatus = true;
                    transaction.ReturnMessage.Add("Order line item successfully created.");
                }
                else
                {
                    transaction.ReturnStatus = ordersBusinessRules.ValidationStatus;
                    transaction.ReturnMessage = ordersBusinessRules.ValidationMessage;
                    transaction.ValidationErrors = ordersBusinessRules.ValidationErrors;
                }

            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                string errorMessage = ex.Message;
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(errorMessage);
            }
            finally
            {
                ordersDataService.CloseSession();
            }

            return order;


        }


        /// <summary>
        /// Update Order Detail Line Item
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="orderDetailID"></param>
        /// <param name="quantity"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public OrderDetail UpdateOrderDetailLineItem(long orderID, Guid orderDetailID, int quantity, out TransactionalInformation transaction)
        {

            transaction = new TransactionalInformation();

            OrdersBusinessRules ordersBusinessRules = new OrdersBusinessRules();

            OrderDetail order = new OrderDetail();
                
            order.Quantity = quantity;
            order.OrderDetailID = orderDetailID;
            order.OrderID = orderID;

            try
            {

                ordersDataService.CreateSession();

                ordersBusinessRules.ValidateOrderDetailLineItem(order, ordersDataService);

                if (ordersBusinessRules.ValidationStatus == true)
                {
                    ordersDataService.BeginTransaction();
                    ordersDataService.UpdateOrderDetailLineItem(order);
                    ordersDataService.CommitTransaction(true);
                    transaction.ReturnStatus = true;
                    transaction.ReturnMessage.Add("Order line item successfully updated.");
                }
                else
                {
                    transaction.ReturnStatus = ordersBusinessRules.ValidationStatus;
                    transaction.ReturnMessage = ordersBusinessRules.ValidationMessage;
                    transaction.ValidationErrors = ordersBusinessRules.ValidationErrors;
                }

            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                string errorMessage = ex.Message;
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(errorMessage);
            }
            finally
            {
                ordersDataService.CloseSession();
            }

            return order;


        }


        /// <summary>
        /// Delete Order Detail Line Item
        /// </summary>
        /// <param name="orderDetailID"></param>
        /// <param name="transaction"></param>
        public void DeleteOrderDetailLineItem(Guid orderDetailID, out TransactionalInformation transaction)
        {

            transaction = new TransactionalInformation();

            OrdersBusinessRules ordersBusinessRules = new OrdersBusinessRules();
       
            try
            {

                ordersDataService.CreateSession();
             
                ordersDataService.BeginTransaction();
                ordersDataService.DeleteOrderDetailLineItem(orderDetailID);
                ordersDataService.CommitTransaction(true);
                transaction.ReturnStatus = true;            
            }
            catch (Exception ex)
            {
                transaction.ReturnMessage = new List<string>();
                string errorMessage = ex.Message;
                transaction.ReturnStatus = false;
                transaction.ReturnMessage.Add(errorMessage);
            }
            finally
            {
                ordersDataService.CloseSession();
            }         

        }
    
    
    
    }

}

