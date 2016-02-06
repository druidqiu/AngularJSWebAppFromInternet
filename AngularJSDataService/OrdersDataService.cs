using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic;

using AngularJSDataServiceInterface;
using AngularJSDataModels;
using System.Linq.Dynamic;

namespace AngularJSDataService
{
    public class OrdersDataService : EntityFrameworkDataService, IOrdersDataService
    {
        /// <summary>
        /// Create Order
        /// </summary>
        /// <param name="order"></param>
        public void CreateOrder(Order order)
        {
            dbConnection.Orders.Add(order);

        }

        /// <summary>
        /// Update Order
        /// </summary>
        /// <param name="order"></param>
        public void UpdateOrder(Order order)
        {
            order.DateUpdated = DateTime.Now;
        }

        /// <summary>
        /// Create Order Detail Line Item
        /// </summary>
        /// <param name="orderDetail"></param>
        public void CreateOrderDetailLineItem(OrderDetail orderDetail)
        {

            int count = dbConnection.OrderDetails.Where(o => o.OrderID == orderDetail.OrderID).Count();
            int itemNumber = 0;
            if (count > 0)
            {
                var maxItemNumber = dbConnection.OrderDetails.Where(o => o.OrderID == orderDetail.OrderID).Max(i => (int?)i.LineItemNumber ?? 0);                      
                itemNumber = Convert.ToInt32(maxItemNumber);               
            }

            itemNumber++;

            DateTime orderCreated = DateTime.Now;

            orderDetail.OrderDetailID = Guid.NewGuid();
            orderDetail.LineItemNumber = itemNumber;
            orderDetail.DateCreated = orderCreated;
            orderDetail.DateUpdated = orderCreated;

            dbConnection.OrderDetails.Add(orderDetail);

            decimal orderTotal = 0;

            if (count > 0)
            {
                orderTotal = (from orderDetails in dbConnection.OrderDetails.Where(o => o.OrderID == orderDetail.OrderID)
                              join products in dbConnection.Products on orderDetails.ProductID equals products.ProductID
                              select new { products.UnitPrice, orderDetails.Quantity }).Sum(o => o.Quantity * o.UnitPrice);

            }
           
            Product product = dbConnection.Products.SingleOrDefault(p => p.ProductID == orderDetail.ProductID);
            orderTotal = orderTotal + ( orderDetail.Quantity * product.UnitPrice );                           
          
            Order order = dbConnection.Orders.Where(o => o.OrderID == orderDetail.OrderID).FirstOrDefault();
            order.OrderTotal = orderTotal;
                     
        }


        /// <summary>
        /// Update Order Detail Line Item
        /// </summary>
        /// <param name="orderDetail"></param>
        public void UpdateOrderDetailLineItem(OrderDetail orderDetail)
        {
            
            OrderDetail order = dbConnection.OrderDetails.SingleOrDefault(o => o.OrderDetailID == orderDetail.OrderDetailID);

            int originalQuantity = order.Quantity;
            
            order.Quantity = orderDetail.Quantity;
            order.DateUpdated = DateTime.Now;     

            Product product = dbConnection.Products.SingleOrDefault(p => p.ProductID == order.ProductID);
            decimal originalAmount = (originalQuantity * product.UnitPrice);
            decimal newAmount = (order.Quantity * product.UnitPrice);
            decimal diff = newAmount - originalAmount;

            Order orderHeader = dbConnection.Orders.Where(o => o.OrderID == orderDetail.OrderID).FirstOrDefault();
            orderHeader.OrderTotal = orderHeader.OrderTotal + diff;

        }

