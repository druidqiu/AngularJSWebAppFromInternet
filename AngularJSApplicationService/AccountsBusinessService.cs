using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AngularJSDataServiceInterface;
using AngularJSDataModels;
using AngularJSUtilities;

namespace AngularJSApplicationService
{
    public class AccountsBusinessService
    {

        IAccountsDataService _accountsDataService;

        private IAccountsDataService accountsDataService
        {
            get { return _accountsDataService; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AccountsBusinessService(IAccountsDataService dataService)
        {
            _accountsDataService = dataService;
        }

        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="userName"></param>
        /// <param name="emailAddress"></param>
        /// <param name="password"></param>
        /// <param name="passwordConfirmation"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public User RegisterUser(string firstName, string lastName, string userName, string emailAddress, string password, string passwordConfirmation, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();   
         
            AccountsBusinessRules accountsBusinessRules = new AccountsBusinessRules();

            User user = new User();

            try
            {
             
                user.FirstName = Utilities.UppercaseFirstLetter(firstName.Trim());
                user.LastName = Utilities.UppercaseFirstLetter(lastName.Trim());
                user.EmailAddress = emailAddress.Trim();
                user.Password = password.Trim();
                user.UserName = userName.Trim();

                accountsDataService.CreateSession();

                accountsBusinessRules.ValidateUser(user, accountsDataService);
                accountsBusinessRules.ValidatePassword(password, passwordConfirmation);

                if (accountsBusinessRules.ValidationStatus == true)
                {
                    accountsDataService.BeginTransaction();
                    accountsDataService.RegisterUser(user);
                    accountsDataService.CommitTransaction(true);
                    transaction.ReturnStatus = true;
                    transaction.ReturnMessage.Add("User registered successfully.");              
                }
                else
                {
                    transaction.ReturnStatus = accountsBusinessRules.ValidationStatus;
                    transaction.ReturnMessage = accountsBusinessRules.ValidationMessage;
                    transaction.ValidationErrors = accountsBusinessRules.ValidationErrors;
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
                accountsDataService.CloseSession();
            }

            return user;

        }


        /// <summary>
        /// Update User
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="userName"></param>
        /// <param name="emailAddress"></param>
        /// <param name="password"></param>
        /// <param name="passwordConfirmation"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public User UpdateUser(Guid userID, string firstName, string lastName,
            string userName, string emailAddress, string password, string passwordConfirmation, 
            out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            AccountsBusinessRules accountsBusinessRules = new AccountsBusinessRules();

            User user = new User();

            try
            {
                        
                accountsDataService.CreateSession();

                user = accountsDataService.GetUser(userID);
                user.FirstName = firstName.Trim();
                user.LastName = lastName.Trim();
                user.EmailAddress = emailAddress.Trim();
                user.Password = password.Trim();
                user.UserName = userName.Trim();
                
                accountsBusinessRules.ValidateExistingUser(user, accountsDataService);
                accountsBusinessRules.ValidatePassword(password, passwordConfirmation);

                if (accountsBusinessRules.ValidationStatus == true)
                {
                    accountsDataService.BeginTransaction();
                    accountsDataService.UpdateUser(user);
                    accountsDataService.CommitTransaction(true);
                    transaction.ReturnStatus = true;                  
                }
                else
                {
                    transaction.ReturnStatus = accountsBusinessRules.ValidationStatus;
                    transaction.ReturnMessage = accountsBusinessRules.ValidationMessage;
                    transaction.ValidationErrors = accountsBusinessRules.ValidationErrors;
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
                accountsDataService.CloseSession();
            }

            return user;

        }
        
        
        /// <summary>
        /// Login 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public User Login(string userName, string password, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            AccountsBusinessRules accountsBusinessRules = new AccountsBusinessRules();

            User user = new User();

            try
            {

                user.UserName = userName.Trim();
                user.Password = password.Trim();
             
                accountsDataService.CreateSession();
                user = accountsDataService.Login(userName, password);

                if (user!=null)              
                {
                    accountsDataService.BeginTransaction();
                    accountsDataService.UpdateLastLogin(user);
                    accountsDataService.CommitTransaction(true);
                    transaction.ReturnStatus = true;                 
                }
                else
                {
                    transaction.ReturnStatus = false;
                    transaction.ReturnMessage.Add("Login invalid.");                   
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
                accountsDataService.CloseSession();
            }

            return user;

        }

        /// <summary>
        /// Get User
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public User GetUser(Guid userID, out TransactionalInformation transaction)
        {
            transaction = new TransactionalInformation();

            AccountsBusinessRules accountsBusinessRules = new AccountsBusinessRules();

            User user = new User();

            try
            {              

                accountsDataService.CreateSession();
                user = accountsDataService.GetUser(userID);

                if (user != null)
                {                   
                    transaction.ReturnStatus = true;
                }
                else
                {
                    transaction.ReturnStatus = false;
                    transaction.ReturnMessage.Add("user id not found.");
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
                accountsDataService.CloseSession();
            }

            return user;

        }

    }
}
