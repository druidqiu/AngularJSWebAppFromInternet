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
    public class CustomersDataService : EntityFrameworkDataService, ICustomersDataService
    {
        /// <summary>
        /// Create Customer
        /// </summary>
        /// <param name="customer"></param>
        public void CreateCustomer(Customer customer)
        {
            customer.CustomerID = Guid.NewGuid();
            DateTime now = DateTime.Now;
            customer.DateCreated = now;
            customer.DateUpdated = now;

            dbConnection.Customers.Add(customer);

        }


        /// <summary>
        /// Update Customer
        /// </summary>
        /// <param name="customer"></param>
        public void UpdateCustomer(Customer customer)
        {           
            DateTime now = DateTime.Now;          
            customer.DateUpdated = now;

        }

        /// <summary>
        /// Validate Duplicate Customer
        /// </summary>
        /// <param name="customerCode"></param>
        /// <returns></returns>
        public Boolean ValidateDuplicateCustomer(string customerCode)
        {
            Customer customer = dbConnection.Customers.SingleOrDefault(c => c.CustomerCode == customerCode);
            if (customer == null) return true;
            return false;
        }

        /// <summary>
        /// Validate Duplicate Customer
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="customerCode"></param>
        /// <returns></returns>
        public Boolean ValidateDuplicateCustomer(Guid customerID, string customerCode)
        {
            Customer customer = dbConnection.Customers.SingleOrDefault(c => c.CustomerCode == customerCode && c.CustomerID != customerID);
            if (customer == null) return true;
            return false;
        }

        /// <summary>
        /// Get Customer
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public Customer GetCustomer(Guid customerID)
        {
            Customer customer = dbConnection.Customers.SingleOrDefault(c => c.CustomerID == customerID);
            return customer;
        }
      
        /// <summary>
        /// Customer Inquiry
        /// </summary>
        /// <param name="customerCode"></param>
        /// <param name="companyName"></param>
        /// <param name="paging"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public List<Customer> CustomerInquiry(string customerCode, string companyName, DataGridPagingInformation paging, out TransactionalInformation transaction)
        {

            transaction = new TransactionalInformation();

            string sortExpression = paging.SortExpression;

            if (paging.SortDirection != string.Empty)
                sortExpression = sortExpression + " " + paging.SortDirection;

            List<Customer> customerInquiry = new List<Customer>();

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

            var customers = from c in customerQuery
                            select new { c.CustomerID, c.CustomerCode, c.CompanyName, c.WebSiteUrl, c.City, c.Country, c.Address, c.Region, c.PostalCode };

            numberOfRows = customers.Count();

            customers = customers.OrderBy(sortExpression);

            var customerList = customers.Skip((paging.CurrentPageNumber - 1) * paging.PageSize).Take(paging.PageSize);

            paging.TotalRows = numberOfRows;
            paging.TotalPages = AngularJSUtilities.Utilities.CalculateTotalPages(numberOfRows, paging.PageSize);

            foreach (var customer in customerList)
            {
                Customer customerData = new Customer();
                customerData.CustomerID = customer.CustomerID;
                customerData.CustomerCode = customer.CustomerCode;
                customerData.CompanyName = customer.CompanyName;
                customerData.City = customer.City;
                customerData.Address = customer.Address;
                customerData.WebSiteUrl = customer.WebSiteUrl;

                if (customer.Region == "NULL")
                    customerData.Region = "NA";
                else 
                    customerData.Region = customer.Region;

                customerData.PostalCode = customer.PostalCode;

                customerInquiry.Add(customerData);
            }


            transaction.TotalPages = paging.TotalPages;
            transaction.TotalRows = paging.TotalRows;
            transaction.ReturnStatus = true;
            transaction.ReturnMessage.Add(numberOfRows.ToString() + " customer records found.");

            return customerInquiry;

        }



    }
}
