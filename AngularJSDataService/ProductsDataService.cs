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
    public class ProductsDataService : EntityFrameworkDataService, IProductsDataService
    {
        /// <summary>
        /// Create Product
        /// </summary>
        /// <param name="product"></param>
        public void CreateProduct(Product product)
        {
            product.ProductID = Guid.NewGuid();
            DateTime now = DateTime.Now;
            product.DateCreated = now;
            product.DateUpdated = now;

            dbConnection.Products.Add(product);

        }


        /// <summary>
        /// Update Product
        /// </summary>
        /// <param name="product"></param>
        public void UpdateProduct(Product product)
        {           
            DateTime now = DateTime.Now;          
            product.DateUpdated = now;

        }

        /// <summary>
        /// Validate Duplicate Product
        /// </summary>
        /// <param name="productCode"></param>
        /// <returns></returns>
        public Boolean ValidateDuplicateProduct(string productCode)
        {
            Product product = dbConnection.Products.SingleOrDefault(c => c.ProductCode == productCode);
            if (product == null) return true;
            return false;
        }

        /// <summary>
        /// Validate Duplicate Product
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="productCode"></param>
        /// <returns></returns>
        public Boolean ValidateDuplicateProduct(Guid productID, string productCode)
        {
            Product product = dbConnection.Products.SingleOrDefault(c => c.ProductCode == productCode && c.ProductID != productID);
            if (product == null) return true;
            return false;
        }

        /// <summary>
        /// Get Product
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public Product GetProduct(Guid productID)
        {
            Product product = dbConnection.Products.SingleOrDefault(c => c.ProductID == productID);
            return product;
        }
      
        /// <summary>
        /// Product Inquiry
        /// </summary>
        /// <param name="productCode"></param>
        /// <param name="description"></param>
        /// <param name="paging"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public List<Product> ProductInquiry(string productCode, string description, DataGridPagingInformation paging, out TransactionalInformation transaction)
        {

            transaction = new TransactionalInformation();

            string sortExpression = paging.SortExpression;

            if (paging.SortDirection != string.Empty)
                sortExpression = sortExpression + " " + paging.SortDirection;

            List<Product> productInquiry = new List<Product>();

            int numberOfRows = 0;

            var productQuery = dbConnection.Products.AsQueryable();

            if (productCode != null && productCode.Trim().Length > 0)
            {
                productQuery = productQuery.Where(c => c.ProductCode.StartsWith(productCode));
            }

            if (description != null && description.Trim().Length > 0)
            {
                productQuery = productQuery.Where(c => c.Description.StartsWith(description));
            }

            var products = from p in productQuery
                            select new { p.ProductID, p.ProductCode, p.Description, p.UnitPrice, p.UnitOfMeasure };

            numberOfRows = products.Count();

            products = products.OrderBy(sortExpression);

            var productList = products.Skip((paging.CurrentPageNumber - 1) * paging.PageSize).Take(paging.PageSize);

            paging.TotalRows = numberOfRows;
            paging.TotalPages = AngularJSUtilities.Utilities.CalculateTotalPages(numberOfRows, paging.PageSize);

            foreach (var product in productList)
            {
                Product productData = new Product();
                productData.ProductID = product.ProductID;
                productData.ProductCode = product.ProductCode;
                productData.Description = product.Description;
                productData.UnitOfMeasure = product.UnitOfMeasure;
                productData.UnitPrice = product.UnitPrice;              

                productInquiry.Add(productData);
            }


            transaction.TotalPages = paging.TotalPages;
            transaction.TotalRows = paging.TotalRows;
            transaction.ReturnStatus = true;
            transaction.ReturnMessage.Add(numberOfRows.ToString() + " product records found.");

            return productInquiry;

        }



    }
}

