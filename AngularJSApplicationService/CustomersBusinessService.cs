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
    public class CustomersBusinessService
    {

        ICustomersDataService _customersDataService;

        private ICustomersDataService customersDataService
        {
            get { return _customersDataService; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomersBusinessService(ICustomersDataService dataService)
        {
            _customersDataService = dataService;
        }

        /// <summary>
        /// customer Inquiry
        /// </summary>
        /// <param name="customerCode"></param>
        /// <param name="companyName"></param>
        /// <param name="paging"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public List<Customer> CustomerInquiry(string customerCode, string companyName, DataGridPagingInformation paging, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            List<Customer> customers = new List<Customer>();

            try
            {             
                customersDataService.CreateSession();
                customers = customersDataService.CustomerInquiry(customerCode, companyName, paging, out transaction);                                            
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
                customersDataService.CloseSession();
            }

            return customers;

        }


        /// <summary>
        /// Get Customer
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public Customer GetCustomer(Guid customerID, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            Customer customer = new Customer();

            try
            {
                customersDataService.CreateSession();
                customer = customersDataService.GetCustomer(customerID);
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
                customersDataService.CloseSession();
            }

            return customer;

        }


        /// <summary>
        /// Create Customer
        /// </summary>
        /// <param name="customerCode"></param>
        /// <param name="companyName"></param>
        /// <param name="address"></param>
        /// <param name="city"></param>
        /// <param name="region"></param>
        /// <param name="postalCode"></param>
        /// <param name="country"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="webSiteUrl"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public Customer CreateCustomer(string customerCode, string companyName, string address, string city, string region, string postalCode, string country, string phoneNumber, string webSiteUrl, out TransactionalInformation transaction)
        {
       
            transaction = new TransactionalInformation();

            CustomersBusinessRules customersBusinessRules = new CustomersBusinessRules();

            Customer customer = new Customer();

            customer.CustomerCode = customerCode;
            customer.CompanyName = companyName;
            customer.Address = address;
            customer.City = city;
            customer.Region = region;
            customer.PostalCode = postalCode;
            customer.Country = country;
            customer.PhoneNumber = phoneNumber;
            customer.WebSiteUrl = webSiteUrl;

            try
            {

                customersDataService.CreateSession();

                customersBusinessRules.ValidateCustomer(customer, customersDataService);

                if (customersBusinessRules.ValidationStatus == true)
                {
                    customersDataService.BeginTransaction();
                    customersDataService.CreateCustomer(customer);
                    customersDataService.CommitTransaction(true);
                    transaction.ReturnStatus = true;
                    transaction.ReturnMessage.Add("Customer successfully created.");
                }
                else
                {
                    transaction.ReturnStatus = customersBusinessRules.ValidationStatus;
                    transaction.ReturnMessage = customersBusinessRules.ValidationMessage;
                    transaction.ValidationErrors = customersBusinessRules.ValidationErrors;
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
                customersDataService.CloseSession();
            }

            return customer;


        }

        /// <summary>
        /// Update Customer
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="customerCode"></param>
        /// <param name="companyName"></param>
        /// <param name="address"></param>
        /// <param name="city"></param>
        /// <param name="region"></param>
        /// <param name="postalCode"></param>
        /// <param name="country"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="webSiteUrl"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public Customer UpdateCustomer(Guid customerID, string customerCode, string companyName, string address, string city, string region, string postalCode, string country, string phoneNumber, string webSiteUrl, out TransactionalInformation transaction)
        {

            transaction = new TransactionalInformation();

            CustomersBusinessRules customersBusinessRules = new CustomersBusinessRules();

            Customer customer = new Customer();

            try
            {

                customersDataService.CreateSession();

                customer = customersDataService.GetCustomer(customerID);
                customer.CustomerCode = customerCode;
                customer.CompanyName = companyName;
                customer.Address = address;
                customer.City = city;
                customer.Region = region;
                customer.PostalCode = postalCode;
                customer.Country = country;
                customer.PhoneNumber = phoneNumber;
                customer.WebSiteUrl = webSiteUrl;

                customersBusinessRules.ValidateCustomerUpdate(customer, customersDataService);

                if (customersBusinessRules.ValidationStatus == true)
                {
                    customersDataService.BeginTransaction();
                    customersDataService.UpdateCustomer(customer);
                    customersDataService.CommitTransaction(true);
                    transaction.ReturnStatus = true;
                    transaction.ReturnMessage.Add("Customer successfully updated at " + customer.DateUpdated.ToString());
                }
                else
                {
                    transaction.ReturnStatus = customersBusinessRules.ValidationStatus;
                    transaction.ReturnMessage = customersBusinessRules.ValidationMessage;
                    transaction.ValidationErrors = customersBusinessRules.ValidationErrors;
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
                customersDataService.CloseSession();
            }

            return customer;


        }        
        
        /// <summary>
        /// Import Customers
        /// </summary>
        /// <param name="transaction"></param>
        public void ImportCustomers(out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
       
            try
            {

                string importFileName = ConfigurationManager.AppSettings["CustomerImportData"];

                System.IO.StreamReader csv_file = File.OpenText(importFileName);

                customersDataService.CreateSession();

                Boolean firstLine = true;
                int customerRecordsAdded = 0;

                while (csv_file.Peek() >= 0)
                {
                    // read and add a line
                    string line = csv_file.ReadLine();
                    string[] columns = line.Split('\t');
                    if (firstLine == false) {
                        if (ImportCustomer(columns)==true)
                            customerRecordsAdded++;
                    }
                    firstLine = false;
                }

                customersDataService.CommitTransaction(true);

                csv_file.Close();
         
                transaction.ReturnStatus = true;
                transaction.ReturnMessage.Add(customerRecordsAdded.ToString() + " customer successfully imported.");

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
                customersDataService.CloseSession();
            }


        }

        /// <summary>
        /// Import Customer
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        private Boolean ImportCustomer(string[] columns)
        {

            Customer customer = new Customer();
            
            customer.CustomerCode = columns[0].Trim();
            customer.CompanyName = columns[1].Trim();
            customer.Address = columns[4].Trim();
            customer.City = columns[5].Trim();
            customer.Region = columns[6].Trim();
            customer.PostalCode = columns[7].Trim();
            customer.Country = columns[8].Trim();
            customer.PhoneNumber = columns[9].Trim();

            Boolean valid = customersDataService.ValidateDuplicateCustomer(customer.CustomerCode);
            if (valid)
            {
                customersDataService.CreateCustomer(customer);
            }

            return valid;

        }


    }
}
