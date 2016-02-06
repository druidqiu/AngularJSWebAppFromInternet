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
    public class ProductsBusinessService
    {

        IProductsDataService _productsDataService;

        private IProductsDataService productsDataService
        {
            get { return _productsDataService; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductsBusinessService(IProductsDataService dataService)
        {
            _productsDataService = dataService;
        }

        /// <summary>
        /// product Inquiry
        /// </summary>
        /// <param name="productCode"></param>
        /// <param name="companyName"></param>
        /// <param name="paging"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public List<Product> ProductInquiry(string productCode, string description, DataGridPagingInformation paging, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            List<Product> products = new List<Product>();

            try
            {             
                productsDataService.CreateSession();
                products = productsDataService.ProductInquiry(productCode, description, paging, out transaction);                                            
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
                productsDataService.CloseSession();
            }

            return products;

        }

        /// <summary>
        /// Get Product
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public Product GetProduct(Guid productID, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            Product product = new Product();

            try
            {
                productsDataService.CreateSession();
                product = productsDataService.GetProduct(productID);
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
                productsDataService.CloseSession();
            }

            return product;

        }


        /// <summary>
        /// Create Product
        /// </summary>
        /// <param name="productCode"></param>
        /// <param name="description"></param>
        /// <param name="unitPrice"></param>
        /// <param name="unitOfMeasure"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public Product CreateProduct(string productCode, string description, decimal unitPrice, string unitOfMeasure, out TransactionalInformation transaction)
        {
       
            transaction = new TransactionalInformation();

            ProductsBusinessRules productsBusinessRules = new ProductsBusinessRules();

            Product product = new Product();

            product.ProductCode = productCode;
            product.Description = description;
            product.UnitPrice = unitPrice;
            product.UnitOfMeasure = unitOfMeasure;          
  
            try
            {

                productsDataService.CreateSession();

                productsBusinessRules.ValidateProduct(product, productsDataService);

                if (productsBusinessRules.ValidationStatus == true)
                {
                    productsDataService.BeginTransaction();
                    productsDataService.CreateProduct(product);
                    productsDataService.CommitTransaction(true);
                    transaction.ReturnStatus = true;
                    transaction.ReturnMessage.Add("Product successfully created.");
                }
                else
                {
                    transaction.ReturnStatus = productsBusinessRules.ValidationStatus;
                    transaction.ReturnMessage = productsBusinessRules.ValidationMessage;
                    transaction.ValidationErrors = productsBusinessRules.ValidationErrors;
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
                productsDataService.CloseSession();
            }

            return product;


        }

        /// <summary>
        /// Update Product
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="productCode"></param>
        /// <param name="description"></param>
        /// <param name="unitPrice"></param>
        /// <param name="unitOfMeasure"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public Product UpdateProduct(Guid productID, string productCode, string description, decimal unitPrice, string unitOfMeasure, out TransactionalInformation transaction)
        {

            transaction = new TransactionalInformation();

            ProductsBusinessRules productsBusinessRules = new ProductsBusinessRules();

            Product product = new Product();

            try
            {

                productsDataService.CreateSession();

                product = productsDataService.GetProduct(productID);
                product.ProductCode = productCode;
                product.Description = description;
                product.UnitPrice = unitPrice;
                product.UnitOfMeasure = unitOfMeasure;            

                productsBusinessRules.ValidateProductUpdate(product, productsDataService);

                if (productsBusinessRules.ValidationStatus == true)
                {
                    productsDataService.BeginTransaction();
                    productsDataService.UpdateProduct(product);
                    productsDataService.CommitTransaction(true);
                    transaction.ReturnStatus = true;
                    transaction.ReturnMessage.Add("Product successfully updated at " + product.DateUpdated.ToString());
                }
                else
                {
                    transaction.ReturnStatus = productsBusinessRules.ValidationStatus;
                    transaction.ReturnMessage = productsBusinessRules.ValidationMessage;
                    transaction.ValidationErrors = productsBusinessRules.ValidationErrors;
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
                productsDataService.CloseSession();
            }

            return product;


        }
        
        /// <summary>
        /// Import Products
        /// </summary>
        /// <param name="transaction"></param>
        public void ImportProducts(out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();
       
            try
            {

                string importFileName = ConfigurationManager.AppSettings["ProductImportData"];

                System.IO.StreamReader csv_file = File.OpenText(importFileName);

                productsDataService.CreateSession();

                Boolean firstLine = true;
                int productRecordsAdded = 0;

                while (csv_file.Peek() >= 0)
                {
                    // read and add a line
                    string line = csv_file.ReadLine();
                    string[] columns = line.Split('\t');
                    if (firstLine == false) {
                        if (ImportProduct(columns)==true)
                            productRecordsAdded++;
                    }
                    firstLine = false;
                }

                productsDataService.CommitTransaction(true);

                csv_file.Close();
         
                transaction.ReturnStatus = true;
                transaction.ReturnMessage.Add(productRecordsAdded.ToString() + " product successfully imported.");

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
                productsDataService.CloseSession();
            }


        }

        /// <summary>
        /// Import Product
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        private Boolean ImportProduct(string[] columns)
        {

            Product product = new Product();

            product.ProductCode = GenerateProductCode(columns[0].Trim(), columns[1].Trim());      
            product.Description = columns[1].Trim();
            product.UnitPrice = Convert.ToDecimal(columns[5].Trim());
            product.UnitOfMeasure = columns[4].Trim();
         
            Boolean valid = productsDataService.ValidateDuplicateProduct(product.ProductCode);
            if (valid)
            {
                productsDataService.CreateProduct(product);
            }

            return valid;

        }

        private string GenerateProductCode(string productCode, string description)
        {
            string newProductCode = string.Empty;

            if (description.Contains(" ") == false)
            {
                newProductCode = description + "-" + productCode;
                newProductCode = newProductCode.ToUpper();
                return newProductCode;
            }

            string[] elements = description.Split(' ');
            for (int i = 0; i < elements.Length; i++ )
            {
                newProductCode = newProductCode + elements[i].Substring(0, 1);
            }
            newProductCode = newProductCode + "-" + productCode;
            newProductCode = newProductCode.ToUpper();

            return newProductCode;

        }

    }
}