        /// <summary>
        /// Delete Order Detail Line Item
        /// </summary>
        /// <param name="orderDetailID"></param>
        public void DeleteOrderDetailLineItem(Guid orderDetailID)
        {

            OrderDetail order = dbConnection.OrderDetails.SingleOrDefault(o => o.OrderDetailID == orderDetailID);
   
            Product product = dbConnection.Products.SingleOrDefault(p => p.ProductID == order.ProductID);
            decimal amount = (order.Quantity * product.UnitPrice);
   
            Order orderHeader = dbConnection.Orders.Where(o => o.OrderID == order.OrderID).FirstOrDefault();
            orderHeader.OrderTotal = orderHeader.OrderTotal - amount;

            dbConnection.OrderDetails.Remove(order);

        }
        
        /// <summary>
        /// Get Shippers
        /// </summary>
        /// <returns></returns>
        public List<Shipper> GetShippers()
        {

            var shipperQuery = dbConnection.Shippers.AsQueryable();
            var shippers = (from s in shipperQuery.OrderBy("ShipperName") select s).ToList();
            return shippers;

        }

        /// <summary>
        /// Get Shippers
        /// </summary>
        /// <returns></returns>
        public List<OrderDetails> GetOrderDetails(long orderID)
        {
            var orderItems = from order in dbConnection.Orders.Where(o => o.OrderID == orderID)
                              join orderDetail in dbConnection.OrderDetails on order.OrderID equals orderDetail.OrderID
                              join product in dbConnection.Products on orderDetail.ProductID equals product.ProductID                              
                              select new { product, order, orderDetail };

            orderItems = orderItems.OrderBy(o => o.orderDetail.LineItemNumber);

            List<OrderDetails> listOfOrderDetails = new List<OrderDetails>();

            foreach (var orderDetail in orderItems)
            {
                OrderDetails orderDetails = new OrderDetails();
                orderDetails.Product = orderDetail.product;
                orderDetails.Order = orderDetail.order;
                orderDetails.OrderDetail = orderDetail.orderDetail;

                listOfOrderDetails.Add(orderDetails);
            }

            return listOfOrderDetails;
            
        }

        /// <summar


        /// <summary>
        /// Get Order Header
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public Order GetOrder(int orderID)
        {          
            Order order = dbConnection.Orders.SingleOrDefault(o => o.OrderID == orderID);
            return order;
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

            string sortExpression = paging.SortExpression;

            if (paging.SortDirection != string.Empty)
                sortExpression = sortExpression + " " + paging.SortDirection;

            List<OrderInquiry> orderInquiryList = new List<OrderInquiry>();

            int numberOfRows = 0;

            var customerQuery = dbConnection.Customers.AsQueryable();

            if (customerCode != null && customerCode.Trim().Length > 0)
            {
                customerQuery = customerQuery.Where(c => c.CustomerCode.StartsWith(customerCode));
            }

            if (companyName != null && companyName.Trim().Length > 0)
            {
                customerQuery = customerQuery.Where(c => c.CompanyName.StartsWith(companyName));
            }

            var orders = from customer in customerQuery
                            join order in dbConnection.Orders on customer.CustomerID equals order.CustomerID
                            select new { customer, order, order.OrderDate, order.OrderTotal, customer.CustomerCode, customer.CompanyName };                            

            numberOfRows = orders.Count();

            orders = orders.OrderBy(sortExpression);

            var listOfOrders = orders.Skip((paging.CurrentPageNumber - 1) * paging.PageSize).Take(paging.PageSize);

            paging.TotalRows = numberOfRows;
            paging.TotalPages = AngularJSUtilities.Utilities.CalculateTotalPages(numberOfRows, paging.PageSize);

            foreach (var order in listOfOrders)
            {
                OrderInquiry o = new OrderInquiry();
                o.Customer = order.customer;
                o.Order = order.order;
                orderInquiryList.Add(o);
            }


            transaction.TotalPages = paging.TotalPages;
            transaction.TotalRows = paging.TotalRows;
            transaction.ReturnStatus = true;
            transaction.ReturnMessage.Add(numberOfRows.ToString() + " orders found.");

            return orderInquiryList;

        }



    }
}

