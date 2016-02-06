using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AngularJSDataModels;

namespace AngularJSWebApplication.WebApiModels
{
  
    public class OrdersApiModel : TransactionalInformation
    {
        public List<OrderInquiry> Orders;
        public List<Shipper> Shippers;
        public Order Order;
        public OrderDetail OrderDetail;
        public List<OrderDetails> OrderDetails;
        public Customer Customer;

        public OrdersApiModel()
        {
            Order = new Order();
            OrderDetail = new OrderDetail();
            Customer = new Customer();
            Orders = new List<OrderInquiry>();
            Shippers = new List<Shipper>();
            OrderDetails = new List<OrderDetails>();
        }

    }

    public class OrderDTO
    {
        public int OrderID { get; set; }
        public Guid CustomerID { get; set; }
        public DateTime RequiredDate { get; set; }
        public int ShipperID { get; set; }
        public string ShipName { get; set; }       
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipRegion { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipCountry { get; set; }             
    }

    public class GetOrderDTO
    {
        public int OrderID { get; set; }       
    }

    public class OrderInquiryDTO
    {
        public string CustomerCode { get; set; }
        public string CompanyName { get; set; }
        public int CurrentPageNumber { get; set; }
        public string SortExpression { get; set; }
        public string SortDirection { get; set; }
        public int PageSize { get; set; }
    }

    public class OrderDetailDTO
    {             
        public int OrderID { get; set; }
        public Guid OrderDetailID { get; set; }
        public Guid ProductID { get; set; }
        public int Quantity { get; set; }    
    }

}