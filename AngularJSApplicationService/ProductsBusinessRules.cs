using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AngularJSDataServiceInterface;
using AngularJSDataModels;

namespace AngularJSApplicationService
{
    public class ProductsBusinessRules : ValidationRules
    {

        IProductsDataService productsDataService;

        /// <summary>
        /// Initialize user Business Rules
        /// </summary>
        /// <param name="user"></param>
        /// <param name="dataService"></param>
        public void InitializeProductsBusinessRules(Product product, IProductsDataService dataService)
        {
            productsDataService = dataService;
            InitializeValidationRules(product);

        }

        /// <summary>
        /// Validate User
        /// </summary>
        /// <param name="user"></param>
        /// <param name="dataService"></param>
        public void ValidateProduct(Product product, IProductsDataService dataService)
        {
            productsDataService = dataService;

            InitializeValidationRules(product);

            ValidateRequired("ProductCode", "Product Code");
            ValidateRequired("Description", "Description");
            ValidateRequired("UnitOfMeasure", "Unit Of Measure");
            ValidateDecimalIsNotZero("UnitPrice", "Unit Price");
            ValidateDecimalGreaterThanZero("UnitPrice", "Unit Price");

            ValidateUniqueProductCode(product.ProductCode);

       
        }

        /// <summary>
        /// Validate Product Update
        /// </summary>
        /// <param name="product"></param>
        /// <param name="dataService"></param>
        public void ValidateProductUpdate(Product product, IProductsDataService dataService)
        {
            productsDataService = dataService;

            InitializeValidationRules(product);

            ValidateRequired("ProductCode", "Product Code");
            ValidateRequired("Description", "Description");
            ValidateRequired("UnitOfMeasure", "UnitOfMeasure");
            ValidateDecimalIsNotZero("UnitPrice", "Unit Price");
            ValidateDecimalGreaterThanZero("UnitPrice", "Unit Price");

            ValidateUniqueProductCode(product.ProductID, product.ProductCode);


        }

        /// <summary>
        /// Validate Unique Product Code
        /// </summary>
        /// <param name="productCode"></param>
        public void ValidateUniqueProductCode(string productCode)
        {
            Boolean valid = productsDataService.ValidateDuplicateProduct(productCode);
            if (valid==false)
            {
                AddValidationError("ProductCode", "Product Code " + productCode + " already exists.");
            }
           
        }

        /// <summary>
        /// Validate Unique Product Code
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="productCode"></param>
        public void ValidateUniqueProductCode(Guid productID, string productCode)
        {
            Boolean valid = productsDataService.ValidateDuplicateProduct(productID, productCode);
            if (valid == false)
            {
                AddValidationError("ProductCode", "Product Code " + productCode + " already exists.");
            }

        }

    }
}
