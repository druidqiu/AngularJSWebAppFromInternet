using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AngularJSDataModels
{
    public class Order
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity),Key()]
        public long OrderID { get; set; }
        public Guid CustomerID { get; set; }
        public DateTime OrderDate  { get; set; }
        public DateTime RequiredDate  { get; set; }
		public int ShipVia { get; set; }	
	    public string ShipName { get; set; }
	    public string ShipAddress { get; set; }
	    public string ShipCity { get; set; }
	    public string ShipRegion { get; set; }
	    public string ShipPostalCode { get; set; }
	    public string ShipCountry { get; set; }
        public decimal OrderTotal { get; set; }
        public DateTime? DateUpdated { get; set; }
    }

    public class OrderDetail
    {
        [Key]
        public Guid OrderDetailID { get; set; }
        public long OrderID { get; set; }
        public int LineItemNumber { get; set; }
        public Guid ProductID { get; set; }
        public int Quantity { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
    }

    public class OrderInquiry
    {
        public Customer Customer;
        public Order Order;        
    }

    public class OrderDetails
    {
        public Customer Customer;
        public Order Order;
        public OrderDetail OrderDetail;
        public Product Product;
    }

}
