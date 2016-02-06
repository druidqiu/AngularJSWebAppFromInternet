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
    

    public class AccountsDataService : EntityFrameworkDataService, IAccountsDataService
    {
        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="userName"></param>
        /// <param name="emailAddress"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public void RegisterUser(User user)
        {
            DateTime now = DateTime.Now;
        
            user.UserId = Guid.NewGuid();          
            user.DateCreated = now;
            user.DateLastLogin = now;
            user.DateUpdated = now;

            dbConnection.Users.Add(user);       
            
        }

        /// <summary>
        /// Update User
        /// </summary>
        /// <param name="user"></param>
        public void UpdateUser(User user)
        {
            DateTime now = DateTime.Now;           
            user.DateUpdated = now;          

        }

        /// <summary>
        /// Get User By UserName
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public User GetUserByUserName(string userName)
        {
            User user = dbConnection.Users.SingleOrDefault(u => u.UserName == userName);
            return user;
        }


        /// <summary>
        /// Get User by ID
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public User GetUser(Guid userID)
        {
            User user = dbConnection.Users.SingleOrDefault(u => u.UserId == userID);
            return user;
        }


        /// <summary>
        /// Login
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User Login(string userName, string password)
        {
            User user = dbConnection.Users.SingleOrDefault(u => u.UserName == userName && u.Password == password);
            return user;
        }

        public void UpdateLastLogin(User user)
        {
            user.DateLastLogin = DateTime.Now;
        }

    }
}
