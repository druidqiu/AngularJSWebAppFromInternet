using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AngularJSDataModels;

namespace AngularJSWebApplication.WebApiModels
{

    public class CustomersApiModel : TransactionalInformation
    {
        public List<Customer> Customers;
        public Customer Customer;

        public CustomersApiModel()
        {
            Customer = new Customer();
            Customers = new List<Customer>();
        }

    }

    public class CustomerDTO
    {        

        public Guid CustomerID { get; set; }
        public string CustomerCode { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string WebSiteUrl { get; set; }
             
    }

    public class CustomerInquiryDTO
    {
        public string CustomerCode { get; set; }
        public string CompanyName { get; set; }
        public int CurrentPageNumber { get; set; }
        public string SortExpression { get; set; }
        public string SortDirection { get; set; }
        public int PageSize { get; set; }
    }
}