using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngularJSDataServiceInterface;
using AngularJSDataModels;

namespace AngularJSApplicationService
{
    public class ApplicationInitializationBusinessService
    {

         IApplicationDataService _applicationDataService;

        private IApplicationDataService applicationDataService
        {
            get { return _applicationDataService; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ApplicationInitializationBusinessService(IApplicationDataService dataService)
        {
            _applicationDataService = dataService;
            
        }

      
        /// <summary>
        /// Initialize Application
        /// </summary>
        /// <param name="transaction"></param>
        public void InitializeApplication(out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            try
            {
                applicationDataService.CreateSession();
                applicationDataService.InitializeApplication();
                applicationDataService.CommitTransaction(true);
                transaction.ReturnStatus = true;
                transaction.ReturnMessage.Add("Application has been initialized.");
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
                applicationDataService.CloseSession();
            }

        }


        /// <summary>
        /// Initialize Application
        /// </summary>
        /// <param name="transaction"></param>
        public List<ApplicationMenu> GetMenuItems(Boolean isAuthenicated, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            List<ApplicationMenu> menuItems = new List<ApplicationMenu>();

            try
            {
                applicationDataService.CreateSession();
                menuItems = applicationDataService.GetMenuItems(isAuthenicated);             
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
                applicationDataService.CloseSession();               
            }

            return menuItems;

        }

    }
}
