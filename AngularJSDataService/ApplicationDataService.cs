using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngularJSDataServiceInterface;
using AngularJSDataModels;
using System.Linq.Dynamic;

namespace AngularJSDataService
{
    public class ApplicationDataService : EntityFrameworkDataService, IApplicationDataService
    {
        public void InitializeApplication()
        {

            int menuItemsCount = MenuItemsCount();
            if (menuItemsCount > 0) return;

            ApplicationMenu menuItem;
           
            menuItem = CreateMenuItem("Home", "#Main/Home", "Main", false, 1);          
            dbConnection.ApplicationMenuItems.Add(menuItem);

            menuItem = CreateMenuItem("About", "#Main/About", "Main", false, 2);
            dbConnection.ApplicationMenuItems.Add(menuItem);

            menuItem = CreateMenuItem("Register", "#Accounts/Register", "Main", false, 3);
            dbConnection.ApplicationMenuItems.Add(menuItem);

            menuItem = CreateMenuItem("Login", "#Accounts/Login", "Main", false, 4);
            dbConnection.ApplicationMenuItems.Add(menuItem);

            menuItem = CreateMenuItem("Customers", "#Customers/CustomerInquiry", "Main", true, 1);
            dbConnection.ApplicationMenuItems.Add(menuItem);

            menuItem = CreateMenuItem("Orders", "#Orders/OrderEntryCustomerInquiry", "Main", true, 2);
            dbConnection.ApplicationMenuItems.Add(menuItem);

            menuItem = CreateMenuItem("Products", "#Products/ProductInquiry", "Main", true, 3);
            dbConnection.ApplicationMenuItems.Add(menuItem);

            menuItem = CreateMenuItem("My Account", "#Accounts/MyAccount", "Main", true, 4);
            dbConnection.ApplicationMenuItems.Add(menuItem);

            menuItem = CreateMenuItem("Logout", "#Accounts/Logout", "Main", true, 5);
            dbConnection.ApplicationMenuItems.Add(menuItem);

            menuItem = CreateMenuItem("Home", "#Main/Home", "Main", true, 6);
            dbConnection.ApplicationMenuItems.Add(menuItem);

            menuItem = CreateMenuItem("About", "#Main/About", "Main", true, 7);
            dbConnection.ApplicationMenuItems.Add(menuItem);

            menuItem = CreateMenuItem("Customer Inquiry", "#Customers/CustomerInquiry", "Customers", true, 1);
            dbConnection.ApplicationMenuItems.Add(menuItem);

            menuItem = CreateMenuItem("Customer Maintenance", "#Customers/CustomerMaintenance", "Customers", true, 2);
            dbConnection.ApplicationMenuItems.Add(menuItem);

            menuItem = CreateMenuItem("Import Customer Test Data", "#Customers/ImportCustomers", "Customers", true, 3);
            dbConnection.ApplicationMenuItems.Add(menuItem);

            menuItem = CreateMenuItem("Order Entry", "#Orders/OrderEntryCustomerInquiry", "Orders", true, 1);
            dbConnection.ApplicationMenuItems.Add(menuItem);

            menuItem = CreateMenuItem("Order Inquiry", "#Orders/OrderInquiry", "Orders", true, 2);
            dbConnection.ApplicationMenuItems.Add(menuItem);          

            menuItem = CreateMenuItem("Product Inquiry", "#Products/ProductInquiry", "Products", true, 1);
            dbConnection.ApplicationMenuItems.Add(menuItem);

            menuItem = CreateMenuItem("Product Maintenance", "#Products/ProductMaintenance", "Products", true, 2);
            dbConnection.ApplicationMenuItems.Add(menuItem);

            menuItem = CreateMenuItem("Import Product Test Data", "#Products/ImportProducts", "Products", true, 4);
            dbConnection.ApplicationMenuItems.Add(menuItem);


            Shipper shipper = new Shipper();
            shipper.ShipperID = 1;
            shipper.ShipperName = "Federal Shipper";

            dbConnection.Shippers.Add(shipper);

            shipper = new Shipper();
            shipper.ShipperID = 2;
            shipper.ShipperName = "United Shipper";

            dbConnection.Shippers.Add(shipper);

            shipper = new Shipper();
            shipper.ShipperID = 3;
            shipper.ShipperName = "Overnight Shipper";

            dbConnection.Shippers.Add(shipper);

            dbConnection.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('Orders', RESEED, 10000)");
          
      
        }

        /// <summary>
        /// Create Menu Item
        /// </summary>
        /// <param name="description"></param>
        /// <param name="route"></param>
        /// <param name="module"></param>
        /// <param name="requiresAuthenication"></param>
        /// <param name="menuOrder"></param>
        /// <returns></returns>
        private ApplicationMenu CreateMenuItem(string description, string route, string module, Boolean requiresAuthenication, int menuOrder)
        {
            ApplicationMenu menuItem = new ApplicationMenu();
            menuItem.MenuId = Guid.NewGuid();
            menuItem.Route = route;
            menuItem.Description = description;
            menuItem.RequiresAuthenication = requiresAuthenication;
            menuItem.MenuOrder = menuOrder;
            menuItem.Module = module;
            return menuItem;
        }

        /// <summary>
        /// Menu Items COunt
        /// </summary>
        /// <returns></returns>
        private int MenuItemsCount()
        {
            int menuItems;
            try
            {
                menuItems = (from m in dbConnection.ApplicationMenuItems.AsEnumerable() select m).Count();
            }
            catch (Exception)
            {
                menuItems = 0;
            }

            return menuItems;
        }

        /// <summary>
        /// Get Menu Items
        /// </summary>
        /// <param name="isAuthenicated"></param>
        /// <returns></returns>
        public List<ApplicationMenu> GetMenuItems(Boolean isAuthenicated)
        {

            var menuQuery = dbConnection.ApplicationMenuItems.AsQueryable();
            var menuItems = (from m in menuQuery.Where(m => m.RequiresAuthenication == isAuthenicated).OrderBy("MenuOrder") select m).ToList();         
            return menuItems;

        }

    }
}
