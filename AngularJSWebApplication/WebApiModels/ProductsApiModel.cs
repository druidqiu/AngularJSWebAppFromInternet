using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AngularJSDataModels;

namespace AngularJSWebApplication.WebApiModels
{

    /// <summary>
    /// Products API Model
    /// </summary>
    public class ProductsApiModel : TransactionalInformation
    {
        public List<Product> Products;
        public Product Product;

        public ProductsApiModel()
        {
            Product = new Product();
            Products = new List<Product>();
        }

    }

    /// <summary>
    /// Product DTO
    /// </summary>
    public class ProductDTO
    {        
        public Guid ProductID { get; set; }
        public string ProductCode { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public string UnitOfMeasure { get; set; }
      
    }

    public class ProductInquiryDTO
    {
        public string ProductCode { get; set; }
        public string Description { get; set; }
        public int CurrentPageNumber { get; set; }
        public string SortExpression { get; set; }
        public string SortDirection { get; set; }
        public int PageSize { get; set; }
    }

}