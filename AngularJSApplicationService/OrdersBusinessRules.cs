using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AngularJSDataServiceInterface;
using AngularJSDataModels;

namespace AngularJSApplicationService
{
    public class OrdersBusinessRules : ValidationRules
    {

        IOrdersDataService ordersDataService;

        /// <summary>
        /// Initialize user Business Rules
        /// </summary>
        /// <param name="user"></param>
        /// <param name="dataService"></param>
        public void InitializeCustomersBusinessRules(Order order, IOrdersDataService dataService)
        {
            ordersDataService = dataService;
            InitializeValidationRules(order);

        }

       
        /// <summary>
        /// Validate Order
        /// </summary>
        /// <param name="orderr"></param>
        /// <param name="dataService"></param>
        public void ValidateOrder(Order order, IOrdersDataService dataService)
        {
            ordersDataService = dataService;

            InitializeValidationRules(order);
           
            ValidateRequired("ShipName", "Ship To Name");
            ValidateRequired("ShipCity", "Ship To City");
            ValidateRequired("ShipRegion", "Ship To Region");
            ValidateRequired("ShipPostalCode", "Ship To Postal Code");
            ValidateRequired("ShipCountry", "Ship To Country");
            ValidateRequired("ShipAddress", "Ship To Address");                            
            ValidateRequiredDate("RequiredDate", "Required Ship Date");
            ValidateSelectedValue("ShipVia", "Ship Via");
       
        }

        /// <summary>
        /// Validate Order Detail Line Item
        /// </summary>
        /// <param name="order"></param>
        /// <param name="dataService"></param>
        public void ValidateOrderDetailLineItem(OrderDetail order, IOrdersDataService dataService)
        {
            ordersDataService = dataService;

            InitializeValidationRules(order);

            ValidateGreaterThanZero("Quantity", "Order Quantity");

        }


    }
}

