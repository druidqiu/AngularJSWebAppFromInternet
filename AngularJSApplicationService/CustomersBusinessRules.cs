using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AngularJSDataServiceInterface;
using AngularJSDataModels;

namespace AngularJSApplicationService
{
    public class CustomersBusinessRules : ValidationRules
    {

        ICustomersDataService customersDataService;

        /// <summary>
        /// Initialize user Business Rules
        /// </summary>
        /// <param name="user"></param>
        /// <param name="dataService"></param>
        public void InitializeCustomersBusinessRules(Customer customer, ICustomersDataService dataService)
        {
            customersDataService = dataService;
            InitializeValidationRules(customer);

        }

        /// <summary>
        /// Validate User
        /// </summary>
        /// <param name="user"></param>
        /// <param name="dataService"></param>
        public void ValidateCustomer(Customer customer, ICustomersDataService dataService)
        {
            customersDataService = dataService;

            InitializeValidationRules(customer);

            ValidateRequired("CustomerCode", "Customer Code");
            ValidateRequired("CompanyName", "Company Name");

            ValidateUniqueCustomerCode(customer.CustomerCode);

       
        }

        public void ValidateCustomerUpdate(Customer customer, ICustomersDataService dataService)
        {
            customersDataService = dataService;

            InitializeValidationRules(customer);

            ValidateRequired("CustomerCode", "Customer Code");
            ValidateRequired("CompanyName", "Company Name");

            ValidateUniqueCustomerCode(customer.CustomerID, customer.CustomerCode);


        }

        /// <summary>
        /// Validate Unique Customer Code
        /// </summary>
        /// <param name="customerCode"></param>
        public void ValidateUniqueCustomerCode(string customerCode)
        {
            Boolean valid = customersDataService.ValidateDuplicateCustomer(customerCode);
            if (valid==false)
            {
                AddValidationError("CustomerCode", "Customer Code " + customerCode + " already exists.");
            }
           
        }

        public void ValidateUniqueCustomerCode(Guid customerID, string customerCode)
        {
            Boolean valid = customersDataService.ValidateDuplicateCustomer(customerID, customerCode);
            if (valid == false)
            {
                AddValidationError("CustomerCode", "Customer Code " + customerCode + " already exists.");
            }

        }

    }
}
